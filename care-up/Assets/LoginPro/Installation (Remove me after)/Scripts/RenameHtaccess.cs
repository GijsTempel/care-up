using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

namespace LoginProAsset
{
    /// <summary>
    /// This class allows us to check if the scenes are presents in the build settings
    /// </summary>
    public class RenameHtaccess : MonoBehaviour
    {
        public Object nextSceneToLoad;

        private Text alertField;
        private RectTransform nextStepButton;
        private RectTransform validIcon;

        private RectTransform errorIcon;
        private SpriteRenderer loader;


        private float timeBeforeStart = 1f;

        void Start()
        {
            // Get the UI components
            alertField = GameObject.Find("AlertMessage").GetComponent<Text>();
            nextStepButton = GameObject.Find("NextStep").GetComponent<RectTransform>();
            validIcon = GameObject.Find("ValidIcon").GetComponent<RectTransform>();
            errorIcon = GameObject.Find("ErrorIcon").GetComponent<RectTransform>();
            loader = GameObject.Find("Loader").GetComponent<SpriteRenderer>();

            // Start the verification
            StartCoroutine(RenameHtaccessFile());
        }

        public void NextInstallationStep()
        {
            LoginPro_Security.Load(nextSceneToLoad.name);
        }

        private IEnumerator RenameHtaccessFile()
        {
            // Wait for the customer to know what we are testing here
            yield return new WaitForSeconds(timeBeforeStart);
            loader.color = new Color(255, 255, 255, 0);

            // Then rename the htaccess file
            HandleHtaccessFile(ConfigurationPaths.htaccessFile);

            // Go to next step
            NextStep();
        }
        private void NextStep()
        {
            nextStepButton.localScale = Vector3.one;
            validIcon.localScale = Vector3.one;
            errorIcon.localScale = Vector3.zero;
            alertField.text = "Htaccess file created, you can continue the installation.";
        }
        private void ShowError(string error)
        {
            nextStepButton.localScale = Vector3.zero;
            validIcon.localScale = Vector3.zero;
            errorIcon.localScale = Vector3.one;
            alertField.text = error;
        }
        private void HandleHtaccessFile(string path)
        {
#if UNITY_WEBPLAYER
		Debug.LogError("No possible file generation while in webplayer plateform, please switch to another plateform in the build settings.");
#else
            // If the .htaccess already exists, no need to execute the renaming
            if (File.Exists(path + ".htaccess"))
            {
                Debug.Log("Success.");
            }
            else
            {
                if (File.Exists(path + "htaccess"))
                {
                    File.Move(path + "htaccess", path + ".htaccess");
                }
                else
                {
                    if (!File.Exists(path + ".htaccess"))
                    {
                        try
                        {
                            File.AppendAllText(path + ".htaccess", "Deny from all");
                        }
                        catch (IOException e)
                        {
                            Debug.LogError(e);
                            ShowError(e.Message);
                        }
                    }
                }
            }
            // Remove the htaccess.meta file
            if (File.Exists(path + "htaccess.meta"))
            {
                File.Delete(path + "htaccess.meta");
            }
#endif
        }
    }
}