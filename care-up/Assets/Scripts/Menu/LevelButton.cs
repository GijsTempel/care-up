﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private static LoadingScreen loadingScreen;

    public string bundleName;
    public string sceneName;
    public string inHouseBundleName = "";
    public string inHouseSceneName = "";
    public bool toLoadInhouse = false;
    public string url;
    public GameObject IsFreeIcon;
    bool demoMark = false;
    bool PreviewIconChanged = false;

    public bool multiple;
    public string displayName;
    public Sprite image;
    public bool testDisabled;
    public bool validated;
    public string totalPoints;
    bool started = false;
    public string[] isInProducts = new string[0];
    Image LevelPreview;
    private static Transform sceneInfoPanel = default(Transform);
    private static PlayerPrefsManager manager;

    private static Transform leaderboard;
    private static Transform scores;
    private static Transform names;
    public bool demoLock = true;
    public Toggle AutoPlayToggle;
    public Toggle AutoPlayToggle2;
    public Text AutoPlayNum;
    public Text AutoPlayNum2;

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

    //--------------------------
    public void SetLockState(bool _lock)
    {
        demoLock = _lock;
        UpdateButtonLockState();
    }

    public void SetDemoMark(bool _mark)
    {
        demoMark = _mark;
    }

    public void UpdateButtonLockState()
    { 
        if (LevelPreview == null)
            LevelPreview = transform.Find("LevelPreview").GetComponent<Image>();
        bool toLock = demoLock;

        if (demoLock && isInProducts.Length > 0)
            if (PlayerPrefsManager.IsScenePurchased(isInProducts))
                toLock = false;

        if (toLock)
        {
            GetComponent<Image>().sprite = Resources.Load("Sprites/nUI/listElement_Base_gray", typeof(Sprite)) as Sprite;
            LevelPreview.sprite = Resources.Load("Sprites/btn_icon_lock", typeof(Sprite)) as Sprite;
            LevelPreview.gameObject.SetActive(true);
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load("Sprites/nUI/listElement_Base", typeof(Sprite)) as Sprite;
            if (!PreviewIconChanged)
                LevelPreview.gameObject.SetActive(false);
        }
        IsFreeIcon.SetActive(demoMark);
        //if(!demoLock)
        //{
        //    GetComponent<Image>().sprite = Resources.Load("Sprites/nUI/listElement_Base_gray", typeof(Sprite)) as Sprite;
        //    GetComponent<Image>().color = new Color(0f, 0.85f, 0.6f);
        //}
    }

    public void SetLevelPreviewIcon(bool iconToShow, Sprite newIcon)
    {
        if (LevelPreview == null)
            LevelPreview = transform.Find("LevelPreview").GetComponent<Image>();
        LevelPreview.gameObject.SetActive(LevelPreview);
        LevelPreview.sprite = newIcon;
        PreviewIconChanged = true;
    }

    public List<Info> variations = new List<Info>();

    public bool buy = false;

    private void Start()
    {
        LevelPreview = transform.Find("LevelPreview").GetComponent<Image>();
        if (!PlayerPrefsManager.simulatePlayerActions)
        {
            AutoPlayToggle.gameObject.SetActive(false);
            AutoPlayToggle2.gameObject.SetActive(false);

        }
        else
        {
            if(inHouseSceneName == "")
                AutoPlayToggle2.gameObject.SetActive(false);
        }
#if !(UNITY_EDITOR || DEVELOPMENT_BUILD)
        AutoPlayToggle.gameObject.SetActive(false);
        AutoPlayToggle2.gameObject.SetActive(false);

#endif

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
        started = true;
    }

    public void AutoPlayStateChanged(int locationID = 0)
    {
        if (started)
        {
            string _sceneName = sceneName;
            string _bundleName = bundleName;
            bool _toggle = AutoPlayToggle.isOn;

            if (locationID == 1)
            {
                _sceneName = inHouseSceneName;
                _bundleName = inHouseBundleName;
                _toggle = AutoPlayToggle2.isOn;
            }
            GameObject.FindObjectOfType<AutoPlayer>().AddSceneToList(_sceneName, _bundleName, _toggle);
            foreach(LevelButton levelButton in GameObject.FindObjectsOfType<LevelButton>())
            {
                levelButton.UpdateAutoPlayToggle();
            }
        }
    }

    public void OnHover()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD 
        GameObject.FindObjectOfType<LevelSelectionScene_UI>().debugSS = sceneName;
#endif
    }


    public void OnExit()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD 
        GameObject.FindObjectOfType<LevelSelectionScene_UI>().debugSS = "";
