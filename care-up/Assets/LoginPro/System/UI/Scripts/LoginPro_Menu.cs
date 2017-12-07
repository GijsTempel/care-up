using System;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    /// <summary>
    /// This class manage the account menu of the player
    /// </summary>
    public class LoginPro_Menu : MonoBehaviour
    {
        public Text UsernameText;
        public Text RoleText;
        public Text MailText;
        public Text RegistrationDateText;
        public Text LastConnectionDateText;
        public Text TimePlayedText;
        public Text News;

        public bool MobileUI = false;
        //public GameObject Achievements;
       // public GameObject ListLeft;
        //public GameObject ListRight;
        //public GameObject UnknownAchievementPrefab;

      //  public AchievementsListScroller ScrollAchievementsList;

//        private bool initiated = false;
        private int fpsCounter = -1;


        void Start()
        {
            // Reinit the achievement list (in case a user is connecting with another account : clear the achievements before receiving it)
            // This avoid to see previous achievements, remaining during some milliseconds (this way everything is cleared)
            // this.UpdateAchievementsList(); ---> change for achievements
            LoginPro.Manager.News(NewsSuccess, NewsError);
        }

        /// <summary>
        /// Set all menu information based on session information
        /// </summary>
        public void UpdateMenu()
        {
            if (this.UsernameText == null)
                Debug.Log("this.UsernameText IS NULLLL");
            this.UsernameText.text = LoginPro.Session.Username;
            this.RoleText.text = LoginPro.Session.Role.ToString();
            this.MailText.text = LoginPro.Session.Mail;
            this.RegistrationDateText.text = "Registratie datum :\n[" + LoginPro.Session.RegistrationDate + "]";
            this.LastConnectionDateText.text = "Laatste inlog datum :\n[" + LoginPro.Session.PreviousConnectionDate + "]";
            this.TimePlayedText.text = LoginPro.Session.TimePlayedText;
        }
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
        /// Update achievements TAGS based on the achievements received from the server
        /// </summary>
       /*public void UpdateAchievementsList() ----> uncomment for usage of achievements 
        {
            // If it's the first time the list must be created
            if (!initiated)
            {
                initiated = true;
                CreateAchievementsList();
                return;
            }

            // Refresh all achievements tags
            // If on Mobile UI : there is only one list
            LoginPro_AchievementTag[] achievementsTags = null;
            if (MobileUI)
                achievementsTags = this.ListLeft.GetComponentsInChildren<LoginPro_AchievementTag>();
            else
                achievementsTags = this.ScrollAchievementsList.GetComponentsInChildren<LoginPro_AchievementTag>();

            foreach (LoginPro_AchievementTag achievementTag in achievementsTags)
            {
                achievementTag.RefreshDisplay();
            }

            Debug.Log("Update achievements");
            // Refresh list
            if (this.ScrollAchievementsList != null)
                this.ScrollAchievementsList.Refresh();
        }

        /// <summary>
        /// Create the menu with a tag for each existing achievement
        /// </summary>
        public void CreateAchievementsList()
        {
            // Error if no objects specified
            if (!MobileUI && (this.Achievements == null || this.ListLeft == null || this.ListRight == null))
            {
                return;
            }

            int rowIndex = 0;
            bool impair = false;
            LoginPro_Achievement[] achievements = this.Achievements.GetComponentsInChildren<LoginPro_Achievement>();
            foreach (LoginPro_Achievement achievement in achievements)
            {
                GameObject list = impair ? ListRight : ListLeft;

                GameObject createdAchievementTag = (GameObject)GameObject.Instantiate(this.UnknownAchievementPrefab);
                createdAchievementTag.transform.SetParent(list.transform);
                createdAchievementTag.name = achievement.name;
                createdAchievementTag.transform.position = list.transform.position;

                LoginPro_AchievementTag tag = createdAchievementTag.GetComponent<LoginPro_AchievementTag>();
                if (MobileUI)
                {
                    tag.RatioHeightWidth = 0.2f;
                    tag.MobileUI = true;
                }
                tag.rowIndex = rowIndex;

                // Show achievmeent only if it's unlocked
                tag.Init(achievement);

                impair = !impair;

                // Keep achievements in the ListLeft only (if on mobile UI)
                if (MobileUI)
                    impair = false;

                // New row only when pair must be done next time
                if (!impair)
                    rowIndex++;
            }

            Debug.Log("Create achievements");
            // Refresh list
            if (this.ScrollAchievementsList != null)
                this.ScrollAchievementsList.Refresh();
        }
        */
        /// <summary>
        /// A timer set to launch Notice process every minutes
        /// </summary>
        void FixedUpdate()
        {
            if (LoginPro.Session.LoggedIn)
            {
                // Update menu only once in a while (every seconds)
                if (this.fpsCounter < 0)
                {
                    this.fpsCounter = 600;
                    this.Launch();
                }
                this.fpsCounter--;
            }
        }

        /// <summary>
        /// This is useful to notice the server that the player is still connected
        /// </summary>
        public void Launch()
        {
            Debug.Log("Notice launched.");
            LoginPro.Manager.Notice(Success, Error);
        }

        /// <summary>
        /// In case the server returns an error during Notice process
        /// </summary>
        /// <param name="errorMessage"></param>
        public void Error(string errorMessage)
        {
            // Show the error
            Debug.LogWarning("Server notice failed (Can happen on disconnection) : " + errorMessage);
        }

        /// <summary>
        /// When the Notice process succeeds : update the menu information like time played...
        /// </summary>
        /// <param name="datas"></param>
        public void Success(string[] datas)
        {
            // Show the server answer
            string minutesPlayedFromServer = datas[0];

            double minutesPlayed = 0;
            double.TryParse(minutesPlayedFromServer, out minutesPlayed);
            LoginPro.Session.MinutesPlayed = minutesPlayed;

            // Update session
            TimeSpan timePlayed = TimeSpan.FromMinutes(LoginPro.Session.MinutesPlayed);
            LoginPro.Session.TimePlayedText = "Gespeeld : " + Math.Round(timePlayed.TotalHours, 0) + " uur en " + timePlayed.Minutes + " minuten";

            // Update menu
            UpdateMenu();
        }
    }
}