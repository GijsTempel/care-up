using System;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    /// <summary>
    /// This class is useful to deal with the login process and getting needed information when the player logs in
    /// </summary>
    public class LoginPro_Login : MonoBehaviour
    {
        public Text News;
        public InputField Username;
        public InputField Password;

        public LoginPro_Menu Menu;
        public LoginPro_AchievementsManager AchievementsManager;

        public UIAnimation AnimationToPlayOnSuccess;
        public UIAnimation_Alert MessageToShowOnResult;
        public UIAnimation AnimationToStopOnResult;

        public Button ButtonSave;
        public Button ButtonDontSave;

        /// <summary>
        /// When the scene starts ask the server for achievements (if connected) or game news (if not connected yet)
        /// Then, load players prefs to prefill fields
        /// </summary>
        void Start()
        {
            // Check that all field are set
            if (Username == null || Password == null)
            {
                Debug.LogError("Please specify all fields needed to the action LoginPro_Login.");
                return;
            }

            // If the player is already logged in : get his achievements
            if (LoginPro.Session.LoggedIn)
            {
                // Get user's achievements
                this.AchievementsManager.GetAchievements();
            }

            // Ask for the news of the game
            LoginPro.Manager.News(NewsSuccess, NewsError);

            // Prefill fields with saved datas
            this.LoadPlayerPrefs();
        }

        /// <summary>
        /// Switch to save information in playerPrefs or not depending on the user's choice
        /// </summary>
        /// <returns></returns>
        private bool SaveIsChecked()
        {
            if (!PlayerPrefs.HasKey("Save") || PlayerPrefs.GetString("Save") == "Save")
            {
                CheckSave();
                return true;
            }
            CheckDontSave();
            return false;
        }
        public void CheckSave()
        {
            PlayerPrefs.SetString("Save", "Save");
            this.ButtonSave.transform.localScale = Vector3.one;
            this.ButtonDontSave.transform.localScale = Vector3.zero;
        }
        public void CheckDontSave()
        {
            PlayerPrefs.SetString("Save", "DontSave");
            this.ButtonSave.transform.localScale = Vector3.zero;
            this.ButtonDontSave.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// The News results from the server :
        /// Show error if any
        /// Show news if any
        /// </summary>
        /// <param name="errorMessage"></param>
        public void NewsError(string errorMessage)
        {
            // Show message in console if error
            Debug.LogWarning(errorMessage);
        }
        public void NewsSuccess(string[] datas)
        {
            // Set the news of the game to display them at startup
            this.News.text = datas[0];
        }

        /// <summary>
        /// Load player prefs saved
        /// </summary>
        public void LoadPlayerPrefs()
        {
            // Prefill fields with saved datas
            if (PlayerPrefs.HasKey("Username") && PlayerPrefs.HasKey("Password"))
            {
                Username.text = PlayerPrefs.GetString("Username");
                Password.text = PlayerPrefs.GetString("Password");
            }
            // Check if login must be saved or not
            SaveIsChecked();
        }

        /// <summary>
        /// The method to be called on UI
        /// </summary>
        public void Launch()
        {
            if (LoginPro.Manager != null)
                LoginPro.Manager.Login(Username.text, Password.text, Success, Error);
        }

        /// <summary>
        /// In case login failed : inform the player
        /// </summary>
        /// <param name="errorMessage"></param>
        public void Error(string errorMessage)
        {
            errorMessage = errorMessage.Replace("ERROR: ", "Login failed: ");

            // Stop animation
            if (this.AnimationToStopOnResult != null)
                this.AnimationToStopOnResult.Stop();

            // Show the error
            Debug.LogWarning(errorMessage);

            // Show message on error
            if (this.MessageToShowOnResult != null)
                this.MessageToShowOnResult.Show(errorMessage, 5);
        }

        /// <summary>
        /// In case of success : get all account information from the server
        /// Say hello to the player
        /// Get achievements
        /// </summary>
        /// <param name="datas"></param>
        public void Success(string[] datas)
        {
            // Save information in session
            LoginPro.Session.Session_id = datas[1];
            LoginPro.Session.LoggedIn = true;
            LoginPro.Session.Role = datas[2].ToEnum<LoginPro_UserRole>();
            LoginPro.Session.Username = Username.text;
            LoginPro.Session.Password = Password.text;
            LoginPro.Session.Mail = datas[3];
            LoginPro.Session.RegistrationDate = datas[4];
            LoginPro.Session.CurrentConnectionDate = DateTime.Now;
            LoginPro.Session.PreviousConnectionDate = datas[5];
            double minutesPlayed = 0;
            double.TryParse(datas[6], out minutesPlayed);
            LoginPro.Session.MinutesPlayed = minutesPlayed;

            // Update the menu
            if (this.Menu != null)
                this.Menu.UpdateMenu();

            // Save information in playerPrefs (if it's specified)
            if (SaveIsChecked())
            {
                PlayerPrefs.SetString("Username", LoginPro.Session.Username);
                PlayerPrefs.SetString("Password", LoginPro.Session.Password);
            }

            // Stop animation
            if (this.AnimationToStopOnResult != null)
                this.AnimationToStopOnResult.Stop();

            // Show message on success
            if (this.MessageToShowOnResult != null)
                this.MessageToShowOnResult.Show(string.Format("Welcome {0}!", LoginPro.Session.Username), 2);

            // Launch animation on success
            if (this.AnimationToPlayOnSuccess != null)
                this.AnimationToPlayOnSuccess.Launch();

            // Allow opening menu
            LoginPro_ShowLogin.MenuShown = false;

            Debug.Log("Login succeeded.");

            // Get user's achievements
            this.AchievementsManager.GetAchievements();
        }
    }
}