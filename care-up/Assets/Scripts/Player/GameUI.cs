﻿using System.Collections;
using System;
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
    private Animator controller;

    public GameObject ItemControlPanel;
    public GameObject combineButton;
    public GameObject decombineButton;
    public GameObject decombineButton_right;

    public GameObject noTargetButton;
    private CameraMode cameraMode;

    public GameObject zoomButtonLeft;
    public GameObject zoomButtonRight;

    float cooldownTime = 0;
    float lastCooldownTime = 0;
    int currentActionsCount = 0;

    bool currentItemControlPanelState = false;
    int currentLeft;
    int currentRight;
    string useOnNTtext;
    PlayerScript ps;
    bool ICPCurrentState = false;
    public bool allowObjectControlUI = true;


    public void MoveBack()
	{
		Player.GetComponent<PlayerScript>().MoveBackButton();
	}

    //public WalkToGroupButton GetWTGButton(string key)
    //{

    //}

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


    public void Examine(bool leftHand = true)
    {
        if (leftHand && handsInventory.LeftHandEmpty())
            return;
        if (!leftHand && handsInventory.RightHandEmpty())
            return;
        GameObject initedObject = null;
        if (leftHand)
            initedObject = handsInventory.leftHandObject.gameObject;
        else
            initedObject = handsInventory.rightHandObject.gameObject;

        cameraMode.selectedObject = initedObject.GetComponent<ExaminableObject>();
        if (cameraMode.selectedObject != null) // if there is a component
        {
            if (tutorialUseOn != null)
            {
                tutorialUseOn.examined = true;
            }
            cameraMode.selectedObject.OnExamine();
            //controls.ResetObject();
        }


    }


    // Use this for initialization
    void Start () {
        useOnNTtext = noTargetButton.transform.GetChild(0).GetComponent<Text>().text;
        ps = GameObject.FindObjectOfType<PlayerScript>();
        controller = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        handsInventory = GameObject.FindObjectOfType<HandsInventory>();
        tutorialCombine = GameObject.FindObjectOfType<Tutorial_Combining>();
        tutorialUseOn = GameObject.FindObjectOfType<Tutorial_UseOn>();
        actionManager = GameObject.FindObjectOfType<ActionManager>();
        //ActionManager.BuildRequirements();
        zoomButtonLeft.SetActive(false);
        zoomButtonRight.SetActive(false);
        combineButton.SetActive(false);
        decombineButton.SetActive(false);
        decombineButton_right.SetActive(false);
        noTargetButton.SetActive(false);
        ItemControlPanel.SetActive(false);

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
    void Update() {
        
        //Don't show object control panel if animation is playing
        //if animation is longer than 0.2 (is not hold animation)
        bool animationUiBlock = true;
        for (int i = 0; i < 3; i++)
        {
            if (controller.GetCurrentAnimatorStateInfo(i).length > 0.2f && controller.GetCurrentAnimatorStateInfo(i).normalizedTime < 1f)
                animationUiBlock = false;
            if (controller.GetNextAnimatorStateInfo(i).length > 0.2f && controller.GetAnimatorTransitionInfo(i).normalizedTime < 0.01)
                animationUiBlock = false;
            if (i < 2)
            {
                if (controller.GetCurrentAnimatorStateInfo(i).length > 0.2f &&
                    controller.GetAnimatorTransitionInfo(i).normalizedTime < 0.01 &&
                    controller.GetNextAnimatorStateInfo(i).length < 0.2f)
                    animationUiBlock = false;
            }
        }

        //to show object control panel if no animation block and action block
        bool showItemControlPanel = allowObjectControlUI && animationUiBlock;

        //if some object was added or removed to hands
        int lHash = 0;
        if (handsInventory.leftHandObject != null)
            lHash = handsInventory.leftHandObject.gameObject.GetHashCode();
        int rHash = 0;
        if (handsInventory.rightHandObject != null)
            rHash = handsInventory.rightHandObject.gameObject.GetHashCode();


        bool handsStateChanged = (currentLeft != lHash || currentRight != rHash
        || (ICPCurrentState != ItemControlPanel.activeSelf) 
        || currentActionsCount != actionManager.actionsCount);

        if (handsStateChanged) 
        {
            currentActionsCount = actionManager.actionsCount;
            //hide panel for the first frame of hands state change
            //prevent quick blinking of buttons before animation starts
            showItemControlPanel = false;

            //Update current hands state 
            currentLeft = lHash;
            currentRight = rHash;

            bool LEmpty = handsInventory.LeftHandEmpty();
            bool REmpty = handsInventory.RightHandEmpty();
            bool showDecomb = (LEmpty && !REmpty) || (!LEmpty && REmpty);
            bool showCombin = !LEmpty && !REmpty;
            bool showZoomLeft = false;
            bool showZoomRight = false;

            bool showNoTarget = false;
            if (!LEmpty)
            {
                if (handsInventory.leftHandObject.GetComponent<ExaminableObject>() != null)
                    showZoomLeft = true;
                if (actionManager.CompareUseOnInfo(handsInventory.leftHandObject.name, ""))
                {
                    showNoTarget = true;
                    noTargetButton.transform.GetChild(0).GetComponent<Text>().text =
                        actionManager.CurrentButtonText(handsInventory.leftHandObject.name);
                }
            }
            if (!REmpty)
            {
                if (handsInventory.rightHandObject.GetComponent<ExaminableObject>() != null)
                    showZoomRight = true;
                if (actionManager.CompareUseOnInfo(handsInventory.rightHandObject.name, ""))
                {
                    showNoTarget = true;
                    noTargetButton.transform.GetChild(0).GetComponent<Text>().text =
                       actionManager.CurrentButtonText(handsInventory.rightHandObject.name);
                }
            }
            zoomButtonLeft.SetActive(showZoomLeft);
            zoomButtonRight.SetActive(showZoomRight);
            noTargetButton.SetActive(showNoTarget);
            decombineButton.SetActive(showDecomb && REmpty);
            decombineButton_right.SetActive(showDecomb && LEmpty);
            combineButton.SetActive(showCombin);
        }

        if (!currentItemControlPanelState && showItemControlPanel)
        {
            cooldownTime = 0.4f;
        }
        lastCooldownTime = cooldownTime;
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;
        currentItemControlPanelState = showItemControlPanel;
        if (cooldownTime > 0)
            showItemControlPanel = false;
        ItemControlPanel.SetActive(showItemControlPanel);

        ICPCurrentState = ItemControlPanel.activeSelf;
        if (WalkToGroupPanel != null)
        {
            
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
