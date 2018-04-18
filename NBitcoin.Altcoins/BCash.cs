using NBitcoin;
using System.Reflection;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NBitcoin.Altcoins
{
	public class BCash
	{
		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)
		static Tuple<byte[], int>[] pnSeed6_main = {
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x01,0x20,0xc8,0x78}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x02,0x21,0x16,0xa1}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x09,0x13,0x6d}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x09,0x1c,0x0a}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x09,0x90,0x53}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x09,0xdc,0x84}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x2c,0x61,0x6e}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x38,0x28,0x01}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x38,0x32,0x71}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x38,0xf7,0x45}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x3d,0x21,0xdc}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x3d,0x28,0x38}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x4f,0x4f,0x96}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x67,0x89,0x92}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x87,0x9d,0x11}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0xbd,0x90,0xfa}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0xbd,0x99,0x85}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0xbd,0xa4,0x93}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0xbd,0xac,0xc8}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0xbd,0xbf,0x7b}, 8335),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0xe6,0x91,0x82}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0xf9,0x3a,0x4c}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x08,0x26,0x58,0x7e}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0c,0x17,0x7f,0xaf}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x36,0x5f,0x93}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x36,0xe0,0x54}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x37,0x83,0xf0}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x37,0xc8,0xb1}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x38,0xa8,0x40}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x44,0xda,0xf6}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x49,0x00,0x3d}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x4e,0x70,0x0b}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x52,0x5c,0xc9}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x5c,0x52,0xeb}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x5e,0x29,0x52}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x72,0x7f,0xbd}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x72,0xee,0xb8}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x72,0xf5,0xa0}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7c,0x6e,0x65}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7d,0x17,0xa4}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7d,0x3b,0x87}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0x0a,0x03}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0x1f,0x8d}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0x20,0x67}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0x28,0x32}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0x5d,0x82}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0x8b,0xb6}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0x9b,0x3f}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0xd1,0x75}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0xe2,0xdd}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0xec,0x29}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0xef,0x57}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0x7e,0xef,0xdb}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xd2,0x1e,0x16}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xd2,0xb0,0x7c}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe4,0x6d,0x99}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe5,0x39,0x19}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe5,0x39,0xd5}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe5,0x3b,0xf0}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe5,0x3e,0xbf}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe5,0x3f,0x32}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe5,0x71,0x8f}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe5,0x7a,0xc4}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0e,0x03,0x26,0xb3}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0e,0x22,0xae,0xb5}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0e,0x3f,0x07,0x3c}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0e,0xa1,0x03,0x88}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x12,0x66,0xde,0x7d}, 8335),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x12,0x66,0xde,0x7e}, 8335),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x12,0x66,0xde,0xeb}, 8335),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x12,0xc4,0x00,0xf2}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x17,0x5b,0xef,0x4c}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x17,0x61,0x4c,0x60}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x17,0x63,0xcc,0x47}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x17,0x7e,0x7e,0x7b}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x17,0xf2,0x89,0xed}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x04,0xdf,0x07}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x06,0x23,0x54}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x06,0xbb,0x4f}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x10,0x4b,0x80}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x1c,0x1f,0x2d}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x2c,0x04,0x5d}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x30,0x0d,0x57}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x41,0x38,0x89}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x47,0x22,0xc6}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x47,0x28,0x2e}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x4c,0x7a,0x6c}, 8090),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x5b,0x54,0xf1}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x71,0xc1,0x18}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x78,0xaf,0xf3}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xb0,0x0d,0x0a}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xbe,0x32,0x71}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xbe,0x73,0x7c}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xbe,0x7a,0xd5}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xc0,0x36,0x80}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xc4,0xb1,0x9f}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xcd,0x8f,0xb2}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xd1,0x74,0x86}, 8333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0xd4,0x8c,0xa3}, 28333),
};

		static Tuple<byte[], int>[] pnSeed6_test = {
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x09,0x96,0x70}, 9696),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0x0a,0x4a,0x72}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x0d,0xe6,0x8c,0x79}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x23,0xb8,0x98,0xad}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x23,0xc1,0x85,0xf4}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x27,0x6a,0xf8,0x2d}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2e,0x65,0xf0,0x47}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2f,0x34,0x1f,0xe8}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2f,0x5b,0xc6,0xae}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x33,0xfe,0xdb,0xd6}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x34,0x32,0x55,0x9d}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x36,0xf9,0x33,0x6d}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x43,0xcd,0xb3,0xa1}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x57,0xec,0xc6,0x07}, 18433),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x5d,0x7c,0x04,0x59}, 38333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x68,0xc6,0xc2,0xa6}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x68,0xee,0xc6,0xa5}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x78,0x4f,0x35,0x22}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x80,0xc7,0x90,0xe8}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x82,0xd3,0xa2,0x7c}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x90,0xd9,0x49,0x56}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x96,0x5f,0x22,0x61}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x9e,0x45,0x77,0x23}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb0,0x09,0x59,0xd9}, 9696),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb0,0x09,0x9a,0x6e}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb9,0x0c,0x07,0x77}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xbc,0x28,0x5d,0xcd}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xcf,0x9a,0xc4,0x95}, 18333),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xcf,0x9a,0xd2,0xde}, 10201),
		Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xda,0xf4,0x92,0x6f}, 18333)
};


		[Obsolete("Use EnsureRegistered instead")]
		public static void Register()
		{
			EnsureRegistered();
		}
		public static void EnsureRegistered()
		{
			if(_LazyRegistered.IsValueCreated)
				return;
			// This will cause RegisterLazy to evaluate
			new Lazy<object>[] { _LazyRegistered }.Select(o => o.Value != null).ToList();
		}
		static Lazy<object> _LazyRegistered = new Lazy<object>(RegisterLazy, false);
		
		public static Network GetNetwork(NetworkType networkType) // @@@
		{
			EnsureRegistered();
			switch (networkType)
			{
				case NetworkType.Main:
					return _Mainnet;
				case NetworkType.Testnet:
					return _Testnet;
				case NetworkType.Regtest:
					return _Regtest;
			}
			return null;
		}
		
		class BCashConsensusFactory : ConsensusFactory
		{
			public override ProtocolCapabilities GetProtocolCapabilities(uint protocolVersion)
			{
				var capabilities = base.GetProtocolCapabilities(protocolVersion);
				capabilities.SupportWitness = false;
				return capabilities;
			}
		}

		public class BTrashPubKeyAddress : BitcoinPubKeyAddress
		{
			BCashAddr.BchAddr.BchAddrData addr;
			internal BTrashPubKeyAddress(string str, BCashAddr.BchAddr.BchAddrData addr) : base(str, new KeyId(addr.Hash), addr.Network)
			{
				this.addr = addr;
			}

			public BitcoinPubKeyAddress AsBitpay()
			{
				return new BitcoinPubKeyAddress(new KeyId(this.addr.Hash), Network);
			}

			protected override Script GeneratePaymentScript()
			{
				return new KeyId(this.addr.Hash).ScriptPubKey;
			}
		}

		public class BTrashScriptAddress : BitcoinScriptAddress
		{
			BCashAddr.BchAddr.BchAddrData addr;
			internal BTrashScriptAddress(string str, BCashAddr.BchAddr.BchAddrData addr) : base(str, new ScriptId(addr.Hash), addr.Network)
			{
				this.addr = addr;
			}

			public BitcoinScriptAddress AsBitpay()
			{
				return new BitcoinScriptAddress(new ScriptId(this.addr.Hash), Network);
			}
		}

		class BCashStringParser : NetworkStringParser
		{
			string _Prefix;
			public BCashStringParser(string prefix)
			{
				_Prefix = prefix;
			}

			public override bool TryParse<T>(string str, Network network, out T result)
			{
				if(typeof(BitcoinAddress).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
				{
					var prefix = _Prefix;
					str = str.Trim();
					if(str.StartsWith($"{prefix}:", StringComparison.OrdinalIgnoreCase))
					{
						try
						{
							var addr = BCashAddr.BchAddr.DecodeAddress(str, prefix, network);
							if(addr.Type == BCashAddr.BchAddr.CashType.P2PKH)
								result = (T)(object)new BTrashPubKeyAddress(str, addr);
							else
								result = (T)(object)new BTrashScriptAddress(str, addr);
							return true;
						}
						catch { }
					}
				}
				return base.TryParse(str, network, out result);
			}

			public override BitcoinPubKeyAddress CreateP2PKH(KeyId keyId, Network network)
			{
				var addr = new BCashAddr.BchAddr.BchAddrData()
				{
					Format = BCashAddr.BchAddr.CashFormat.Cashaddr,
					Prefix = _Prefix,
					Hash = keyId.ToBytes(true),
					Type = BCashAddr.BchAddr.CashType.P2PKH,
					Network = network
				};
				var str = BCashAddr.BchAddr.EncodeAsCashaddr(addr);
				return new BTrashPubKeyAddress(str, addr);
			}
			public override BitcoinScriptAddress CreateP2SH(ScriptId scriptId, Network network)
			{
				var addr = new BCashAddr.BchAddr.BchAddrData()
				{
					Format = BCashAddr.BchAddr.CashFormat.Cashaddr,
					Prefix = _Prefix,
					Hash = scriptId.ToBytes(true),
					Type = BCashAddr.BchAddr.CashType.P2SH,
					Network = network
				};
				var str = BCashAddr.BchAddr.EncodeAsCashaddr(addr);
				return new BTrashScriptAddress(str, addr);
			}
		}

		private static object RegisterLazy()
		{
			#region Mainnet
			var port = 8333;
			NetworkBuilder builder = new NetworkBuilder();
			_Mainnet = builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000,
				BIP34Hash = new uint256("000000000000024b89b42a942fe0d9fea3bb44ab7bd1b19115dd6a759c0808b8"),
				PowLimit = new Target(new uint256("00000000ffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(14 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(10 * 60),
				PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1916,
				MinerConfirmationWindow = 2016,
				CoinbaseMaturity = 100,
				HashGenesisBlock = new uint256("000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f"),
				MinimumChainWork = new uint256("0000000000000000000000000000000000000000007e5dbf54c7f6b58a6853cd"),
				ConsensusFactory = new BCashConsensusFactory(),
				SupportSegwit = false
			})
			// See https://support.bitpay.com/hc/en-us/articles/115004671663-BitPay-s-Adopted-Conventions-for-Bitcoin-Cash-Addresses-URIs-and-Payment-Requests
			// Note: This is not compatible with Bitcoin ABC
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 28 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 40 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 128 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("bch"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("bch"))
			.SetMagic(0xe8f3e1e3)
			.SetPort(port)
			.SetRPCPort(8332)
			.SetNetworkStringParser(new BCashStringParser("bitcoincash"))
			.SetName("bch-main")
			.AddAlias("bch-mainnet")
			.AddAlias("bcash-mainnet")
			.AddAlias("bcash-main")			
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("bitcoinabc.org", "seed.bitcoinabc.org"),
				new DNSSeedData("bitcoinforks.org", "seed-abc.bitcoinforks.org"),
				new DNSSeedData("bitcoinunlimited.info", "btccash-seeder.bitcoinunlimited.info"),
				new DNSSeedData("bitprim.org", "seed.bitprim.org"),
				new DNSSeedData("deadalnix.me", "seed.deadalnix.me"),
				new DNSSeedData("criptolayer.net", "seeder.criptolayer.net"),
			})
			.AddSeeds(ToSeed(pnSeed6_main))
			.SetGenesis("0100000000000000000000000000000000000000000000000000000000000000000000003ba3edfd7a7b12b27ac72c3e67768f617fc81bc3888a51323a9fb8aa4b1e5e4a29ab5f49ffff001d1dac2b7c0101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4d04ffff001d0104455468652054696d65732030332f4a616e2f32303039204368616e63656c6c6f72206f6e206272696e6b206f66207365636f6e64206261696c6f757420666f722062616e6b73ffffffff0100f2052a01000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000")
			.SetNetworkType(NetworkType.Main) // @@@
			.BuildAndRegister();
			#endregion

			#region Testnet
			builder = new NetworkBuilder();
			port = 18333;
			_Testnet = builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 51,
				MajorityRejectBlockOutdated = 75,
				MajorityWindow = 2016,
				BIP34Hash = new uint256("0000000023b3a96d3484e5abb3755c413e7d41500f8e2a5c3f0dd01299cd8ef8"),
				PowLimit = new Target(new uint256("00000000ffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(14 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(10 * 60),
				PowAllowMinDifficultyBlocks = true,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1512,
				MinerConfirmationWindow = 2016,
				CoinbaseMaturity = 100,
				HashGenesisBlock = new uint256("000000000933ea01ad0ee984209779baaec3ced90fa3f408719526f8d77f4943"),
				MinimumChainWork = new uint256("00000000000000000000000000000000000000000000002888c34d61b53a244a"),
				ConsensusFactory = new BCashConsensusFactory(),
				SupportSegwit = false
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 111 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 196 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tbch"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tbch"))
			.SetMagic(0xf4f3e5f4)
			.SetPort(port)
			.SetRPCPort(18332)
			.SetNetworkStringParser(new BCashStringParser("bchtest"))
			.SetName("bch-test")
			.AddAlias("bch-testnet")
			.AddAlias("bcash-test")
			.AddAlias("bcash-testnet")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("bitcoinabc.org", "testnet-seed.bitcoinabc.org"),
				new DNSSeedData("bitcoinforks.org", "testnet-seed-abc.bitcoinforks.org"),
				new DNSSeedData("bitprim.org", "testnet-seed.bitprim.org"),
				new DNSSeedData("deadalnix.me", "testnet-seed.deadalnix.me"),
				new DNSSeedData("criptolayer.net", "testnet-seeder.criptolayer.net"),
			})
			.AddSeeds(ToSeed(pnSeed6_test))
			.SetGenesis("0100000000000000000000000000000000000000000000000000000000000000000000003ba3edfd7a7b12b27ac72c3e67768f617fc81bc3888a51323a9fb8aa4b1e5e4adae5494dffff001d1aa4ae180101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4d04ffff001d0104455468652054696d65732030332f4a616e2f32303039204368616e63656c6c6f72206f6e206272696e6b206f66207365636f6e64206261696c6f757420666f722062616e6b73ffffffff0100f2052a01000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000")
			.SetNetworkType(NetworkType.Testnet) // @@@
			.BuildAndRegister();
			#endregion

			#region Regtest
			builder = new NetworkBuilder();
			port = 18444;
			_Regtest = builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 150,
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 144,
				BIP34Hash = new uint256(),
				PowLimit = new Target(new uint256("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(14 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(10 * 60),
				PowAllowMinDifficultyBlocks = true,
				MinimumChainWork = uint256.Zero,
				PowNoRetargeting = true,
				RuleChangeActivationThreshold = 108,
				MinerConfirmationWindow = 144,
				CoinbaseMaturity = 100,
				HashGenesisBlock = new uint256("0f9188f13cb7b2c71f2a335e3a4fc328bf5beb436012afca590b1a11466e2206"),
				ConsensusFactory = new BCashConsensusFactory(),
				SupportSegwit = false
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 111 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 196 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tbch"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tbch"))
			.SetMagic(0xfabfb5da)
			.SetPort(port)
			.SetRPCPort(18443)
			.SetNetworkStringParser(new BCashStringParser("bchreg"))
			.SetName("bch-reg")
			.AddAlias("bch-regtest")
			.AddAlias("bcash-reg")
			.AddAlias("bcash-regtest")
			.SetGenesis("0100000000000000000000000000000000000000000000000000000000000000000000003ba3edfd7a7b12b27ac72c3e67768f617fc81bc3888a51323a9fb8aa4b1e5e4adae5494dffff7f20020000000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4d04ffff001d0104455468652054696d65732030332f4a616e2f32303039204368616e63656c6c6f72206f6e206272696e6b206f66207365636f6e64206261696c6f757420666f722062616e6b73ffffffff0100f2052a01000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000")
			.SetNetworkType(NetworkType.Regtest) // @@@
			.BuildAndRegister();
			#endregion

			var home = Environment.GetEnvironmentVariable("HOME");
			var localAppData = Environment.GetEnvironmentVariable("APPDATA");

			if(string.IsNullOrEmpty(home) && string.IsNullOrEmpty(localAppData))
				return new object();

			if(!string.IsNullOrEmpty(home))
			{
				var bitcoinFolder = Path.Combine(home, ".bitcoin");

				var mainnet = Path.Combine(bitcoinFolder, ".cookie");
				RPCClient.RegisterDefaultCookiePath(BCash._Mainnet, mainnet);

				var testnet = Path.Combine(bitcoinFolder, "testnet3", ".cookie");
				RPCClient.RegisterDefaultCookiePath(BCash._Testnet, testnet);

				var regtest = Path.Combine(bitcoinFolder, "regtest", ".cookie");
				RPCClient.RegisterDefaultCookiePath(BCash._Regtest, regtest);
			}
			else if(!string.IsNullOrEmpty(localAppData))
			{
				var bitcoinFolder = Path.Combine(localAppData, "Bitcoin");

				var mainnet = Path.Combine(bitcoinFolder, ".cookie");
				RPCClient.RegisterDefaultCookiePath(BCash._Mainnet, mainnet);

				var testnet = Path.Combine(bitcoinFolder, "testnet3", ".cookie");
				RPCClient.RegisterDefaultCookiePath(BCash._Testnet, testnet);

				var regtest = Path.Combine(bitcoinFolder, "regtest", ".cookie");
				RPCClient.RegisterDefaultCookiePath(BCash._Regtest, regtest);
			}
			return new object();
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
				EnsureRegistered();
				return _Mainnet;
			}
		}

		private static Network _Regtest;
		public static Network Regtest
		{
			get
			{
				EnsureRegistered();
				return _Regtest;
			}
		}

		private static Network _Testnet;
		public static Network Testnet
		{
			get
			{
				EnsureRegistered();
				return _Testnet;
			}
		}
	}
}




