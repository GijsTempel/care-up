using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{/*
    /// <summary>
    /// This class allow you to understand how achievements work, and how to unlock them
    /// An achievement is a child of the object "Achievements" (in LoginPro gameObject)
    /// It has an image (a background) and a message
    /// 
    /// An achievement is unlock as soon as it's percent is > 0
    /// Everytime its percent changes a little popup shows it
    /// 
    /// Locking an achievement can be done by 2 ways :
    /// - Call achievement.Lock() on it
    /// - Call achievement.Unlock(0) on it (percent is zero or less : it's locked)
    /// 
    /// An achievement TAG is a little achievement trophy a player can see in his account menu (in game where he can find and change his account information)
    /// It's automatically generated based on player's unlocked (and locked) achievements
    /// </summary>
    public class LoginPro_AchievementUnlocker : MonoBehaviour
    {
        //public LoginPro_Achievement Achievement;
        public InputField Percent;

        /// <summary>
        /// Unlock achievement with a percentage
        /// </summary>
        public void Unlock()
        {
            int percent = 0;
            int.TryParse(Percent.text, out percent);
            this.Achievement.Unlock(percent);
        }

        /// <summary>
        /// Lock achievement
        /// </summary>
        public void Lock()
        {
            this.Achievement.Lock();
        }
    }*/
}