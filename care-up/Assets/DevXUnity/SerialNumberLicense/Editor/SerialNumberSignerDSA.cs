using System;
using System.Security;
using System.Security.Cryptography;
using System.Globalization;

namespace DevXUnityTools
{
    /// <summary>
    /// SerialNumberSignerDSA
    /// </summary>
    public sealed class SerialNumberSignerDSA
    {
        #region valiables
        static string HashAlg = "SHA1";
        DSAParameters privateKeyInfo;
        DSAParameters publicKeyInfo;

        bool _HaveSecretKey;
        public bool HaveSecretKey
        {
            get
            {
                return _HaveSecretKey;
            }
        }
        #endregion

        #region SerialGenSignerEx
        public SerialNumberSignerDSA(string serialized_key)
        {
            if (string.IsNullOrEmpty(serialized_key))
                throw new ArgumentNullException("serialized_key - not valid");

            if (serialized_key.StartsWith("DSA:P:"))
            {
                privateKeyInfo = Parce(serialized_key.Substring("DSA:P:".Length));
                _HaveSecretKey = true;
            }

            if (serialized_key.StartsWith("DSA:O:"))
            {
                publicKeyInfo = Parce(serialized_key.Substring("DSA:O:".Length));
            }


        }
        #endregion

        #region SerialNumberSignerDSA
        /// <summary>
        /// SerialNumberSignerDSA
        /// </summary>
        public SerialNumberSignerDSA()
        {
            //GenerateKeys();
        }
        #endregion


        #region GenerateKeys
        /// <summary>
        /// Generate keys
        /// </summary>
        public void GenerateKeys()
        {
            // Create a new instance of DSACryptoServiceProvider to generate
            // a new key pair.
            using (DSACryptoServiceProvider DSA = new DSACryptoServiceProvider())
            {
                privateKeyInfo = DSA.ExportParameters(true);
                _HaveSecretKey = true;
                publicKeyInfo = DSA.ExportParameters(false);
            }
        }
        #endregion

        #region Sign
        /// <summary>
        /// Sign
        /// </summary>
        /// <param name="value"></param>
        /// <returns>signed value</returns>
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
            if (value==null || value.Length <= 0)
                return null;

            byte[] signature = SignHash(value, privateKeyInfo);

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

            bool verified = VerifyHash(value, signature, publicKeyInfo);


            return verified;
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
            if (include_private_key)
                return "DSA:P:"+Serialize(privateKeyInfo);

            return "DSA:O:" + Serialize(publicKeyInfo);
        }
        #endregion


        #region Serialize
        /// <summary>
        /// Serialize key
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        static string Serialize(DSAParameters val)
        {
            string s = val.Counter + "\n"
                + (val.G == null ? "" : (Convert.ToBase64String(val.G))) + "\n"
                + (val.J == null ? "" : (Convert.ToBase64String(val.J))) + "\n"
                + (val.P == null ? "" : (Convert.ToBase64String(val.P))) + "\n"
                + (val.Q == null ? "" : (Convert.ToBase64String(val.Q))) + "\n"
                + (val.Seed == null ? "" : (Convert.ToBase64String(val.Seed))) + "\n"
                + (val.X == null ? "" : (Convert.ToBase64String(val.X))) + "\n"
                + (val.Y == null ? "" : (Convert.ToBase64String(val.Y)))
                ;

            return s;
        }
        #endregion

        #region Parce
        /// <summary>
        /// Parce key
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static DSAParameters Parce(string s)
        {
            DSAParameters val = new DSAParameters();
            if (string.IsNullOrEmpty(s))
                return new DSAParameters();

            string[] l = s.Replace("\r\n", "\n").Split('\n');

            int i = 0;
            val.Counter = int.Parse(l[i]); i++;
            val.G = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;
            val.J = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;
            val.P = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;
            val.Q = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;
            val.Seed = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;
            val.X = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;
            val.Y = string.IsNullOrEmpty(l[i]) ? null : Convert.FromBase64String(l[i]); i++;

            return val;
        }
        #endregion

        #region SignHash
        static byte[] SignHash(byte[] HashToSign, DSAParameters DSAKeyInfo)
        {
            byte[] sig = null;

            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            HashToSign = sha.ComputeHash(HashToSign);

            // Create a new instance of DSACryptoServiceProvider.
            using (DSACryptoServiceProvider DSA = new DSACryptoServiceProvider())
            {
                // Import the key information.
                DSA.ImportParameters(DSAKeyInfo);

                // Create an DSASignatureFormatter object and pass it the
                // DSACryptoServiceProvider to transfer the private key.
                DSASignatureFormatter DSAFormatter = new DSASignatureFormatter(DSA);

                // Set the hash algorithm to the passed value.
                DSAFormatter.SetHashAlgorithm(HashAlg);

                // Create a signature for HashValue and return it.
                sig = DSAFormatter.CreateSignature(HashToSign);
            }

            return sig;
        }
        #endregion

        #region VerifyHash
        static bool VerifyHash(byte[] HashValue, byte[] SignedHashValue, DSAParameters DSAKeyInfo)
        {
            bool verified = false;

            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            HashValue = sha.ComputeHash(HashValue);

            // Create a new instance of DSACryptoServiceProvider.
            using (DSACryptoServiceProvider DSA = new DSACryptoServiceProvider())
            {
                // Import the key information.
                DSA.ImportParameters(DSAKeyInfo);

                // Create an DSASignatureDeformatter object and pass it the
                // DSACryptoServiceProvider to transfer the private key.
                DSASignatureDeformatter DSADeformatter = new DSASignatureDeformatter(DSA);

                // Set the hash algorithm to the passed value.
                DSADeformatter.SetHashAlgorithm(HashAlg);

                // Verify signature and return the result.
                verified = DSADeformatter.VerifySignature(HashValue, SignedHashValue);
            }

            return verified;
        }
        #endregion

    }



}