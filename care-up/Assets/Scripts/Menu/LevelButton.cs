using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    private static LoadingScreen loadingScreen;

    public string bundleName;
    public string sceneName;

    public bool multiple;
    public string displayName;
    public Sprite image;
    public bool testDisabled;
    public bool validated;
    public string totalPoints;

    private static Transform sceneInfoPanel = default(Transform);
    private static PlayerPrefsManager manager;

    private static Transform leaderboard;
    private static Transform scores;
    private static Transform names;
    public bool demoLock = true;

    // saving info
    public struct Info
    {
        public string bundleName;
        public string sceneName;
        public string displayName;
        public string description;
        public string result;
        public Sprite image;
        public bool testDisabled;
        public bool validated;
        public string totalPoints;
    };

    public List<Info> variations = new List<Info>();

    public bool buy = false;

    private void Start()
    {
        if (GameObject.Find("Preferences") != null && loadingScreen == null)
        {
            loadingScreen = GameObject.Find("Preferences").GetComponent<LoadingScreen>();
            if (loadingScreen == null) Debug.LogError("No loading screen found");
        }

        if (manager == null)
        {
            if (GameObject.Find("Preferences") != null)
            {
                manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
                if (manager == null) Debug.LogWarning("No prefs manager ( start from 1st scene? )");
            }
            else
            {
                Debug.LogWarning("No prefs manager ( start from 1st scene? )");
            }
        }
    }

    public void OnLevelButtonClick()
    {
        if (buy || demoLock)
        {
            // show dialogue now instead
            GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(5);
        }
        else
        {
            LevelButton mainBtn = GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/DialogTestPractice/Panel_UI/Buttons/Start").GetComponent<LevelButton>();

            if (multiple)
            {
                // we need to fill info in the dialogue
                GameObject dialogue = GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/Dialog 1");

                // setting title i assume
                dialogue.transform.Find("Content/Panel_UI/Top/Title").GetComponent<Text>().text = displayName;
                if (manager != null)
                {
                    manager.currentSceneVisualName = displayName;
                    manager.validatedScene = validated;
                }

                // filling up options
                for (int i = 0; i < variations.Count; ++i)
                {
                    LevelSelectionScene_UI_Option option =
                        dialogue.transform.Find("Content/Buttons/Option_" + (i + 1)).GetComponent<LevelSelectionScene_UI_Option>();
                    option.bundleName = variations[i].bundleName;
                    option.sceneName = variations[i].sceneName;
                    option.transform.GetComponentInChildren<Text>().text = variations[i].displayName;

                    if (i == 0)
                    {
                        // set 1st option as default
                        option.SetSelected();
                    }
                }

                // we need to show this dialogue only for scenes with variations
                GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(0);
            }
            else
            {
                // filling info for loading
                mainBtn.bundleName = bundleName;
                mainBtn.sceneName = sceneName;

                // for single variation we can skip into practice/test dialogue
                GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(3);

                if (manager != null)
                {
                    manager.currentSceneVisualName = displayName;
                    manager.validatedScene = validated;
                }
            }

            // maybe disable test button
            //GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/DialogTestPractice/Panel_UI/TestButton")
            //    .GetComponent<Button>().interactable = !testDisabled;

            //making button not interactable was not noticable (maybe change design), hiding instead
            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/DialogTestPractice/Panel_UI/Buttons/TestButton")
                .SetActive(!testDisabled);

            // now we can fetch practice plays number in order to know whethere to hide or show test button
            // making test button inactive from the beginning before fetching
            Button testBtn = GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
            "DialogTestPractice/Panel_UI/Buttons/TestButton").GetComponent<Button>();

            string formattedSceneName = PlayerPrefsManager.FormatSceneName(manager.currentSceneVisualName);

            int practicePlays;
            int.TryParse(DatabaseManager.FetchField("PracticePlays", formattedSceneName), out practicePlays);

            testBtn.interactable = practicePlays >= 3;

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/TestButton/contentlocked/practiceamount")
                .GetComponent<Text>().text = (3 - practicePlays).ToString() + " keer";

            if (testBtn.interactable)
            {
                float testHighscore;
                float.TryParse(DatabaseManager.FetchField("TestHighscores",
                    formattedSceneName).Replace(",", "."), out testHighscore);
                GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/TestButton/contentunlocked/percentage")
                    .GetComponent<Text>().text = Mathf.RoundToInt(testHighscore).ToString() + "%";
            }

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                "DialogTestPractice/Panel_UI/Buttons/TestButton/contentunlocked").SetActive(testBtn.interactable);
            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                "DialogTestPractice/Panel_UI/Buttons/TestButton/contentlocked").SetActive(!testBtn.interactable);

            int practiceHighscore, practiceStars;
            int.TryParse(DatabaseManager.FetchField("PracticeHighscores", "score_" + formattedSceneName), out practiceHighscore);
            int.TryParse(DatabaseManager.FetchField("PracticeHighscores", "stars_" + formattedSceneName), out practiceStars);

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                "DialogTestPractice/Panel_UI/Buttons/PracticeButton/content/score").
            GetComponent<Text>().text = practiceHighscore.ToString();

            Sprite grey = Resources.Load<Sprite>("Sprites/Stars/star 1");
            Sprite gold = Resources.Load<Sprite>("Sprites/Stars/star_128x128px");

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/PracticeButton/content/Stars/Star1")
                .GetComponent<Image>().sprite = (practiceStars >= 1.0f) ? gold : grey;

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/PracticeButton/content/Stars/Star2")
                .GetComponent<Image>().sprite = (practiceStars >= 2.0f) ? gold : grey;

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/PracticeButton/content/Stars/Star3")
                .GetComponent<Image>().sprite = (practiceStars >= 3.0f) ? gold : grey;

            SetPointsAmount();
        }
    }

    public void OnStartButtonClick()
    {
        PlayerPrefsManager.AddOneToPlaysNumber();

        if (manager.practiceMode)
        {
            PlayerPrefsManager.AddOneToPracticePlays(manager.currentSceneVisualName);
        }
        else
        {
            PlayerPrefsManager.AddOneToTestPlays(manager.currentSceneVisualName);
        }

        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void GetSceneDatabaseInfo_Success(string[] info)
    {
        if (info.Length > 1)
        {
            int iTime;
            string time = (int.TryParse(info[2], out iTime)) ? string.Format("Tijd: {0}m{1:00}s", iTime / 60, iTime % 60) : "";
            string text = " Score: " + info[1] + "  - " + time;
            sceneInfoPanel.Find("Result").GetComponent<Text>().text = text;
        }
        else
        {
            sceneInfoPanel.Find("Result").GetComponent<Text>().text = " Niet voltooid";
        }
    }

    public void UpdateHighScore()
    {
        manager.GetSceneDatabaseInfo(sceneName, GetSceneDatabaseInfo_Success);
    }

    public void SetPointsAmount()
    {
        Text points = GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/DialogTestPractice/Panel_UI/PointsAmount").transform.GetChild(0).GetComponent<Text>();
        points.text = validated ? "Te behalen accreditatiepunten: " + totalPoints : "";
    }
}
