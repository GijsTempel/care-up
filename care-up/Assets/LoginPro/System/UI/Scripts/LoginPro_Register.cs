using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class LoginPro_Register : MonoBehaviour
    {
        public InputField Username;
        public InputField Mail;
        public InputField Password;
        public InputField ConfirmPassword;

        public LoginPro_Login LoginAction;
        public LoginPro_Modify ModifyAction;
        public LoginPro_Forgot ForgotAction;

        //public UIAnimation AnimationButtonToStop;
       // public UIAnimation AnimationHideCurrentWindow;
       // public UIAnimation_Alert AnimationShowMessage;
       // public UIAnimation AnimationShowLogin;
        public SceneSelection menuOnSuccess;
        public void Launch()
        {
            // Check that all field are set
            if (Username == null || Mail == null || Password == null || ConfirmPassword == null)
            {
                Debug.LogError("Vul alle velden in om een account aan te maken.");
                return;
            }

            // Check if passwords match
            if (Password.text != ConfirmPassword.text)
            {
                string errorMessage = "De wachtwoorden komen niet overeen.";
                Debug.LogWarning(errorMessage);
                Error(errorMessage);
                return;
            }

            Debug.Log("Registration launched.");
            LoginPro.Manager.Register(Username.text, Mail.text, Password.text, Success, Error);
        }

        public void Error(string errorMessage)
        {
            // Stop button rotation animation
          //  if (this.AnimationButtonToStop != null)
              //  this.AnimationButtonToStop.Stop();

            // Show the error
            Debug.LogWarning(errorMessage);

            // Show message on error
           // if (this.AnimationShowMessage != null)
           //     this.AnimationShowMessage.Show(errorMessage, 3);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
        }

        public void Success(string[] datas)
        {
            // Stop button rotation animation
          //  if (this.AnimationButtonToStop != null)
              //  this.AnimationButtonToStop.Stop();

            // Save information in playerPrefs
            PlayerPrefs.SetString("Username", Username.text);
            PlayerPrefs.SetString("Mail", Mail.text);
            PlayerPrefs.SetString("Password", Password.text);
            if (this.LoginAction != null)
                this.LoginAction.LoadPlayerPrefs();
            if (this.ModifyAction != null)
                this.ModifyAction.LoadPlayerPrefs();
            if (this.ForgotAction != null)
                this.ForgotAction.LoadPlayerPrefs();

            // Launch all animations one after the other
           // StartCoroutine(LaunchRegisterAnimations());
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Registratie voltooid! Je kunt nu inloggen met je gebruikersnaam & wachtwoord.");
            GameObject.Find("LoginPro").GetComponent<LoginMenuManager>().ShowMenu(menuOnSuccess);
            Debug.Log("Registration succeeded.");
        }

        /*private IEnumerator LaunchRegisterAnimations()
        {
            // Hide current window
            if (this.AnimationHideCurrentWindow != null)
                yield return this.AnimationHideCurrentWindow.Launch();

            // Show message on success
            if (this.AnimationShowMessage != null)
				yield return this.AnimationShowMessage.Show("Registratie voltooid!\nClick op de link in de e-mail, die naar je is toegestuurd, om je account te verifiëren", 7);

            // Launch animation on success
            if (this.AnimationShowLogin != null)
                yield return this.AnimationShowLogin.Launch();

            yield return null;
        }*/
    }
}