namespace BCashAddr
{
	// https://github.com/bitcoincashjs/bchaddrjs
	internal static class BchAddr
	{
		public enum CashFormat
		{
			Legacy,
			Bitpay,
			Cashaddr
		}

		public enum CashType
		{
			P2PKH,
			P2SH
		}

		public class BchAddrData
		{
			public CashFormat Format
			{
				get; set;
			}
			public Network Network
			{
				get; set;
			}
			public CashType Type
			{
				get; set;
			}
			public byte[] Hash
			{
				get; set;
			}

			public string GetHash()
			{
				if(Hash == null)
					return null;
				return Encoders.Hex.EncodeData(Hash);
			}

			public string Prefix
			{
				get;
				internal set;
			}

			public static BchAddrData Create(CashFormat format, Network network, CashType type, byte[] hash)
			{
				return new BchAddrData
				{
					Format = format,
					Network = network,
					Type = type,
					Hash = hash,
				};
			}
		}

		/// <summary>
		/// Encodes the given decoded address into cashaddr format
		/// </summary>
		/// <param name="decoded"></param>
		/// <returns></returns>
		public static string EncodeAsCashaddr(BchAddrData decoded)
		{
			var prefix = decoded.Prefix;
			var type = decoded.Type == CashType.P2PKH ? "P2PKH" : "P2SH";
			var hash = decoded.Hash;
			return CashAddr.Encode(prefix, type, hash);
		}

