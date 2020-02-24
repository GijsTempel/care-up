using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using MBS;
using CareUpAvatar;

/// <summary>
/// Handles Scene selection module
/// </summary>
public class LevelSelectionScene_UI : MonoBehaviour
{
    public string debugSS;
    private PlayerPrefsManager ppManager;

    // leaderboard stuff
    public ScoreLine[] _Scores;
    public GameObject scoreLines;

    public List<Transform> variations;

    private Sprite completedSceneIcon;

    private void Awake()
    {
        Transform leaderPanel = GameObject.Find("UMenuProManager/MenuCanvas/LayoutPanel/Tabs/Leaderboard").transform;
        _Scores = scoreLines.GetComponentsInChildren<ScoreLine>();

        for (int i = 0; i < _Scores.Length; i++)
        {
            _Scores[i].SetScoreLine("", "", i);
        }

        // variations buttons should be disabled from the beginning
        //for (int i = 0; i < 3; ++i)
        //{
        //    Transform v = leaderPanel.parent.Find("InfoBar/menu/d" + (i + 1));
        //    variations.Add(v);
        //    v.gameObject.SetActive(false);
        //}
    }

    /// <summary>
    /// Load xml file, set all door variables.
    /// </summary>
    void Start()
    {
        ppManager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        completedSceneIcon = Resources.Load<Sprite>("Sprites/check_circle_on");

        UpdateSceneUI();
    }

    void ClearUI()
    {
        Transform parent1 = GameObject.Find("UMenuProManager/MenuCanvas/Play/InfoHolder/ProtocolList/ProtocolsHolder/Protocols/content").transform;
        for (int i = 0; i < parent1.childCount; ++i)
        {
            Destroy(parent1.GetChild(i).gameObject);
        }

        Transform parent2 = GameObject.Find("UMenuProManager/MenuCanvas/Leaderboard/InfoHolder/ProtocolsHolder/Scroll View/Viewport/Content").transform;
        for (int i = 0; i < parent1.childCount; ++i)
        {
            Destroy(parent2.GetChild(i).gameObject);
        }
    }

