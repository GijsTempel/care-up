using System;
using System.Security;
using System.Security.Cryptography;
using System.Globalization;

namespace DevXUnityTools
{
    /// <summary>
    /// SerialNumberSignerDSA
    /// </summary>
    public sealed class SerialNumberVerifyDSA
    {
        #region valiables
        static string HashAlg = "SHA1";
        DSAParameters publicKeyInfo;
        #endregion

        #region SerialNumberVerifyDSA
        public SerialNumberVerifyDSA(string serialized_key)
        {
            if (string.IsNullOrEmpty(serialized_key))
                return;

            if (serialized_key.StartsWith("DSA:O:"))
            {
                publicKeyInfo = Parce(serialized_key.Substring("DSA:O:".Length));
            }


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
            if (HashValue==null || HashValue.Length==0 || SignedHashValue==null || SignedHashValue.Length != 40)
                return false;

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