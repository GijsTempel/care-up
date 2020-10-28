using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using MBS;
using CareUpAvatar;


public class SceleInfo
{
    public string sceneType = "";
    public bool activated = true;
    public bool demoLock = false;
    public bool hidden = false;
    public string[] isInProducts = null;
    public string bundleName = "";
    public string displayName = "";
    public string description = "";
    public string sceneName = "";
    public string image = "";
    public bool validated = false;
    public string totalPoints = "0";
    public string mainScene = "";
    public bool multiple = false;
    public bool testDisabled = false;
    public string inHouseBundleName = "";
    public string inHouseSceneName = "";
}

/// <summary>
/// Handles Scene selection module
/// </summary>
/// 
public class LevelSelectionScene_UI : MonoBehaviour
{
    public string debugSS;
    private PlayerPrefsManager ppManager;
    float initTime = 0f;
    bool sceneButtonsUpdated = false;

    // leaderboard stuff
    public ScoreLine[] _Scores;
    public GameObject scoreLines;

    public List<Transform> variations;

    private Sprite completedSceneIcon;

    public void UpdateScenePurchases()
    {
        WPServer.RequestPurchases(WULogin.UID);
    }

    public void RefrashSceneSelectionButtons()
    {
        foreach(LevelButton levelButton in GameObject.FindObjectsOfType<LevelButton>())
        {
            levelButton.UpdateButtonLockState();
        }
    }

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
        initTime = Time.realtimeSinceStartup;
        ppManager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        completedSceneIcon = Resources.Load<Sprite>("Sprites/check_circle_on");

