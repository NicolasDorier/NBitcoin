using System;
using System.Collections.Generic;
using System.Linq;

namespace NBitcoin.DataEncoders
{
	public class Blech32Encoder : DataEncoder
	{
		private static readonly byte[] Byteset = Encoders.ASCII.DecodeData("qpzry9x8gf2tvdw0s3jn54khce6mua7l");
		private static readonly ulong[] Generator = { 0x7d52fba40bd886, 0x5e8dbf1a03950c, 0x1c3a3c74072a18, 0x385d72fa0e5139, 0x7093e5a608865b };

		internal Blech32Encoder(string hrp) : this(hrp == null ? null : Encoders.ASCII.DecodeData(hrp))
		{
		}
		public Blech32Encoder(byte[] hrp)
		{
			if (hrp == null)
				throw new ArgumentNullException(nameof(hrp));

			_Hrp = hrp;
			var len = hrp.Length;
			_HrpExpand = new byte[(2 * len) + 1];
			for (int i = 0; i < len; i++)
			{
				_HrpExpand[i] = (byte)(hrp[i] >> 5);
				_HrpExpand[i + len + 1] = (byte)(hrp[i] & 31);
			}
		}

		private readonly byte[] _HrpExpand;
		private readonly byte[] _Hrp;
		public byte[] HumanReadablePart
		{
			get
			{
				return _Hrp;
			}
		}

		private static ulong Polymod(byte[] values)
		{
			ulong chk = 1;
			foreach (var value in values)
			{
				var top = chk >> 55;
				chk = value ^ ((chk & 0x7fffffffffffff) << 5);
				foreach (var i in Enumerable.Range(0, 5))
				{
					chk ^= ((top >> i) & 1) == 1 ? Generator[i] : 0;
				}
			}
			return chk;
		}

		bool VerifyChecksum(byte[] data)
		{
			var values = _HrpExpand.Concat(data);
			return Polymod(values) == 1;
		}

		private byte[] CreateChecksum(byte[] data, int offset, int count)
		{
			var values = new byte[_HrpExpand.Length + count + 12];
			var valuesOffset = 0;
			Array.Copy(_HrpExpand, 0, values, valuesOffset, _HrpExpand.Length);
			valuesOffset += _HrpExpand.Length;
			Array.Copy(data, offset, values, valuesOffset, count);
			valuesOffset += count;
			var polymod = Polymod(values) ^ 1;
			var ret = new byte[12];
			foreach (var i in Enumerable.Range(0, 12))
			{
				ret[i] = (byte)((polymod >> 5 * (11 - i)) & 31);
			}
			return ret;
		}


		public override string EncodeData(byte[] data, int offset, int count)
		{
			var combined = new byte[_Hrp.Length + 1 + count + 6];
			int combinedOffset = 0;
			Array.Copy(_Hrp, 0, combined, 0, _Hrp.Length);
			combinedOffset += _Hrp.Length;
			combined[combinedOffset] = 49;
			combinedOffset++;
			Array.Copy(data, offset, combined, combinedOffset, count);
			combinedOffset += count;
			var checkSum = CreateChecksum(data, offset, count);
			Array.Copy(checkSum, 0, combined, combinedOffset, 6);
			combinedOffset += 6;
			for (int i = 0; i < count + 6; i++)
			{
				combined[_Hrp.Length + 1 + i] = Byteset[combined[_Hrp.Length + 1 + i]];
			}
			return Encoders.ASCII.EncodeData(combined);
		}

		public static Blech32Encoder ExtractEncoderFromString(string test)
		{
			var i = test.IndexOf('1');
			if (i == -1)
				throw new FormatException("Invalid Blech32 string");
			return Encoders.Blech32(test.Substring(0, i));
		}

		internal static void CheckCase(string hrp)
		{
			if (hrp.ToLowerInvariant().Equals(hrp))
				return;
			if (hrp.ToUpperInvariant().Equals(hrp))
				return;
			throw new FormatException("Invalid blech32 string, mixed case detected");
		}
		public override byte[] DecodeData(string encoded)
		{
			throw new NotSupportedException();
		}
		byte[] DecodeDataCore(string encoded)
		{
			if (encoded == null)
				throw new ArgumentNullException(nameof(encoded));
			CheckCase(encoded);
			var buffer = Encoders.ASCII.DecodeData(encoded);
			if (buffer.Any(b => b < 33 || b > 126))
			{
				throw new FormatException("bech chars are out of range");
			}
			encoded = encoded.ToLowerInvariant();
			buffer = Encoders.ASCII.DecodeData(encoded);
			var pos = encoded.LastIndexOf("1", StringComparison.OrdinalIgnoreCase);
			if (pos < 1 || pos + 7 > encoded.Length || encoded.Length > 90)
			{
				throw new FormatException("bech missing separator, separator misplaced or too long input");
			}
			if (buffer.Skip(pos + 1).Any(x => !Byteset.Contains(x)))
			{
				throw new FormatException("bech chars are out of range");
			}

			buffer = Encoders.ASCII.DecodeData(encoded);
			var hrp = Encoders.ASCII.DecodeData(encoded.Substring(0, pos));
			if (!hrp.SequenceEqual(_Hrp))
			{
				throw new FormatException("Mismatching human readable part");
			}
			var data = new byte[encoded.Length - pos - 1];
			for (int j = 0, i = pos + 1; i < encoded.Length; i++, j++)
			{
				data[j] = (byte)Array.IndexOf(Byteset, buffer[i]);
			}

			if (!VerifyChecksum(data))
			{
				throw new FormatException("Error while verifying Blech32 checksum");
			}
			return data.Take(data.Length - 6).ToArray();
		}

		private static byte[] ConvertBits(IEnumerable<byte> data, int fromBits, int toBits, bool pad = true)
		{
			var acc = 0;
			var bits = 0;
			var maxv = (1 << toBits) - 1;
			var ret = new List<byte>();
			foreach (var value in data)
			{
				if ((value >> fromBits) > 0)
					throw new FormatException("Invalid Blech32 string");
				acc = (acc << fromBits) | value;
				bits += fromBits;
				while (bits >= toBits)
				{
					bits -= toBits;
					ret.Add((byte)((acc >> bits) & maxv));
				}
			}
			if (pad)
			{
				if (bits > 0)
				{
					ret.Add((byte)((acc << (toBits - bits)) & maxv));
				}
			}
			else if (bits >= fromBits || (byte)(((acc << (toBits - bits)) & maxv)) != 0)
			{
				throw new FormatException("Invalid Blech32 string");
			}
			return ret.ToArray();
		}

		public byte[] Decode(string addr, out byte witnessVerion)
		{
			if (addr == null)
				throw new ArgumentNullException(nameof(addr));
			CheckCase(addr);
			var data = DecodeDataCore(addr);

			var decoded = ConvertBits(data.Skip(1), 5, 8, false);
			if (decoded.Length < 2 || decoded.Length > 40)
				throw new FormatException("Invalid decoded data length");

			witnessVerion = data[0];
			if (witnessVerion > 16)
				throw new FormatException("Invalid decoded witness version");

			if (witnessVerion == 0 && decoded.Length != 20 && decoded.Length != 32)
				throw new FormatException("Decoded witness program with unknown length");
			return decoded;
		}

		public string Encode(byte witnessVerion, byte[] witnessProgramm)
		{
			var data = (new[] { witnessVerion }).Concat(ConvertBits(witnessProgramm, 8, 5));
			var ret = EncodeData(data);
			return ret;
		}
	}
}