#endif
    }

    public void UpdateAutoPlayToggle()
    {
        AutoPlayNum.text = "";
        AutoPlayNum2.text = "";

        if (!PlayerPrefsManager.simulatePlayerActions)
        {
            AutoPlayToggle.gameObject.SetActive(false);
            AutoPlayToggle2.gameObject.SetActive(false);
        }
        else
        {
            AutoPlayToggle.gameObject.SetActive(true);
            if (inHouseSceneName != "")
                AutoPlayToggle2.gameObject.SetActive(true);

            int autoPlayNumValue = GameObject.FindObjectOfType<AutoPlayer>().IsSceneInList(sceneName);
            int autoPlayNumValue2 = GameObject.FindObjectOfType<AutoPlayer>().IsSceneInList(inHouseSceneName);

            AutoPlayToggle.isOn = autoPlayNumValue != -1;
            AutoPlayToggle2.isOn = autoPlayNumValue2 != -1;
            if (autoPlayNumValue >= 0)
                AutoPlayNum.text = (autoPlayNumValue + 1).ToString();
            if (autoPlayNumValue2 >= 0)
                AutoPlayNum2.text = (autoPlayNumValue2 + 1).ToString();
        }
    }

    void OnEnable()
    {
        UpdateAutoPlayToggle();
    }

    public void OnLevelButtonClick()
    {
        MainMenu.storeUrl = url;

        bool locked = (buy || demoLock);
        if (locked && isInProducts.Length > 0)
            if (PlayerPrefsManager.IsScenePurchased(isInProducts))
                locked = false;
        if (locked)
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

                mainBtn.inHouseBundleName = inHouseBundleName;
                mainBtn.inHouseSceneName = inHouseSceneName;
                mainBtn.toLoadInhouse = false;

                // for single variation we can skip into practice/test dialogue
                if (inHouseSceneName != "")
                    GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(11);
                else
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
            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/DialogTestPractice/Panel_UI/Buttons/right/TestButton")
                .SetActive(!testDisabled);

            // now we can fetch practice plays number in order to know whethere to hide or show test button
            // making test button inactive from the beginning before fetching
            Button testBtn = GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
            "DialogTestPractice/Panel_UI/Buttons/right/TestButton").GetComponent<Button>();

            string formattedSceneName = PlayerPrefsManager.FormatSceneName(manager.currentSceneVisualName);

            int practicePlays;
            int.TryParse(DatabaseManager.FetchField("PracticePlays", formattedSceneName), out practicePlays);

            testBtn.interactable = practicePlays >= 1;

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/right/TestButton/contentlocked/practiceamount")
                .GetComponent<Text>().text = (1 - practicePlays).ToString() + " keer";

            if (testBtn.interactable)
            {
                float testHighscore;
                float.TryParse(DatabaseManager.FetchField("TestHighscores",
                    formattedSceneName).Replace(",", "."), out testHighscore);
                GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/right/TestButton/contentunlocked/percentage")
                    .GetComponent<Text>().text = Mathf.RoundToInt(testHighscore).ToString() + "%";
            }

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                "DialogTestPractice/Panel_UI/Buttons/right/TestButton/contentunlocked").SetActive(testBtn.interactable);
            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                "DialogTestPractice/Panel_UI/Buttons/right/TestButton/contentlocked").SetActive(!testBtn.interactable);

            int practiceHighscore, practiceStars;
            int.TryParse(DatabaseManager.FetchField("PracticeHighscores", "score_" + formattedSceneName), out practiceHighscore);
            int.TryParse(DatabaseManager.FetchField("PracticeHighscores", "stars_" + formattedSceneName), out practiceStars);

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                "DialogTestPractice/Panel_UI/Buttons/left/PracticeButton/content/score").
            GetComponent<Text>().text = practiceHighscore.ToString();

            Sprite grey = Resources.Load<Sprite>("Sprites/Stars/star 1");
            Sprite gold = Resources.Load<Sprite>("Sprites/Stars/star_128x128px");

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/left/PracticeButton/content/Stars/Star1")
                .GetComponent<Image>().sprite = (practiceStars >= 1.0f) ? gold : grey;

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/left/PracticeButton/content/Stars/Star2")
                .GetComponent<Image>().sprite = (practiceStars >= 2.0f) ? gold : grey;

            GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/" +
                    "DialogTestPractice/Panel_UI/Buttons/left/PracticeButton/content/Stars/Star3")
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
        if (toLoadInhouse)
            bl_SceneLoaderUtils.GetLoader.LoadLevel(inHouseSceneName, inHouseBundleName);
        else
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
