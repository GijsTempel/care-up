using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemControlsUI : MonoBehaviour {
    
    public GameObject initedObject;

    private Controls controls;
    private HandsInventory handsInventory;
    private CameraMode cameraMode;
    private ActionManager actionManager;
    private TutorialManager tutorial;
    private PlayerScript player;

    private GameObject closeButton;

    private GameObject pickButton;
    private GameObject examineButton;
    private GameObject useButton;
    private GameObject talkButton;
    private GameObject useOnButton;
    private GameObject useOnNTButton;
    private GameObject combineButton;
    private GameObject dropButton;

    public Vector2 cursorOffset;

    private string useOnNTtext;
    private string useText;

    private bool UIhover;

    private void OnEnterHover()
    {
        UIhover = true;
    }

    private void OnExitHover()
    {
        UIhover = false;
    }

    void Awake () {

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        handsInventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        tutorial = GameObject.Find("GameLogic").GetComponent<TutorialManager>();
       
        pickButton = transform.Find("PickButton").gameObject;
        examineButton = transform.Find("ExamineButton").gameObject;
        useButton = transform.Find("UseButton").gameObject;
        talkButton = transform.Find("TalkButton").gameObject;

        useOnButton = transform.Find("UseOnButton").gameObject;
        useOnNTButton = transform.Find("UseOnButton_noTarget").gameObject;
        combineButton = transform.Find("CombineButton").gameObject;

        dropButton = transform.Find("DropButton").gameObject;

        closeButton = transform.Find("CloseButton").gameObject;

        useOnNTtext = useOnNTButton.transform.GetChild(0).GetComponent<Text>().text;
        useText = useButton.transform.GetChild(0).GetComponent<Text>().text;
        
        //------------

        EventTrigger.Entry event1 = new EventTrigger.Entry();
        event1.eventID = EventTriggerType.PointerEnter;
        event1.callback.AddListener((eventData) => { OnEnterHover(); });

        EventTrigger.Entry event2 = new EventTrigger.Entry();
        event2.eventID = EventTriggerType.PointerExit;
        event2.callback.AddListener((eventData) => { OnExitHover(); });

        EventTrigger.Entry event3 = new EventTrigger.Entry();
        event3.eventID = EventTriggerType.PointerClick;
        event3.callback.AddListener((eventData) => { OnExitHover(); });

        closeButton.AddComponent<EventTrigger>();
        closeButton.GetComponent<EventTrigger>().triggers.Add(event1);
        closeButton.GetComponent<EventTrigger>().triggers.Add(event2);
        closeButton.GetComponent<EventTrigger>().triggers.Add(event3);

        pickButton.AddComponent<EventTrigger>();
        pickButton.GetComponent<EventTrigger>().triggers.Add(event1);
        pickButton.GetComponent<EventTrigger>().triggers.Add(event2);
        pickButton.GetComponent<EventTrigger>().triggers.Add(event3);

        examineButton.AddComponent<EventTrigger>();
        examineButton.GetComponent<EventTrigger>().triggers.Add(event1);
        examineButton.GetComponent<EventTrigger>().triggers.Add(event2);
        examineButton.GetComponent<EventTrigger>().triggers.Add(event3);

        useButton.AddComponent<EventTrigger>();
        useButton.GetComponent<EventTrigger>().triggers.Add(event1);
        useButton.GetComponent<EventTrigger>().triggers.Add(event2);
        useButton.GetComponent<EventTrigger>().triggers.Add(event3);

        talkButton.AddComponent<EventTrigger>();
        talkButton.GetComponent<EventTrigger>().triggers.Add(event1);
        talkButton.GetComponent<EventTrigger>().triggers.Add(event2);
        talkButton.GetComponent<EventTrigger>().triggers.Add(event3);

        useOnButton.AddComponent<EventTrigger>();
        useOnButton.GetComponent<EventTrigger>().triggers.Add(event1);
        useOnButton.GetComponent<EventTrigger>().triggers.Add(event2);
        useOnButton.GetComponent<EventTrigger>().triggers.Add(event3);

        useOnNTButton.AddComponent<EventTrigger>();
        useOnNTButton.GetComponent<EventTrigger>().triggers.Add(event1);
        useOnNTButton.GetComponent<EventTrigger>().triggers.Add(event2);
        useOnNTButton.GetComponent<EventTrigger>().triggers.Add(event3);

        combineButton.AddComponent<EventTrigger>();
        combineButton.GetComponent<EventTrigger>().triggers.Add(event1);
        combineButton.GetComponent<EventTrigger>().triggers.Add(event2);
        combineButton.GetComponent<EventTrigger>().triggers.Add(event3);

        dropButton.AddComponent<EventTrigger>();
        dropButton.GetComponent<EventTrigger>().triggers.Add(event1);
        dropButton.GetComponent<EventTrigger>().triggers.Add(event2);
        dropButton.GetComponent<EventTrigger>().triggers.Add(event3);
    }

    public void Init(GameObject iObject)
    {
        if (cameraMode.CurrentMode != CameraMode.Mode.Free)
        {
            return;
        }
        
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerScript>();
        }

        initedObject = iObject;

        if (initedObject != null && initedObject.GetComponent<InteractableObject>() != null)
        {
            cameraMode.ToggleCameraMode(CameraMode.Mode.ItemControlsUI);

            if (initedObject.GetComponent<PickableObject>() != null
                 && handsInventory.IsInHand(initedObject))
            {
                useOnButton.SetActive(true);
                useOnNTButton.SetActive(true);
                combineButton.SetActive(true);
                dropButton.SetActive(true);

                pickButton.SetActive(false);
                useButton.SetActive(false);
                talkButton.SetActive(false);

                examineButton.SetActive(initedObject.GetComponent<ExaminableObject>() != null);
            }
            else
            {
                if (initedObject.GetComponent<ExaminableObject>() != null)
                {
                    examineButton.SetActive(!initedObject.GetComponent<ExaminableObject>().animationExamine);
                }
                else
                {
                    examineButton.SetActive(false);
                }

                pickButton.SetActive(initedObject.GetComponent<PickableObject>() != null);
                useButton.SetActive(initedObject.GetComponent<UsableObject>() != null);

                talkButton.SetActive(initedObject.GetComponent<PersonObjectPart>() != null);
                if (talkButton.activeSelf)
                {
                    initedObject = initedObject.GetComponent<PersonObjectPart>().Person.gameObject;
                }

                useOnButton.SetActive(false);
                useOnNTButton.SetActive(false);
                combineButton.SetActive(false);
                dropButton.SetActive(false);
            }

            closeButton.SetActive(true);

            //talkin removed
            if (talkButton.activeSelf)
            {
                if (controls.CanInteract)
                {
                    Talk();
                }
                else
                {
                    cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
                    return;
                }
            }
            else if (pickButton.activeSelf && !examineButton.activeSelf)
            {
                Pick();
            }
            else if (!pickButton.activeSelf && examineButton.activeSelf && !handsInventory.IsInHand(initedObject))
            {
                Examine();
            }
            else if (useButton.activeSelf)
            {
                Use();
            }
            else
            {
                transform.position = Input.mousePosition + new Vector3(cursorOffset.x, cursorOffset.y);
                gameObject.SetActive(true);
                GameObject.FindObjectOfType<RobotManager>().ToggleTrigger(false);

                GameObject descrGroup = GameObject.Find("ItemDescriptionGroup");

                float lowerBound = 330.0f;
                if (transform.position.y < lowerBound)
                {
                    float difference = lowerBound - transform.position.y;
                    transform.position += new Vector3(0.0f, difference, 0.0f);
                    if (descrGroup)
                    {
                        descrGroup.transform.position += new Vector3(0.0f, difference, 0.0f);
                    }
                }

                float rightBound = -125.0f;
                if (transform.position.x > Screen.width - rightBound)
                {
                    float difference = (Screen.width - rightBound) - transform.position.x;
                    transform.position += new Vector3(difference, 0.0f, 0.0f);
                    if (descrGroup)
                    {
                        descrGroup.transform.position += new Vector3(difference, 0.0f, 0.0f);
                    }
                }

                useOnNTButton.transform.GetChild(0).GetComponent<Text>().text =
                    (actionManager.CompareUseOnInfo(initedObject.name, "") ?
                    actionManager.CurrentButtonText : useOnNTtext);

                useButton.transform.GetChild(0).GetComponent<Text>().text =
                    (actionManager.CompareUseObject(initedObject.name)) ? actionManager.CurrentButtonText : useText;

                useOnNTButton.SetActive(actionManager.CompareUseOnInfo(initedObject.name, ""));
                useButton.SetActive(actionManager.CompareUseObject(initedObject.name));
            }

            if (!pickButton.activeSelf && 
                !examineButton.activeSelf && 
                !useButton.activeSelf && 
                !talkButton.activeSelf && 
                !useOnButton.activeSelf && 
                !useOnNTButton.activeSelf && 
                !combineButton.activeSelf && 
                !dropButton.activeSelf)
            {
                Close();
            }
        }
    }

    private void Update()
    {
        if (initedObject != null)
        {
            if (!UIhover && Input.GetMouseButtonDown(0))
            {
                Close();
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        GameObject.FindObjectOfType<RobotManager>().ToggleTrigger(true);
        if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
        {
            cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
        }
        UIhover = false;
    }

    public void Use()
    {
        if (initedObject.name == "ClothPackage")
        {
            initedObject.GetComponent<UsableObject>().Use();
        }
        else if (actionManager.CompareUseObject(initedObject.name) ||
            (actionManager.CompareUseObject("HandCleaner") 
            && initedObject.name == "WorkField"))
        {
            if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
            {
                if (handsInventory.Empty())
                {
                    initedObject.GetComponent<UsableObject>().Use();
                }
                else
                {
                    string message = "Zorg ervoor dat alle materialen die je hebt gebruikt op het werkveld liggen. Maak je handen vrij door eventuele objecten terug te leggen op het werkveld.";
                    Camera.main.transform.Find("UI").Find("EmptyHandsWarning").
                            GetComponent<TimedPopUp>().Set(message);
                }
            }
        }

        Close();
    }

    public void Examine()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
        {
            cameraMode.selectedObject = initedObject.GetComponent<ExaminableObject>();
            if (cameraMode.selectedObject != null) // if there is a component
            {
                cameraMode.selectedObject.OnExamine();
                controls.ResetObject();
            }
            /*else if (initedObject.GetComponent<SystemObject>() != null)
            {
                cameraMode.doorSelected = controls.SelectedObject.GetComponent<SystemObject>();
                cameraMode.doorSelected.Use();
            }*/
        }

        Close();
    }

    public void Pick()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
        {
            if (initedObject != null)
            {
                PickableObject item = initedObject.GetComponent<PickableObject>();
                if (item != null)
                {
                    if (tutorial == null ||
                        (tutorial != null &&
                            (item.name == tutorial.itemToPick ||
                            item.name == tutorial.itemToPick2)))
                    {
                        handsInventory.PickItem(item);
                    }
                    else
                    {
                        string message = "Volg de tips links bovenin het scherm om verder te gaan.";
                        Camera.main.transform.Find("UI").Find("EmptyHandsWarning").
                                GetComponent<TimedPopUp>().Set(message);
                    }

                    controls.ResetObject();
                }
            }
        }

        Close();
    }

    public void Talk()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
        {
            if (initedObject != null)
            {
                initedObject.GetComponent<PersonObject>().CreateSelectionDialogue();
                GameObject.Find("ItemDescription").SetActive(false);
            }
        }

        Close();
    }

    public void Combine()
    {
        if (handsInventory.LeftHandEmpty() ^ handsInventory.RightHandEmpty())
        {
            handsInventory.OnCombineAction();
        }

        Close();
    }

    public void Drop()
    {
        if (initedObject != null)
        {
            if (handsInventory.LeftHandObject == initedObject)
            {
                handsInventory.DropLeft();
            }
            else if (handsInventory.RightHandObject == initedObject)
            {
                handsInventory.DropRight();
            }
        }

        Close();
    }

    public void UseOn()
    {
        player.usingOnHand = initedObject == handsInventory.LeftHandObject;
        player.ToggleUsingOnMode(true);

        Close();
    }

    public void UseOnNoTarget()
    {
        if (initedObject == handsInventory.LeftHandObject)
        {
            handsInventory.LeftHandObject.GetComponent<PickableObject>().Use(true, true);
        }
        else
        {
            handsInventory.RightHandObject.GetComponent<PickableObject>().Use(false, true);
        }

        Close();
    }
}