		/// <summary>
		/// Encodes the given decoded address into cashaddr format without a prefix
		/// </summary>
		/// <param name="decoded"></param>
		/// <returns></returns>
		public static string EncodeAsCashaddrNoPrefix(BchAddrData decoded)
		{
			var address = EncodeAsCashaddr(decoded);
			if(address.IndexOf(":") != -1)
			{
				return address.Split(':')[1];
			}
			throw new Validation.ValidationError($"Invalid BchAddrData");
		}

		/// <summary>
		/// Detects what is the given address' format
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public static BchAddrData DecodeAddress(string address, string prefix, Network network)
		{
			try
			{
				return DecodeCashAddress(address, prefix, network);
			}
			catch { }
			throw new Validation.ValidationError($"Invalid address {address}");
		}

		/// <summary>
		/// Attempts to decode the given address assuming it is a cashaddr address
		/// </summary>
		/// <param name="address">A valid Bitcoin Cash address in any format</param>
		/// <returns></returns>
		private static BchAddrData DecodeCashAddress(string address, string prefix, Network network)
		{
			//if(address.IndexOf(":") != -1)
			//{
			//	return DecodeCashAddressWithPrefix(address);
			//}
			//else
			//{
			try
			{
				var result = DecodeCashAddressWithPrefix(address, network);
				return result;
			}
			catch { }
			//}
			throw new Validation.ValidationError($"Invalid address {address}");
		}

