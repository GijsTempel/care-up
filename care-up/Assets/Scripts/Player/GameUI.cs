using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;


public class GameUI : MonoBehaviour {
	GameObject Player;
	public Animator Blink;
	public Animator IPadBlink;
	public bool BlinkState = false;
	public bool testValue;
	GameObject donePanel;
	GameObject closeButton;
	GameObject closeDialog;
    GameObject donePanelYesNo;
    GameObject WalkToGroupPanel;
    public GameObject MoveBackButton;
    public WalkToGroupButton LeftSideButton;
    public WalkToGroupButton RightSideButton;
    public Dictionary<string, WalkToGroupButton> WTGButtons;
    WalkToGroup prevWalkToGroup = null;
    private Tutorial_Combining tutorialCombine;
    private Tutorial_UseOn tutorialUseOn;
    private HandsInventory handsInventory;
    private ActionManager actionManager;

    public GameObject ItemControlPanel;
    public GameObject combineButton;
    public GameObject decombineButton;
    public GameObject noTargetButton;
    PickableObject currentLeft;
    PickableObject currentRight;
    string useOnNTtext;
    bool ICPCurrentState = false;

    public void MoveBack()
	{
		Player.GetComponent<PlayerScript>().MoveBackButton();
	}

    //public WalkToGroupButton GetWTGButton(string key)
    //{

    //}

    void Awake()
    {
        useOnNTtext = noTargetButton.transform.GetChild(0).GetComponent<Text>().text;
        handsInventory = GameObject.FindObjectOfType<HandsInventory>();
        tutorialCombine = GameObject.FindObjectOfType<Tutorial_Combining>();
        tutorialUseOn = GameObject.FindObjectOfType<Tutorial_UseOn>();
        actionManager = GameObject.FindObjectOfType<ActionManager>();
    }

    public void UseOn()
    {

        if (!(handsInventory.LeftHandEmpty() && handsInventory.RightHandEmpty()))
        {
            handsInventory.OnCombineAction();
        }
    }



    public void UseOnNoTarget()
    {
        if (tutorialUseOn != null && !tutorialUseOn.ventAllowed)
        {
            return;
        }

        if (!handsInventory.LeftHandEmpty())
        {
            if (actionManager.CompareUseOnInfo(handsInventory.leftHandObject.name, ""))
            {
                handsInventory.LeftHandObject.GetComponent<PickableObject>().Use(true, true);

                if (tutorialUseOn != null)
                {
                    handsInventory.LeftHandObject.GetComponent<PickableObject>().tutorial_usedOn = true;
                }
                return;
            }
        }
        if (!handsInventory.RightHandEmpty())
        {
            if (actionManager.CompareUseOnInfo(handsInventory.rightHandObject.name, ""))
            {
                handsInventory.RightHandObject.GetComponent<PickableObject>().Use(false, true);

                if (tutorialUseOn != null)
                {
                    handsInventory.RightHandObject.GetComponent<PickableObject>().tutorial_usedOn = true;
                }
            }
        }
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

		if (value)
		{
			GameObject.FindObjectOfType<PlayerScript>().robotUIopened = true;
		}
		else
		{
			GameObject.FindObjectOfType<PlayerScript>().robotUIopened = false;
		}
	}

