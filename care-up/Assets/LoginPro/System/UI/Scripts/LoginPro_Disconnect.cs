using UnityEngine;
using System.Collections;

namespace LoginProAsset
{
    public class LoginPro_Disconnect : MonoBehaviour
    {
       // public UIAnimation AnimationHideCurrentWindow;
       // public UIAnimation_Alert AnimationShowMessage;
       // public UIAnimation AnimationShowLogin;

        //public UIAnimation AnimationHideReports;
       // public UIAnimation AnimationHideFriends;

        public void Launch()
        {
            // Clear the session
            LoginPro.Session.ClearSession();
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Je bent uitgelogd");
            // Hide menu and show login
            // Launch all animations one after the other
            // StartCoroutine(LaunchRegisterAnimations());
        }

        /*private IEnumerator LaunchRegisterAnimations()
        {
            // Hide report window
            if (this.AnimationHideReports != null)
                yield return this.AnimationHideReports.Launch();

            // Hide friends window
            if (this.AnimationHideFriends != null)
                yield return this.AnimationHideFriends.Launch();

            // Hide current window
            if (this.AnimationHideCurrentWindow != null)
                yield return this.AnimationHideCurrentWindow.Launch();

            // Show message on success
            if (this.AnimationShowMessage != null)
                yield return this.AnimationShowMessage.Show("U bent uitgelogd.", 2);

            // Launch animation on success
            if (this.AnimationShowLogin != null)
                yield return this.AnimationShowLogin.Launch();

            yield return null;
        }*/
    }
}