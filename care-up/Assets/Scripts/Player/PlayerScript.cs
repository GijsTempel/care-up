﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.EventSystems;
    
public class PlayerScript : MonoBehaviour {
    [HideInInspector]
    public bool tutorial_movementLock = false;
    [HideInInspector]
    public float tutorial_totalLookAround = 0.0f;
    [HideInInspector]
    public float tutorial_totalMoveAround = 0.0f;
    [HideInInspector]
    public bool tutorial_movedBack = false;
    [HideInInspector]
    public bool tutorial_movedTo = false;
    [HideInInspector]
    public bool tutorial_robotUI_opened = false;
    [HideInInspector]
    public bool tutorial_robotUI_closed = false;
    [HideInInspector]
    public bool tutorial_itemControls = false;
    [HideInInspector]
    public bool tutorial_UseOnControl = false;

    public Camera cam;
    public MouseLook mouseLook = new MouseLook();
    public bool freeLook = false;

    PlayerPrefsManager prefs;
    Controls controls;
    HandsInventory handsInv;

    public bool away = true;
    private Vector3 savedPos;
    private Quaternion savedRot;
    private List<WalkToGroup> groups;
    private WalkToGroup currentWalkPosition;

    RobotManager robot;
    private Vector3 savedRobotPos;
    private Quaternion savedRobotRot;

    private bool fade;
    private float fadeTime = 1f;
    private float fadeTimer = 0.0f;
    Texture fadeTex;

    MoveBackButton moveBackButton;
    public ItemControlsUI itemControls;

    public bool usingOnMode = false;
    public bool usingOnHand;

    private GameObject usingOnText;
    private GameObject usingOnCancelButton;

    private GameObject closeButton;

    private float rotated = 0.0f;

    [HideInInspector]
    public QuizTab quiz;
    
    public bool robotUIopened = false;
    private GameObject robotSavedLeft;
    private GameObject robotSavedRight;

    GameObject devHintUI;
	GameObject UIObject;
    GameObject tutorialCanvas;

    Tutorial_UI tutorial_UI;

    bool moveBackBtnActiveForIpad = false;
    bool devHintActiveForIpad = false;
    
    public GameObject MoveBackButtonObject
    {
        get { return moveBackButton.gameObject; }
    }
    
    private void Start()
    {
        mouseLook.Init(transform, cam.transform);
		UIObject = GameObject.FindObjectOfType<GameUI>().gameObject;
        if (GameObject.Find("Preferences") != null)
        {
            prefs = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        }
        
        GetComponent<Crosshair>().enabled = ( prefs == null ) ? false : prefs.VR;

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();

        groups = new List<WalkToGroup>(
            GameObject.FindObjectsOfType<WalkToGroup>());

        fadeTex = Resources.Load<Texture>("Sprites/Black");

        moveBackButton = GameObject.Find("MoveBackButton").GetComponent<MoveBackButton>();
        moveBackButton.gameObject.SetActive(false);
        
        itemControls = GameObject.FindObjectOfType<ItemControlsUI>();
        itemControls.gameObject.SetActive(false);

        handsInv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        usingOnText = GameObject.Find("UsingOnModeText");
		usingOnCancelButton = GameObject.Find("CancelUseOnButton").gameObject;
        usingOnText.SetActive(false);

        quiz = GameObject.FindObjectOfType<QuizTab>(); 
        
        devHintUI = GameObject.Find("DevHint").gameObject;

        GameObject wrongActionPopUp = GameObject.Find("WrongAction").gameObject;
		GameObject warningPopUp = GameObject.Find("EmptyHandsWarning").gameObject;

        wrongActionPopUp.AddComponent<EventTrigger>();
        warningPopUp.AddComponent<EventTrigger>();
        
        EventTrigger.Entry closePopUp = new EventTrigger.Entry();
        closePopUp.eventID = EventTriggerType.PointerClick;
        closePopUp.callback.AddListener((eventData) => { TimedPopUp.ForceHide(); });
        
        wrongActionPopUp.GetComponent<EventTrigger>().triggers.Add(closePopUp);
        warningPopUp.GetComponent<EventTrigger>().triggers.Add(closePopUp);

        if (GameObject.Find("GameLogic").GetComponent<TutorialManager>() != null)
        {
            GameObject tutorialEndUi = GameObject.Find("TutorialDonePanel");
            tutorialEndUi.SetActive(false);
        }

        savedPos = transform.position;
        savedRot = transform.rotation;

        robot = GameObject.FindObjectOfType<RobotManager>();
        savedRobotPos = robot.transform.position;
        savedRobotRot = robot.transform.rotation;

        tutorial_UI = GameObject.FindObjectOfType<Tutorial_UI>();
    }

