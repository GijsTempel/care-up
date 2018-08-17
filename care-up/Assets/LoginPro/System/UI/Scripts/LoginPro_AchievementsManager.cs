using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoginProAsset
{
    /// <summary>
    /// This class manages all achievements
    /// When the server is sending the achievements list : all achievements are set as Locked or Unlocked with their percentage
    /// This class is turned off for now because we have no achievements
    /// </summary>
    public class LoginPro_AchievementsManager : MonoBehaviour
    {
        //public LoginPro_Menu MenuWindow;

        public LoginPro_Achievement[] achievements;
        public AchivementsManuButton[] menuAchievements;

        void Awake()
        {
            // Get all achievements gameObjects
            this.achievements = transform.GetComponentsInChildren<LoginPro_Achievement>();
            this.menuAchievements = transform.GetComponentsInChildren<AchivementsManuButton>();
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "UMenuPro")
            {
                GetMenuAchievements();
            }
        }

        /// <summary>
        /// Ask the server for all achievements of the account
        /// </summary>
        public void GetAchievements()
        {
            // As kthe server for achievements list
            LoginPro.Manager.GetAchievements(RefreshAchievements, FailedToRefreshAchievements);
        }

        public void GetMenuAchievements()
        {
            // Ask the server for achievements list
            LoginPro.Manager.GetAchievements(RefreshMenuAchievements, FailedToRefreshAchievements);
        }

        /// <summary>
        /// In case the achievements list could not be found
        /// </summary>
        /// <param name="errorMessage"></param>
        public void FailedToRefreshAchievements(string errorMessage)
        {
            errorMessage = errorMessage.Replace("ERROR: ", "Get achievements list failed: ");

            // Show the error in the console
            Debug.LogError(errorMessage);
        }

        public void RefreshMenuAchievements(string[] datas)
        {
            Debug.Log(datas[0]);

            // Lock all achievements
            foreach (AchivementsManuButton achievement in this.menuAchievements)
            {
                achievement.opened = false;
            }

            // Unlock those who are set as unlocked
            for (int i = 1; i < datas.Length; i = i + 3)
            {
                int percent = 0;
                int.TryParse(datas[i + 1], out percent);
                unlockMenuAchievement(datas[i], percent);
            }
        }

        private void unlockMenuAchievement(string name, int percent)
        {
            Debug.Log("UnlockAchievement::" + name);
            foreach (AchivementsManuButton achievement in this.menuAchievements)
            {
                if (achievement.name == name)
                {
                    achievement.opened = percent >= 100;
                    achievement.UpdateElement();
                    return;
                }
            }
        }

        /// <summary>
        /// This method is called by the LoginPro_Manager when an event is sent on an achievement (or all)
        /// This refresh the whole list of achievements
        /// </summary>
        /// <param name="datas"></param>
        public void RefreshAchievements(string[] datas)
        {
            Debug.Log(datas[0]);

            // Lock all achievements
            foreach (LoginPro_Achievement achievement in this.achievements)
            {
                achievement.Unlocked = false;
                achievement.PercentToUnlock = 0;
            }

            // Unlock those who are set as unlocked
            for (int i = 1; i < datas.Length; i = i + 3)
            {
                int percent = 0;
                int.TryParse(datas[i + 1], out percent);
                unlockAchievement(datas[i], percent);
            }

            // Refresh achievements tags list
            //this.MenuWindow.UpdateAchievementsList();
        }

        /// <summary>
        /// Unlock one achievement by its name and its percentage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="percent"></param>
        private void unlockAchievement(string name, int percent)
        {
            //GameObject.Find("Examples").transform.Find("ActiveScenes").gameObject.SetActive(true);
            
            foreach (LoginPro_Achievement achievement in this.achievements)
            {
                if (achievement.Name == name)
                {
                    achievement.Unlocked = percent > 0;
                    achievement.PercentToUnlock = percent;
                    return;
                }
            } 
        }
    }
}