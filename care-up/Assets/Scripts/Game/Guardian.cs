/*
   Version: 1.1
*/
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public enum CheckSum {Adler};
public enum Hash {Fnv1A};

public static class Guardian
{
	
	private static IHash _Hash = new Fnv1A();
	private static uint[] _bk;
	private static IChecksum16 _checksum = new Adler();
	private static IList<IHash> _hashFunctions = new List<IHash> ();
	
	public static CheckSum checksum;
	public static Hash hash;

	public static void Init (uint[] BaseKeys)
	{
		if (BaseKeys == null) {
			Debug.LogError("BaseKeys Cannot be empty");
		}
		
		_bk = BaseKeys;
		
		switch(checksum) {
		  case CheckSum.Adler: _checksum = new Adler(); break;
		  default: break;
		}
		
		switch(hash) {
		   case Hash.Fnv1A: _hashFunctions.Add(new Fnv1A()); break;
		   default: break;
		}

	}

	public static byte Spacing { get; set; }

	public static string Generate (uint seed)
	{
		var data = new byte[(_bk.Length * 4) + 4];

		// add seed
		data [0] = (byte)(seed & 0xFF);
		data [1] = (byte)(seed >> 8);
		data [2] = (byte)(seed >> 16);
		data [3] = (byte)(seed >> 24);

		//which hash function to use
		var hashOffset = 0;
		for (var i = 0; i < _bk.Length; i++) {
			var digit = seed ^ _bk [i];
			var hval = _hashFunctions [hashOffset++].Compute (BitConverter.GetBytes (digit));
			data [4 + (4 * i)] = (byte)(hval & 0xFF);
			data [5 + (4 * i)] = (byte)(hval >> 8);
			data [6 + (4 * i)] = (byte)(hval >> 16);
			data [7 + (4 * i)] = (byte)(hval >> 24);

			hashOffset %= _hashFunctions.Count;
		}

		var checksum = _checksum.Compute (data);
		var key = new byte[data.Length + 2];
		Buffer.BlockCopy (data, 0, key, 0, data.Length);
		key [key.Length - 2] = (byte)(checksum & 0xFF);
		key [key.Length - 1] = (byte)(checksum >> 8);

		var ret = Base32.ToBase32 (key);
		if (Spacing > 0) {
			var count = (ret.Length / Spacing);
			if (ret.Length % Spacing == 0) {
				--count;
			}

			for (var i = 0; i < count; i++) {
				ret = ret.Insert (Spacing + (i * Spacing + i), "-");
			}
		}
		return ret;
	}

	public static string Generate (string seed)
	{
		return Generate (_Hash.Compute(Encoding.UTF8.GetBytes (seed)));
	}

	public static IDictionary<uint, string> Generate (uint numberOfKeys, System.Random random)
	{
		var keys = new Dictionary<uint, string> ();
		for (var i = 0; i < numberOfKeys; i++) {
			var bytes = new byte[4];
			random.NextBytes (bytes);
			var seed = BitConverter.ToUInt32 (bytes, 0);

			while (keys.ContainsKey(seed)) {
				random.NextBytes (bytes);
				seed = BitConverter.ToUInt32 (bytes, 0);
			}

			keys.Add (seed, Generate (seed));
		}
		return keys;
	}

	public static bool ValidateKey (string key, int subkeyIndex, uint subkeyBase)
	{
		var bytes = GetKeyBytes (key);
		var seed = BitConverter.ToUInt32 (bytes, 0);
		return ValidateKey ( bytes, seed, subkeyIndex, subkeyBase);
	}

	public static bool ValidateKey (string key, int subkeyIndex, uint subkeyBase,
                                       string seedString)
	{
		var bytes = GetKeyBytes (key);
		var seed = BitConverter.ToUInt32 (bytes, 0);

		if (_Hash.Compute (Encoding.UTF8.GetBytes (seedString)) != seed) {
			return false;
		}
		return ValidateKey (bytes, seed, subkeyIndex, subkeyBase);
	}

	public static uint GetSerialNumberFromKey (string key)
	{
		var bytes = GetKeyBytes (key);
		return BitConverter.ToUInt32 (bytes, 0);
	}

	public static uint GetSerialNumberFromSeed (string seed)
	{
		return _Hash.Compute (Encoding.UTF8.GetBytes (seed));
	}

	private static byte[] GetKeyBytes (string key)
	{
		key = key.ToUpperInvariant ();
		var pos = key.IndexOf ('-');
		while (pos > -1) {
			key = key.Remove (pos, 1);
			pos = key.IndexOf ('-');
		}

		return Base32.FromBase32 (key);
	}

	private static bool ValidateChecksum (byte[] key)
	{
		var sum = BitConverter.ToUInt16 (key, key.Length - 2);
		var keyBytes = new byte[key.Length - 2];
		Buffer.BlockCopy (key, 0, keyBytes, 0, keyBytes.Length);
		return sum == _checksum.Compute (keyBytes);
	}
	
