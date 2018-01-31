using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace LoginProAsset
{
    public class LoginPro_ShowReport : MonoBehaviour
    {
        public string AdministrationSceneName;
        //public UIAnimator Animator;
       // public UIAnimation_Alert Popup;

        [HideInInspector]
        public static string ScreenshotTaken = "";

        public void Start()
        {
            // Check if the Administration scene is in the build settings
            CheckSceneIsInBuildSettings(AdministrationSceneName);
        }

        public void Launch()
        {
            // If the user is admin go to account administration
            if (LoginPro.Session.Role == LoginPro_UserRole.Admin)
            {
                LoginPro_Security.Load(AdministrationSceneName);    // The connection is not established, go back to login menu
                return;
            }

            //StartCoroutine(LaunchCapture());
        }
        /*
        private void LaunchReport()
        {
            // Inform the player that a screenshot has been taken
            if (ScreenshotTaken == "")
            {
                this.Popup.Show("Error : No screenshot taken.", 5);
                return;
            }
            else
                this.Popup.Show("A screenshot has been taken, please explain the situation to the administrator.", 10);

            // Show report window
            this.Animator.OneAfterTheOther();
        }*/

        private IEnumerator LaunchCapture()
        {
            // The path of the buffer file
            string screenshotPath = Application.persistentDataPath + "/Screenshot.png";
            // Remove the file if exists
            if (File.Exists(screenshotPath))
            {
                Debug.Log("Screenshot.png file exists : remove it.");
                File.Delete(screenshotPath);
            }

            // Capture the screenshot
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(screenshotPath);

            // Wait for the screenshot to be taken (end of the fixed update)
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.2f);

            // Wait some seconds in case the screen is not taken
            if (!File.Exists(screenshotPath))
                yield return new WaitForSeconds(2f);

            // If still not there : something went wrong
            if (!File.Exists(screenshotPath))
            {
                Debug.Log("Screenshot missed. Please retry.");
               // this.Popup.Show("Screenshot missed. Please retry.", 3);
                yield break;
            }

            // The frame is over and the screenshot took
            byte[] bytes = LoginPro_Security.readFile(screenshotPath);

            // Screenshot compression
            Texture2D tex = new Texture2D(862, 415);
            tex.LoadImage(bytes);
            bytes = tex.EncodeToJPG(75);

            // Get final screenshot
            ScreenshotTaken = Convert.ToBase64String(bytes);

            //LaunchReport();
        }

        private void CheckSceneIsInBuildSettings(string requiredSceneName)
        {
#if UNITY_EDITOR
            foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    string sceneName = scene.path.Substring(scene.path.LastIndexOf('/') + 1);
                    sceneName = sceneName.Substring(0, sceneName.Length - 6);

                    if (sceneName == requiredSceneName)
                        return;
                }
            }
            Debug.LogError(string.Format("LoginPro_ShowReport : The scene {0} is not in the build settings, caution!", requiredSceneName));
#endif
        }
    }
}