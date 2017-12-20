using NBitcoin.DataEncoders;
using System.Linq;
using NBitcoin.Protocol;
using System;
using System.Net;
using System.Collections.Generic;

namespace NBitcoin.Dash
{
	public class Networks
	{
		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)
		static Tuple<byte[], int>[] pnSeed6_main = {
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb3,0x2b,0x80,0xef}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x80,0x7f,0x6a,0xeb}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x25,0x9d,0xfa,0x0a}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xa2,0xd1,0x63,0x23}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x6c,0x3d,0xd2,0x36}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xac,0xf5,0x05,0x84}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2e,0xa2,0x42,0x0a}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x4e,0x6d,0xb2,0xc3}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x8a,0x80,0xa9,0x5e}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x34,0x0b,0x8d,0xe5}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x25,0x3b,0x15,0x3a}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2e,0x69,0x76,0x0f}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb2,0x21,0x7e,0xdd}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x68,0xec,0x17,0x83}, 9999),
            Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x6c,0x3d,0xd1,0x25}, 9999)
        };
		static Tuple<byte[], int>[] pnSeed6_test = {
	        Tuple.Create(new byte[]{0xfd,0x87,0xd8,0x7e,0xeb,0x43,0x99,0xcb,0x26,0x31,0xba,0x48,0x51,0x31,0x39,0x0d}, 18333),
	        Tuple.Create(new byte[]{0xfd,0x87,0xd8,0x7e,0xeb,0x43,0x44,0xf4,0xf4,0xf0,0xbf,0xf7,0x7e,0x6d,0xc4,0xe8}, 18333)
        };

		public static void Register()
		{
			var port = 9999;
			NetworkBuilder builder = new NetworkBuilder();
			_Mainnet = builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210240, // Note: actual number of blocks per calendar year with DGW v3 is ~200700 (for example 449750 - 249050),
                MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000,
				BIP34Hash = new uint256("0x000007d91d1254d60e2dd1ae580383070a4ddffa4c64c2eeb4a2f9ecc0414343"),
				PowLimit = new Target(new uint256("00000fffff000000000000000000000000000000000000000000000000000000")),
				PowTargetTimespan = TimeSpan.FromSeconds(24 * 60 * 60), // Dash: 1 day
                PowTargetSpacing = TimeSpan.FromSeconds(2.5 * 60), // Dash: 2.5 minutes
                PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1916, // 95% of 2016
                MinerConfirmationWindow = 2016, // nPowTargetTimespan / nPowTargetSpacing
                CoinbaseMaturity = 100,
				HashGenesisBlock = new uint256("0x00000ffd590b1485b3caadc19b22e6379c733355108f107a430458cdf3407ab6"),
				GetPoWHash = GetPoWHash,
				LitecoinWorkCalculation = true
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 76 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 16 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 204 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("dash"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("dash"))
			//.SetMagic(0xdbb6c0fb)
			.SetPort(port)
			.SetRPCPort(9998)
			.SetName("dash-main")
			.AddAlias("dash-mainnet")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("dash.org", "dnsseed.dash.org"),
				new DNSSeedData("dashdot.io", "dnsseed.dashdot.io"),
				new DNSSeedData("masternode.io", "dnsseed.masternode.io"),
				new DNSSeedData("dashpay.io", "dnsseed.dashpay.io"),
			})
			.AddSeeds(ToSeed(pnSeed6_main))
            .SetGenesis(new Block(Encoders.Hex.DecodeData("010000000000000000000000000000000000000000000000000000000000000000000000c762a6567f3cc092f0684bb62b7e00a84890b990f07cc71a6bb58d64b98e02e0b9968054ffff7f20ffba10000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff6204ffff001d01044c5957697265642030392f4a616e2f3230313420546865204772616e64204578706572696d656e7420476f6573204c6976653a204f76657273746f636b2e636f6d204973204e6f7720416363657074696e6720426974636f696e73ffffffff0100f2052a010000004341040184710fa689ad5023690c80f3a49c8f13f8d45b8c857fbcbc8bc4a8e4d3eb4b10f4d4604fa08dce601aaf0f470216fe1b51850b4acf21b179c45070ac7b03a9ac00000000")))
            .BuildAndRegister();

            builder = new NetworkBuilder();
			port = 19994;
			_Testnet = builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210240,
				MajorityEnforceBlockUpgrade = 51,
				MajorityRejectBlockOutdated = 75,
				MajorityWindow = 100,
				PowLimit = new Target(new uint256("00000fffff000000000000000000000000000000000000000000000000000000")),
				PowTargetTimespan = TimeSpan.FromSeconds(24 * 60 * 60), // Dash: 1 day
				PowTargetSpacing = TimeSpan.FromSeconds(2.5 * 60), // Dash: 2.5 minutes
				PowAllowMinDifficultyBlocks = true,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1512, // 75% for testchains
				MinerConfirmationWindow = 2016, // nPowTargetTimespan / nPowTargetSpacing
				CoinbaseMaturity = 100,
				HashGenesisBlock = new uint256("0x00000bafbc94add76cb75e2ec92894837288a481e5c005f6563d91623bf8bc2c"),
				GetPoWHash = GetPoWHash,
				LitecoinWorkCalculation = true
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 140 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 19 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })

            .SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tdash"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tdash"))
			//.SetMagic(0xf1c8d2fd)
			.SetPort(port)
			.SetRPCPort(19998)
			.SetName("dash-test")
			.AddAlias("dash-testnet")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("dashdot.io",  "testnet-seed.dashdot.io"),
				new DNSSeedData("masternode.io", "test.dnsseed.masternode.io"),
			})
			.AddSeeds(ToSeed(pnSeed6_test))
            .SetGenesis(new Block(Encoders.Hex.DecodeData("010000000000000000000000000000000000000000000000000000000000000000000000c762a6567f3cc092f0684bb62b7e00a84890b990f07cc71a6bb58d64b98e02e0dee1e352f0ff0f1ec3c927e60101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff6204ffff001d01044c5957697265642030392f4a616e2f3230313420546865204772616e64204578706572696d656e7420476f6573204c6976653a204f76657273746f636b2e636f6d204973204e6f7720416363657074696e6720426974636f696e73ffffffff0100f2052a010000004341040184710fa689ad5023690c80f3a49c8f13f8d45b8c857fbcbc8bc4a8e4d3eb4b10f4d4604fa08dce601aaf0f470216fe1b51850b4acf21b179c45070ac7b03a9ac00000000")))
            .BuildAndRegister();
        }

        static uint256 GetPoWHash(BlockHeader header)
		{
			var headerBytes = header.ToBytes();
			var h = NBitcoin.Crypto.SCrypt.ComputeDerivedKey(headerBytes, headerBytes, 1024, 1, 1, null, 32);
			return new uint256(h);
		}

		private static IEnumerable<NetworkAddress> ToSeed(Tuple<byte[], int>[] tuples)
		{
			return tuples
					.Select(t => new NetworkAddress(new IPAddress(t.Item1), t.Item2))
					.ToArray();
		}

		private static Network _Mainnet;
		public static Network Mainnet
		{
			get
			{
				AssertRegistered();
				return _Mainnet;
			}
		}

		private static void AssertRegistered()
		{
			if(_Mainnet == null)
				throw new InvalidOperationException("You need to call DashNetworks.Register() before using the dash networks");
		}

		private static Network _Testnet;
		public static Network Testnet
		{
			get
			{
				AssertRegistered();
				return _Testnet;
			}
		}
	}
}
