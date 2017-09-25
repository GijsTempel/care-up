using System;
using UnityEngine;

using System.IO;
using System.Xml;
using System.Security.Cryptography;



namespace DevXUnityTools
{
    #region SerialNumberValidateTools
    /// <summary>
    /// SerialNumber validation class
    /// </summary>
    internal class SerialNumberValidateTools
    {

        #region Verify
        /// <summary>
        /// Verify current user serial number
        /// </summary>
        /// <returns></returns>
        internal static bool Verify()
        {
            var lic_file = Resources.Load<TextAsset>("SN-License-OpenKey");
            if (lic_file == null || string.IsNullOrEmpty(lic_file.text))
                return false;
            string open_key = lic_file.text;
            var signer=new SerialNumberVerify(open_key);

            string hardwareID = HardwareID;
            bool res = signer.VerifySignature(hardwareID, SerialNumberKey);
            DateTime date = DateTime.Now;
            int i = 0;
            while (res == false && i < 400)
            {
                res = signer.VerifySignature("DateExpiration:"+DateTime.Now.AddDays(i).ToString("yyyy.MM.dd"), SerialNumberKey);
                res = signer.VerifySignature(hardwareID+"DateExpiration:" + DateTime.Now.AddDays(i).ToString("yyyy.MM.dd"), SerialNumberKey);
                i++;
            }

            return res;
        }
        #endregion


        #region SerialNumberKey
        /// <summary>
        /// Set or get - User Serial Number key
        /// </summary>
        internal static string SerialNumberKey
        {
            set
            {
                UnityEngine.PlayerPrefs.SetString("SerialNumberKey", value);
            }
            get
            {
                return UnityEngine.PlayerPrefs.GetString("SerialNumberKey");
            }
        }
        #endregion

        //

        #region HardwareID
        /// <summary>
        /// Hardware ID
        /// </summary>
        static internal string HardwareID
        {
            get
            {
                return GetStringHashAsHex(UnityEngine.SystemInfo.deviceName + UnityEngine.SystemInfo.graphicsDeviceID);
            }
        }
        #endregion

        #region GetStringHash
        /// <summary>
        /// String Hash
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static internal uint GetStringHash(string str)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(encoding.GetBytes(str));
            return BitConverter.ToUInt32(result, 0);
        }
        #endregion

        #region GetStringHashAsHex
        internal static string GetStringHashAsHex(string s)
        {
            return string.Format("{0:X}", GetStringHash(s));
        }
        #endregion

    }
    #endregion

    

}