    public void FreeLookButton()
    {
        freeLook = !freeLook;
        rotated = 0.0f;

        mouseLook.Init(transform, Camera.main.transform);

        if (freeLook)
        {
            foreach (WalkToGroup g in groups)
            {
                if (g != currentWalkPosition)
                {
                    g.HighlightGroup(false);
                    g.enabled = true;
                    g.GetComponent<Collider>().enabled = true;
                }
            }
        }
    }

    private void Update()
    {
        if (prefs != null)
        {
            if (!prefs.VR)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        if (freeLook && !robotUIopened)
        {
            rotated += mouseLook.LookRotation(transform, Camera.main.transform);
        }

        if (!freeLook && controls.MouseClicked() && !moveBackButton.mouseOver && !robotUIopened)
        {
            if (!away && controls.SelectedObject != null 
                && controls.SelectedObject.GetComponent<InteractableObject>() != null
                && !itemControls.gameObject.activeSelf)
            {
                if (usingOnMode)
                {
                    if (usingOnHand)
                    {
                        handsInv.LeftHandUse();

                        ToggleUsingOnMode(false);
                    }
                    else
                    {
                        handsInv.RightHandUse();

                        ToggleUsingOnMode(false);
                    }
                }
                else
                {
                    itemControls.Init(controls.SelectedObject);
                    tutorial_itemControls = true;
                }
            }
            else
            {
                FreeLookButton();
            }
        }
        else if (Input.GetMouseButtonDown(1) && usingOnMode)
        {
            ToggleUsingOnMode(false);
        }
        else if (Controls.MouseReleased() && freeLook)
        {
            if (rotated < 3.0f && controls.SelectedObject != null &&
                controls.SelectedObject.GetComponent<WalkToGroup>())
            {
                WalkToGroup(controls.SelectedObject.GetComponent<WalkToGroup>());
            }
            else
            {
                FreeLookButton();
            }
        }
        
        moveBackButton.GetComponent<Button>().interactable = !tutorial_movementLock;
    }

    public void ToggleUsingOnMode(bool value)
    {   
        usingOnMode = value;
        if (value)
        {
            usingOnText.GetComponent<Text>().text = "Selecteer een object waarmee je " +
                (usingOnHand ?
                    (handsInv.LeftHandObject.GetComponent<InteractableObject>().description == ""
                    ? handsInv.LeftHandObject.name : handsInv.LeftHandObject.GetComponent<InteractableObject>().description)
                :
                    (handsInv.RightHandObject.GetComponent<InteractableObject>().description == ""
                    ? handsInv.RightHandObject.name : handsInv.RightHandObject.GetComponent<InteractableObject>().description)
                )
                + " wilt gebruiken.";
        }
        usingOnText.SetActive(value);

        if (value)
        {
            tutorial_UseOnControl = true;
        }
    }

    public void WalkToGroup(WalkToGroup group)
    {
        ToggleAway();
        transform.position = group.position;
        if (prefs == null || (prefs != null && !prefs.VR))
        {
            transform.rotation = Quaternion.Euler(0.0f, group.rotation.y, 0.0f);
            Camera.main.transform.localRotation = Quaternion.Euler(group.rotation.x, 0.0f, 0.0f);
        }
        currentWalkPosition = group;

        robot.transform.position = group.robotPosition;
        robot.transform.rotation = Quaternion.Euler(group.robotRotation);
    }

    private void ToggleAway(bool _away = false)
    {
        fade = true;
        away = _away;
        foreach (WalkToGroup g in groups)
        {
            g.HighlightGroup(false);
            g.enabled = away;
            g.GetComponent<Collider>().enabled = away;
        }
        moveBackButton.mouseOver = false;
        moveBackButton.gameObject.SetActive(!away);
        
        itemControls.Close();

        if (away)
        {
            tutorial_movedBack = true;
        }
        else
        {
            tutorial_movedTo = true;
        }

        freeLook = false;
        mouseLook.savedRot = false;
        mouseLook.ToggleMode(freeLook, transform, Camera.main.transform);
    }

    private void OnGUI()
    {
        if (fade)
        {
            if (fadeTimer > fadeTime)
            {
                fadeTimer = 0.0f;
                fade = false;
            }
            else
            {
                GUI.color = new Color(0.0f, 0.0f, 0.0f, 1.0f -  
                    Mathf.InverseLerp(0.0f, fadeTime, fadeTimer));
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTex);
                fadeTimer += Time.deltaTime;
            }
        }
    }

