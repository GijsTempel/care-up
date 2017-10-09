using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This class manages the communication with the server
/// Every action made from the game to the server is done here
/// Encryption is automatically done by calling the method "ExecuteOnServer"
/// 
/// ExecuteOnServer(ACTION_NAME, methodForSuccess, methodForError, DATAS);
/// ACTION_NAME is the name of the action in Server.php in the ACTION ZONE
/// DATAS is the datas array you get when you are on your action script
/// 
/// See SendData.php and GetData.php for good examples
/// </summary>
namespace LoginProAsset
{
    public class LoginPro_Manager : MonoBehaviour
    {
        /// <summary>
        /// Makes the whole login entity persistent between scenes
        /// </summary>
        void Awake()
        {
            // Register to LoginPro as THE manager
            LoginPro.Manager = this;
        }

        // This function is called when the scene changes
        // The session will be kept
        // The process of changing scene is transparent
        void OnDestroy()
        {
            // Remove every reference to the current gameobject : it's gonna be destroyed
            LoginPro.Manager = null;
        }

        /// <summary>
        /// Create tables needed for the login system
        /// Call methodForSuccess if login succeeded
        /// Call methodForError if login failed
        /// </summary>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void CreateTables(Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            ExecuteOnServer("CreateTables", methodForSuccess, methodForError, null);
        }

