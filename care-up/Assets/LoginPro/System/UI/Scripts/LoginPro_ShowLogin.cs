using UnityEngine;

namespace LoginProAsset
{
    public class LoginPro_ShowLogin : MonoBehaviour
    {
        public UIAnimation AnimationWhenLoggedIn;
        public UIAnimation AnimationWhenNotLoggedIn;

        public static bool MenuShown = false;

        /// <summary>
        /// Useful when the scene changes : menu is hidden
        /// </summary>
        void Awake()
        {
            MenuShown = false;
        }

        /// <summary>
        /// Show the login window if not connected yet
        /// Show the account menu window if connected
        /// </summary>
        public void ShowLoginMenu()
        {
            // Activate only if the menu is not already shown
            if (!MenuShown)
            {
                // If the user is logged in : show his menu
                if (LoginPro.Session.LoggedIn)
                {
                    if (this.AnimationWhenLoggedIn != null)
                    {
                        MenuShown = true;
                        this.AnimationWhenLoggedIn.Launch();
                    }
                }
                // Otherwise ask him to log in
                else
                {
                    if (this.AnimationWhenNotLoggedIn != null)
                    {
                        MenuShown = true;
                        this.AnimationWhenNotLoggedIn.Launch();
                    }
                }
            }
        }

        /// <summary>
        /// Set the flag LoginPro_ShowLogin.MenuShown as false when the menu is closed
        /// </summary>
        public void FlagMenuAsClosed()
        {
            MenuShown = false;
        }
    }
}