		/// <summary>
		/// Attempts to decode the given address assuming it is a cashaddr address with explicit prefix
		/// </summary>
		/// <param name="address">A valid Bitcoin Cash address in any format</param>
		/// <returns></returns>
		private static BchAddrData DecodeCashAddressWithPrefix(string address, Network network)
		{
			var decoded = CashAddr.Decode(address);
			var type = decoded.Type == "P2PKH" ? CashType.P2PKH : CashType.P2SH;
			return BchAddrData.Create(CashFormat.Cashaddr, network, type, decoded.Hash);
		}
	}

	// https://github.com/bitcoincashjs/cashaddrjs
	internal static class CashAddr
	{
		public class CashAddrData
		{
			public string Prefix
			{
				get; set;
			}
			public string Type
			{
				get; set;
			}
			public byte[] Hash
			{
				get; set;
			}
		}

		/// <summary>
		/// Encodes a hash from a given type into a Bitcoin Cash address with the given prefix
		/// </summary>
		/// <param name="prefix">prefix Network prefix. E.g.: 'bitcoincash'</param>
		/// <param name="type">type Type of address to generate. Either 'P2PKH' or 'P2SH'</param>
		/// <param name="hash">hash Hash to encode represented as an array of 8-bit integers</param>
		/// <returns></returns>
		public static string Encode(string prefix, string type, byte[] hash)
		{
			var prefixData = Concat(PrefixToByte5Array(prefix), new byte[1]);
			var versionByte = GetTypeBits(type) + GetHashSizeBits(hash);
			var payloadData = ToByte5Array(Concat(new byte[1] { (byte)versionByte }, hash));
			var checksumData = Concat(Concat(prefixData, payloadData), new byte[8]);
			var payload = Concat(payloadData, ChecksumToByte5Array(Polymod(checksumData)));
			return prefix + ':' + Base32.Encode(payload);
		}

