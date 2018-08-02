﻿using NBitcoin.DataEncoders;
using NBitcoin.Protocol;
using System;
using System.Diagnostics;
using System.Linq;
using NBitcoin.Crypto;

namespace NBitcoin.Altcoins
{
	// Reference: https://github.com/Dystem/Dystem/blob/master/src/chainparams.cpp
	public class Dystem:NetworkSetBase
	{

		public static Dystem Instance { get; } = new Dystem();

		public override string CryptoCode => "DTEM";

		private Dystem()
		{

		}


		public class DystemConsensusFactory : ConsensusFactory
		{
			private DystemConsensusFactory()
			{
			}

			public static DystemConsensusFactory Instance { get; } = new DystemConsensusFactory();

			public override BlockHeader CreateBlockHeader()
			{
				return new DystemBlockHeader();
			}
			public override Block CreateBlock()
			{
				return new DystemBlock(new DystemBlockHeader());
			}
		}

#pragma warning disable CS0618 // Type or member is obsolete
		public class DystemBlockHeader : BlockHeader
		{
			// https://github.com/Dystemp/Dystem/blob/e596762ca22d703a79c6880a9d3edb1c7c972fd3/src/primitives/block.cpp#L13
			static byte[] CalculateHash(byte[] data, int offset, int count)
			{
				Debug.Print(Encoders.Hex.EncodeData(data));
				return new HashQuark.Quark().ComputeBytes(data.Skip(offset).Take(count).ToArray());
			}

			protected override HashStreamBase CreateHashStream()
			{
				return BufferedHashStream.CreateFrom(CalculateHash);
			}

		}

		public class DystemBlock : Block
		{
#pragma warning disable CS0612 // Type or member is obsolete
			public DystemBlock(DystemBlockHeader h) : base(h)
#pragma warning restore CS0612 // Type or member is obsolete
			{

			}
			public override ConsensusFactory GetConsensusFactory()
			{
				return Dystem.Instance.Mainnet.Consensus.ConsensusFactory;
			}
		}
#pragma warning restore CS0618 // Type or member is obsolete