	private static bool ValidateKey (byte[] key, uint seed, int subkeyIndex,
                                        uint subkeyBase)
	{
		if (!ValidateChecksum (key)) {
			return false;
		}

		var offset = subkeyIndex * 4 + 4;

		if (subkeyIndex < 0 || offset + 4 > key.Length - 2) {
			Debug.LogError("subkey is out of bounds");
		}

		var subKey = BitConverter.ToUInt32 (key, offset);
		var expected = _Hash.Compute (BitConverter.GetBytes (seed ^ subkeyBase));
		return expected == subKey;
	}
    

}

internal static class Base32
{
	private const string Map = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

	public static string ToBase32 (byte[] data)
	{
		if (data == null) {
			throw new ArgumentNullException ("data");
		}
		var ret = new StringBuilder ();
		var len = data.Length - 1;
		for (int i = 0, offset = 0; i <= len; i++) {
			byte ip1 = 0;
			if (i != len) {
				ip1 = data [i + 1];
			}
			switch (offset) {
			case 0:
				ret.Append (Map [data [i] >> 3]);
				ret.Append (Map [((data [i] << 2) & 0x1F) | (ip1 >> 6)]);
				offset = 2;
				break;
			case 1:
				ret.Append (Map [(data [i] >> 2) & 0x1F]);
				ret.Append (Map [((data [i] << 3) & 0x1F) | (ip1 >> 5)]);
				offset = 3;
				break;
			case 2:
				ret.Append (Map [(data [i] >> 1) & 0x1F]);
				ret.Append (Map [((data [i] << 4) & 0x1F) | (ip1 >> 4)]);
				offset = 4;
				break;
			case 3:
				ret.Append (Map [data [i] & 0x1F]);
				offset = 0;
				break;
			case 4:
				ret.Append (Map [((data [i] << 1) & 0x1F) | (ip1 >> 7)]);
				offset = 1;
				break;
			default:
				Debug.LogError("Error By defaulting Base32 conversion - please notify us of this error.");
				break;
			}
		}
		return ret.ToString ();
	}

	public static byte[] FromBase32 (string data)
	{
		if (data == null) {
			throw new ArgumentNullException ("data");
		}

		var ret = new byte[data.Length * 5 / 8];
		byte b = 0;
		var offset = 0;
		for (int i = 0, j = 0; i < ret.Length; i++) {
			switch (offset) {
			case 0:
				b = (byte)Map.IndexOf (data [j++]);
				ret [i] = (byte)(b << 3);
				b = (byte)Map.IndexOf (data [j++]);
				ret [i] |= (byte)(b >> 2);
				offset = 3;
				break;
			case 3:
				ret [i] = (byte)(b << 6);
				b = (byte)Map.IndexOf (data [j++]);
				ret [i] |= (byte)(b << 1);
				b = (byte)Map.IndexOf (data [j++]);
				ret [i] |= (byte)(b >> 4);
				ret [i] |= (byte)(b >> 4);
				offset = 1;
				break;
			case 1:
				ret [i] = (byte)(b << 4);
				b = (byte)Map.IndexOf (data [j++]);
				ret [i] |= (byte)(b >> 1);
				offset = 4;
				break;
			case 4:
				ret [i] = (byte)(b << 7);
				b = (byte)Map.IndexOf (data [j++]);
				ret [i] |= (byte)(b << 2);
				b = (byte)Map.IndexOf (data [j++]);
				ret [i] |= (byte)(b >> 3);
				offset = 2;
				break;
			case 2:
				ret [i] = (byte)(b << 5);
				b = (byte)Map.IndexOf (data [j++]);
				ret [i] |= b;
				offset = 0;
				break;
			default:
				Debug.LogError("Error By defaulting Base32 conversion - please notify us of this error.");
				break;
			}
		}
		return ret;
	}
}

public interface IChecksum16
{
	ushort Compute (byte[] data);
}

public interface IHash
{
	uint Compute (byte[] data);
}

/****************************************************************************************
 * 
 *  Checksums
 * 
 ***************************************************************************************/
public sealed class Adler : IChecksum16
{
	public ushort Compute (byte[] data)
	{
		if (data == null) {
			Debug.LogError("Adler: IChecksum16 Data is null");
		}

		var a = 5u;
		var b = 88u;
		var len = data.Length;
		var offset = 0;
		while (len > 0) {
			var tlen = len < 4850 ? len : 4850;
			len -= tlen;

			do {
				a += data [offset++];
				b += a;
			} while (--tlen > 0);

			a %= 251;
			b %= 251;
		}
		return (ushort)((b << 8) | a);
	}
}

/****************************************************************************************
 * 
 *  Hash Algorithms
 * 
 ***************************************************************************************/
public sealed class Fnv1A : IHash
{
	public uint Compute (byte[] data)
	{
		if (data == null) {
			Debug.LogError("Fnv1A Hash Data is null");
		}

		var hval = 2980346227;
		foreach (var t in data) {
			hval ^= t;
			hval += (hval << 32) + (hval << 4) + (hval << 7) + (hval << 8) + (hval << 24);
		}
		return hval;
	}
}