		/// <summary>
		/// Decodes the given address into its constituting prefix, type and hash
		/// </summary>
		/// <param name="address">Address to decode. E.g.: 'bitcoincash:qpm2qsznhks23z7629mms6s4cwef74vcwvy22gdx6a'</param>
		/// <returns>DecodeData</returns>
		public static CashAddrData Decode(string address)
		{
			var pieces = address.ToLower().Split(':');
			Validation.Validate(pieces.Length == 2, $"Missing prefix: {address}");
			var prefix = pieces[0];
			var payload = Base32.Decode(pieces[1]);
			Validation.Validate(ValidChecksum(prefix, payload), $"Invalid checksum: {address}");
			var data = payload.Take(payload.Length - 8).ToArray();
			var payloadData = FromByte5Array(data);
			var versionByte = payloadData[0];
			var hash = payloadData.Skip(1).ToArray();
			Validation.Validate(GetHashSize((byte)versionByte) == hash.Length * 8, $"Invalid hash size: {address}");
			var type = GetType((byte)versionByte);
			return new CashAddrData
			{
				Prefix = prefix,
				Type = type,
				Hash = hash
			};
		}

		/// <summary>
		/// Retrieves the address type from its bit representation within the version byte
		/// </summary>
		/// <param name="versionByte"></param>
		/// <returns></returns>
		public static string GetType(byte versionByte)
		{
			switch(versionByte & 120)
			{
				case 0:
					return "P2PKH";
				case 8:
					return "P2SH";
				default:
					throw new Validation.ValidationError($"Invalid address type in version byte: {versionByte}");
			}
		}