		protected override void PostInit()
		{
			RegisterDefaultCookiePath(Mainnet, ".cookie");
			RegisterDefaultCookiePath(Regtest, "regtest", ".cookie");
			RegisterDefaultCookiePath(Testnet, "testnet3", ".cookie");
		}

		
		static uint256 GetPoWHash(BlockHeader header)
		{
			var headerBytes = header.ToBytes();
			var h = NBitcoin.Crypto.SCrypt.ComputeDerivedKey(headerBytes, headerBytes, 1024, 1, 1, null, 32);
			return new uint256(h);
		}

		
		protected override NetworkBuilder CreateMainnet()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
				{
					SubsidyHalvingInterval = 210240,
					MajorityEnforceBlockUpgrade = 750,
					MajorityRejectBlockOutdated = 950,
					MajorityWindow = 1000,
					BIP34Hash = new uint256("0x000007d91d1254d60e2dd1ae580383070a4ddffa4c64c2eeb4a2f9ecc0414343"),
					PowLimit = new Target(new uint256("0x000007d91d1254d60e2dd1ae580383070a4ddffa4c64c2eeb4a2f9ecc0414343")),
					MinimumChainWork = new uint256("0x000000000000000000000000000000000000000000000100a308553b4863b755"),
					PowTargetTimespan = TimeSpan.FromSeconds(24 * 60 * 60),
					PowTargetSpacing = TimeSpan.FromSeconds(2.5 * 60),
					PowAllowMinDifficultyBlocks = false,
					CoinbaseMaturity = 30,
					PowNoRetargeting = false,
					RuleChangeActivationThreshold = 1916,
					MinerConfirmationWindow = 2016,
					ConsensusFactory = DystemConsensusFactory.Instance,
					SupportSegwit = false
				})
				.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] {30})
				.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] {68})
				.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] {58})
				.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] {0x04, 0x88, 0xB2, 0x1E})
				.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] {0x04, 0x88, 0xAD, 0xE4})
				
				.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("Dystem"))
				.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("Dystem"))
				.SetMagic(0xBD6B0CBF)
				.SetPort(16443)
				.SetRPCPort(17100)
				.SetMaxP2PVersion(70208)
				.SetName("Dystem-main")
				.AddAlias("Dystem-mainnet")
				.AddDNSSeeds(new[]
				{
					new DNSSeedData("seed.dystem.io", "seed.dystem.io"),
					new DNSSeedData("seed2.dystem.io", "seed2.dystem.io"),
					new DNSSeedData("seed3.dystem.io", "seed3.dystem.io"),
					new DNSSeedData("seed.hashbeat.io", "seed.hashbeat.io")
				})
				.AddSeeds(new NetworkAddress[0])
				.SetGenesis("01000000000000000000000000000000000000000000000000000000000000000000000027cc0d8f6a20e41f445b1045d1c73ba4b068ee60b5fd4aa34027cbbe5c2e161e1546db5af0ff0f1e18cb3f010101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff6704ffff001d01044c5e4120736c69702d75702062792073757065726d61726b6574204173646120696e20616e206f6e6c696e65206f7264657220736177206120776f6d616e206368617267656420c2a339333020666f7220612073696e676c652062616e616e61ffffffff010000000000000000434104575f641084f76b9e94aae509ce78f6213ee4855d5c245b76d931fa190a1b453edf3ecf2b28288a338ac186d07eedc6d99256838cb57322406edc697f239a0a6eac00000000");
		
			return builder;
		}
		
		protected override NetworkBuilder CreateTestnet()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
				{
					SubsidyHalvingInterval = 210240,
					MajorityEnforceBlockUpgrade = 51,
					MajorityRejectBlockOutdated = 75,
					MajorityWindow = 100,
					BIP34Hash = new uint256("0x0000047d24635e347be3aaaeb66c26be94901a2f962feccd4f95090191f208c1"),
					PowLimit = new Target(new uint256("0x00000fffff000000000000000000000000000000000000000000000000000000")),
					MinimumChainWork = new uint256("0x000000000000000000000000000000000000000000000000000924e924a21715"),
					PowTargetTimespan = TimeSpan.FromSeconds(24 * 60 * 60),
					PowTargetSpacing = TimeSpan.FromSeconds(2.5 * 60),
					PowAllowMinDifficultyBlocks = true,
					CoinbaseMaturity = 30,
					PowNoRetargeting = false,
					RuleChangeActivationThreshold = 1512,
					MinerConfirmationWindow = 2016,
					ConsensusFactory = DystemConsensusFactory.Instance,
					SupportSegwit = false
				})
				.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] {140})
				.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] {19})
				.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] {239})
				.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] {0x04, 0x35, 0x87, 0xCF})
				.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] {0x04, 0x35, 0x83, 0x94})
				.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tDystem"))
				.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tDystem"))
				.SetMagic(0xFFCAE2CE)
				.SetPort(19999)
				.SetRPCPort(19998)
				.SetMaxP2PVersion(70208)
				.SetName("Dystem-test")
				.AddAlias("Dystem-testnet")
				.AddDNSSeeds(new[]
				{
					new DNSSeedData("Dystemdot.io", "testnet-seed.Dystemdot.io"),
					new DNSSeedData("masternode.io", "test.dnsseed.masternode.io")
				})
				.AddSeeds(new NetworkAddress[0])
				.SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000c762a6567f3cc092f0684bb62b7e00a84890b990f07cc71a6bb58d64b98e02e0dee1e352f0ff0f1ec3c927e60101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff6204ffff001d01044c5957697265642030392f4a616e2f3230313420546865204772616e64204578706572696d656e7420476f6573204c6976653a204f76657273746f636b2e636f6d204973204e6f7720416363657074696e6720426974636f696e73ffffffff0100f2052a010000004341040184710fa689ad5023690c80f3a49c8f13f8d45b8c857fbcbc8bc4a8e4d3eb4b10f4d4604fa08dce601aaf0f470216fe1b51850b4acf21b179c45070ac7b03a9ac00000000");

			return builder;
		}

		protected override NetworkBuilder CreateRegtest()
		{
			var builder = new NetworkBuilder();
			var res = builder.SetConsensus(new Consensus()
				{
					SubsidyHalvingInterval = 210240,
					MajorityEnforceBlockUpgrade = 750,
					MajorityRejectBlockOutdated = 950,
					MajorityWindow = 1000,
					BIP34Hash = new uint256("0x000007d91d1254d60e2dd1ae580383070a4ddffa4c64c2eeb4a2f9ecc0414343"),
					PowLimit = new Target(new uint256("0x000007d91d1254d60e2dd1ae580383070a4ddffa4c64c2eeb4a2f9ecc0414343")),
					MinimumChainWork = new uint256("0x000000000000000000000000000000000000000000000100a308553b4863b755"),
					PowTargetTimespan = TimeSpan.FromSeconds(24 * 60 * 60),
					PowTargetSpacing = TimeSpan.FromSeconds(2.5 * 60),
					PowAllowMinDifficultyBlocks = false,
					CoinbaseMaturity = 30,
					PowNoRetargeting = false,
					RuleChangeActivationThreshold = 1916,
					MinerConfirmationWindow = 2016,
					ConsensusFactory = DystemConsensusFactory.Instance,
					SupportSegwit = false
				})
				.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] {30})
				.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] {68})
				.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] {58})
				.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] {0x04, 0x88, 0xB2, 0x1E})
				.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] {0x04, 0x88, 0xAD, 0xE4})
				.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tDystem"))
				.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tDystem"))
				.SetMagic(0xDCB7C1FC)
				.SetPort(19994)
				.SetRPCPort(17100)
				.SetMaxP2PVersion(70208)
				.SetName("Dystem-reg")
				.AddAlias("Dystem-regtest")
				.AddDNSSeeds(new DNSSeedData[0])
				.AddSeeds(new NetworkAddress[0])
				.SetGenesis("01000000000000000000000000000000000000000000000000000000000000000000000027cc0d8f6a20e41f445b1045d1c73ba4b068ee60b5fd4aa34027cbbe5c2e161e1546db5af0ff0f1e18cb3f010101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff6704ffff001d01044c5e4120736c69702d75702062792073757065726d61726b6574204173646120696e20616e206f6e6c696e65206f7264657220736177206120776f6d616e206368617267656420c2a339333020666f7220612073696e676c652062616e616e61ffffffff010000000000000000434104575f641084f76b9e94aae509ce78f6213ee4855d5c245b76d931fa190a1b453edf3ecf2b28288a338ac186d07eedc6d99256838cb57322406edc697f239a0a6eac00000000");
	

			return builder;
		}
		

	}
}