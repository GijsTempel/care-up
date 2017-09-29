using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// This class 
/// </summary>
namespace LoginProAsset
{
    public class GameInformation : MonoBehaviour
    {
        public Object nextSceneToLoad;

        private InputField nameField;
        private InputField versionField;
        private RectTransform nextStepButton;

        // File utils
        private List<string> lines;


        void Start()
        {
            // Get the UI components
            nameField = GameObject.Find("GameName").GetComponent<InputField>();
            versionField = GameObject.Find("GameVersion").GetComponent<InputField>();
            nextStepButton = GameObject.Find("NextStep").GetComponent<RectTransform>();

            // Get already saved hashCode
            readInformationSaved();
        }

        public void NextInstallationStep()
        {
            if (saveConfigurationFile())
            {
                LoginPro_Security.Load(nextSceneToLoad.name);
            }
            else
            {
                Debug.LogError("Can't save the version and the hash code in the file [" + ConfigurationPaths.LocalConfigurationFile + "]. Make sure it's accessible and not read only.");
            }
        }

        private void showNextStep()
        {
            nextStepButton.localScale = Vector3.one;
        }

        private bool saveConfigurationFile()
        {
            // If the configuration file didn't even exist or didn't contain the URL, we can't go any further, the user has to complete the fourth step before the fifth
            if (lines.Count < 1)
            {
                Debug.LogError("Please launch the step 4 before the step 5 because no URL is configured yet.");
                return false;
            }

            // Specify the new game version and hash code
            string name = nameField.text;
            string version = versionField.text;

            if (lines.Count < 3)
            {
                lines.Add(name);
                lines.Add(version);
            }
            else
            {
                lines[0] = name;
                lines[1] = version;
            }

            bool noError = true;

#if UNITY_WEBPLAYER
		Debug.LogError("No possible file generation while in webplayer plateform, please switch to another plateform in the build settings.");
#else
            string path = ConfigurationPaths.LocalConfigurationFile;
            try
            {
                File.WriteAllLines(path, lines.ToArray());
            }
            catch (IOException e)
            {
                noError = false;
                Debug.LogError(e);
            }
#endif
            return noError;
        }
        private void readInformationSaved()
        {
            string path = ConfigurationPaths.LocalConfigurationFile;
            lines = new List<string>();

            // If the file exists, read the URL from it if you can
            if (File.Exists(path))
            {
                try
                {
                    lines.AddRange(File.ReadAllLines(path));
                }
                catch (IOException e)
                {
                    Debug.LogError(e);
                }
            }
            if (lines.Count >= 2)
            {
                nameField.text = lines[0];
                versionField.text = lines[1];
            }
            else if (lines.Count <= 0)
            {
                Debug.LogError("Please launch the step 4 before the step 5 because no URL is configured yet.");
            }
        }
    }


}