        /// <summary>
        /// Get news of the game
        /// Call methodForSuccess if login succeeded
        /// Call methodForError if login failed
        /// </summary>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void News(Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            ExecuteOnServer("News", methodForSuccess, methodForError, null);
        }

        /// <summary>
        /// Login with credentials
        /// Call methodForSuccess if login succeeded
        /// Call methodForError if login failed
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void Login(string username, string password, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            // If datas are empty : no need to send the form
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                methodForError("Vul alle velden in om in te loggen.");
                return;
            }
            if (username.Length < 3)
            {
                methodForError("Gebruikersnaam moet minimaal 3 characters bevatten.");
                return;
            }
            if (password.Length < 3)
            {
                methodForError("Wachtwoord moet minimaal 3 characters bevatten.");
                return;
            }

            // Information to send to the server (encrypted with RSA)
            string[] datas = new string[2];
            datas[0] = username;
            datas[1] = LoginPro_Security.hash(password); // No the password here is NOT salted, only the server can read it (the only one to have the certificate private key). So we let the server take care of the salt (if it's a registration it will generate a new salt and save it besides the salted password, if it's a login it will use the saved salt of the account)
            ExecuteOnServer("Login", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Register with user information
        /// Call methodForSuccess if register succeeded
        /// Call methodForError if register failed
        /// </summary>
        /// <param name="username"></param>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void Register(string username, string mail, string password, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            // If datas are empty : no need to send the form
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(password))
            {
                methodForError("Vul alle velden in om te registreren.");
                return;
            }
            if (username.Length < 3)
            {
                methodForError("Gebruikersnaam moet minimaal 3 characters bevatten.");
                return;
            }
            if (mail.Length < 3 || !mail.Contains("@"))
            {
                methodForError("Het e-mailadres is niet geldig.");
                return;
            }
            if (password.Length < 3)
            {
                methodForError("Wachtwoord moet minimaal 3 characters bevatten.");
                return;
            }

            // Information to send to the server (encrypted with RSA)
            string[] datas = new string[3];
            datas[0] = mail;
            datas[1] = username;
            datas[2] = LoginPro_Security.hash(password);
            ExecuteOnServer("Register", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Modify user information
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="username"></param>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void Modify(string username, string mail, string password, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            // If datas are empty : no need to send the form
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(password))
            {
                methodForError("Vul alle velden in om je acount te veranderen. ");
                return;
            }
            if (username.Length < 3)
            {
                methodForError("Gebruikersnaam moet minimaal 3 characters bevatten.");
                return;
            }
            if (mail.Length < 3 || !mail.Contains("@"))
            {
                methodForError("Het e-mailadres is niet geldig. ");
                return;
            }
            if (password.Length < 3)
            {
                methodForError("Wachtwoord moet minimaal 3 characters bevatten.");
                return;
            }

            // Information to send to the server (encrypted with RSA)
            string[] datas = new string[3];
            datas[0] = mail;
            datas[1] = username;
            datas[2] = LoginPro_Security.hash(password);
            ExecuteOnServer("Modify", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Notice the server the player is still connected
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void Notice(Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            ExecuteOnServer("Notice", methodForSuccess, methodForError, null);
        }

        /// <summary>
        /// Get email to retrieve user information
        /// Call methodForSuccess if email sent
        /// Call methodForError if email not sent
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void Forgot(string mail, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            // If datas are empty : no need to send the form
            if (string.IsNullOrEmpty(mail))
            {
                methodForError("Vul alle velden in om een nieuw wachtwoord te genereren. ");
                return;
            }
            if (mail.Length < 3 || !mail.Contains("@"))
            {
                methodForError("E-mailadres is niet geldig. ");
                return;
            }

            // Information to send to the server (encrypted with RSA)
            string[] datas = new string[1];
            datas[0] = mail;
            ExecuteOnServer("Forgot", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Get achievements of the user
        /// Call methodForSuccess if report sent
        /// Call methodForError if report not sent
        /// </summary>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void GetAchievements(Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            ExecuteOnServer("Achievements", methodForSuccess, methodForError, null);
        }

        /// <summary>
        /// Unlock achievement with a percent, set it to 0 to lock it again
        /// Call methodForSuccess if report sent
        /// Call methodForError if report not sent
        /// </summary>
        /// <param name="achievementName"></param>
        /// <param name="percent"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void UpdateAchievement(string achievementName, int percent, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            // If achievementName is empty : no need to send the form
            if (string.IsNullOrEmpty(achievementName))
            {
                methodForError("Please set the name of the achievement you want to unlock.");
                return;
            }

            string[] datas = new string[2];
            datas[0] = achievementName;
            datas[1] = percent.ToString();
            ExecuteOnServer("UnlockAchievement", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Report any abuse in game
        /// Call methodForSuccess if report sent
        /// Call methodForError if report not sent
        /// </summary>
        /// <param name="message"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void Report(string message, string screenshot, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            // If datas are empty : no need to send the form
            if (string.IsNullOrEmpty(message) || message.Length < 3)
            {
                methodForError("Please explain the situation to report an abuse.");
                return;
            }
            // If datas are empty : no need to send the form
            if (string.IsNullOrEmpty(screenshot))
            {
                methodForError("No screenshot taken.");
                return;
            }

            // Information to send to the server (encrypted with RSA)
            string[] datas = new string[2];
            datas[0] = message;
            datas[1] = screenshot;
            ExecuteOnServer("Report", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Get the reports list of players
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void ReportsList(Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            if (LoginPro.Session.Role != LoginPro_UserRole.Admin)
            {
                methodForError("Only administrators get execute this action.");
                return;
            }
            ExecuteOnServer("Administration", methodForSuccess, methodForError, null);
        }

        /// <summary>
        /// Get the screenshot taken in a report
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void GetScreenshot(string reportId, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            if (LoginPro.Session.Role != LoginPro_UserRole.Admin)
            {
                methodForError("Only administrators get execute this action.");
                return;
            }

            string[] datas = new string[1];
            datas[0] = reportId;
            ExecuteOnServer("GetScreenshot", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Save reports list
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void SaveReportsList(string[] datas, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            if (LoginPro.Session.Role != LoginPro_UserRole.Admin)
            {
                methodForError("Only administrators get execute this action.");
                return;
            }
            ExecuteOnServer("SaveAdministration", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Add a new to the game
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="newTitle"></param>
        /// <param name="newText"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void AddNew(string newTitle, string newText, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            if (LoginPro.Session.Role != LoginPro_UserRole.Admin)
            {
                methodForError("Only administrators get execute this action.");
                return;
            }
            string[] datas = new string[2];
            datas[0] = newTitle;
            datas[1] = newText;
            ExecuteOnServer("AddNew", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Ban a specific user
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="usernameToBan"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void BanUser(string usernameToBan, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            if (LoginPro.Session.Role != LoginPro_UserRole.Admin)
            {
                methodForError("Only administrators get execute this action.");
                return;
            }
            string[] datas = new string[1];
            datas[0] = usernameToBan;
            ExecuteOnServer("BanUser", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// UnBan a specified user
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="usernameToUnBan"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void UnBanUser(string usernameToUnBan, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            if (LoginPro.Session.Role != LoginPro_UserRole.Admin)
            {
                methodForError("Only administrators get execute this action.");
                return;
            }
            string[] datas = new string[1];
            datas[0] = usernameToUnBan;
            ExecuteOnServer("UnBanUser", methodForSuccess, methodForError, datas);
        }

        /// <summary>
        /// Change the role of a specified user
        /// Call methodForSuccess if modification succeeded
        /// Call methodForError if modification failed
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <param name="methodForSuccess"></param>
        /// <param name="methodForError"></param>
        public void ChangeUserRole(string username, string role, Action<string[]> methodForSuccess, Action<string> methodForError)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                methodForError("You must login before calling this action.");
                return;
            }
            if (LoginPro.Session.Role != LoginPro_UserRole.Admin)
            {
                methodForError("Only administrators get execute this action.");
                return;
            }
            string[] datas = new string[2];
            datas[0] = username;
            datas[1] = role;
            ExecuteOnServer("ChangeUserRole", methodForSuccess, methodForError, datas);
        }






        /// <summary>
        /// Automatic sending data over the server and handling datas received
        /// Just specify a method to handle result with this signature
        /// void [METHOD_NAME] (string[] serverDatas)
        /// and test if serverDatas[0] contains "ERROR" or not
        /// If it contains "ERROR":
        /// -> it means something went wrong
        /// -> otherwise your get the server answers in 'serverDatas' array !ALREADY DECRYPTED!
        /// </summary>
        public void ExecuteOnServer(string action, Action<string[]> methodForSuccess, Action<string> methodForError, string[] datas = null)
        {
            StartCoroutine(createForm(action, methodForSuccess, methodForError, datas));
        }

        /// <summary>
        /// This is the main function to communicate with the server
        /// Every script must use it because it tests absolutely everything automatically
        /// (See LoginManager and Example1Manager to see how it works)
        /// - If the parameter 'session' is set as NotConnectedYet -> the encryption will be made with RSA&AES: creation of a session on the server, generation (client side) of AES keys decrypted with RSA private certificate by the server
        /// - If the parameter 'session' is set as Connected -> the session token (generated client side) will be checked (both sides): see this session token as the key of the session
        /// </summary>
        private IEnumerator createForm(string action, Action<string[]> methodForResult, Action<string> methodForError, string[] infoToEncrypt = null)
        {
            // If no action has been specified -> stop
            if (action == "")
            {
                Debug.LogError("createForm: No action has been set.");
                yield break;
            }

            // The optionnal information to send (encrypted of course)
            string concatenatedData = "";
            // Only set this field if there is something to send (it could be an action)
            if (infoToEncrypt != null)
            {
                // Transform info from string[] to string (with separators)
                for (int i = 0; i < infoToEncrypt.Length; i++)
                {
                    concatenatedData += infoToEncrypt[i] + LoginPro_Security.separator;
                }
            }
            // Create the web form to send to the server
            WWWForm form = new WWWForm();
            form.AddField("Action", action);
            form.AddField("GameName", LoginPro_LocalSettings.GameName);
            form.AddField("GameVersion", LoginPro_LocalSettings.GameVersion);

            if (LoginPro.Session.LoggedIn) // SSL connection already established
            {
                // Add the session ID (not encrypted of course, because the server has to get AES keys back if they exist in session array)
                if (LoginPro.Session.Session_id == "") { Debug.LogError("ERROR, createForm: LoginPro_Session.session_id is empty."); }
                form.AddField("SID", LoginPro.Session.Session_id);
            }
            else // SSL connection not established yet (encrypt information with RSA)
            {
                LoginPro_Security.PrepareSecurityInformation();    // Generate AES keys and encrypt it with RSA public key + Generate random session token + Read RSA public key from certificate
                string aesKeys = LoginPro.Session.AES_Key + LoginPro_Security.separator + LoginPro.Session.AES_IV;      // Brand new generated AES key and AES IV
                form.AddField("AESKeys", LoginPro_Security.RSA_encrypt(aesKeys));                                       // Encrypt AES keys to send with RSA public key
                form.AddField("NotConnectedYet", "true");                                                               // Say that the server has to decrypt AES Keys
            }

            concatenatedData += LoginPro.Session.Session_token;                     // Add the session token
            concatenatedData = LoginPro_Security.AES_encrypt(concatenatedData);     // Encrypt the message with AES
            form.AddField("EncryptedInfo", concatenatedData);                       // Send encrypted data

            // Create and return the form
            WWW w = new WWW("http://" + LoginPro_LocalSettings.URLtoServer, form);
            yield return w;                                                         // Wait for the result

            // RESULT ARRIVED:
            if (!string.IsNullOrEmpty(w.error)) // ERROR
            {
                Debug.LogError(w.error);
                Debug.LogError("Server can't be reached. Did you configure the AccountServerSettings.cs script ?\nMake sure the Server.php script is well placed, and your AccountServerSettings.cs (in game) AND AccountServerSettings.php (on server) scripts are corrects.\n" + w.error);
                methodForError("Server can't be reached: " + w.error);

                // Clear the form
                w.Dispose();
                yield break;
            }
            else if (w.text.Contains("ERROR") || w.text.Contains("Error") || w.text.Contains("error")) // ERROR
            {
                string errorMessage = w.text;
                // Clear the form
                w.Dispose();
                // Call the method with error
                methodForError(errorMessage);
                yield break;
            }
            else
            {
                string[] serverDatas = readServerDatas(w.text);
                // Clear the form
                w.Dispose();

                if (serverDatas != null)
                {
                    // Call the method with success
                    string[] serverDatasWithoutToken = new string[serverDatas.Length - 1];
                    for (int i = 0; i < serverDatasWithoutToken.Length; i++)
                    {
                        serverDatasWithoutToken[i] = serverDatas[i];
                    }
                    methodForResult(serverDatasWithoutToken);
                    yield break;
                }
                else
                {
                    methodForError("Session tokens don't match!");
                    yield break;
                }
            }
        }

        /// <summary>
        /// The method to call when the server answers.
        /// The AES decryption is made to read the server's answer.
        /// It tests the session_token (to ensure security).
        /// And gives an array with all datas (the exact copy of what you sent in your PHP script).
        /// </summary>
        public static string[] readServerDatas(string encryptedData)
        {
            if (encryptedData == "") { Debug.LogError(""); return null; }
            if (LoginPro.Session.AES_Key == "" || LoginPro.Session.AES_IV == "") { Debug.LogError("Disconnected."); return null; }

            // If the answer does not contains the delimitor : there is a problem
            if (!encryptedData.Contains(LoginPro_Security.delimitor))
            {
                Debug.LogError("The server's answer does not contains any encryption delimitor, the server's answer is [" + encryptedData + "]");
                return null;
            }

            // Get the first string after the delimitor
            string encryptedDatasReceived = encryptedData.Split(new string[] { LoginPro_Security.delimitor }, StringSplitOptions.None)[1];
            // Split and return data once it's decrypted
            string[] datas = LoginPro_Security.AES_decrypt(encryptedDatasReceived).Split(new string[] { LoginPro_Security.separator }, StringSplitOptions.None);

            // Check if session token match !
            if (datas.Length <= 0 || datas[datas.Length - 1] != LoginPro.Session.Session_token)
            {
                Debug.LogError("Session tokens don't match!");
                return null;
            }
            // If the session token is correct it means we talked to the server
            // Only the authentic server could possibly decrypt our RSA encrypted session token
            // It's here returned AES encrypted (so the server decrypted it with RSA private key)
            // Only the server could do that -> we talk to the server : continue our session
            return datas;
        }
    }
}