        UpdateSceneUI();
    }

    private void Update()
    {
        if (!sceneButtonsUpdated)
        {
            if (Time.realtimeSinceStartup - initTime > 5f)
            {
                sceneButtonsUpdated = true;
                RefrashSceneSelectionButtons();
                Debug.Log("Updated menu");
            }
        }
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

        textAsset = (TextAsset)Resources.Load("Xml/Scenes");

        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlSceneList = xmlFile.FirstChild.NextSibling.FirstChild.ChildNodes;

        // leaderboard stuff
        bool firstScene = true;
        LeaderBoardSceneButton.buttons.Clear();
// FindObjectOfType<PlayerPrefsManager>().demoVersion

        Transform protocolsTransorm = GameObject.Find("UMenuProManager/MenuCanvas/LayoutPanel/Tabs/Play/ContentPanel/PlayElements/ProtocolPanel/Panel/ProtocolList/ProtocolsHolder/Protocols/content").transform;

        Dictionary<string, SceleInfo> scenesInfo = new Dictionary<string, SceleInfo>();


        //Load data for scenes
        foreach (XmlNode xmlSceneNode in xmlSceneList)
        {
            SceleInfo sceneInfo = new SceleInfo();
            // bool activated = PlayerPrefs.GetInt(xmlSceneNode.Attributes["id"].Value + " activated") == 1;
            sceneInfo.activated = true;
            if (xmlSceneNode.Attributes["type"] != null)
                sceneInfo.sceneType = xmlSceneNode.Attributes["type"].Value;

            if (xmlSceneNode.Attributes["hidden"] != null)
                sceneInfo.hidden = xmlSceneNode.Attributes["hidden"].Value == "true";

            sceneInfo.demoLock = !(xmlSceneNode.Attributes["demo"] != null);

            if (xmlSceneNode.Attributes["isInProducts"] != null)
            {
                sceneInfo.isInProducts = xmlSceneNode.Attributes["isInProducts"].Value.Split('|');
            }

            sceneInfo.sceneName = xmlSceneNode.Attributes["sceneName"].Value;

            sceneInfo.bundleName = xmlSceneNode.Attributes["bundleName"].Value;
            if (xmlSceneNode.Attributes["name"] != null)
                sceneInfo.displayName = xmlSceneNode.Attributes["name"].Value;

            if (xmlSceneNode.Attributes["description"] != null)
                sceneInfo.description = xmlSceneNode.Attributes["description"].Value;

            /* setting image
            if (xmlSceneNode.Attributes["image"] != null)
            {
                sceneUnit.image = Resources.Load<Sprite>("Sprites/ScenePreview/" + xmlSceneNode.Attributes["image"].Value);
                sceneUnit.transform.Find("LevelPreview").GetComponent<Image>().sprite = sceneUnit.image;
            } */

            if (xmlSceneNode.Attributes["validated"] != null)
                sceneInfo.validated = xmlSceneNode.Attributes["validated"].Value == "true";
            if (xmlSceneNode.Attributes["totalPoints"] != null)
                sceneInfo.totalPoints = xmlSceneNode.Attributes["totalPoints"].Value;

            if (xmlSceneNode.Attributes["test"] != null)
                sceneInfo.testDisabled = xmlSceneNode.Attributes["test"].Value == "disabled";

            if (xmlSceneNode.Attributes["mainScene"] != null)
                sceneInfo.mainScene = xmlSceneNode.Attributes["mainScene"].Value;
            string sceneType = "";
            if (xmlSceneNode.Attributes["type"] != null)
                sceneType = xmlSceneNode.Attributes["type"].Value;


            if (!scenesInfo.ContainsKey(sceneInfo.sceneName) && sceneInfo.mainScene == "")
                scenesInfo.Add(sceneInfo.sceneName, sceneInfo);

            if (sceneInfo.mainScene != "" && sceneType != "")
            {
                if(scenesInfo.ContainsKey(sceneInfo.mainScene))
                {
                    if (sceneType == "in_house")
                    {
                        scenesInfo[sceneInfo.mainScene].inHouseBundleName = sceneInfo.bundleName;
                        scenesInfo[sceneInfo.mainScene].inHouseSceneName = sceneInfo.sceneName;
                    }
                }
            }
        }


        //Creation of menu elements from loaded data

        Dictionary<string, SceleInfo> scenesInfoUnlocked = new Dictionary<string, SceleInfo>();
        Dictionary<string, SceleInfo> scenesInfoLocked = new Dictionary<string, SceleInfo>();
        Dictionary<string, SceleInfo> scenesInfoDemo = new Dictionary<string, SceleInfo>();

        foreach (string key in scenesInfo.Keys)
        {
            SceleInfo sceneInfo = scenesInfo[key];

            if ((!sceneInfo.activated && sceneInfo.hidden) || sceneInfo.hidden)
            {
                // not activated and hidden scene should not even create a panel, so just end up here
                continue;
            }

            if (!sceneInfo.demoLock)
            {
                scenesInfoDemo.Add(key, scenesInfo[key]);
            }
            else
            {
                scenesInfoLocked.Add(key, scenesInfo[key]);
            }
        }

        Dictionary<string, SceleInfo> scenesInfoSorted = new Dictionary<string, SceleInfo>();
        foreach (string key in scenesInfoDemo.Keys)
            scenesInfoSorted.Add(key, scenesInfoDemo[key]);

        foreach (string key in scenesInfoLocked.Keys)
            scenesInfoSorted.Add(key, scenesInfoLocked[key]);


        foreach (string key in scenesInfoSorted.Keys)
        {
            SceleInfo sceneInfo = scenesInfoSorted[key];
            if ((!sceneInfo.activated && sceneInfo.hidden) || sceneInfo.hidden)
            {
                // not activated and hidden scene should not even create a panel, so just end up here
                continue;
            }
        

            GameObject sceneUnitObject = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/SceneSelectionUnit"), protocolsTransorm);
            sceneUnitObject.name = "SceneSelectionUnit"; // i dont like that 'clone' word at the end, ugh

            LevelButton sceneUnit = sceneUnitObject.GetComponent<LevelButton>();

            sceneUnit.inHouseBundleName = sceneInfo.inHouseBundleName;
            sceneUnit.inHouseSceneName = sceneInfo.inHouseSceneName;

            if (!sceneInfo.activated && !sceneInfo.hidden)
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
            if (sceneInfo.isInProducts != null)
                sceneUnit.isInProducts = sceneInfo.isInProducts;

            sceneUnit.sceneName = sceneInfo.sceneName;
            sceneUnit.bundleName = sceneInfo.bundleName;

            // setting scene title
            sceneUnit.transform.Find("Title").GetComponent<Text>().text
                = sceneUnit.displayName = sceneInfo.displayName;

            ppManager.currentSceneVisualName = sceneUnit.displayName;
            ppManager.CreateBlankHighscore(); // has a check inside for no DB info already

            // override image if scene is completed
            float fscore = 0.0f;
            float.TryParse(DatabaseManager.FetchField("TestHighscores",
                PlayerPrefsManager.FormatSceneName(sceneUnit.displayName)).Replace(",", "."), out fscore);
            if (Mathf.FloorToInt(fscore) >= 70)
            {
                sceneUnit.image = completedSceneIcon;
                sceneUnit.GetComponent<LevelButton>().SetLevelPreviewIcon(true, sceneUnit.image);
                //sceneUnit.transform.Find("LevelPreview").gameObject.SetActive(true);
                //sceneUnit.transform.Find("LevelPreview").GetComponent<Image>().sprite = sceneUnit.image;
            }
            sceneUnit.validated = sceneInfo.validated;
            sceneUnit.transform.Find("Validation").GetComponent<Text>().text =
                sceneUnit.validated ? "Geaccrediteerd" : "";

            sceneUnit.totalPoints = sceneInfo.totalPoints;

            // leaderboard stuff
            if (pp.subscribed)
                sceneUnit.SetLockState(false);
            else
                sceneUnit.SetLockState(sceneInfo.demoLock);

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

            sceneUnit.testDisabled = sceneInfo.testDisabled;
            sceneUnit.UpdateAutoPlayToggle();
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
