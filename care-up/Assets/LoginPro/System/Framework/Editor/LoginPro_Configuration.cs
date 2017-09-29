using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace LoginProAsset
{
    class LoginPro_Configuration : EditorWindow
    {
        private Vector2 scrollPos = Vector2.zero;
        // Window size
        static int windowWidth = 800;
        static int windowHeight = 860;
        private static int headerHeight = 80;

        // Files
        static UnityEngine.Object ClientSettings;
        static UnityEngine.Object AccountServerSettings;

        // Game information
        static string GameName = "My Game";
        static string GameVersion = "1.0";
        static string Domain = "www.my-server.com";
        static string SecureLoginFolder = "LoginPro/Game";
        static string SERVER_host = "localhost";
        static string SERVER_user = "my_server_user";
        static string SERVER_password = "my_server_password";
        static string DB_name = "db_name";
        static string AccountTable = "Account";
        static string AchievementsTable = "Achievements";
        static string GameTable = "Game";
        static string GamingTable = "Gaming";
        static string IPTable = "IP";
        static string NewsTable = "News";
        static string AttemptsTable = "Attempts";
        static string SaveGameTable = "SaveGameTable";
        static string ReportTable = "Report";
        static string SERVER_email = "my.email.adress@mail.com";
        static string SERVER_emailPassword = "my mailbox password";
        static string AvailableAttemptsBeforeBlocking = "10";
        static bool ScanIPClient = true;


        // Utils
        static string message = string.Empty;

        // Execution
        static bool initializationDone = false;
        static bool initDone = false;
        static string SAVED_ClientSettings = string.Empty;
        static string SAVED_AccountServerSettings = string.Empty;
        static string SAVED_GameName = string.Empty;
        static string SAVED_GameVersion = string.Empty;
        static string SAVED_Domain = string.Empty;
        static string SAVED_SecureLoginFolder = string.Empty;
        static string SAVED_SERVER_host = string.Empty;
        static string SAVED_SERVER_user = string.Empty;
        static string SAVED_SERVER_password = string.Empty;
        static string SAVED_DB_name = string.Empty;
        static string SAVED_AccountTable = string.Empty;
        static string SAVED_AchievementsTable = string.Empty;
        static string SAVED_GameTable = string.Empty;
        static string SAVED_GamingTable = string.Empty;
        static string SAVED_IPTable = string.Empty;
        static string SAVED_NewsTable = string.Empty;
        static string SAVED_AttemptsTable = string.Empty;
        static string SAVED_SaveGameTable = string.Empty;
        static string SAVED_ReportTable = string.Empty;
        static string SAVED_SERVER_email = string.Empty;
        static string SAVED_SERVER_emailPassword = string.Empty;
        static string SAVED_AvailableAttemptsBeforeBlocking = string.Empty;
        static bool SAVED_ScanIPClient = false;

        // Text
        static Texture settingsImage;
        static GUIStyle textStyle;
        static GUIStyle areaLabelStyle;
        static GUIStyle labelStyle;
        static GUIStyle textStyleBold;
        static GUIStyle textStyleBoldBlack;
        static GUIStyle buttonImageStyle;
        static int buttonWidth = 200;
        static int buttonHeight = 80;
        // Background
        private static Texture2D backColor;
        private static Texture backImage;
        private static Texture backIcon;


        [MenuItem("Assets/LoginPro Settings")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<LoginPro_Configuration>();
        }

        void Start()
        {
            //this.minSize = new Vector2 (windowWidth-1, windowHeight-1);
            this.minSize = Vector2.zero;
            this.maxSize = new Vector2(windowWidth, windowHeight);

            backColor = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            backColor.SetPixel(0, 0, Color.black);
            backColor.Apply();
            Texture2D labelAreaTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            labelAreaTexture.SetPixel(0, 0, new Color(200, 200, 200, 0f));

            labelAreaTexture.Apply();
            Texture2D labelBackTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            labelBackTexture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 0.6f));
            labelBackTexture.Apply();

            textStyle = new GUIStyle();
            textStyle.normal.textColor = Color.white;

            areaLabelStyle = new GUIStyle();
            areaLabelStyle.normal.background = labelAreaTexture;
            areaLabelStyle.padding = new RectOffset(30, 30, 0, 0);

            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.white;
            labelStyle.normal.background = labelBackTexture;
            labelStyle.margin = new RectOffset(2, 2, 2, 2);
            labelStyle.clipping = TextClipping.Clip;
            //labelStyle.padding = new RectOffset (0, 500, 0, 0);

            textStyleBold = new GUIStyle();
            textStyleBold.fontStyle = FontStyle.Bold;
            textStyleBold.fontSize = 12;
            textStyleBold.normal.textColor = Color.white;

            textStyleBoldBlack = new GUIStyle();
            textStyleBoldBlack.fontStyle = FontStyle.Bold;
            textStyleBoldBlack.fontSize = 12;
            textStyleBoldBlack.normal.textColor = Color.black;

            buttonImageStyle = new GUIStyle();
            buttonImageStyle.fixedWidth = 0;
            buttonImageStyle.fixedHeight = 0;
            buttonImageStyle.stretchWidth = true;
            buttonImageStyle.stretchHeight = true;

            backImage = Resources.Load("UI Utils/WindowBack") as Texture;
            backIcon = Resources.Load("UI Utils/AssetIcon") as Texture;

            settingsImage = Resources.Load("UI Utils/LoadConfig") as Texture;
        }

        void OnGUI()
        {
            // Initialization
            if (!initDone)
            {
                Start();
                initDone = false;
            }
            // Background
            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backColor, ScaleMode.StretchToFill);
            GUI.DrawTexture(new Rect(0, 0, position.width, position.height), backImage, ScaleMode.ScaleAndCrop);
            GUI.DrawTexture(new Rect(0, 0, position.width, position.height * 0.2f), backIcon, ScaleMode.ScaleToFit);

            if (!initializationDone)
            {
                GUILayout.BeginArea(new Rect(0, position.height * 0.2f, position.width, position.height * 0.3f));

                GUILayout.Label("\n     Configuration window :", textStyleBold);
                GUILayout.TextArea("This window allow you to generate all the configuration files you need."
                                   + "\n\nEvery fields are filled with information you tested earlier, in the installation process.\n"
                                   + "You will still be able to use ths installation process even if you modify something here.\n"
                                   + "\n-> You can see this window as a summary of your installation.\n"
                                   + "This window and the installation process are using the same information, if you modify something, it impacts both.",
                                   GUILayout.MaxWidth(position.width));
                GUILayout.EndArea();

                buttonWidth = 350;
                buttonHeight = 350;
                GUILayout.BeginArea(new Rect((position.width / 2) - buttonWidth / 2, position.height * 0.5f, buttonWidth, buttonHeight));
                if (GUILayout.Button(settingsImage, buttonImageStyle, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                {
                    InitWindowConfiguration();
                }
                GUILayout.EndArea();
            }
            else
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                GUILayout.Label(" ", textStyleBoldBlack, GUILayout.Width(windowWidth), GUILayout.Height(windowHeight));
                GUILayout.EndScrollView();

                // HEADER
                GUILayout.BeginArea(new Rect(-scrollPos.x, (windowHeight * 0.2f) - scrollPos.y, windowWidth, headerHeight), areaLabelStyle);
                GUILayout.Label("\nFiles to generate", textStyleBoldBlack);
                ClientSettings = EditorGUILayout.ObjectField("Local settings", ClientSettings, typeof(UnityEngine.Object), false) as UnityEngine.Object;
                AccountServerSettings = EditorGUILayout.ObjectField("Server settings", AccountServerSettings, typeof(UnityEngine.Object), false) as UnityEngine.Object;
                GUILayout.EndArea();

                // DATA WINDOW
                GUILayout.BeginArea(new Rect(-scrollPos.x, (windowHeight * 0.3f) - scrollPos.y, windowWidth, windowHeight * 0.71f), areaLabelStyle);
                // Game information
                GUILayout.Label("\nGame information", textStyleBoldBlack);
                GameName = EditorGUILayout.TextField("Game name", GameName, labelStyle);
                GameVersion = EditorGUILayout.TextField("Game version", GameVersion, labelStyle);

                // Server address
                GUILayout.Label("\nServer", textStyleBoldBlack);
                Domain = EditorGUILayout.TextField("Server URL", Domain, labelStyle);
                SecureLoginFolder = EditorGUILayout.TextField("Uploaded folder", SecureLoginFolder, labelStyle);

                // Database
                GUILayout.Label("\nDatabase", textStyleBoldBlack);
                SERVER_host = EditorGUILayout.TextField("Database host", SERVER_host, labelStyle);
                DB_name = EditorGUILayout.TextField("Database name", DB_name, labelStyle);
                SERVER_user = EditorGUILayout.TextField("Database user", SERVER_user, labelStyle);
                SERVER_password = EditorGUILayout.TextField("Database password", SERVER_password, labelStyle);

                // Database tables
                GUILayout.Label("\nTables", textStyleBoldBlack);
                AccountTable = EditorGUILayout.TextField("Accounts table", AccountTable, labelStyle);
                AchievementsTable = EditorGUILayout.TextField("Achievements table", AchievementsTable, labelStyle);
                GameTable = EditorGUILayout.TextField("Game table", GameTable, labelStyle);
                GamingTable = EditorGUILayout.TextField("Gaming table", GamingTable, labelStyle);
                IPTable = EditorGUILayout.TextField("IPs table", IPTable, labelStyle);
                NewsTable = EditorGUILayout.TextField("News table", NewsTable, labelStyle);
                AttemptsTable = EditorGUILayout.TextField("Attempts table", AttemptsTable, labelStyle);
                SaveGameTable = EditorGUILayout.TextField("SaveGame table", SaveGameTable, labelStyle);
                ReportTable = EditorGUILayout.TextField("Report table", ReportTable, labelStyle);


                // Other settings
                GUILayout.Label("\nOther settings", textStyleBoldBlack);
                SERVER_email = EditorGUILayout.TextField("Contact email", SERVER_email, labelStyle);
                SERVER_emailPassword = EditorGUILayout.TextField("Mailbox password", SERVER_emailPassword, labelStyle);
                AvailableAttemptsBeforeBlocking = EditorGUILayout.TextField("Attempts before blocking", AvailableAttemptsBeforeBlocking, labelStyle);
                ScanIPClient = EditorGUILayout.Toggle("Scan clients IP", ScanIPClient);

                GUILayout.Label(" \n", textStyleBoldBlack);
                GUILayout.Label(message, textStyleBoldBlack);

                CheckModifications();

                GUILayout.BeginArea(new Rect(((windowWidth / 2) - 100) - scrollPos.x, windowHeight * 0.626f, 200, windowHeight * 0.1f));
                if (GUILayout.Button("Generate configuration files"))
                {
                    GenerateConfigurationFiles();
                }
                GUILayout.EndArea();
                GUILayout.EndArea();
            }
        }

        static void CheckModifications()
        {
            bool modificationsOccured = false;
            if (!string.Equals(SAVED_ClientSettings, AssetDatabase.GetAssetPath(ClientSettings))) { SAVED_ClientSettings = AssetDatabase.GetAssetPath(ClientSettings); modificationsOccured = true; }
            if (!string.Equals(SAVED_AccountServerSettings, AssetDatabase.GetAssetPath(AccountServerSettings))) { SAVED_AccountServerSettings = AssetDatabase.GetAssetPath(AccountServerSettings); modificationsOccured = true; }
            if (!string.Equals(SAVED_GameName, GameName)) { SAVED_GameName = GameName; modificationsOccured = true; }
            if (!string.Equals(SAVED_GameVersion, GameVersion)) { SAVED_GameVersion = GameVersion; modificationsOccured = true; }
            if (!string.Equals(SAVED_Domain, Domain)) { SAVED_Domain = Domain; modificationsOccured = true; }
            if (!string.Equals(SAVED_SecureLoginFolder, SecureLoginFolder)) { SAVED_SecureLoginFolder = SecureLoginFolder; modificationsOccured = true; }
            if (!string.Equals(SAVED_SERVER_host, SERVER_host)) { SAVED_SERVER_host = SERVER_host; modificationsOccured = true; }
            if (!string.Equals(SAVED_SERVER_user, SERVER_user)) { SAVED_SERVER_user = SERVER_user; modificationsOccured = true; }
            if (!string.Equals(SAVED_SERVER_password, SERVER_password)) { SAVED_SERVER_password = SERVER_password; modificationsOccured = true; }
            if (!string.Equals(SAVED_DB_name, DB_name)) { SAVED_DB_name = DB_name; modificationsOccured = true; }
            if (!string.Equals(SAVED_AccountTable, AccountTable)) { SAVED_AccountTable = AccountTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_AchievementsTable, AchievementsTable)) { SAVED_AchievementsTable = AchievementsTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_GameTable, GameTable)) { SAVED_GameTable = GameTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_GamingTable, GamingTable)) { SAVED_GamingTable = GamingTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_IPTable, IPTable)) { SAVED_IPTable = IPTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_NewsTable, NewsTable)) { SAVED_NewsTable = NewsTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_AttemptsTable, AttemptsTable)) { SAVED_AttemptsTable = AttemptsTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_SaveGameTable, SaveGameTable)) { SAVED_SaveGameTable = SaveGameTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_ReportTable, ReportTable)) { SAVED_ReportTable = ReportTable; modificationsOccured = true; }
            if (!string.Equals(SAVED_SERVER_email, SERVER_email)) { SAVED_SERVER_email = SERVER_email; modificationsOccured = true; }
            if (!string.Equals(SAVED_SERVER_emailPassword, SERVER_emailPassword)) { SAVED_SERVER_emailPassword = SERVER_emailPassword; modificationsOccured = true; }
            if (!string.Equals(SAVED_AvailableAttemptsBeforeBlocking, AvailableAttemptsBeforeBlocking)) { SAVED_AvailableAttemptsBeforeBlocking = AvailableAttemptsBeforeBlocking; modificationsOccured = true; }
            if (SAVED_ScanIPClient != ScanIPClient) { SAVED_ScanIPClient = ScanIPClient; modificationsOccured = true; }

            if (modificationsOccured)
            {
                saveModifications();
            }
        }

        static void InitWindowConfiguration()
        {
            try
            {
                if (!File.Exists(ConfigurationPaths.LocalConfigurationFile))
                {
                    Debug.LogError(string.Format("Can't find the ProjectConfiguration.cfg file at path : {0}.", ConfigurationPaths.LocalConfigurationFile));
                }
                using (StreamReader sr = new StreamReader(ConfigurationPaths.LocalConfigurationFile))
                {
                    // The files to generate
                    ClientSettings = AssetDatabase.LoadMainAssetAtPath(ConfigurationPaths.LocalSettings);
                    AccountServerSettings = AssetDatabase.LoadMainAssetAtPath(ConfigurationPaths.ServerSettings);

                    // Information from the file
                    GameName = sr.ReadLine();
                    GameVersion = sr.ReadLine();
                    Domain = sr.ReadLine();
                    SecureLoginFolder = sr.ReadLine();
                    SERVER_host = sr.ReadLine();
                    SERVER_user = sr.ReadLine();
                    SERVER_password = sr.ReadLine();
                    DB_name = sr.ReadLine();
                    AccountTable = sr.ReadLine();
                    AchievementsTable = sr.ReadLine();
                    GameTable = sr.ReadLine();
                    GamingTable = sr.ReadLine();
                    IPTable = sr.ReadLine();
                    NewsTable = sr.ReadLine();
                    AttemptsTable = sr.ReadLine();
                    SaveGameTable = sr.ReadLine();
                    ReportTable = sr.ReadLine();
                    SERVER_email = sr.ReadLine();
                    SERVER_emailPassword = sr.ReadLine();
                    AvailableAttemptsBeforeBlocking = sr.ReadLine();
                    ScanIPClient = sr.ReadLine().Equals("True");

                    // Initialize the saving variables
                    SAVED_GameName = GameName;
                    SAVED_GameVersion = GameVersion;
                    SAVED_Domain = Domain;
                    SAVED_SecureLoginFolder = SecureLoginFolder;
                    SAVED_SERVER_host = SERVER_host;
                    SAVED_SERVER_user = SERVER_user;
                    SAVED_SERVER_password = SERVER_password;
                    SAVED_DB_name = DB_name;
                    SAVED_AccountTable = AccountTable;
                    SAVED_AchievementsTable = AchievementsTable;
                    SAVED_GameTable = GameTable;
                    SAVED_GamingTable = GamingTable;
                    SAVED_IPTable = IPTable;
                    SAVED_NewsTable = NewsTable;
                    SAVED_AttemptsTable = AttemptsTable;
                    SAVED_SaveGameTable = SaveGameTable;
                    SAVED_ReportTable = ReportTable;
                    SAVED_SERVER_email = SERVER_email;
                    SAVED_SERVER_emailPassword = SERVER_emailPassword;
                    SAVED_AvailableAttemptsBeforeBlocking = AvailableAttemptsBeforeBlocking;
                    SAVED_ScanIPClient = ScanIPClient;

                    initializationDone = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("The ProjectConfiguration.cfg file doesn't contain the correct information (not enough lines in it). " + e.ToString());
            }
        }

        static void saveModifications()
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(ConfigurationPaths.LocalConfigurationFile))
                {
                    sr.WriteLine(GameName);
                    sr.WriteLine(GameVersion);
                    sr.WriteLine(Domain);
                    sr.WriteLine(SecureLoginFolder);
                    sr.WriteLine(SERVER_host);
                    sr.WriteLine(SERVER_user);
                    sr.WriteLine(SERVER_password);
                    sr.WriteLine(DB_name);
                    sr.WriteLine(AccountTable);
                    sr.WriteLine(AchievementsTable);
                    sr.WriteLine(GameTable);
                    sr.WriteLine(GamingTable);
                    sr.WriteLine(IPTable);
                    sr.WriteLine(NewsTable);
                    sr.WriteLine(AttemptsTable);
                    sr.WriteLine(SaveGameTable);
                    sr.WriteLine(ReportTable);
                    sr.WriteLine(SERVER_email);
                    sr.WriteLine(SERVER_emailPassword);
                    sr.WriteLine(AvailableAttemptsBeforeBlocking);
                    sr.WriteLine(ScanIPClient ? "True" : "False");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Can't write in the ProjectConfiguration.cfg file. " + e.ToString());
            }
        }

        public static void GenerateConfigurationFiles()
        {
            if (ClientSettings == null || AccountServerSettings == null)
            {
                Debug.LogError("Please fill the 2 configuration files fields before generate them.");
                message = "Error : Please fill the 2 configuration files fields before generate them.";
                return;
            }

            string absolute_path = Application.dataPath;
            absolute_path = absolute_path.Substring(0, absolute_path.Length - 6);

            string path_ClientSettings = absolute_path + AssetDatabase.GetAssetPath(ClientSettings);
            string path_AccountServerSettings = absolute_path + AssetDatabase.GetAssetPath(AccountServerSettings);

            string clientFile = GenerateLocalSettings();
            string serverFile = GenerateServerSettings();

            File.WriteAllText(path_ClientSettings, clientFile);
            File.WriteAllText(path_AccountServerSettings, serverFile);

            message = "Configuration files generated -> Upload the folder 'Upload my content/LoginPro_Server' on your server now.";
        }

        public static string GenerateLocalSettings()
        {
            string content = @"
                    public static class LoginPro_LocalSettings
                    {
                        // The URL to reach the connection script named 'Server.php'
                        public static string URLtoServer = " +
                            "\"" + Domain + "/" + SecureLoginFolder + "/Server.php\""
                            + @";

                        // The game name
                        public static string GameName = " +
                            "\"" + GameName + "\""
                            + @";

                        // The version of the game, the server will compare this number with his own and alert the client if a new version is available
                        public static string GameVersion = " +
                            "\"" + GameVersion + "\""
                            + @";
                    }
                    ";

            return content;
        }

        public static string GenerateServerSettings()
        {
            string ipClients = ScanIPClient ? "TRUE" : "FALSE";
            string content = "";
            content += @"<?php
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	GOAL : Configure your server information
//
//	This file HAS TO BE protected !!
//	IF SOMEONE CAN READ THIS FILE : YOU PROTECTION IS DEAD !
//	.htaccess forbid all access to the entire folder so be caution not to place it elsewhere !
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Your domain
$_SESSION['Domain'] = '" + Domain + @"';
// The folder (or path) where you put the LoginAccount+ProSecure folder (initially : LoginAccount+ProSecure, if you didn't change it)
$_SESSION['SecureLoginFolder'] = '" + SecureLoginFolder + @"';

// Your host 
$_SESSION['SERVER_host'] = '" + SERVER_host + @"'; // Caution : keep 'localhost' EXCEPT if your database server is not on your server (expert only)
// Your username to connect to the database
$_SESSION['SERVER_user'] = '" + SERVER_user + @"';
// Your password to connect to the database 
$_SESSION['SERVER_password'] = '" + SERVER_password + @"';
// The name of your database
$_SESSION['DB_name'] = '" + DB_name + @"';
// The table where your accounts are saved
$_SESSION['AccountTable'] = '" + AccountTable + @"';
// The table where your achievements are saved
$_SESSION['AchievementsTable'] = '" + AchievementsTable + @"';
// The table where your games are saved
$_SESSION['GameTable'] = '" + GameTable + @"';
// The table where players gaming sessions are saved
$_SESSION['GamingTable'] = '" + GamingTable + @"';
// The table where your IPs are saved
$_SESSION['IPTable'] = '" + IPTable + @"';
// The table where game news are saved
$_SESSION['NewsTable'] = '" + NewsTable + @"';
// The table where your blocked IPs are saved
$_SESSION['AttemptsTable'] = '" + AttemptsTable + @"';
// The table where the saveGame information example are saved
$_SESSION['SaveGame'] = '" + SaveGameTable + @"';
// The table used to report abuses
$_SESSION['Report'] = '" + ReportTable + @"';
// The table where friend list and friend requests are saved
$_SESSION['Friends'] = 'Friends';
// The table of the servers
$_SESSION['Server'] = 'Server';
// The table of the chat message
$_SESSION['ChatMessage'] = 'ChatMessage';

// Your contact email (in case you want to send email validations), players will receive email from this email address (you could create a contact email address for example)
$_SESSION['SERVER_email'] = '" + SERVER_email + @"';
$_SESSION['SERVER_emailPassword'] = '" + SERVER_emailPassword + @"';

// The maximum number of wrong attempts before IP being blocked for an account
$_SESSION['AvailableAttemptsBeforeBlocking'] = " + AvailableAttemptsBeforeBlocking + @";

// Scan clients IP
define('SCAN_IP_ACTIVATED', " + ipClients + @", TRUE);

?>";

            return content;
        }
    }
}