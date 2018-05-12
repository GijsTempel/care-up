using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    private static LoadingScreen loadingScreen;

    public string bundleName;
    public string sceneName;
    public bool multiple;
    public Sprite image;

    private static Transform sceneInfoPanel;
    private static PlayerPrefsManager manager;

    private static Transform leaderboard;
    private static Transform scores;
    private static Transform names;

    private void Start()
    {
        if (GameObject.Find("Preferences") != null && loadingScreen == null)
        {
            loadingScreen = GameObject.Find("Preferences").GetComponent<LoadingScreen>();
            if (loadingScreen == null) Debug.LogError("No loading screen found");
        }

        if (sceneInfoPanel == null)
        {
            sceneInfoPanel = GameObject.Find("SceneInfo").transform.GetChild(0);
            if (sceneInfoPanel == null) Debug.LogError("No sceneInfo panel");
        }

        if (manager == null)
        {
            manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
            if (manager == null) Debug.LogError("No prefs manager ( start from 1st scene? )");
        }

        if (leaderboard == null)
        {
            leaderboard = sceneInfoPanel.Find("LeaderBoard");
            if (leaderboard == null)
            {
                Debug.LogError("No leaderboard panel");
            }
            else
            {
                scores = leaderboard.Find("Scores");
                names = leaderboard.Find("Player_names");
            }
        }
        
    }

    public void OnLevelButtonClick()
    {
        if (image != null)
        {
            sceneInfoPanel.Find("Image").GetComponent<Image>().sprite = image;
        }

        sceneInfoPanel.Find("Start").GetComponent<LevelButton>().bundleName = bundleName;
        sceneInfoPanel.Find("Start").GetComponent<LevelButton>().sceneName = sceneName;
        sceneInfoPanel.Find("Name").GetComponent<Text>().text =
            manager.currentSceneVisualName =
            transform.Find("Name").GetComponent<Text>().text;
        sceneInfoPanel.Find("Description").GetComponent<Text>().text = 
            transform.Find("Description").GetComponent<Text>().text;

        UpdateHighScore();

        for (int i = 1; i <= 3; ++i)
        {
            string name = "Option_" + i;
            LevelSelectionScene_UI_Option to =
                sceneInfoPanel.Find(name).GetComponent<LevelSelectionScene_UI_Option>();
            LevelSelectionScene_UI_Option from =
                transform.Find(name).GetComponent<LevelSelectionScene_UI_Option>();

            if (multiple) {
                to.gameObject.SetActive(true);
            } else {
                to.gameObject.SetActive(false);
            }
            to.transform.Find("Text").GetComponent<Text>().text =
                    from.transform.Find("Text").GetComponent<Text>().text;

            to.sceneName = from.sceneName;
            to.bundleName = from.bundleName;
            to.description = from.description;
            to.result = from.result;
            to.image = from.image;

            if (i == 1 && multiple)
                to.SetSelected();
        }

        // leaderboard stuff, yay
        UpdateLeaderBoard();
    }

    public void OnStartButtonClick()
    {
        //loadingScreen.LoadLevel(sceneName);
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void GetSceneLeaders_Success(string[] info)
    {
        for (int i = 0; i < info.Length / 3; ++i)
        {
            string name  = info[i * 3];
            string score = info[i * 3 + 1];
            string time  = info[i * 3 + 2];

            names.Find("PlayerName_" + (i + 1)).GetComponent<Text>().text = name;
            scores.Find("Score_" + (i + 1)).GetComponent<Text>().text = score;
        }
    }

    public void UpdateLeaderBoard()
    {
        // let's clear current UI first, it might have some editor text or info from other scene we loaded before
        foreach (Text t in scores.GetComponentsInChildren<Text>())
        {
            t.text = "";
        }
        foreach (Text t in names.GetComponentsInChildren<Text>())
        {
            t.text = "";
        }

        // maybe launch loading icon or something? it isnt instant
        manager.GetSceneLeaders(sceneName, 5, GetSceneLeaders_Success);
    }

    public void GetSceneDatabaseInfo_Success(string[] info)
    {
        if (info.Length > 1)
        {
            int iTime;
            string time = (int.TryParse(info[2], out iTime)) ? string.Format("Tijd: {0}m{1:00}s", iTime / 60, iTime % 60) : "";
            string text = " " + info[1] + " Score: - " + time;
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
}
