using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class LoginPro_Forgot : MonoBehaviour
    {
        public InputField Mail;
        public SceneSelection menuOnSuccess;

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
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);

            // Show the error
            Debug.LogWarning(errorMessage);

        }

        public void Success(string[] datas)
        {
            // Launch all animations one after the other
           // StartCoroutine(LaunchForgotAnimations());
            GameObject.Find("LoginPro").GetComponent<LoginMenuManager>().ShowMenu(menuOnSuccess);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Succes, Er is een e-mail naar je toegestuurd met een nieuw wachtwoord.");
            Debug.Log("Forgot succeeded.");
        }
  
    }
}