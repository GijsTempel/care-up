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

    public Camera cam;
    public MouseLook mouseLook = new MouseLook();

    PlayerPrefsManager prefs;
    Controls controls;
    HandsInventory handsInv;

    public bool away = true;
    private Vector3 savedPos;
    private Quaternion savedRot;
    private List<WalkToGroup> groups;

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
    private bool onButtonHover = false;

    private GameObject closeButton;

    [HideInInspector]
    public QuizTab quiz;

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

        moveBackButton = GameObject.Find("MoveBackButton").GetComponent<MoveBackButton>();
        moveBackButton.gameObject.SetActive(false);

        itemControls = GameObject.FindObjectOfType<ItemControlsUI>();
        itemControls.gameObject.SetActive(false);

        handsInv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        usingOnText = GameObject.Find("UsingOnModeText");
        usingOnCancelButton = usingOnText.transform.GetChild(0).gameObject;
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

        closeButton = GameObject.Find("TouchEscapeButton").gameObject;
        closeButton.AddComponent<EventTrigger>();
        closeButton.GetComponent<EventTrigger>().triggers.Add(event1);
        closeButton.GetComponent<EventTrigger>().triggers.Add(event2);
        closeButton.GetComponent<EventTrigger>().triggers.Add(event3);

        GameObject robotUI = Camera.main.transform.Find("UI (1)").Find("RobotUI").gameObject;
        robotUI.AddComponent<EventTrigger>();
        robotUI.GetComponent<EventTrigger>().triggers.Add(event1);
        robotUI.GetComponent<EventTrigger>().triggers.Add(event2);
    }

    public void EnterHover()
    {
        onButtonHover = true;
    }

    public void ExitHover()
    {
        onButtonHover = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            quiz.NextQuizQuestion(); // trigger quiz question
        }

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

        if (controls.MouseClicked() && !moveBackButton.mouseOver)
        {
            if (away && controls.SelectedObject != null &&
                controls.SelectedObject.GetComponent<WalkToGroup>())
            {
                WalkToGroup(controls.SelectedObject.GetComponent<WalkToGroup>());
            }
            else if (!away && controls.SelectedObject != null
                && !itemControls.gameObject.activeSelf && !onButtonHover)
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
        }
        else if (Input.GetMouseButtonDown(1) && usingOnMode)
        {
            ToggleUsingOnMode(false);
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
    }

    public void WalkToGroup(WalkToGroup group)
    {
        if (away && !onButtonHover)
        {
            ToggleAway();
            savedPos = transform.position;
            savedRot = transform.GetChild(0).GetChild(0).rotation;
            transform.position = group.position;
            if ( prefs == null || (prefs != null && !prefs.VR))
            {
                transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(group.rotation);
            }
        }
    }

    private void ToggleAway()
    {
        fade = true;
        away = !away;
        foreach (WalkToGroup g in groups)
        {
            g.HighlightGroup(false);
            g.enabled = away;
            g.GetComponent<Collider>().enabled = away;
        }
        moveBackButton.mouseOver = false;
        moveBackButton.gameObject.SetActive(!away);

        if (away)
        {
            itemControls.Close();
        }

        if (away)
        {
            tutorial_movedBack = true;
        }
        else
        {
            tutorial_movedTo = true;
        }
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
            ToggleAway();
            transform.position = savedPos;
            if (prefs == null || (prefs != null && !prefs.VR))
            {
                transform.GetChild(0).GetChild(0).rotation = savedRot;
            }
        }
    }
}
