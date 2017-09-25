using System;

namespace DevXUnityTools
{
    /// <summary>
    /// Serial Generator Signer
    /// </summary>
    internal sealed class SerialNumberSigner
    {
        SerialNumberSignerSimple simple;
        SerialNumberSignerDSA dsa;

        #region SerialGenSigner
        SerialNumberSigner()
        {

        }
        #endregion

        #region CreateDSA
        internal static SerialNumberSigner CreateDSA()
        {
            SerialNumberSigner obj = new DevXUnityTools.SerialNumberSigner();
            obj.dsa = new DevXUnityTools.SerialNumberSignerDSA();

            return obj;
        }
        #endregion

        #region CreateSimple
        internal static SerialNumberSigner CreateSimple(int signatureLength = 8, int keyStoreLength = 1000)
        {
            SerialNumberSigner obj = new DevXUnityTools.SerialNumberSigner();
            obj.simple = new DevXUnityTools.SerialNumberSignerSimple(signatureLength: signatureLength, keyStoreLength: keyStoreLength);

            return obj;
        }
        #endregion

        #region SerialGenSignerSimple
        internal SerialNumberSigner(string serialized_key)
        {
            if (string.IsNullOrEmpty(serialized_key))
                throw new ArgumentNullException("serialized_key - not valid");

            if (serialized_key.StartsWith("DSA:"))
            {
                dsa = new DevXUnityTools.SerialNumberSignerDSA(serialized_key);
            }
            else
                simple = new DevXUnityTools.SerialNumberSignerSimple(serialized_key);
        }
        #endregion

        #region GenerateKeys
        /// <summary>
        /// Generate open and close keys
        /// </summary>
        internal void GenerateKeys()
        {
            if (dsa != null)
            {
                dsa.GenerateKeys();
            }
            if (simple != null)
            {
                simple.GenerateKeys();
            }

        }
        #endregion

        #region Sign
        /// <summary>
        /// Make serial number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string Sign(string value)
        {
            return Sign(System.Text.Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Sign
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string Sign(byte[] value)
        {
            if (dsa != null)
            {
               return BytesConvertToHexString(dsa.Sign(value));
            }
            if (simple != null)
            {
                return BytesConvertToHexString(simple.Sign(value));
            }
            return null;
        }
        #endregion

        #region VerifySignature
        /// <summary>
        /// Verify signature
        /// </summary>
        /// <param name="value"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        internal bool VerifySignature(string value, string signature)
        {
            return VerifySignature(System.Text.Encoding.UTF8.GetBytes(value), signature);
        }
        /// <summary>
        /// Verify Signature
        /// </summary>
        /// <param name="value"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        internal bool VerifySignature(byte[] value, string signature)
        {
            if (dsa != null)
            {
                return dsa.VerifySignature(value, HexStringToBytes(signature));
            }
            if (simple != null)
            {
                return simple.VerifySignature(value, HexStringToBytes(signature));
            }
            return false;
        }
        #endregion

        #region SerializeKeys
        /// <summary>
        /// Serialize keys
        /// </summary>
        /// <param name="include_private_key">include private key</param>
        /// <returns>key as string</returns>
        internal string SerializeKeys(bool include_private_key)
        {
            if (dsa != null)
            {
                return dsa.SerializeKeys(include_private_key);
            }
            if (simple != null)
            {
                return simple.SerializeKeys(include_private_key);
            }
            return null;
        }
        #endregion


        #region BytesConvertToHexString
        public static string BytesConvertToHexString(byte[] buff)
        {
            string hex = "";
            foreach (byte c in buff)
            {
                int tmp = c;
                hex += String.Format("{0:X2}", c);
            }
            return hex;
        }
        #endregion

        #region HexStringToBytes
        public static byte[] HexStringToBytes(string hexString)
        {
            if (hexString == null)
            {
                return null;
            }
            if (hexString.Contains("-"))
                hexString = hexString.Replace("-", "").Trim();

            if ((hexString.Length & 1) != 0)
            {
                return null;
            }

            byte[] result = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                byte b;
                if (byte.TryParse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber, null, out b) == false)
                    return null;

                result[i / 2] = b;
            }
            return result;
        }
        #endregion
    }



}