		/// <summary>
		/// Verify that the payload has not been corrupted by checking that the checksum is valid
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="payload"></param>
		/// <returns></returns>
		public static bool ValidChecksum(string prefix, byte[] payload)
		{
			var prefixData = Concat(PrefixToByte5Array(prefix), new byte[1]);
			var checksumData = Concat(prefixData, payload);
			return Polymod(checksumData).Equals(0);
		}


		/// <summary>
		/// Returns the concatenation a and b
		/// </summary>
		public static byte[] Concat(byte[] a, byte[] b)
		{
			return a.Concat(b).ToArray();
		}

		/// <summary>
		/// Returns an array representation of the given checksum to be encoded within the address' payload
		/// </summary>
		/// <param name="checksum"></param>
		/// <returns></returns>
		public static byte[] ChecksumToByte5Array(long checksum)
		{
			var result = new byte[8];
			for(var i = 0; i < 8; ++i)
			{
				result[7 - i] = (byte)(checksum & 31);
				checksum = checksum >> 5;
			}
			return result;
		}

		/// <summary>
		/// Computes a checksum from the given input data as specified for the CashAddr
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static long Polymod(byte[] data)
		{
			var GENERATOR = new long[] { 0x98f2bc8e61, 0x79b76d99e2, 0xf33e5fb3c4, 0xae2eabe2a8, 0x1e4f43e470 };
			long checksum = 1;
			for(var i = 0; i < data.Length; ++i)
			{
				var value = data[i];
				var topBits = checksum >> 35;
				checksum = ((checksum & 0x07ffffffff) << 5) ^ value;
				for(var j = 0; j < GENERATOR.Length; ++j)
				{
					if(((topBits >> j) & 1).Equals(1))
					{
						checksum = checksum ^ GENERATOR[j];
					}
				}
			}
			return checksum ^ 1;
		}

