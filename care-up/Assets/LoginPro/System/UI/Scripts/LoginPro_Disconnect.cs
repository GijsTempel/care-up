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

    }
}