using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBitcoin.JsonConverters;
using NBitcoin.RPC;
using Newtonsoft.Json.Linq;

namespace NBitcoin.Altcoins.Elements
{
	public static class ElementsRPCClientExtensions
	{
		public enum ElementsRPCOperations
		{
			decoderawtransaction,
			importblindingkey,
			getnewaddress,
			getaddressinfo,
			getbalance
		}
		public static Task<RPCResponse> SendCommandAsync(this RPCClient rpcClient, ElementsRPCOperations commandName,
			params object[] parameters)
		{
			return rpcClient.SendCommandAsync(commandName.ToString(), parameters);
		}

		public static async Task<ElementsTransaction> DecodeRawTransaction(
			this RPCClient rpcClient, string transactionHex, Network network = null)
		{
			var result = await rpcClient.SendCommandAsync(ElementsRPCOperations.decoderawtransaction, transactionHex);
			result.ThrowIfError();
			return Serializer.ToObject<ElementsTransaction>(result.Result.ToString(),
				network ?? Liquid.Instance.Mainnet);
		}

		public static Task<RPCResponse> ImportBlindingKey(this RPCClient rpcClient,
			BitcoinBlindedAddress bitcoinBlindedAddress, Key blindingKey)
		{
			return rpcClient.SendCommandAsync(ElementsRPCOperations.importblindingkey,
				bitcoinBlindedAddress.ToString(), blindingKey.ToHex());
		}

		/// <summary>
		/// This is an aggregate of multiple RPC commands to unblind a confidential transaction. It calls importblindingkey and unblindrawtransaction.
		/// </summary>
		/// <param name="rpcClient"></param>
		/// <param name="addressBlindingKeys"></param>
		/// <param name="transaction"></param>
		/// <param name="network"></param>
		/// <returns></returns>
		public static async Task<ElementsTransaction> UnblindTransaction(
			this RPCClient rpcClient, List<(BitcoinBlindedAddress address, Key blindingKey)> addressBlindingKeys,
			ElementsTransaction transaction, Network network)
		{
			addressBlindingKeys.ForEach(async tuple =>
			{
				var blindImportResponse = await rpcClient.ImportBlindingKey(tuple.address, tuple.blindingKey);
				blindImportResponse.ThrowIfError();
			});

			return await rpcClient.DecodeRawTransaction(transaction.ToHex(), network);
		}

		public static async Task<BitcoinAddress> GetNewAddressAsync(this RPCClient rpcClient, Network network)
		{
			var result = await rpcClient.SendCommandAsync(ElementsRPCOperations.getnewaddress).ConfigureAwait(false);
			return new BitcoinBlindedAddress(result.ResultString, network);
		}


		public static async Task<ElementsGetAddressInfoResponse> GetAddressInfoAsync(this RPCClient rpcClient, string address, Network network)
		{
			var response = await rpcClient.SendCommandAsync(ElementsRPCOperations.getaddressinfo, address);

			return ElementsGetAddressInfoResponse.FromJsonResponse((JObject)response.Result, network);
		}

		public static async Task<Dictionary<string,Money>> GetBalancesAsync(this RPCClient rpcClient)
		{
			var response = await rpcClient.SendCommandAsync(ElementsRPCOperations.getbalance);

			return response.Result.Children().ToDictionary(token => (token as JProperty).Name,
				token => Money.Parse(((JProperty) token).Value.ToString()));
		}


	}

	public class ElementsGetAddressInfoResponse : GetAddressInfoResponse
	{
		public BitcoinBlindedAddress Confidential { get; set; }
		public BitcoinAddress Unconfidential { get; set; }
		public PubKey ConfidentialKey { get; set; }
		public override GetAddressInfoResponse LoadFromJson(JObject raw, Network network)
		{
			base.LoadFromJson(raw, network);
			Confidential = new BitcoinBlindedAddress(raw.Property("confidential").Value.Value<string>(), network);
			Unconfidential = BitcoinAddress.Create(raw.Property("unconfidential").Value.Value<string>(), network);
			ConfidentialKey = new PubKey(raw.Property("confidential_key").Value.Value<string>());
			return this;
		}

		public new static ElementsGetAddressInfoResponse FromJsonResponse(JObject raw, Network network)
		{
			return (ElementsGetAddressInfoResponse) new ElementsGetAddressInfoResponse().LoadFromJson(raw, network);
		}
	}
}
