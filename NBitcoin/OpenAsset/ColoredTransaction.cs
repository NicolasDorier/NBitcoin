﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NBitcoin.OpenAsset
{
	public class ColoredEntry : IBitcoinSerializable
	{
		public ColoredEntry()
		{

		}
		public ColoredEntry(int index, Asset asset)
		{
			if(asset == null)
				throw new ArgumentNullException("asset");
			Index = index;
			Asset = asset;
		}
		uint _Index;
		public int Index
		{
			get
			{
				return (int)_Index;
			}
			set
			{
				_Index = (uint)value;
			}
		}
		Asset _Asset = new Asset();
		public Asset Asset
		{
			get
			{
				return _Asset;
			}
			set
			{
				_Asset = value;
			}
		}
		#region IBitcoinSerializable Members

		public void ReadWrite(BitcoinStream stream)
		{
			stream.ReadWriteAsVarInt(ref _Index);
			stream.ReadWrite(ref _Asset);
		}

		#endregion

		public override string ToString()
		{
			if(Asset == null)
				return "[" + Index + "]";
			else
				return "[" + Index + "] " + Asset;
		}
	}
	public class ColoredTransaction : IBitcoinSerializable
	{
		public static ColoredTransaction FetchColors(Transaction tx, IColoredTransactionRepository repo)
        {
            return FetchColorsAsync(tx, repo).Result;
        }

		public static async Task<ColoredTransaction> FetchColorsAsync(Transaction tx, IColoredTransactionRepository repo)
		{
			return await FetchColorsAsync(null, tx, repo);
		}

		public static ColoredTransaction FetchColors(uint256 txId, IColoredTransactionRepository repo)
        {
            return FetchColorsAsync(txId, repo).Result;
        }

		public static async Task<ColoredTransaction> FetchColorsAsync(uint256 txId, IColoredTransactionRepository repo)
		{
			if(repo == null)
				throw new ArgumentNullException("repo");
			repo = EnsureCachedRepository(repo);
			var colored = await repo.GetAsync(txId);
			if(colored != null)
				return colored;
			var tx = await repo.Transactions.GetAsync(txId);
			if(tx == null)
				throw new TransactionNotFoundException("Transaction " + txId + " not found in transaction repository", txId);
			return await FetchColorsAsync(txId, tx, repo);
		}

		public static ColoredTransaction FetchColors(uint256 txId, Transaction tx, IColoredTransactionRepository repo)
        {
            return FetchColorsAsync(txId, tx, repo).Result;
        }

        public static async Task<ColoredTransaction> FetchColorsAsync(uint256 txId, Transaction tx, IColoredTransactionRepository repo)
		{
            Debug.Assert(tx != null);

			txId = txId ?? tx.GetHash();
			var result = await repo.GetAsync(txId);
			if(result != null)
				return result;

			//The following code is to prevent recursion of FetchColors that would fire a StackOverflow if the origin of traded asset were deep in the transaction dependency tree
			repo = EnsureCachedRepository(repo);
            ColoredTransaction coloredTx = await ResolveAncestorsAsync(txId, tx, repo);

            if (coloredTx != null)
            {
                coloredTx.Tx = tx;
            }

			return coloredTx;
		}

        private static async Task<ColoredTransaction> ResolveAncestorsAsync(uint256 txId, Transaction tx, IColoredTransactionRepository repo)
        {
            ColoredTransaction lastColored = null;
            HashSet<uint256> invalidColored = new HashSet<uint256>();
            Stack<Tuple<uint256, Transaction>> ancestors = new Stack<Tuple<uint256, Transaction>>();
            ancestors.Push(Tuple.Create(txId, tx));
            while (ancestors.Count != 0)
            {
                var peek = ancestors.Peek();
                txId = peek.Item1;
                tx = peek.Item2;
                bool isComplete = true;
                if (!tx.HasValidColoredMarker() && ancestors.Count != 1)
                {
                    invalidColored.Add(txId);
                    ancestors.Pop();
                    continue;
                }

                for (int i = 0; i < tx.Inputs.Count; i++)
                {
                    var txin = tx.Inputs[i];
                    if (await repo.GetAsync(txin.PrevOut.Hash) == null && !invalidColored.Contains(txin.PrevOut.Hash))
                    {
                        var prevTx = await repo.Transactions.GetAsync(txin.PrevOut.Hash);
                        if (prevTx == null)
                            throw new TransactionNotFoundException("Transaction " + txin.PrevOut.Hash + " not found in transaction repository", txId);
                        ancestors.Push(Tuple.Create(txin.PrevOut.Hash, prevTx));
                        isComplete = false;
                    }
                }
                if (isComplete)
                {
                    lastColored = await FetchColorsWithAncestorsSolved(txId, tx, repo);
                    ancestors.Pop();
                }
            }

            return lastColored;
        }

		private static IColoredTransactionRepository EnsureCachedRepository(IColoredTransactionRepository repo)
		{
			if(repo is CachedColoredTransactionRepository)
				return repo;
			repo = new CachedColoredTransactionRepository(repo);
			return repo;
		}

		private static async Task<ColoredTransaction> FetchColorsWithAncestorsSolved(uint256 txId, Transaction tx, IColoredTransactionRepository repo)
		{
			ColoredTransaction colored = new ColoredTransaction();

			Queue<ColoredEntry> previousAssetQueue = new Queue<ColoredEntry>();
			for(int i = 0 ; i < tx.Inputs.Count ; i++)
			{
				var txin = tx.Inputs[i];
				var prevColored = await repo.GetAsync(txin.PrevOut.Hash);
				if(prevColored == null)
					continue;
				var prevAsset = prevColored.GetColoredEntry(txin.PrevOut.N);
				if(prevAsset != null)
				{
					var input = new ColoredEntry()
					{
						Index = i,
						Asset = prevAsset.Asset
					};
					previousAssetQueue.Enqueue(input);
					colored.Inputs.Add(input);
				}
			}

			int markerPos = 0;
			var marker = ColorMarker.Get(tx, out markerPos);
			if(marker == null)
			{
				await repo.PutAsync(txId, colored);
				return colored;
			}
			colored.Marker = marker;
			if(!marker.HasValidQuantitiesCount(tx))
			{
				await repo.PutAsync(txId, colored);
				return colored;
			}

			ScriptId issuedAsset = null;
			for(int i = 0 ; i < markerPos ; i++)
			{
				var entry = new ColoredEntry();
				entry.Index = i;
				entry.Asset.Quantity = i >= marker.Quantities.Length ? 0 : marker.Quantities[i];
				if(entry.Asset.Quantity == 0)
					continue;

				if(issuedAsset == null)
				{
					var txIn = tx.Inputs.FirstOrDefault();
					if(txIn == null)
						continue;
					var prev = await repo.Transactions.GetAsync(txIn.PrevOut.Hash);
					if(prev == null)
						throw new TransactionNotFoundException("This open asset transaction is issuing assets, but it needs a parent transaction in the TransactionRepository to know the address of the issued asset (missing : " + txIn.PrevOut.Hash + ")", txIn.PrevOut.Hash);
					issuedAsset = prev.Outputs[(int)txIn.PrevOut.N].ScriptPubKey.ID;
				}
				entry.Asset.Id = issuedAsset;
				colored.Issuances.Add(entry);
			}

			ulong used = 0;
			for(int i = markerPos + 1 ; i < tx.Outputs.Count ; i++)
			{
				var entry = new ColoredEntry();
				entry.Index = i;
				//If there are less items in the  asset quantity list  than the number of colorable outputs (all the outputs except the marker output), the outputs in excess receive an asset quantity of zero.
				entry.Asset.Quantity = (i - 1) >= marker.Quantities.Length ? 0 : marker.Quantities[i - 1];
				if(entry.Asset.Quantity == 0)
					continue;

				//If there are less asset units in the input sequence than in the output sequence, the transaction is considered invalid and all outputs are uncolored. 
				if(previousAssetQueue.Count == 0)
				{
					colored.Transfers.Clear();
					colored.Issuances.Clear();
					await repo.PutAsync(txId, colored);
					return colored;
				}
				entry.Asset.Id = previousAssetQueue.Peek().Asset.Id;
				var remaining = entry.Asset.Quantity;
				while(remaining != 0)
				{
					if(previousAssetQueue.Count == 0 || previousAssetQueue.Peek().Asset.Id != entry.Asset.Id)
					{
						colored.Transfers.Clear();
						colored.Issuances.Clear();
						await repo.PutAsync(txId, colored);
						return colored;
					}
					var assertPart = Math.Min(previousAssetQueue.Peek().Asset.Quantity - used, remaining);
					remaining = remaining - assertPart;
					used += assertPart;
					if(used == previousAssetQueue.Peek().Asset.Quantity)
					{
						previousAssetQueue.Dequeue();
						used = 0;
					}
				}
				colored.Transfers.Add(entry);
			}
			await repo.PutAsync(txId, colored);
			return colored;
		}

		public ColoredEntry GetColoredEntry(uint n)
		{
			return Issuances
				.Concat(Transfers)
				.FirstOrDefault(i => i.Index == n);
		}
		public ColoredTransaction()
		{
			Issuances = new List<ColoredEntry>();
			Transfers = new List<ColoredEntry>();
			Inputs = new List<ColoredEntry>();
		}

        public Transaction Tx { get; set; }

		ColorMarker _Marker;
		public ColorMarker Marker
		{
			get
			{
				return _Marker;
			}
			set
			{
				_Marker = value;
			}
		}

		List<ColoredEntry> _Issuances;
		public List<ColoredEntry> Issuances
		{
			get
			{
				return _Issuances;
			}
			set
			{
				_Issuances = value;
			}
		}

		List<ColoredEntry> _Transfers;
		public List<ColoredEntry> Transfers
		{
			get
			{
				return _Transfers;
			}
			set
			{
				_Transfers = value;
			}
		}

		public Asset[] GetDestroyedAssets()
		{
			var burned = Inputs
				.GroupBy(i => i.Asset.Id)
				.Select(g => new
				{
					Id = g.Key,
					Quantity = g.Aggregate(BigInteger.Zero, (a, o) => a + o.Asset.Quantity)
				});

			var transfered =
				Transfers
				.GroupBy(i => i.Asset.Id)
				.Select(g => new
				{
					Id = g.Key,
					Quantity = -g.Aggregate(BigInteger.Zero, (a, o) => a + o.Asset.Quantity)
				});

			return burned.Concat(transfered)
				.GroupBy(o => o.Id)
				.Select(g => new Asset()
				{
					Id = g.Key,
					Quantity = (ulong)g.Aggregate(BigInteger.Zero, (a, o) => a + o.Quantity)
				})
				.Where(a => a.Quantity != 0)
				.ToArray();
		}

		#region IBitcoinSerializable Members

		public void ReadWrite(BitcoinStream stream)
		{
			if(stream.Serializing)
			{
				if(_Marker != null)
					stream.ReadWrite(ref _Marker);
				else
					stream.ReadWrite(new Script());
			}
			else
			{
				Script script = new Script();
				stream.ReadWrite(ref script);
				if(script.Length != 0)
				{
					_Marker = new ColorMarker(script);
				}
				else
				{
				}
			}
			stream.ReadWrite(ref _Inputs);
			stream.ReadWrite(ref _Issuances);
			stream.ReadWrite(ref _Transfers);
		}

		#endregion

		List<ColoredEntry> _Inputs;
		public List<ColoredEntry> Inputs
		{
			get
			{
				return _Inputs;
			}
			set
			{
				_Inputs = value;
			}
		}

		//00000000000000001c7a19e8ef62d815d84a473f543de77f23b8342fc26812a9 at 299220 Monday, May 5, 2014 3:47:37 PM first block
		public static readonly DateTimeOffset FirstColoredDate = new DateTimeOffset(2014, 05, 4, 0, 0, 0, TimeSpan.Zero);
	}
}
