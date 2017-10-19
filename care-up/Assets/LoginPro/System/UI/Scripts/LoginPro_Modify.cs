using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class LoginPro_Modify : MonoBehaviour
    {
        public InputField Username;
        public InputField Mail;
        public InputField Password;
        public InputField ConfirmPassword;

        public LoginPro_Menu MenuAction;

        public LoginPro_Login LoginAction;
        public LoginPro_Forgot ForgotAction;

        public UIAnimation AnimationButtonToStop;
        public UIAnimation AnimationHideCurrentWindow;
        public UIAnimation_Alert AnimationShowMessage;
        public UIAnimation AnimationWindowToShow;

        void Start()
        {
            // Prefill fields with saved datas
            this.LoadPlayerPrefs();
        }

        public void LoadPlayerPrefs()
        {
            // Prefill fields with saved datas
            if (PlayerPrefs.HasKey("Username"))
                Username.text = PlayerPrefs.GetString("Username");
            if (PlayerPrefs.HasKey("Mail"))
                Mail.text = PlayerPrefs.GetString("Mail");
            if (PlayerPrefs.HasKey("Password"))
            {
                Password.text = PlayerPrefs.GetString("Password");
                ConfirmPassword.text = PlayerPrefs.GetString("Password");
            }
        }

        public void LoadSession()
        {
            // Prefill fields with saved datas
            Username.text = LoginPro.Session.Username;
            Mail.text = LoginPro.Session.Mail;
            Password.text = LoginPro.Session.Password;
            ConfirmPassword.text = LoginPro.Session.Password;
        }

        public void Launch()
        {
            // Check that all field are set
            if (Username == null || Mail == null || Password == null || ConfirmPassword == null)
            {
                Debug.LogError("Vul alle velden in om uw account aan te passen.");
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

            Debug.Log("Modification launched.");
            LoginPro.Manager.Modify(Username.text, Mail.text, Password.text, Success, Error);
        }

        public void Error(string errorMessage)
        {
            // Stop button rotation animation
            if (this.AnimationButtonToStop != null)
                this.AnimationButtonToStop.Stop();

            // Show the error
            Debug.LogWarning(errorMessage);

            // Show message on error
            if (this.AnimationShowMessage != null)
                this.AnimationShowMessage.Show(errorMessage, 5);
        }

        public void Success(string[] datas)
        {
            // Stop button rotation animation
            if (this.AnimationButtonToStop != null)
                this.AnimationButtonToStop.Stop();

            // Update session
            LoginPro.Session.Username = Username.text;
            LoginPro.Session.Mail = Mail.text;
            LoginPro.Session.Password = Password.text;

            // Save information in playerPrefs
            PlayerPrefs.SetString("Username", Username.text);
            PlayerPrefs.SetString("Mail", Mail.text);
            PlayerPrefs.SetString("Password", Password.text);
            if (this.LoginAction != null)
                this.LoginAction.LoadPlayerPrefs();
            if (this.ForgotAction != null)
                this.ForgotAction.LoadPlayerPrefs();

            // Update Menu
            if (this.MenuAction != null)
                this.MenuAction.UpdateMenu();

            // Launch all animations one after the other
            StartCoroutine(LaunchModifyAnimations());

            Debug.Log("Aanpassingen voltooid.");
        }

        private IEnumerator LaunchModifyAnimations()
        {
            // Hide current window
            if (this.AnimationHideCurrentWindow != null)
                yield return this.AnimationHideCurrentWindow.Launch();

            // Show message on success
            if (this.AnimationShowMessage != null)
                yield return this.AnimationShowMessage.Show("Account informatie aangepast!", 3);

            // Launch animation on success
            if (this.AnimationWindowToShow != null)
                yield return this.AnimationWindowToShow.Launch();

            yield return null;
        }
    }
}