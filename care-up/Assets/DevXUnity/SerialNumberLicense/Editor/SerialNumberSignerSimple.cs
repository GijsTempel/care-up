using System;
using System.Security;
using System.Security.Cryptography;
using System.Globalization;

namespace DevXUnityTools
{
    /// <summary>
    /// SerialGenSigner
    /// </summary>
    public sealed class SerialNumberSignerSimple
    {
        #region valiables
        MD4 hash_md4 = new MD4();

        private const int hash_len = 16;

        private int _keyLen=10000;// key pair len *_signLen
        private int _signLen=8; // output key len

        private byte[] _publicKey;
        private byte[] _secretKey;
        
        public bool HaveSecretKey
        {
            get
            {
                if (_secretKey == null)
                    return false;

                return true;
            }
        }
        #endregion

        #region SerialNumberSignerSimple
        /// <summary>
        /// SerialNumberSignerSimple by serialized_key
        /// </summary>
        /// <param name="serialized_key"></param>
        public SerialNumberSignerSimple(string serialized_key)
        {
            if (string.IsNullOrEmpty(serialized_key))
                throw new ArgumentNullException("serialized_key - not valid");

            string[] l = serialized_key.Replace("\r\n", "\n").Trim().Split('\n');

            int i = 0;
            _signLen = int.Parse(l[i]); i++;
            _secretKey = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;
            _publicKey = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;

            _keyLen = _publicKey.Length / _signLen;

            if (_keyLen <= 0)
                throw new ArgumentOutOfRangeException("keyLength");

            if (_signLen <= 0 || _signLen > hash_len)
                throw new ArgumentOutOfRangeException("signatureLength");
        }
        #endregion

        #region SerialNumberSignerSimple
        /// <summary>
        /// SerialNumberSignerSimple by default values
        /// </summary>
        public SerialNumberSignerSimple()
        {

        }
        /// <summary>
        /// SerialGenSignerSimple
        /// </summary>
        /// <param name="signatureLength"></param>
        /// <param name="keyStoreLength"></param>
        public SerialNumberSignerSimple(int signatureLength=8, int keyStoreLength=1000)
        {
            if (keyStoreLength <= 0)
                throw new ArgumentOutOfRangeException("keyLength");

            if (signatureLength <= 0 || signatureLength > hash_len)
                throw new ArgumentOutOfRangeException("signatureLength");

            _keyLen = keyStoreLength;
            _signLen = signatureLength;

            //GenerateKeys();
        }
        #endregion


        #region GenerateKeys
        /// <summary>
        /// GenerateKeys
        /// </summary>
        public void GenerateKeys()
        {
            _publicKey = new byte[_keyLen * _signLen];
            _secretKey = new byte[_keyLen * _signLen];

            // generate private key
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_secretKey);

            // generate public key
            byte[] toHash = new byte[_signLen];
            for (int i = 0; i < _keyLen; i++)
            {
                Array.Copy(_secretKey, i * _signLen, toHash, 0, _signLen);

                byte[] hash = MakeHash(toHash);
                Array.Copy(hash, 0, _publicKey, i * _signLen, _signLen);
            }
        }
        #endregion

