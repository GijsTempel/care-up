using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// Handles Scene selection module
/// </summary>
public class LevelSelectionScene_UI : MonoBehaviour
{
    private PlayerPrefsManager ppManager;

    // leaderboard stuff
    private Text[] leaderNames;
    private Text[] leaderScores;
    private Text[] leaderTimes;
    public List<Transform> variations;

    private void Awake()
    {
        Transform leaderPanel = GameObject.Find("UMenuProManager/MenuCanvas/Leaderboard/InfoBar").transform;

        // set up all the Text objects for leaderboard
        leaderNames = leaderPanel.Find("LeaderNames").GetComponentsInChildren<Text>();
        leaderScores = leaderPanel.Find("LeaderScores").GetComponentsInChildren<Text>();
        leaderTimes = leaderPanel.Find("LeaderTimes").GetComponentsInChildren<Text>();

        // clear them
        for (int i = 0; i < 10; ++i)
        {
            leaderNames[i].text = "";
            leaderScores[i].text = "";
            leaderTimes[i].text = "";
        }

        // variations buttons should be disabled from the beginning
        for (int i = 0; i < 3; ++i)
        {
            Transform v = leaderPanel.parent.Find("SceneVariation" + (i+1));
            variations.Add(v);
            v.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Load xml file, set all door variables.
    /// </summary>
    void Start()
    {
        ppManager = GameObject.FindObjectOfType<PlayerPrefsManager>();

        // load xml
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Scenes");
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlSceneList = xmlFile.FirstChild.NextSibling.FirstChild.ChildNodes;
        
        foreach (XmlNode xmlSceneNode in xmlSceneList)
        {
            bool activated = PlayerPrefs.GetInt(xmlSceneNode.Attributes["id"].Value + " activated") == 1;
            bool hidden = xmlSceneNode.Attributes["hidden"] != null;
            if (!activated && hidden)
            {
                // not activated and hidden scene should not even create a panel, so just end up here
                continue;
            }

            // if we're here, then we have real scene, that is not hidden
            // instantiating panel
            GameObject sceneUnitObject = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneSelectionUnit"),
                GameObject.Find("UMenuProManager/MenuCanvas/Play/LevelPanel/ViewPoint/List").transform);
            sceneUnitObject.name = "SceneSelectionUnit"; // i dont like that 'clone' word at the end, ugh
            LevelButton sceneUnit = sceneUnitObject.GetComponent<LevelButton>();
            
            if (!activated && !hidden)
            {
                // but if scene is not activated and NOT hidden either
                // just disable play button, but show the panel to the player
                sceneUnit.transform.Find("BottomBar/Play").GetComponent<Button>().interactable = false;
            }

            // now let's fill some actual info about the scene
            if (xmlSceneNode.Attributes["multiple"] != null)
            {
                sceneUnit.multiple = true;

                // so we're setting only scene title and picture 
                // saving everything else for dialogues

                // setting scene title
                sceneUnit.transform.Find("Degradado/Title").GetComponent<Text>().text
                    = sceneUnit.displayName = xmlSceneNode.Attributes["name"].Value;

                // saving bundle name for later
                string bundleName = sceneUnit.bundleName = xmlSceneNode.Attributes["bundleName"].Value;

                int i = 0;
                foreach (XmlNode variation in xmlSceneNode.ChildNodes)
                {
                    LevelButton.Info info = new LevelButton.Info();
                    // saving all the info for scene variation, use in dialogue
                    info.bundleName = bundleName;
                    info.sceneName = variation.Attributes["name"].Value;
                    info.displayName = variation.Attributes["displayname"].Value;
                    info.description = variation.Attributes["description"].Value;
                    info.image = Resources.Load<Sprite>("Sprites/ScenePreview/" + variation.Attributes["image"].Value);

                    sceneUnit.variations.Add(info);

                    if (i == 0)
                    {
                        // set the image as main if this is 1st variation
                        sceneUnit.image = sceneUnit.variations[i].image;
                        sceneUnit.transform.Find("LevelPreview").GetComponent<Image>().sprite = sceneUnit.image;

                        // also make 1st option 'selected'
                        sceneUnit.sceneName = sceneUnit.variations[i].sceneName;
                    }

                    // setting max 3 for now, dont see that UI supports more
                    if (++i > 2)
                        break;
                }
            }
            else
            {
                // saving info for loading
                sceneUnit.multiple = false;
                sceneUnit.sceneName = xmlSceneNode.Attributes["sceneName"].Value;
                sceneUnit.bundleName = xmlSceneNode.Attributes["bundleName"].Value;

                // setting scene title
                sceneUnit.transform.Find("Degradado/Title").GetComponent<Text>().text
                    = sceneUnit.displayName = xmlSceneNode.Attributes["name"].Value;

                // setting description
                if (xmlSceneNode.Attributes["description"].Value != "")
                {
                    // no description atm
                    //  = scene.Attributes["description"].Value;
                }

                // setting image
                if (xmlSceneNode.Attributes["image"] != null)
                {
                    sceneUnit.image = Resources.Load<Sprite>("Sprites/ScenePreview/" + xmlSceneNode.Attributes["image"].Value);
                    sceneUnit.transform.Find("LevelPreview").GetComponent<Image>().sprite = sceneUnit.image;
                }
            }

            // leaderboard stuff
            GameObject button = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/LeaderBoardSceneButton"),
                GameObject.Find("UMenuProManager/MenuCanvas/Leaderboard/LeftBar/Scroll View/Viewport/Content").transform);
            LeaderBoardSceneButton buttonInfo = button.GetComponent<LeaderBoardSceneButton>();

            button.transform.GetChild(0).GetComponent<Text>().text = sceneUnit.displayName;
            buttonInfo.sceneName = sceneUnit.sceneName;
            buttonInfo.multiple = sceneUnit.multiple;
            if (buttonInfo.multiple)
            {
                foreach(LevelButton.Info v in sceneUnit.variations)
                {
                    buttonInfo.sceneNames.Add(v.sceneName);
                    buttonInfo.buttonNames.Add(v.displayName);
                }
            }
        }
    }
    
    public void UpdateLeaderBoard(string sceneName)
    {
        Debug.Log("UpdateLeaderBoard:::" + sceneName);
        // let's clear current UI first, it might have some editor text or info from other scene we loaded before
        for (int i = 0; i < leaderNames.Length; ++i )
        {
            leaderNames[i].text =
            leaderScores[i].text =
            leaderTimes[i].text = "";
        }

        // maybe launch loading icon or something? it isnt instant
        ppManager.GetSceneLeaders(sceneName, 10, GetSceneLeaders_Success);
    }

    public void GetSceneLeaders_Success(string[] info)
    {
        for (int i = 0; i < info.Length / 3; ++i)
        {
            string name = info[i * 3];
            string score = info[i * 3 + 1];
            string time = info[i * 3 + 2];

            TimeSpan timeSpan = TimeSpan.FromSeconds(double.Parse(time));
            string timeFormated = string.Format("{0:D2}m:{1:D2}s",
                timeSpan.Minutes, timeSpan.Seconds);

            leaderNames[i].text = name;
            leaderScores[i].text = score;
            leaderTimes[i].text = timeFormated;
        }
    }
}
