using System.Collections;
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
    [HideInInspector]
    public string itemControlsToInit = "";

    public Camera cam;
    public MouseLook mouseLook = new MouseLook();
    public bool freeLook = false;

    PlayerPrefsManager prefs;
    Controls controls;
    HandsInventory handsInv;
    CameraMode cameraMode;

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

    Button moveBackButton;
    public ItemControlsUI itemControls;

    public bool usingOnMode = false;
    public bool usingOnHand;

    private GameObject usingOnText;
    private GameObject usingOnCancelButton;
    private bool onButtonHover = false;

    private GameObject closeButton;

    private float rotated = 0.0f;

    [HideInInspector]
    public static QuizTab quiz;
    private static PlayerScript instance; // fix for coroutines
    
    public bool robotUIopened = false;
    private bool robotUINotOpenedYet = true;
    public GameObject robotSavedLeft;
    public GameObject robotSavedRight;

    GameObject devHintUI = null;
    GameObject biggerDevHintUI = null;
    GameObject tutorialCanvas;

    Tutorial_UI tutorial_UI;
    Tutorial_Theory tutorial_theory;

    bool moveBackBtnActiveForIpad = false;
    bool devHintActiveForIpad = false;
    bool biggerDevHintActiveForIpad = false;

    GameObject extraButton;
    bool extraBtnActiveForIpad = false;
    GameObject extraPanel;
    bool extraPanelActiveForIpad = false;

    public static bool actionsLocked = false;

    public GameObject MoveBackButtonObject
    {
        get { return moveBackButton.gameObject; }
    }

    public bool UIHover
    {
        get { return onButtonHover; }
    }

    public void ResetUIHover()
    {
        onButtonHover = false;
    }

    private void Start()
    {
        instance = this;
        actionsLocked = false;

        mouseLook.Init(transform, cam.transform);

        if (GameObject.Find("Preferences") != null)
        {
            prefs = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        }
        
        GetComponent<Crosshair>().enabled = ( prefs == null ) ? false : prefs.VR;

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();

        groups = new List<WalkToGroup>(
            GameObject.FindObjectsOfType<WalkToGroup>());

        fadeTex = Resources.Load<Texture>("Sprites/Black");

        moveBackButton = GameObject.Find("MoveBackButton").GetComponent<Button>();
        moveBackButton.gameObject.SetActive(false);

        extraButton = GameObject.Find("ExtraButton");
        extraPanel = GameObject.Find("Extra");
        
        itemControls = GameObject.FindObjectOfType<ItemControlsUI>();
        itemControls.gameObject.SetActive(false);

        handsInv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();

        usingOnText = GameObject.Find("UsingOnModeText");
		usingOnCancelButton = GameObject.Find("CancelUseOnButton").gameObject;
        usingOnText.SetActive(false);

        quiz = GameObject.FindObjectOfType<QuizTab>(); 

        EventTrigger.Entry event1 = new EventTrigger.Entry();
        event1.eventID = EventTriggerType.PointerEnter;
        event1.callback.AddListener((eventData) => { EnterHover(); });
        
        EventTrigger.Entry event2 = new EventTrigger.Entry();
        event2.eventID = EventTriggerType.PointerExit;
        event2.callback.AddListener((eventData) => { ExitHover(); });

        EventTrigger.Entry event3 = new EventTrigger.Entry();
        event3.eventID = EventTriggerType.PointerClick;
        event3.callback.AddListener((eventData) => { ExitHover(); });

        usingOnCancelButton.AddComponent<EventTrigger>();
        usingOnCancelButton.GetComponent<EventTrigger>().triggers.Add(event1);
        usingOnCancelButton.GetComponent<EventTrigger>().triggers.Add(event2);
        usingOnCancelButton.GetComponent<EventTrigger>().triggers.Add(event3);
        
        GameObject robotUI = GameObject.Find("RobotUI");
        robotUI.AddComponent<EventTrigger>();
        robotUI.GetComponent<EventTrigger>().triggers.Add(event1);
        robotUI.GetComponent<EventTrigger>().triggers.Add(event2);

        if (GameObject.Find("DevHint") != null)
            devHintUI = GameObject.Find("DevHint").gameObject;
        if (GameObject.Find ("BiggerDevHint") != null)
            biggerDevHintUI = GameObject.Find ("BiggerDevHint").gameObject;

        GameObject wrongActionPopUp = GameObject.Find("WrongAction").gameObject;
		//GameObject warningPopUp = GameObject.Find("EmptyHandsWarning").gameObject;

        wrongActionPopUp.AddComponent<EventTrigger>();
        wrongActionPopUp.GetComponent<EventTrigger>().triggers.Add(event1);
        wrongActionPopUp.GetComponent<EventTrigger>().triggers.Add(event2);
        wrongActionPopUp.GetComponent<EventTrigger>().triggers.Add(event3);

       // warningPopUp.AddComponent<EventTrigger>();
        //warningPopUp.GetComponent<EventTrigger>().triggers.Add(event1);
       // warningPopUp.GetComponent<EventTrigger>().triggers.Add(event2);
       // warningPopUp.GetComponent<EventTrigger>().triggers.Add(event3);
        
        EventTrigger.Entry closePopUp = new EventTrigger.Entry();
        event3.eventID = EventTriggerType.PointerClick;
        event3.callback.AddListener((eventData) => { TimedPopUp.ForceHide(); });
        
        wrongActionPopUp.GetComponent<EventTrigger>().triggers.Add(closePopUp);
        //warningPopUp.GetComponent<EventTrigger>().triggers.Add(closePopUp);

        if (GameObject.Find("GameLogic").GetComponent<TutorialManager>() != null)
        {
            GameObject tutorialEndUi = GameObject.Find("TutorialDonePanel");
            tutorialEndUi.AddComponent<EventTrigger>();
            tutorialEndUi.GetComponent<EventTrigger>().triggers.Add(event1);
            tutorialEndUi.GetComponent<EventTrigger>().triggers.Add(event2);
            tutorialEndUi.SetActive(false);
        }

        savedPos = transform.position;
        savedRot = transform.rotation;

        mouseLook.SaveRot(transform, Camera.main.transform);

        robot = GameObject.FindObjectOfType<RobotManager>();
        savedRobotPos = robot.transform.position;
        savedRobotRot = robot.transform.rotation;

        tutorial_UI = GameObject.FindObjectOfType<Tutorial_UI>();
        tutorial_theory = GameObject.FindObjectOfType<Tutorial_Theory>();
    }

    public void EnterHover()
    {
        onButtonHover = true;
    }

    public void ExitHover()
    {
        onButtonHover = false;
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
        
        if (freeLook && !robotUIopened && cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            rotated += mouseLook.LookRotation(transform, Camera.main.transform);
        }

        if (!freeLook && controls.MouseClicked() && !robotUIopened)
        {
            if (!away && controls.SelectedObject != null 
                && controls.SelectedObject.GetComponent<InteractableObject>() != null
                && !itemControls.gameObject.activeSelf && !actionsLocked)
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

        if (!value)
        {
            onButtonHover = false;
        }
        else
        {
            tutorial_UseOnControl = true;
        }
    }

    public void WalkToGroup(WalkToGroup group)
    {
        if (robotUIopened)
            return;

        ToggleAway();
        transform.position = group.Position;
        if ( prefs == null || (prefs != null && !prefs.VR))
        {
            transform.rotation = Quaternion.Euler(0.0f, group.Rotation.y, 0.0f);
            Camera.main.transform.localRotation = Quaternion.Euler(group.Rotation.x, 0.0f, 0.0f);
            mouseLook.SaveRot(transform, Camera.main.transform);
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
    }

    public void ResetFreeLook()
    {
        transform.rotation = mouseLook.SavedCharRot;
        Camera.main.transform.rotation = mouseLook.SavedCamRot;
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
        if (true)
        {
            ToggleAway(true);
            transform.position = savedPos;
            if (prefs == null || (prefs != null && !prefs.VR))
            {
                transform.rotation = Quaternion.Euler(0.0f, savedRot.eulerAngles.y, 0.0f);
                Camera.main.transform.localRotation = Quaternion.Euler(savedRot.eulerAngles.x, 0.0f, 0.0f);
                mouseLook.SaveRot(transform, Camera.main.transform);
            }
            currentWalkPosition = null;

            robot.transform.position = savedRobotPos;
            robot.transform.rotation = savedRobotRot;
        }
    }

    public void OpenRobotUI()
    {
        if (robotUIopened)
            return;

        if ((tutorial_UI != null && tutorial_UI.expectedRobotUIstate == false) ||
            (tutorial_theory != null && tutorial_theory.expectedRobotUIstate == false))
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
        else
        {
            robotSavedLeft = robotSavedRight = null;
        }

        PlayerAnimationManager.PlayAnimation("IpadCloseUp");
        robotUIopened = true;

        if (devHintUI != null)
        {
            devHintActiveForIpad = devHintUI.activeSelf;
            devHintUI.SetActive(false);
        }

        if (biggerDevHintUI != null) {
            biggerDevHintActiveForIpad = biggerDevHintUI.activeSelf;
            biggerDevHintUI.SetActive (false);
        }

        if (extraButton != null)
        {
            extraBtnActiveForIpad = extraButton.activeSelf;
            extraButton.SetActive(false);
        }

        if (extraPanel == null)
        {
            extraPanel = GameObject.Find("Extra");
        }

        if (extraPanel != null)
        {
            extraPanelActiveForIpad = extraPanel.activeSelf;
            extraPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Did not find hints extra panel.");
        }

        RobotManager.SetUITriggerActive(false);
        Camera.main.transform.localRotation = Quaternion.Euler(8.0f, 0.0f, 0.0f);

        // no switching to message tab anymore :<
        /*if (RobotManager.NotificationNumber > 0)
        {
            GameObject.FindObjectOfType<RobotUIMessageTab>().OnTabSwitch();
        }*/

        tutorial_robotUI_opened = true;

        moveBackBtnActiveForIpad = MoveBackButtonObject.activeSelf;
        MoveBackButtonObject.SetActive(false);

        if (robotUINotOpenedYet)
        {
            string title = "Hygiënisch smartphone- en tabletgebruik";
            string message = "Telefoons en tablet bevatten erg veel micro-organismen. Bij het gebruik van een smartphone of tablet heeft handhygiëne de grootste prioriteit. Zowel voor als na het gebruiken van een mobiel communicatiemiddel moet je je handen goed reinigen. Je kunt het gebruik van een mobiel apparaat tijdens werkzaamheden zien als het beëindigen en opnieuw aangaan van handcontact met de cliënt. In Care Up is dit niet nodig omdat het de gebruikerservaring negatief beïnvloed maar zorg in de praktijk dus voor goede hygiëne tijdens het gebruik van mobiele apparaten.";
            GameObject.FindObjectOfType<RobotUIMessageTab>().NewMessage(title, message, RobotUIMessageTab.Icon.Warning);

            robotUINotOpenedYet = false;
        }
    }

    public void CloseRobotUI()
    {
        if ((tutorial_UI != null && tutorial_UI.expectedRobotUIstate == true) ||
            (tutorial_theory != null && tutorial_theory.expectedRobotUIstate == true))
        {
            return;
        }

        QuizTab quizTab = GameObject.FindObjectOfType<QuizTab>();
        if (quizTab != null && quizTab.continueBtn)
        {
            GameObject.FindObjectOfType<QuizTab>().OnContinueButton();
        }

        PlayerAnimationManager.PlayAnimation("IPadFarAway");
        robotUIopened = false;
        
        if (GameObject.FindObjectOfType<TutorialManager>() == null 
            || tutorial_UI != null || tutorial_theory != null)
        {
            if (devHintUI != null)
            {
                devHintUI.SetActive(devHintActiveForIpad);
            }

            if (biggerDevHintUI != null) {
                biggerDevHintUI.SetActive (biggerDevHintActiveForIpad);
            }

            if (extraButton != null)
            {
                extraButton.SetActive(extraBtnActiveForIpad);
            }

            if (extraPanel != null)
            {
                extraPanel.SetActive(extraPanelActiveForIpad);
            }
        }

        tutorial_robotUI_closed = true;

        MoveBackButtonObject.SetActive(moveBackBtnActiveForIpad);
    }

    public void PickItemsBackAfterRobotUI()
    {
        StartCoroutine(DelayedPickItemsAfterIpad(0.5f));

        if (robotSavedLeft != null)
        {
            PlayerAnimationManager.SetHandItem(true, robotSavedLeft.gameObject);
        }

        if (robotSavedRight != null)
        {
            PlayerAnimationManager.SetHandItem(false, robotSavedRight.gameObject);
        }
    }

    IEnumerator DelayedPickItemsAfterIpad(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (robotSavedLeft != null)
        {
            handsInv.ForcePickItem(robotSavedLeft.name, true);
            robotSavedLeft.GetComponent<PickableObject>().CreateGhostObject();
            robotSavedLeft = null; // reset
        }
        else
        {
            PlayerAnimationManager.SetHandItem(true, null);
        }

        if (robotSavedRight != null)
        {
            handsInv.ForcePickItem(robotSavedRight.name, false);
            robotSavedRight.GetComponent<PickableObject>().CreateGhostObject();
            robotSavedRight = null; // reset
        }
        else
        {
            PlayerAnimationManager.SetHandItem(false, null);
        }

        robotSavedLeft = robotSavedRight = null;
    }

    /// <summary>
    /// Function triggers next quiz question from xml file,
    /// that was set in PlayerSpawn object. 
    /// If there is no questions left - it will do nothing.
    /// </summary>
    /// <param name="delay">Delay before opening ipad.</param>
    public static void TriggerQuizQuestion(float delay = 0.0f)
    {
        // just dont trigger quiz if it's a tutorial for all cases
        if (GameObject.FindObjectOfType<TutorialManager>() != null)
            return;

        // lock actions so player does nothing to break until quiz triggers
        PlayerScript.actionsLocked = true;
        // close itemDescription if active, cuz we locked actions, so it's not updating
        GameObject itemDescription = GameObject.Find("ItemDescription");
        if (itemDescription)
        {
            itemDescription.SetActive(false);
        }
        // trigger quiz with delay
        instance.StartCoroutine(QuizCoroutine(delay));
    }

    private static IEnumerator QuizCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        quiz.NextQuizQuestion();
    }
}
