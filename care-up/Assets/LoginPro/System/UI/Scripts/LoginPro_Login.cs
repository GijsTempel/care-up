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
        //public LoginPro_AchievementsManager AchievementsManager; --->Achemvements are not used and turned off

        public Button ButtonSave;
        public Button ButtonDontSave;

        public SceneSelection menuOnSuccess;

        /// <summary>
        /// When the scene starts ask the server for achievements (if connected) or game news (if not connected yet)
        /// Then, load players prefs to prefill fields
        /// </summary>
        /// 
        void Awake()
        {
            // Load username & Password 
            LoadPlayerPrefs();

        }
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
                // this.AchievementsManager.GetAchievements(); ---> Achemvements are not used and turned off
            }

            // Ask for the news of the game
            //LoginPro.Manager.News(NewsSuccess, NewsError);

            // Prefill fields with saved datas
            this.LoadPlayerPrefs();
        }
        /*
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
            Debug.Log("save ");
        }
        public void CheckDontSave()
        {
            PlayerPrefs.SetString("Save", "DontSave");
            Debug.Log("Geen save ");
            this.ButtonSave.transform.localScale = Vector3.zero;
            this.ButtonDontSave.transform.localScale = Vector3.one;
        }*/

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
       /* public void NewsSuccess(string[] datas)
        {
            // Set the news of the game to display them at startup
            this.News.text = datas[0];
        }*/

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
           // SaveIsChecked();
        }

        /// <summary>
        /// The method to be called on UI
        /// </summary>
        public void Launch()
        {
            if (LoginPro.Manager != null)
                LoginPro.Manager.Login(Username.text, Password.text, Success, Error);

            Debug.Log("Login launched");
        }

        /// <summary>
        /// In case login failed : inform the player
        /// </summary>
        /// <param name="errorMessage"></param>
        public void Error(string errorMessage)
        {
          
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
            errorMessage = errorMessage.Replace("ERROR: ", "Login mislukt: ");


            // Show the error
            Debug.LogWarning(errorMessage);
        }

        /// <summary>
        /// In case of success : get all account information from the server
        /// Say hello to the player
        /// Get achievements
        /// </summary>
        /// <param name="datas"></param>
        public void Success(string[] datas)
        {

            //GameObject.Find("LoginPro").GetComponent<LoginMenuManager>().ShowMenu(menuOnSuccess);
            //GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Welkom!" + LoginPro.Session.Username);
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
           
                PlayerPrefs.SetString("Username", LoginPro.Session.Username);
                PlayerPrefs.SetString("Password", LoginPro.Session.Password);
           

            // Stop animation
           // if (this.AnimationToStopOnResult != null)
           //     this.AnimationToStopOnResult.Stop();

            // Show message on success
           // if (this.MessageToShowOnResult != null)
            //    this.MessageToShowOnResult.Show(string.Format("Welkom {0}!", LoginPro.Session.Username), 2);

            // Launch animation on success
           // if (this.AnimationToPlayOnSuccess != null)
            //   this.AnimationToPlayOnSuccess.Launch();

            // Allow opening menu
//            LoginPro_ShowLogin.MenuShown = false;

            Debug.Log("Login succeeded.");

            if (GameObject.Find("Preferences") != null)
            {
                GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().AfterLoginCheck();
            }
            else
            {
                Debug.LogError("Where are Preferences GameObject?!");
            }

            LoginPro.Manager.ExecuteOnServer("IncrementLoginNumber", Debug.Log, Debug.LogError);
            //LoginPro_Security.Load("UMenuPro");
       

            // Get user's achievements
            //this.AchievementsManager.GetAchievements(); --->Achemvements are not used and turned off
        }
    }
}