using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class GameUI : MonoBehaviour {
	GameObject Player;
    public TextAsset xmlFile;
    public Animator Blink;
	public Animator IPadBlink;
	public bool BlinkState = false;
	public bool testValue;
	GameObject donePanel;
	GameObject closeButton;
	GameObject closeDialog;
    
    

	public void MoveBack()
	{
		Player.GetComponent<PlayerScript>().MoveBackButton();
	}

	public void OpenRobotUI()
    {
		RobotManager.UIElementsState[0] = false;
        Player.GetComponent<PlayerScript>().OpenRobotUI();
    }

	public void ToggleUsingOnMode()
    {
		Player.GetComponent<PlayerScript>().ToggleUsingOnMode(false);
    }

	public void CloseButtonPressed(bool value)
	{
		closeDialog.SetActive(value);
		closeButton.SetActive(!value);
	}

	public void CloseGame()
	{
		bl_SceneLoaderUtils.GetLoader.LoadLevel("UMenuPro");
	}

    void addAchivements(string xmlData)
    {
        GameObject aList = transform.Find("AchievementsList").gameObject;


        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));
        string xmlPathPattern = "//achivements/achivement";
        XmlNodeList nodeList = xmlDoc.SelectNodes(xmlPathPattern);
        foreach (XmlNode node in nodeList)
        {
            XmlNode xName = node.FirstChild;
            if (aList.transform.Find(xName.InnerText) == null)
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Achievements"));
                go.name = xName.InnerText;
                go.transform.parent = aList.transform;
                go.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

                Text description = go.transform.Find("Message").GetComponent<Text>();
                Image icon = go.transform.Find("Image").GetComponent<Image>();

                XmlNode xIconID = xName.NextSibling;
                XmlNode xDescr = xIconID.NextSibling;

                if (!string.IsNullOrEmpty(xDescr.InnerText))
                    description.text = xDescr.InnerText;
                if (!string.IsNullOrEmpty(xIconID.InnerText))
                {
                    Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/achivements");
                    icon.sprite = sprites[int.Parse(xIconID.InnerText)];
                
                }
            }
        }

    }



    public void ButtonBlink(bool ToBlink)
	{

		if (BlinkState == ToBlink)
			return;
		BlinkState = ToBlink;
        if (transform.Find("Extra").gameObject.activeSelf)
        {
            Blink.SetTrigger("BlinkOnes");
			BlinkState = false;
        }
        else if (ToBlink)
        {
            Blink.SetTrigger("BlinkStart");
			RobotManager.UIElementsState[1] = true;
        }
        else
        {
            Blink.SetTrigger("BlinkStop");
			RobotManager.UIElementsState[1] = false;
        }
	}

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		closeButton = transform.Find("CloseBtn").gameObject;
		closeDialog = transform.Find("CloseDialog").gameObject;
		closeDialog.SetActive(false);
		donePanel = transform.Find("DonePanel").gameObject;
		donePanel.SetActive(false);

        string xData = xmlFile.text;
        addAchivements(xData);

        //Debug.Log(Application.isEditor);
    }

	public void ShowDonePanel(bool value)
	{
		donePanel.SetActive(value);
	}
    
	public void EndScene()
    {
		if (GameObject.Find("Preferences") != null){
			GameObject.Find("Preferences").GetComponent<EndScoreManager>().LoadEndScoreScene();
		}else{
			bl_SceneLoaderUtils.GetLoader.LoadLevel("UMenuPro");
		}
    }
	
	// Update is called once per frame
	void Update () {
		testValue = RobotManager.UIElementsState[0];
	}
}
