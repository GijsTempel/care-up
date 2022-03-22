using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class DialogLevelSelect : MonoBehaviour
{
    private struct LevelInfoDataStruct
    {
        public string name;
        public string icon;
        public string text;
    }

    public LevelButton mainBtn;
    public Button VideoLevelSelectButton;
    public Button ButtonLevel2;
    public Button ButtonLevel3;
    public Button ButtonLevel4;
    public Button ButtonLevel5;
    public GameObject WaitPanel;
    List<Button> LevelScoreButtons = new List<Button>();
    List<Button> LevelInfoButtons = new List<Button>();

    List<LevelInfoDataStruct> levelInfoData = new List<LevelInfoDataStruct>();
    Image LevelInfoIcon;
    Text LevelInfoNameText;
    Text LevelInfoTextText;

    public GameObject LevelInfoPanel;

    PlayerPrefsManager manager;
    public float timeoutValue = 3f;
    private void OnEnable()
    {
        SetupButtons();
        timeoutValue = 3f;
        WaitPanel.SetActive(true);
        if (manager == null)
            manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        VideoLevelSelectButton.interactable = false;
        LockAllButtons();
    }

    private void Start()
    {
        LevelInfoIcon = LevelInfoPanel.transform.Find("InfoPanel2/InfoIconPanel/Icon").GetComponent<Image>();
        LevelInfoNameText = LevelInfoPanel.transform.Find("InfoPanel2/InfoIconPanel/Icon/LevelLabel").GetComponent<Text>();
        LevelInfoTextText = LevelInfoPanel.transform.Find("InfoPanel2/InfoTextPanel/Text").GetComponent<Text>();
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/LevelInfo");
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList _levelsInfo = xmlFile.FirstChild.NextSibling.ChildNodes;
        foreach (XmlNode s in _levelsInfo)
        {
            LevelInfoDataStruct _levelData = new LevelInfoDataStruct();

            _levelData.icon = (s.Attributes["icon"].Value);
            _levelData.name = (s.Attributes["name"].Value);
            _levelData.text = (s.Attributes["text"].Value);

            levelInfoData.Add(_levelData);
        }
    }

    void LockAllButtons()
    {
        List<Button> buttons = new List<Button> { VideoLevelSelectButton, ButtonLevel2, ButtonLevel3, ButtonLevel4, ButtonLevel5 };
        for (int i = 0; i < buttons.Count; i++ )
        {
            buttons[i].interactable = false;
            //if (LevelScoreButtons.Count > i)
            //{
            //    if (LevelScoreButtons[i] != null)
            //        LevelScoreButtons[i].interactable = false;
            //}
        }
    }

    public void ShowLevelInfo(int levelID)
    {
        if (levelID == -1 || levelInfoData.Count == 0)
        {
            LevelInfoPanel.SetActive(false);
            return;
        }
        LevelInfoPanel.SetActive(true);
        var sp = Resources.Load("Resources/Sprites/nUI") as Sprite;
        LevelInfoIcon.sprite = Resources.Load("Sprites/nUI/" + levelInfoData[levelID].icon, typeof(Sprite)) as Sprite;
        LevelInfoNameText.text = levelInfoData[levelID].name.ToUpper();
        LevelInfoTextText.text = levelInfoData[levelID].text;

    }

    public void ShowLevelScore( int levelID)
    {

    }

    private void Update()
    {
        if (timeoutValue > 0)
        {
            timeoutValue -= Time.deltaTime;
            if (timeoutValue <= 0)
            {
                UnlockLevelButtons();
            }
        }
    }
    public void ResetPracticePlays()
    {
        PlayerPrefsManager.SetValueToSceneInCategory(manager.currentSceneVisualName, "PracticePlays", 0);
    }

    public void SetupButtons()
    {
        List<Button> buttons = new List<Button> { VideoLevelSelectButton, ButtonLevel2, ButtonLevel3, ButtonLevel4, ButtonLevel5 };

        if (LevelInfoButtons.Count == 0)
        {
            foreach(Button b in buttons)
            {
                Transform p = b.transform.parent;
                Transform levelInfoButtonTrans = p.transform.Find("LevelInfoButton");
                if (levelInfoButtonTrans != null)
                {
                    LevelInfoButtons.Add(levelInfoButtonTrans.GetComponent<Button>());
                }
                else
                {
                    LevelInfoButtons.Add(null);
                }
            }
        }
        if (LevelScoreButtons.Count == 0)
        {
            foreach (Button b in buttons)
            {
                Transform p = b.transform.parent;
                Transform levelScoreButtonTrans = p.transform.Find("LevelScoreButton");
                if (levelScoreButtonTrans != null)
                {
                    LevelScoreButtons.Add(levelScoreButtonTrans.GetComponent<Button>());
                }
                else
                {
                    LevelScoreButtons.Add(null);
                }
            }
        }
        LevelScoreButtons[0].gameObject.SetActive(false);
    }

    void UnlockLevelButtons()
    {
        WaitPanel.SetActive(false);
        Debug.Log(manager.currentPracticePlays);
        SceneInfo selectedSceneInfo = manager.GetSceneInfoByName(mainBtn.sceneName);
        if (selectedSceneInfo != null)
        {
            VideoLevelSelectButton.interactable = selectedSceneInfo.hasVideoMode;
        }
        List<Button> buttons = new List<Button> { VideoLevelSelectButton, ButtonLevel2, ButtonLevel3, ButtonLevel4, ButtonLevel5 };

        buttons[1].interactable = true;
        buttons[2].interactable = true;
           
        if (manager.currentPracticePlays >= 1)
        {
            buttons[3].interactable = true;
        }
        if (manager.currentPracticePlays >= 3)
        {
            buttons[4].interactable = true;
        }
    }
}