		/// <summary>
		/// Derives an array from the given prefix to be used in the computation of the address checksum
		/// </summary>
		/// <param name="prefix">Network prefix. E.g.: 'bitcoincash'</param>
		/// <returns></returns>
		public static byte[] PrefixToByte5Array(string prefix)
		{
			var result = new byte[prefix.Length];
			int i = 0;
			foreach(char c in prefix.ToCharArray())
			{
				result[i++] = (byte)(c & 31);
			}
			return result;
		}

		/// <summary>
		/// Returns true if, and only if, the given string contains either uppercase or lowercase letters, but not both
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool HasSingleCase(string str)
		{
			return str == str.ToLower() || str == str.ToUpper();
		}

		/// <summary>
		/// Returns the bit representation of the length in bits of the given hash within the version byte
		/// </summary>
		/// <param name="hash">Hash to encode represented as an array of 8-bit integers</param>
		/// <returns></returns>
		public static byte GetHashSizeBits(byte[] hash)
		{
			switch(hash.Length * 8)
			{
				case 160:
					return 0;
				case 192:
					return 1;
				case 224:
					return 2;
				case 256:
					return 3;
				case 320:
					return 4;
				case 384:
					return 5;
				case 448:
					return 6;
				case 512:
					return 7;
				default:
					throw new Validation.ValidationError($"Invalid hash size: {hash.Length}");
			}
		}

		/// <summary>
		/// Retrieves the the length in bits of the encoded hash from its bit representation within the version byte
		/// </summary>
		/// <param name="versionByte"></param>
		/// <returns></returns>
		public static int GetHashSize(byte versionByte)
		{
			switch(versionByte & 7)
			{
				case 0:
					return 160;
				case 1:
					return 192;
				case 2:
					return 224;
				case 3:
					return 256;
				case 4:
					return 320;
				case 5:
					return 384;
				case 6:
					return 448;
				case 7:
					return 512;
				default:
					throw new Validation.ValidationError($"Invalid versionByte: {versionByte}");
			}
		}

		/// <summary>
		/// Returns the bit representation of the given type within the version byte
		/// </summary>
		/// <param name="type">Address type. Either 'P2PKH' or 'P2SH'</param>
		/// <returns></returns>
		public static byte GetTypeBits(string type)
		{
			switch(type)
			{
				case "P2PKH":
					return 0;
				case "P2SH":
					return 8;
				default:
					throw new Validation.ValidationError($"Invalid type: {type}");
			}
		}

		/// <summary>
		/// Converts an array of 8-bit integers into an array of 5-bit integers, right-padding with zeroes if necessary
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] ToByte5Array(byte[] data)
		{
			return ConvertBits.Convert(data, 8, 5);
		}

