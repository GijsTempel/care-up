using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class LoginPro_Forgot : MonoBehaviour
    {
        public InputField Mail;

        public UIAnimation AnimationButtonToStop;
        public UIAnimation AnimationHideCurrentWindow;
        public UIAnimation_Alert AnimationShowMessage;
        public UIAnimation AnimationShowLogin;

        void Start()
        {
            // Check that all field are set
            if (Mail == null)
            {
                Debug.LogError("Vul uw e-mailadres in.");
                return;
            }

            // Load playerPrefs to get last saved email address
            this.LoadPlayerPrefs();
        }

        public void Launch()
        {
            Debug.Log("Forgot launched.");
            LoginPro.Manager.Forgot(Mail.text, Success, Error);

            // Prefill fields with saved datas
            LoadPlayerPrefs();
        }

        public void LoadPlayerPrefs()
        {
            // Prefill fields with saved datas
            if (PlayerPrefs.HasKey("Mail"))
            {
                Mail.text = PlayerPrefs.GetString("Mail");
            }
        }

        public void Error(string errorMessage)
        {
            errorMessage = errorMessage.Replace("ERROR: ", "");

            // Stop animation
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

            // Launch all animations one after the other
            StartCoroutine(LaunchForgotAnimations());

            Debug.Log("Forgot succeeded.");
        }

        private IEnumerator LaunchForgotAnimations()
        {
            // Hide current window
            if (this.AnimationHideCurrentWindow != null)
                yield return this.AnimationHideCurrentWindow.Launch();

            // Show message on success
            if (this.AnimationShowMessage != null)
                yield return this.AnimationShowMessage.Show(string.Format("Information sent to : {0}\nCheck your emails", Mail.text), 5);

            // Launch animation on success
            if (this.AnimationShowLogin != null)
                yield return this.AnimationShowLogin.Launch();

            yield return null;
        }
    }
}