    void OnGUI()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD 
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(0.3f, 1f, 0.3f);
        style.fontSize = 20;
        GUI.Label(new Rect(30, 10, 1000, 100), debugSS, style);
#endif
    }

    public void ReinitializeUI()
    {      
        ClearUI();
        UpdateSceneUI();           
    }

    void UpdateSceneUI()
    {
       
        // load xml
        TextAsset textAsset;

        PlayerPrefsManager pp = GameObject.FindObjectOfType<PlayerPrefsManager>();
        // if (pp != null && pp.demoVersion)
        // {
        //     textAsset = (TextAsset)Resources.Load("Xml/Scenes_Demo");
        // }
        // else
        // {
        //     textAsset = (TextAsset)Resources.Load("Xml/Scenes");
        // }

        textAsset = (TextAsset)Resources.Load("Xml/Scenes");

        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlSceneList = xmlFile.FirstChild.NextSibling.FirstChild.ChildNodes;

        // leaderboard stuff
        bool firstScene = true;
        LeaderBoardSceneButton.buttons.Clear();
// FindObjectOfType<PlayerPrefsManager>().demoVersion

        Transform protocolsTransorm = GameObject.Find("UMenuProManager/MenuCanvas/LayoutPanel/Tabs/Play/ContentPanel/PlayElements/ProtocolPanel/Panel/ProtocolList/ProtocolsHolder/Protocols/content").transform;
        foreach (XmlNode xmlSceneNode in xmlSceneList)
        {
            // bool activated = PlayerPrefs.GetInt(xmlSceneNode.Attributes["id"].Value + " activated") == 1;
            bool activated = true;
            bool hidden = xmlSceneNode.Attributes["hidden"] != null;
            bool demoLock = !(xmlSceneNode.Attributes["demo"] != null);

            if ((!activated && hidden) || hidden)
            {
                // not activated and hidden scene should not even create a panel, so just end up here
                continue;
            }

            // if we're here, then we have real scene, that is not hidden
            // instantiating panel
            GameObject sceneUnitObject = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/SceneSelectionUnit"), protocolsTransorm);
            sceneUnitObject.name = "SceneSelectionUnit"; // i dont like that 'clone' word at the end, ugh
            LevelButton sceneUnit = sceneUnitObject.GetComponent<LevelButton>();

            if (!activated && !hidden)
            {
                // but if scene is not activated and NOT hidden either
                // just disable play button, but show the panel to the player
                //sceneUnit.transform.Find("BottonBar/Play").GetChild(0).GetComponent<Text>().text = "Bestellen";
                sceneUnit.buy = true;
                // grey out ?
                ColorBlock colorBlock = sceneUnit.GetComponent<Button>().colors;
                colorBlock.normalColor = Color.grey;
                colorBlock.highlightedColor = Color.grey;
                sceneUnit.GetComponent<Button>().colors = colorBlock;
            }

            // now let's fill some actual info about the scene
            if (xmlSceneNode.Attributes["isInProducts"] != null)
            {
                sceneUnit.isInProducts = xmlSceneNode.Attributes["isInProducts"].Value.Split('|');
            }
            if (xmlSceneNode.Attributes["multiple"] != null)
            {
                sceneUnit.multiple = true;

                // so we're setting only scene title and picture 
                // saving everything else for dialogues

                // setting scene title
                sceneUnit.transform.Find("Title").GetComponent<Text>().text
                    = sceneUnit.displayName = xmlSceneNode.Attributes["name"].Value;

                ppManager.currentSceneVisualName = sceneUnit.displayName;
                //ppManager.UpdateTestHighscore(0.0f);
                ppManager.validatedScene = sceneUnit.validated;

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
                    info.validated = variation.Attributes["validated"] != null ?
                         variation.Attributes["validated"].Value == "true" : false;
                    info.totalPoints = variation.Attributes["totalPoints"].Value;

                    sceneUnit.variations.Add(info);

                    if (i == 0)
                    {
                        // set the image as main if this is 1st variation
                        sceneUnit.image = sceneUnit.variations[i].image;

                        sceneUnit.validated = sceneUnit.variations[i].validated;                      
                        sceneUnit.transform.Find("Validation").GetComponent<Text>().text =
                            sceneUnit.validated ? "Geaccrediteerd" : "";

                        sceneUnit.totalPoints = sceneUnit.variations[i].totalPoints;

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
                sceneUnit.transform.Find("Title").GetComponent<Text>().text
                    = sceneUnit.displayName = xmlSceneNode.Attributes["name"].Value;

                ppManager.currentSceneVisualName = sceneUnit.displayName;
                ppManager.CreateBlankHighscore(); // has a check inside for no DB info already

                // setting description
                if (xmlSceneNode.Attributes["description"].Value != "")
                {
                    // no description atm
                    //  = scene.Attributes["description"].Value;
                }

                /* setting image
                if (xmlSceneNode.Attributes["image"] != null)
                {
                    sceneUnit.image = Resources.Load<Sprite>("Sprites/ScenePreview/" + xmlSceneNode.Attributes["image"].Value);
                    sceneUnit.transform.Find("LevelPreview").GetComponent<Image>().sprite = sceneUnit.image;
                } */

                // override image if scene is completed
                float fscore = 0.0f;
                float.TryParse(DatabaseManager.FetchField("TestHighscores",
                    PlayerPrefsManager.FormatSceneName(sceneUnit.displayName)).Replace(",", "."), out fscore);
                if (Mathf.FloorToInt(fscore) >= 70)
                {
                    sceneUnit.image = completedSceneIcon;
                    //sceneUnit.transform.Find("LevelPreview").gameObject.SetActive(true);
                    sceneUnit.transform.Find("LevelPreview").GetComponent<Image>().sprite = sceneUnit.image;
                }

                if (xmlSceneNode.Attributes["validated"] != null)
                {
                    sceneUnit.validated = xmlSceneNode.Attributes["validated"].Value == "true";
                    sceneUnit.transform.Find("Validation").GetComponent<Text>().text =
                        sceneUnit.validated ? "Geaccrediteerd" : "";
                }
                
                if (xmlSceneNode.Attributes["totalPoints"] != null)
                {
                    sceneUnit.totalPoints = xmlSceneNode.Attributes["totalPoints"].Value;
                }
            }

            // leaderboard stuff
            if (pp.subscribed)
                sceneUnit.SetLockState(false);
            else
                sceneUnit.SetLockState(demoLock);
            GameObject button = Instantiate<GameObject>(Resources.Load<GameObject>("NecessaryPrefabs/UI/LeaderBoardSceneButton"),
                GameObject.Find("/UMenuProManager/MenuCanvas/LayoutPanel/Tabs/Leaderboard/ContentPanel/Scenes/ProtocolPanel/Panel/ProtocolList/ProtocolsHolder/Protocols/content").transform);
            LeaderBoardSceneButton buttonInfo = button.GetComponent<LeaderBoardSceneButton>();
            button.transform.Find("Text").GetComponent<Text>().text = sceneUnit.displayName;

            buttonInfo.sceneName = sceneUnit.sceneName;
            buttonInfo.multiple = sceneUnit.multiple;


            if (buttonInfo.multiple)
            {
                foreach (LevelButton.Info v in sceneUnit.variations)
                {
                    buttonInfo.sceneNames.Add(v.sceneName);
                    buttonInfo.buttonNames.Add(v.displayName);
                }
            }


            if (firstScene)
            {
                firstScene = false;
                buttonInfo.OnMainButtonClick();
            }

            sceneUnit.testDisabled = (xmlSceneNode.Attributes["test"] != null
                && xmlSceneNode.Attributes["test"].Value == "disabled");
            sceneUnit.UpdateAutoPlayToggle();
            //---------------------
        }
        
        ScrollRect levelScroll =  GameObject.Find("/UMenuProManager/MenuCanvas/LayoutPanel/Tabs/Play/ContentPanel/PlayElements/ProtocolPanel/Panel/ProtocolList/ProtocolsHolder").GetComponent<ScrollRect>();
        
        levelScroll.verticalNormalizedPosition = ppManager.LevelScrollPosition;
    }


    public void LevelScrollChanged()
    {
        ScrollRect levelScroll =  GameObject.Find("/UMenuProManager/MenuCanvas/LayoutPanel/Tabs/Play/ContentPanel/PlayElements/ProtocolPanel/Panel/ProtocolList/ProtocolsHolder").GetComponent<ScrollRect>();
       
        ppManager.LevelScrollPosition = levelScroll.verticalNormalizedPosition;
    }

    public void UpdateLeaderBoard(string sceneName)
    {
        //Debug.Log("UpdateLeaderBoard:::" + sceneName);
        // let's clear current UI first, it might have some editor text or info from other scene we loaded before
        for (int i = 0; i < _Scores.Length; i++)
        {
            _Scores[i].SetScoreLine("", "", i);
        }

        // maybe launch loading icon or something? it isnt instant
        //ppManager.GetSceneLeaders(sceneName, 10, GetSceneLeaders_Success);

        MBS.WUScoring.onFetched = GetSceneLeaders_Success;

        // hashes are NOT a clean solution
        int hash = Mathf.Abs(sceneName.GetHashCode());
        MBS.WUScoring.FetchScores(0, hash);
    }

    public void GetSceneLeaders_Success(MBS.CML cml)
    {
        List<MBS.CMLData> entries = cml.AllNodesOfType("person");
        List<MBS.CMLData> sortedEntries = entries.OrderByDescending(x => int.Parse(x.String("score"))).ToList();

        LeaderBoard lb = GameObject.FindObjectOfType<LeaderBoard>();
        if (entries == null) return;

        for (int i = 0; i < 10; ++i)
        {
            string name = sortedEntries[i].String("dname");
            string score = sortedEntries[i].String("score");
            string uid = sortedEntries[i].String("uid");

            if (i < _Scores.Length)
                _Scores[i].SetScoreLine(name, score, i, uid);
        }

        // loading icon is shown
        if (lb.leftBar.activeSelf)
            lb.infoBar.SetActive(false);
        else
        {
            
            GameObject.FindObjectOfType<UMP_Manager>().LeaderBoardSearchBar.gameObject.SetActive(false);
            lb.top.SetActive(false);
            lb.backButton.GetComponent<Button>().interactable = true;
            lb.topDescription.SetActive(false);
            lb.infoBar.SetActive(true);
            lb.description.GetComponent<Text>().text = LeaderBoardSceneButton.Descripton;
            lb.leaderboard.SetActive(false);
        }       
    }

    public void RequestCharacterInfoByUID(int uid)
    {
        // start loading animation?
        // actual load stuff
        WUData.FetchUserCategory(uid, "AccountStats", RequestCharacterInfoByUID_success);
    }

public void RequestCharacterInfoByUID_success(CML response)
    {
        //loading done, stop loading animation, open UI
        string sex = response.Elements[1]["Sex"];
        string head = response.Elements[1]["Head"];
        string body = response.Elements[1]["Body"];
        string glasses = response.Elements[1]["Glasses"];
        string hat = response.Elements[1]["Hat"]; // didnt find info
        bool toShowPlayer = false;
        if (!string.IsNullOrEmpty(head))
        {
            PlayerAvatar mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();
            PlayerAvatarData prevCharData = new PlayerAvatarData();
            if (sex == "Female")
                prevCharData.gender = Gender.Female;
            int.TryParse(head, out prevCharData.headType);
            if (prevCharData.headType < mainAvatar.GetMaxHeadNum(prevCharData.gender))
            {
                int.TryParse(body, out prevCharData.bodyType);
               
                int glassesType = -1;
                int.TryParse(glasses, out glassesType);
                if (glassesType < 3000000)
                    glassesType += 3000000;
                prevCharData.glassesType = glassesType;
                prevCharData.hat = hat;
                mainAvatar.avatarData = prevCharData;
                mainAvatar.UpdateCharacter();
                toShowPlayer = true;
            }
            else
                Debug.Log("No correct head");
        }
        else
            Debug.Log("No avatar data");
        GameObject.FindObjectOfType<HighscoreCharacterPanel>().HideContent(false, toShowPlayer);
    }
}