    public void MoveBackButton()
    {
        if (!away)
        {
            ToggleAway(true);
            transform.position = savedPos;
            if (prefs == null || (prefs != null && !prefs.VR))
            {
                transform.rotation = Quaternion.Euler(0.0f, savedRot.eulerAngles.y, 0.0f);
                Camera.main.transform.localRotation = Quaternion.Euler(savedRot.eulerAngles.x, 0.0f, 0.0f);
            }
            currentWalkPosition = null;

            robot.transform.position = savedRobotPos;
            robot.transform.rotation = savedRobotRot;
        }
    }

    public void OpenRobotUI()
    {
        if (tutorial_UI != null && tutorial_UI.expectedRobotUIstate == false)
        {
            return;
        }

        if (!handsInv.Empty())
        {
            robotSavedLeft = handsInv.LeftHandObject;
            robotSavedRight = handsInv.RightHandObject;

            handsInv.DropLeftObject();
            handsInv.DropRightObject();
        }

        PlayerAnimationManager.PlayAnimation("IpadCloseUp");
        robotUIopened = true;

        devHintActiveForIpad = devHintUI.activeSelf;
		//devHintUI.SetActive(false);
		UIObject.SetActive(false);

        RobotManager.SetUITriggerActive(false);
        Camera.main.transform.localRotation = Quaternion.Euler(8.0f, 0.0f, 0.0f);

        if (RobotManager.NotificationNumber > 0)
        {
            GameObject.FindObjectOfType<RobotUIMessageTab>().OnTabSwitch();
        }

        tutorial_robotUI_opened = true;

        moveBackBtnActiveForIpad = MoveBackButtonObject.activeSelf;
        MoveBackButtonObject.SetActive(false);
    }

    public void CloseRobotUI()
    {
        if (tutorial_UI != null && tutorial_UI.expectedRobotUIstate == true)
        {
            return;
        }

        PlayerAnimationManager.PlayAnimation("IPadFarAway");
        robotUIopened = false;

        if (GameObject.FindObjectOfType<TutorialManager>() == null 
            || GameObject.FindObjectOfType<Tutorial_UI>() != null)
        {
            devHintUI.SetActive(devHintActiveForIpad);
        }

        RobotManager.SetUITriggerActive(true);
        tutorial_robotUI_closed = true;
		UIObject.SetActive(true);

        MoveBackButtonObject.SetActive(moveBackBtnActiveForIpad);
    }

    public void PickItemsBackAfterRobotUI()
    {
        if (robotSavedLeft != null)
        {
            handsInv.ForcePickItem(robotSavedLeft.name, true);
            PlayerAnimationManager.SetHandItem(true, robotSavedLeft.gameObject);
        }

        if (robotSavedRight != null)
        {
            handsInv.ForcePickItem(robotSavedRight.name, false);
            PlayerAnimationManager.SetHandItem(false, robotSavedRight.gameObject);
        }
    }
}
