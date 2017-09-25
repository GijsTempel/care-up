using System;
using System.Collections.Generic;

namespace DevXUnityTools
{
    /// <summary>
    /// Serial Generator Signer
    /// </summary>
    internal sealed class SerialNumberVerify
    {
        SerialNumberVerifySimple simple;
        SerialNumberVerifyDSA dsa;

        #region SerialNumberVerify
        internal SerialNumberVerify(string serialized_key)
        {
            if (string.IsNullOrEmpty(serialized_key))
                return;

            if (serialized_key.StartsWith("DSA:"))
            {
                dsa = new DevXUnityTools.SerialNumberVerifyDSA(serialized_key);
            }
            else
                simple = new DevXUnityTools.SerialNumberVerifySimple(serialized_key);
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


        #region HexStringToBytes
        public static byte[] HexStringToBytes(string hexString)
        {
            if (hexString == null)
            {
                return null;
            }
            if(hexString.Contains("-"))
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