		/// <summary>
		/// Converts an array of 5-bit integers back into an array of 8-bit integers
		/// removing extra zeroes left from padding if necessary.
		/// Throws a ValidationError if input is not a zero-padded array of 8-bit integers
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] FromByte5Array(byte[] data)
		{
			return ConvertBits.Convert(data, 5, 8, true);
		}

	}

	internal static class ConvertBits
	{
		/// <summary>
		/// Converts an array of integers made up of 'from' bits into an
		/// array of integers made up of 'to' bits.The output array is
		/// zero-padded if necessary, unless strict mode is true.
		/// </summary>
		/// <param name="data">data Array of integers made up of 'from' bits</param>
		/// <param name="from">from Length in bits of elements in the input array</param>
		/// <param name="to">to Length in bits of elements in the output array</param>
		/// <param name="strictMode">strictMode Require the conversion to be completed without padding</param>
		/// <returns></returns>
		public static byte[] Convert(byte[] data, int from, int to, bool strictMode = false)
		{
			Validation.Validate(from > 0, "Invald 'from' parameter");
			Validation.Validate(to > 0, "Invald 'to' parameter");
			Validation.Validate(data.Length > 0, "Invald data");
			var d = data.Length * from / (double)to;
			var length = strictMode ? (int)Math.Floor(d) : (int)Math.Ceiling(d);
			var mask = (1 << to) - 1;
			var result = new byte[length];
			var index = 0;
			var accumulator = 0;
			var bits = 0;
			for(var i = 0; i < data.Length; ++i)
			{
				var value = data[i];
				Validation.Validate(0 <= value && (value >> from) == 0, $"Invalid value: {value}");
				accumulator = (accumulator << from) | value;
				bits += from;
				while(bits >= to)
				{
					bits -= to;
					result[index] = (byte)((accumulator >> bits) & mask);
					++index;
				}
			}
			if(!strictMode)
			{
				if(bits > 0)
				{
					result[index] = (byte)((accumulator << (to - bits)) & mask);
					++index;
				}
			}
			else
			{
				Validation.Validate(
				  bits < from && ((accumulator << (to - bits)) & mask) == 0,
				  $"Input cannot be converted to {to} bits without padding, but strict mode was used"
				);
			}
			return result;
		}
	}

	internal static class Base32
	{
		private static readonly char[] DIGITS;
		private static Dictionary<char, int> CHAR_MAP = new Dictionary<char, int>();

		static Base32()
		{
			DIGITS = "qpzry9x8gf2tvdw0s3jn54khce6mua7l".ToCharArray();
			for(int i = 0; i < DIGITS.Length; i++)
				CHAR_MAP[DIGITS[i]] = i;
		}

		/// <summary>
		/// Decodes the given base32-encoded string into an array of 5-bit integers
		/// </summary>
		/// <param name="encoded"></param>
		/// <returns></returns>
		public static byte[] Decode(string encoded)
		{
			if(encoded.Length == 0)
			{
				throw new CashaddrBase32EncoderException("Invalid encoded string");
			}
			var result = new byte[encoded.Length];
			int next = 0;
			foreach(char c in encoded.ToCharArray())
			{
				if(!CHAR_MAP.ContainsKey(c))
				{
					throw new CashaddrBase32EncoderException($"Invalid character: {c}");
				}
				result[next++] = (byte)CHAR_MAP[c];
			}
			return result;
		}

		/// <summary>
		/// Encodes the given array of 5-bit integers as a base32-encoded string
		/// </summary>
		/// <param name="data">data Array of integers between 0 and 31 inclusive</param>
		/// <returns></returns>
		public static string Encode(byte[] data)
		{
			if(data.Length == 0)
			{
				throw new CashaddrBase32EncoderException("Invalid data");
			}
			string base32 = String.Empty;
			for(var i = 0; i < data.Length; ++i)
			{
				var value = data[i];
				if(0 <= value && value < 32)
					base32 += DIGITS[value];
				else
					throw new CashaddrBase32EncoderException($"Invalid value: {value}");
			}
			return base32;
		}

		private class CashaddrBase32EncoderException : Exception
		{
			public CashaddrBase32EncoderException(string message) : base(message)
			{
			}
		}
	}

	internal static class Validation
	{
		public static void Validate(bool condition, string message)
		{
			if(!condition)
			{
				throw new ValidationError(message);
			}
		}

		internal class ValidationError : Exception
		{
			public ValidationError(string message) : base(message)
			{
			}
		}
	}
}
