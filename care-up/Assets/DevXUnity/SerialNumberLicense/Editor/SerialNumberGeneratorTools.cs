using System;
using UnityEngine;

using System.IO;
using System.Xml;

using System.Security.Cryptography;
using System.Collections.Generic;

namespace DevXUnityTools.Plugins
{

    /// <summary>
    /// License generator tools
    /// </summary>
    internal class SerialNumberGeneratorTools
    {
        #region BasePath
        static string _base_path ="AllSNLicenses";
        /// <summary>
        /// BasePath to licenses
        /// </summary>
        internal static string BasePath
        {
            get
            {
                if (System.IO.Directory.Exists(_base_path) == false)
                    System.IO.Directory.CreateDirectory(_base_path);
                return _base_path;
            }
        }
        #endregion

        #region LicenseInfo
        /// <summary>
        /// License Info Class
        /// </summary>
        internal class LicenseInfo
        {
            internal string name;
            internal DateTime create_date;
            internal string comment;
            internal string eMail;
            internal string LicenseContent;
        }
        #endregion



        #region UpdateKeys
        /// <summary>
        /// UpdateKeys
        /// </summary>
        /// <param name="re_create">requred regenerate keys</param>
        /// <param name="as_dsa">use DSA algoritm</param>
        internal static void UpdateKeys(bool re_create=false, bool as_dsa=false)
        {
            if(CloseKey==null || re_create)
            {
                string config = "";
                string config_name = System.IO.Path.Combine(BasePath, "SerialNumberSigner.config");

                if (System.IO.File.Exists(config_name))
                    config = System.IO.File.ReadAllText(config_name);

                SerialNumberSigner signer = null;
                
                if (as_dsa)
                {
                    signer = SerialNumberSigner.CreateDSA();
                    config = "DSA";
                }
                else
                {
                    signer = SerialNumberSigner.CreateSimple();
                    config = "Simple" + "\n" + "8" + "\n" + "10000";
                }

                if (signer!=null)
                {
                    System.IO.File.WriteAllText(config_name, config);

                    signer.GenerateKeys();
                    SerialNumberGeneratorTools.CloseKey = signer.SerializeKeys(include_private_key:true);
                    SerialNumberGeneratorTools.OpenKey= signer.SerializeKeys(include_private_key: false);
                }
            }
        }
        #endregion


        #region MakeLicense
        /// <summary>
        /// MakeLicense and save into folder
        /// </summary>
        /// <param name="hardware_id">hardware id</param>
        /// <param name="comment">comment</param>
        /// <param name="email">email</param>
        /// <returns>User license</returns>
        internal static string MakeLicense(string hardware_id, DateTime? expiration_date, string comment, string email)
        {
            UpdateKeys();

            if (CloseKey == null)
                return null;

            string path = BasePath;

            SerialNumberSigner signer = new DevXUnityTools.SerialNumberSigner(CloseKey);

            string lic= signer.Sign(hardware_id+(expiration_date.HasValue? "DateExpiration:"+expiration_date.Value.ToString("yyyy.MM.dd") : null));

            string lic_in=lic;
            string lic_out= "";
            for (int i=0; i<lic.Length; i+=4)
            {
                if (string.IsNullOrEmpty(lic_out)==false)
                        lic_out += "-";
                lic_out += lic_in.Substring(0, Math.Min(4, lic_in.Length));
                lic_in= lic_in.Remove(0, Math.Min(4, lic_in.Length));
            }
            if (lic_in.Length > 0)
            {
                lic_out += lic_in.Substring(0, Math.Min(4, lic_in.Length));
                lic_in = lic_in.Remove(0, Math.Min(4, lic_in.Length));
            }
            lic = lic_out;

            string file = Path.Combine(path, hardware_id+ (expiration_date.HasValue ? (string.IsNullOrEmpty(hardware_id)?"":"-")+"Expiration-" + expiration_date.Value.ToString("yyyy.MM.dd"):"" )+ ".lic");


            System.IO.File.WriteAllText(file, lic);
            System.IO.File.WriteAllText(file+".comment", comment);
            System.IO.File.WriteAllText(file + ".email", email);


            return lic;
        }
        #endregion


        #region GetLicenseList
        internal static List<LicenseInfo> LicenseList;
        /// <summary>
        /// Return all generated licenses
        /// </summary>
        /// <returns>License list</returns>
        internal static List<LicenseInfo> GetLicenseList()
        {
            if (LicenseList != null)
                return LicenseList;

            string path = BasePath;

            SortedList<long,LicenseInfo> list = new SortedList<long,LicenseInfo>();
            foreach(string file in System.IO.Directory.GetFiles(path,"*.lic"))
            {
                LicenseInfo lic = new Plugins.SerialNumberGeneratorTools.LicenseInfo();
                lic.name = Path.GetFileNameWithoutExtension(file);
                lic.LicenseContent = System.IO.File.ReadAllText(file);

                var inf = new FileInfo(file);
                lic.create_date = inf.CreationTime;
                if (System.IO.File.Exists(file + ".comment")) lic.comment= System.IO.File.ReadAllText(file + ".comment");
                if (System.IO.File.Exists(file + ".email")) lic.eMail = System.IO.File.ReadAllText(file + ".email");

                list.Add(inf.LastWriteTimeUtc.ToFileTimeUtc(), lic);
            }
            LicenseList = new List<LicenseInfo>(list.Values);
            LicenseList.Reverse();
            return LicenseList;
        }
        #endregion




        #region CloseKey
        /// <summary>
        /// RSA close key
        /// </summary>
        internal static string CloseKey
        {
            set
            {
                System.IO.File.WriteAllText("SN-License-CloseKey.txt", value);
            }
            get
            {
                if (System.IO.File.Exists("SN-License-CloseKey.txt") == false)
                    return null;

                return System.IO.File.ReadAllText("SN-License-CloseKey.txt");
            }
        }
        #endregion 

        #region OpenKey
        /// <summary>
        /// RSA open key 
        /// </summary>
        internal static string OpenKey
        {
            set
            {
                string path = Path.Combine("Assets", "Resources");
                if (System.IO.Directory.Exists(path) == false)
                    System.IO.Directory.CreateDirectory(path);

                System.IO.File.WriteAllText(Path.Combine(path, "SN-License-OpenKey.txt"), value);
            }
            get
            {
                string path = Path.Combine("Assets", "Resources");
                if (System.IO.Directory.Exists(path) == false)
                    return null;
                string file = Path.Combine(path, "SN-License-OpenKey.txt");

                if (System.IO.File.Exists(file) == false)
                    return null;

                return System.IO.File.ReadAllText(file);
            }
        }
        #endregion 
    }

    

}