        #region Sign
        /// <summary>
        /// Sign
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] Sign(string value)
        {
            return Sign(System.Text.Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Sign
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] Sign(byte[] value)
        {
            if (null == value || value.Length <= 0)
                return null;

            if (_secretKey == null)
                return null;

            byte[] hash = MakeHash(value);

            uint k = BitConverter.ToUInt32(hash, hash.Length - _signLen);

            k = k % ((uint)_keyLen);
            var step = k * _signLen;

            byte[] signature = new byte[_signLen];
            for (int i = 0; i < _signLen; i++)
            {
                signature[i] = _secretKey[step + i];
            }

            return signature;
        }
        #endregion

        #region VerifySignature
        /// <summary>
        /// Verify signature
        /// </summary>
        /// <param name="value"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool VerifySignature(string value, byte[] signature)
        {
            return VerifySignature(System.Text.Encoding.UTF8.GetBytes(value), signature);
        }
        /// <summary>
        /// Verify Signature
        /// </summary>
        /// <param name="value"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool VerifySignature(byte[] value, byte[] signature)
        {
            if (value == null || value.Length == 0 || signature == null || signature.Length == 0)
                return false;

            if (_publicKey == null) return false;


            byte[] hash = MakeHash(value);

            uint k = BitConverter.ToUInt32(hash, hash.Length - _signLen);

            k = k % ((uint)_keyLen);
            var step = k * _signLen;

            byte[] etalone = new byte[_signLen];

            for (int i = 0; i < _signLen; i++)
            {
                etalone[i] = _publicKey[step + i];
            }

            byte[] signatureHash = MakeHash(signature);

            for (int i = 0; i < _signLen; i++)
            {
                if (etalone[i] != signatureHash[i])
                    return false;
            }

            return true;
        }
        #endregion

        #region SerializeKeys
        /// <summary>
        /// Serialize keys
        /// </summary>
        /// <param name="include_private_key">include private key</param>
        /// <returns>key as string</returns>
        public string SerializeKeys(bool include_private_key)
        {
            string s = _signLen + "\n"
                + (_secretKey == null || include_private_key == false ? "" : (Convert.ToBase64String(_secretKey))) + "\n"
                + (_publicKey == null ? "" : (Convert.ToBase64String(_publicKey)))
                ;
            return s;
        }
        #endregion

        #region MakeHash
        /// <summary>
        /// Make Hash
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private byte[] MakeHash(byte[] value)
        {
            hash_md4.Initialize();
            var value2 = hash_md4.ComputeHash(value);

            // for slow only
            for (int i = 0; i < 100; i++)
            {
                value[0] = (byte)i;
                hash_md4.Initialize();
                value = hash_md4.ComputeHash(value);
            }

            for(int i=0; i<value.Length; i++)
            {
                value2[i] = (byte)( value[i] ^ (value2[i]+i));
            }
            
            return value2;
        }
        #endregion

        #region MD4
        /// <summary>
        /// Computes the MD4 hash value for the input data. 
        /// </summary>
        public sealed class MD4
        {

            private const int BITS_IN_UINT = 32;
            private const int BITS_IN_BYTE = 8;
            private const int BYTES_IN_UINT = 4;

            private const int HASH_SIZE = 16;
            private const int CONTEXT_SIZE = HASH_SIZE / BYTES_IN_UINT;
            private const int COUNT_SIZE = 8;
            private const int BLOCK_SIZE = 64;
            private const int LAST_BIT_MASK = 0x80;
            private const int FINAL_SCOPE = 56;

            private const int UINT_BLOCK_SIZE = BLOCK_SIZE / BYTES_IN_UINT;

            private const uint I0 = 0x67452301;
            private const uint I1 = 0xEFCDAB89;
            private const uint I2 = 0x98BADCFE;
            private const uint I3 = 0x10325476;

            private const uint C2 = 0x5A827999;
            private const uint C3 = 0x6ED9EBA1;

            private const int FS1 = 3;
            private const int FS2 = 7;
            private const int FS3 = 11;
            private const int FS4 = 19;

            private const int GS1 = 3;
            private const int GS2 = 5;
            private const int GS3 = 9;
            private const int GS4 = 13;

            private const int HS1 = 3;
            private const int HS2 = 9;
            private const int HS3 = 11;
            private const int HS4 = 15;

            private uint[] _context;
            private readonly byte[] _count;

            /// <summary>
            /// Initializes a new instance of the <see cref="MD4"></see> class.
            /// </summary>
            public MD4()
            {
                _context = new uint[CONTEXT_SIZE];
                _count = new byte[COUNT_SIZE];

                initContext();
            }

            /// <summary>
            /// Initializes an instance of MD4.
            /// </summary>
            public void Initialize()
            {
                initContext();
                Array.Clear(_count, 0, COUNT_SIZE);
            }


            /// <summary>
            /// Computes the hash value for the specified byte array.
            /// </summary>
            /// <param name="array">The input to compute the hash code for.</param>
            /// <returns>The computed hash code.</returns>
            public byte[] ComputeHash(byte[] array)
            {
                return ComputeHash(array, 0, array.Length);
            }

            /// <summary>
            /// Computes the hash value for the specified region of the specified byte array.
            /// </summary>
            /// <param name="array">The input to compute the hash code for.</param>
            /// <param name="offset">The offset into the byte array from which to begin using data.</param>
            /// <param name="count">The number of bytes in the array to use as data.</param>
            /// <returns>The computed hash code.</returns>
            /// <exception cref="ArgumentNullException">array is null.</exception>
            /// <exception cref="ArgumentOutOfRangeException">array is empty or invalid count.</exception>
            public byte[] ComputeHash(byte[] array, int offset, int count)
            {
                uint[] temp = ComputeUInt32Hash(array, offset, count);

                byte[] result = new byte[HASH_SIZE];
                Buffer.BlockCopy(temp, 0, result, 0, HASH_SIZE);

                return result;
            }

            /// <summary>
            /// Computes the hash value for the specified region of the specified byte array.
            /// </summary>
            /// <param name="array">The input to compute the hash code for.</param>
            /// <returns>The computed hash code.</returns>
            /// <exception cref="ArgumentNullException">array is null.</exception>
            /// <exception cref="ArgumentOutOfRangeException">array is empty.</exception>
            public uint[] ComputeUInt32Hash(byte[] array)
            {
                return ComputeUInt32Hash(array, 0, array.Length);
            }

            /// <summary>
            /// Computes the hash value for the specified region of the specified byte array.
            /// </summary>
            /// <param name="array">The input to compute the hash code for.</param>
            /// <param name="offset">The offset into the byte array from which to begin using data.</param>
            /// <param name="count">The number of bytes in the array to use as data.</param>
            /// <returns>The computed hash code.</returns>
            /// <exception cref="ArgumentNullException">array is null.</exception>
            /// <exception cref="ArgumentOutOfRangeException">array is empty or invalid count.</exception>
            public uint[] ComputeUInt32Hash(byte[] array, int offset, int count)
            {
                if (null == array)
                    throw new ArgumentNullException("array");

                if (0 == array.Length)
                    throw new ArgumentOutOfRangeException("array");

                int len = offset + count;

                if (array.Length < len)
                    throw new ArgumentOutOfRangeException("count");

                uint[] block = new uint[UINT_BLOCK_SIZE];
                int pos = offset;

                for (; pos <= len - BLOCK_SIZE; pos += BLOCK_SIZE)
                {
                    Buffer.BlockCopy(array, pos, block, 0, BLOCK_SIZE);
                    transformBlock(block);
                }

                ulong bitsCount = (ulong)len * BITS_IN_BYTE;
                BitConverter.GetBytes(bitsCount).CopyTo(_count, 0); // длина в битах

                Array.Clear(block, 0, UINT_BLOCK_SIZE);
                Buffer.BlockCopy(array, pos, block, 0, len - pos);
                transformFinalBlock(block, len - pos);

                return _context;
            }

            /// <summary>
            /// Releases all resources used by the <see cref="MD4"></see> class.
            /// </summary>
            public void Clear()
            {
                Array.Clear(_context, 0, CONTEXT_SIZE);
                Array.Clear(_count, 0, COUNT_SIZE);
            }

            private void initContext()
            {
                _context = new uint[CONTEXT_SIZE];

                _context[0] = I0;
                _context[1] = I1;
                _context[2] = I2;
                _context[3] = I3;
            }

            private void transformFinalBlock(uint[] block, int len)
            {
                Buffer.BlockCopy(new byte[] { LAST_BIT_MASK }, 0, block, len, 1);

                if (len < FINAL_SCOPE)
                {
                    Buffer.BlockCopy(_count, 0, block, FINAL_SCOPE, COUNT_SIZE);
                    transformBlock(block);
                }
                else
                {
                    transformBlock(block);
                    Array.Clear(block, 0, UINT_BLOCK_SIZE);
                    Buffer.BlockCopy(_count, 0, block, FINAL_SCOPE, COUNT_SIZE);
                    transformBlock(block);
                }
            }

            private void transformBlock(uint[] value)
            {
                if (value.LongLength != UINT_BLOCK_SIZE)
                    throw new ArgumentOutOfRangeException("value");

                uint a = _context[0];
                uint b = _context[1];
                uint c = _context[2];
                uint d = _context[3];

                a = ff(a, b, c, d, value[0], FS1);
                d = ff(d, a, b, c, value[1], FS2);
                c = ff(c, d, a, b, value[2], FS3);
                b = ff(b, c, d, a, value[3], FS4);
                a = ff(a, b, c, d, value[4], FS1);
                d = ff(d, a, b, c, value[5], FS2);
                c = ff(c, d, a, b, value[6], FS3);
                b = ff(b, c, d, a, value[7], FS4);
                a = ff(a, b, c, d, value[8], FS1);
                d = ff(d, a, b, c, value[9], FS2);
                c = ff(c, d, a, b, value[10], FS3);
                b = ff(b, c, d, a, value[11], FS4);
                a = ff(a, b, c, d, value[12], FS1);
                d = ff(d, a, b, c, value[13], FS2);
                c = ff(c, d, a, b, value[14], FS3);
                b = ff(b, c, d, a, value[15], FS4);

                a = gg(a, b, c, d, value[0], GS1);
                d = gg(d, a, b, c, value[4], GS2);
                c = gg(c, d, a, b, value[8], GS3);
                b = gg(b, c, d, a, value[12], GS4);
                a = gg(a, b, c, d, value[1], GS1);
                d = gg(d, a, b, c, value[5], GS2);
                c = gg(c, d, a, b, value[9], GS3);
                b = gg(b, c, d, a, value[13], GS4);
                a = gg(a, b, c, d, value[2], GS1);
                d = gg(d, a, b, c, value[6], GS2);
                c = gg(c, d, a, b, value[10], GS3);
                b = gg(b, c, d, a, value[14], GS4);
                a = gg(a, b, c, d, value[3], GS1);
                d = gg(d, a, b, c, value[7], GS2);
                c = gg(c, d, a, b, value[11], GS3);
                b = gg(b, c, d, a, value[15], GS4);

                a = hh(a, b, c, d, value[0], HS1);
                d = hh(d, a, b, c, value[8], HS2);
                c = hh(c, d, a, b, value[4], HS3);
                b = hh(b, c, d, a, value[12], HS4);
                a = hh(a, b, c, d, value[2], HS1);
                d = hh(d, a, b, c, value[10], HS2);
                c = hh(c, d, a, b, value[6], HS3);
                b = hh(b, c, d, a, value[14], HS4);
                a = hh(a, b, c, d, value[1], HS1);
                d = hh(d, a, b, c, value[9], HS2);
                c = hh(c, d, a, b, value[5], HS3);
                b = hh(b, c, d, a, value[13], HS4);
                a = hh(a, b, c, d, value[3], HS1);
                d = hh(d, a, b, c, value[11], HS2);
                c = hh(c, d, a, b, value[7], HS3);
                b = hh(b, c, d, a, value[15], HS4);

                _context[0] += a;
                _context[1] += b;
                _context[2] += c;
                _context[3] += d;
            }

            static uint rot(uint t, int s)
            {
                uint result = (t << s) | (t >> (BITS_IN_UINT - s));

                return result;
            }

            static uint f(uint x, uint y, uint z)
            {
                uint t = (x & y) | (~x & z);

                return t;
            }

            static uint g(uint x, uint y, uint z)
            {
                uint t = (x & y) | (x & z) | (y & z);

                return t;
            }

            static uint h(uint x, uint y, uint z)
            {
                uint t = x ^ y ^ z;

                return t;
            }

            static uint ff(uint a, uint b, uint c, uint d, uint x, int s)
            {
                uint t = a + f(b, c, d) + x;

                return rot(t, s);
            }

            static uint gg(uint a, uint b, uint c, uint d, uint x, int s)
            {
                uint t = a + g(b, c, d) + x + C2;

                return rot(t, s);
            }

            static uint hh(uint a, uint b, uint c, uint d, uint x, int s)
            {
                uint t = a + h(b, c, d) + x + C3;

                return rot(t, s);
            }
        }
        #endregion
    }



}