using UnityEngine;

namespace LoginProAsset
{
    //[RequireComponent(typeof(PlaceUIElement))]
    public class LoginPro_Achievement : MonoBehaviour
    {
        [HideInInspector]
        public string Name;

        public bool Unlocked = false;
        //[Range(0, 10)]
        //public int DisplayTime = 5;
        public int PercentToUnlock = 100;

        //public UIAnimation_Place animationShow;

        private LoginPro_AchievementsManager AchievementsManager;

        void Awake()
        {
            this.Name = gameObject.name;
            //this.AchievementsManager = transform.parent.GetComponent<LoginPro_AchievementsManager>();
            this.AchievementsManager = GameObject.FindObjectOfType<LoginPro_AchievementsManager>();

        }

        /// <summary>
        /// Unlock achievement and refresh the list of whole achievements and achievements tags
        /// </summary>
        public void Unlock(int percent = 100)
        {
            // Call server to unock this achievement for this user
            LoginPro.Manager.UpdateAchievement(this.Name, percent, UnlockSuccess, UnlockError);
        }
        public void UnlockError(string errorMessage)
        {
            errorMessage = errorMessage.Replace("ERROR: ", "Unlock achievement failed: ");

            // Show the error in the console
            Debug.LogWarning(errorMessage);
        }
        public void UnlockSuccess(string[] achievements)
        {
            // Refresh achievements tags list
            this.AchievementsManager.RefreshAchievements(achievements);

            // Show the unlocked achievement IF the server said it has been unlocked
            if (this.Unlocked)
            {
                //animationShow.Launch();
                GetComponent<Animator>().SetTrigger("Show");
            }
        }

        /// <summary>
        /// Lock achievement and refresh the list of whole achievements and achievements tags
        /// </summary>
        public void Lock()
        {
            // Call server to lock this achievement for this user
            LoginPro.Manager.UpdateAchievement(this.Name, 0, this.AchievementsManager.RefreshAchievements, LockError);
        }

        public void LockError(string errorMessage)
        {
            errorMessage = errorMessage.Replace("ERROR: ", "Lock achievement failed: ");

            // Show the error in the console
            Debug.LogWarning(errorMessage);
        }
    }
}