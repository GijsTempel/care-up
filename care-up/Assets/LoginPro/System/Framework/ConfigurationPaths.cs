using UnityEngine;

namespace LoginProAsset
{
    public static class ConfigurationPaths
    {
        // Htaccess
        public static string htaccessFile
        { get { return Application.dataPath + "/LoginPro/Upload my content/LoginPro_Server/Game/Includes/"; } }

        // Configuration file
        public static string LocalConfigurationFile
        { get { return Application.dataPath + "/LoginPro/System/Framework/ProjectConfiguration.cfg"; } }

        // Local settings to reach the server
        public static string LocalSettings
        { get { return "Assets/LoginPro/System/Framework/LoginPro_LocalSettings.cs"; } }

        // Server settings : database access + certificates + ...
        public static string ServerSettings
        { get { return "Assets/LoginPro/Upload my content/LoginPro_Server/Game/Includes/ServerSettings.php"; } }

        // Public certificate
        public static string publicCertificateFile
        { get { return Application.dataPath + "/LoginPro/System/Framework/Resources/PublicCertificate.txt"; } }

        // Private certificate
        public static string privateCertificateFile
        { get { return Application.dataPath + "/LoginPro/Upload my content/LoginPro_Server/Game/Includes/PrivateCertificate.crt"; } }


    }
}
