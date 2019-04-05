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

    public void MoveBack()
	{
		Player.GetComponent<PlayerScript>().MoveBackButton();
	}

    //public WalkToGroupButton GetWTGButton(string key)
    //{

    //}

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
            foreach (WalkToGroup g in GameObject.FindObjectsOfType<WalkToGroup>())
            {
                switch (g.WalkToGroupType) 
                {
                    case WalkToGroup.GroupType.WorkField:
                        WTGButtons["WorkField"].setWalkToGroup(g);
                        WTGButtons["WorkField"].gameObject.SetActive(true);
                        break;
                    case WalkToGroup.GroupType.Doctor:
                        WTGButtons["Doctor"].setWalkToGroup(g);
                        WTGButtons["Doctor"].gameObject.SetActive(true);
                        break;
                    case WalkToGroup.GroupType.Patient:
                        WTGButtons["Patient"].setWalkToGroup(g);
                        WTGButtons["Patient"].gameObject.SetActive(true);
                        break;
                    case WalkToGroup.GroupType.Sink:
                        WTGButtons["Sink"].setWalkToGroup(g);
                        WTGButtons["Sink"].gameObject.SetActive(true);
                        break;
                }
            }

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
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = Input.mousePosition;
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);
        foreach (RaycastResult h in hits)
        {
            if (h.gameObject.GetComponent<WalkToGroupButton>() != null)
                h.gameObject.GetComponent<WalkToGroupButton>().HighlightButton(true);
        }

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