	public void CloseGame()
	{
		bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
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

#if !UNITY_EDITOR
        if(GameObject.Find("ActionsPanel") != null)
            GameObject.Find("ActionsPanel").SetActive(false);
#endif
        WalkToGroupPanel = GameObject.Find("MovementButtons");
        Player = GameObject.Find("Player");
		closeButton = transform.Find("CloseBtn").gameObject;
		closeDialog = transform.Find("CloseDialog").gameObject;
		closeDialog.SetActive(false);
		donePanel = transform.Find("DonePanel").gameObject;
		donePanel.SetActive(false);

        donePanelYesNo = transform.Find("DonePanelYesNo").gameObject;
        donePanelYesNo.SetActive(false);
        if (WalkToGroupPanel != null)
        {
            WTGButtons = new Dictionary<string, WalkToGroupButton>();

            foreach (WalkToGroupButton b in GameObject.FindObjectsOfType<WalkToGroupButton>())
            {
                if (b.name == "MoveLeft")
                {
                    LeftSideButton = b;
                }
                else if (b.name == "MoveRight")
                {
                    RightSideButton = b;
                }
                else
                {
                    string key = "";
                    switch (b.name)
                    {
                        case "MoveWorkfield":
                            key = "WorkField";
                            break;
                        case "Movepatient":
                            key = "Patient";
                            break;
                        case "MoveCollegue":
                            key = "Doctor";
                            break;
                        case "MoveSink":
                            key = "Sink";
                            break;
                    }
                    if (key != "")
                    {
                        WTGButtons.Add(key, b);
                    }
                }
                b.gameObject.SetActive(false);
            }
            int activeGroupButtons = 0;
            foreach (WalkToGroup g in GameObject.FindObjectsOfType<WalkToGroup>())
            {
                switch (g.WalkToGroupType) 
                {
                    case WalkToGroup.GroupType.WorkField:
                        WTGButtons["WorkField"].setWalkToGroup(g);
                        WTGButtons["WorkField"].gameObject.SetActive(true);
                        activeGroupButtons++;
                        break;
                    case WalkToGroup.GroupType.Doctor:
                        WTGButtons["Doctor"].setWalkToGroup(g);
                        WTGButtons["Doctor"].gameObject.SetActive(true);
                        activeGroupButtons++;
                        break;
                    case WalkToGroup.GroupType.Patient:
                        WTGButtons["Patient"].setWalkToGroup(g);
                        WTGButtons["Patient"].gameObject.SetActive(true);
                        break;
                    case WalkToGroup.GroupType.Sink:
                        WTGButtons["Sink"].setWalkToGroup(g);
                        WTGButtons["Sink"].gameObject.SetActive(true);
                        activeGroupButtons++;
                        break;
                }
            }
            if (!WTGButtons["Sink"].gameObject.activeSelf || activeGroupButtons < 2)
                WalkToGroupPanel.transform.Find("spacer0").gameObject.SetActive(false);
            if (!WTGButtons["Patient"].gameObject.activeSelf || activeGroupButtons < 2)
                WalkToGroupPanel.transform.Find("spacer2").gameObject.SetActive(false);
            if (activeGroupButtons < 2)
                WalkToGroupPanel.transform.Find("spacer1").gameObject.SetActive(false);

        }
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

        donePanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        ItemControlPanel.SetActive(GameObject.Find("RobotUITrigger") != null);
        if (currentLeft != handsInventory.leftHandObject || currentRight != handsInventory.rightHandObject
        || (ICPCurrentState != ItemControlPanel.activeSelf && ItemControlPanel.activeSelf))
        {
            currentLeft = handsInventory.leftHandObject;
            currentRight = handsInventory.rightHandObject;
            bool LEmpty = handsInventory.LeftHandEmpty();
            bool REmpty = handsInventory.RightHandEmpty();
            bool showDecomb = (LEmpty && !REmpty) || (!LEmpty && REmpty);
            bool showCombin = !LEmpty && !REmpty;
            if (!LEmpty)
            {
                if (handsInventory.leftHandObject.name == "ipad")
                {
                    showDecomb = false;
                    showCombin = false;
                }
            }
            if (!REmpty)
            {
                if (handsInventory.rightHandObject.name == "ipad")
                {
                    showDecomb = false;
                    showCombin = false;
                }
            }
            bool showNoTarget = false;
            if (!LEmpty)
            {
                if (actionManager.CompareUseOnInfo(handsInventory.leftHandObject.name, ""))
                {
                    showNoTarget = true;
                    noTargetButton.transform.GetChild(0).GetComponent<Text>().text =
                        actionManager.CurrentButtonText(handsInventory.leftHandObject.name);
                }
            }
            if (!REmpty)
            {
                if (actionManager.CompareUseOnInfo(handsInventory.rightHandObject.name, ""))
                {
                    showNoTarget = true;
                    noTargetButton.transform.GetChild(0).GetComponent<Text>().text =
                       actionManager.CurrentButtonText(handsInventory.rightHandObject.name);
                }
            }
            noTargetButton.SetActive(showNoTarget);
            decombineButton.SetActive(showDecomb);
            combineButton.SetActive(showCombin);
        }

        ICPCurrentState = ItemControlPanel.activeSelf;
        if (WalkToGroupPanel != null)
        {
            PlayerScript ps = GameObject.FindObjectOfType<PlayerScript>();
            if (prevWalkToGroup != ps.currentWalkPosition)
            {
                WalkToGroupPanel.SetActive(ps.away);
                if (!ps.away)
                {

                    LeftSideButton.gameObject.SetActive(ps.currentWalkPosition.LeftWalkToGroup != null);
                    RightSideButton.gameObject.SetActive(ps.currentWalkPosition.RightWalkToGroup != null);
                    if (ps.currentWalkPosition.LeftWalkToGroup != null)
                    {
                        LeftSideButton.setWalkToGroup(ps.currentWalkPosition.LeftWalkToGroup);
                    }

                    if (ps.currentWalkPosition.RightWalkToGroup != null)
                    {
                        RightSideButton.setWalkToGroup(ps.currentWalkPosition.RightWalkToGroup);
                    }
                }
                else
                {

                    LeftSideButton.gameObject.SetActive(false);
                    RightSideButton.gameObject.SetActive(false);
                }
                prevWalkToGroup = ps.currentWalkPosition;
            }
            if (!MoveBackButton.activeSelf)
            {

                LeftSideButton.gameObject.SetActive(false);
                RightSideButton.gameObject.SetActive(false);
                prevWalkToGroup = null;
            }

        }

        testValue = RobotManager.UIElementsState[0];
	}

    public void OpenDonePanelYesNo()
    {
        donePanelYesNo.SetActive(true);
    }

    public void DonePanelYes()
    {
        FindObjectOfType<ActionManager>().OnUseAction("PaperAndPen");
        donePanelYesNo.SetActive(false);
    }

    public void DonePanelNo()
    {
        donePanelYesNo.SetActive(false);
    }
}
