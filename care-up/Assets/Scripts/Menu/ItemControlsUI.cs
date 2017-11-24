using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
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
    }

    public void Init()
    {
        cameraMode.ToggleCameraMode(CameraMode.Mode.ItemControlsUI);

        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerScript>();
        }

        initedObject = controls.SelectedObject;

        if (initedObject != null && initedObject.GetComponent<InteractableObject>() != null)
        {
            if (initedObject.GetComponent<PickableObject>() != null
                 && handsInventory.IsInHand(initedObject))
            {
                useOnButton.SetActive(true);
                useOnNTButton.SetActive(true);
                combineButton.SetActive(true);
                dropButton.SetActive(true);

                pickButton.SetActive(false);
                examineButton.SetActive(false);
                useButton.SetActive(false);
                talkButton.SetActive(false);
            }
            else
            {
                pickButton.SetActive(initedObject.GetComponent<PickableObject>() != null);
                examineButton.SetActive(initedObject.GetComponent<ExaminableObject>() != null);
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

            //transform.position = initedObject.transform.position +
            //    Camera.main.transform.right * 0.2f;
            //transform.rotation = Camera.main.transform.rotation;

            //talkin removed
            if (talkButton.activeSelf)
            {
                Talk();
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (initedObject != null)
        {
            if (Input.GetMouseButton(1))
            {
                Close();
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
        {
            cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
        }
    }

    public void Use()
    {
        if (actionManager.CurrentUseObject == initedObject.name ||
            (actionManager.CurrentUseObject == "HandCleaner" 
            && initedObject.name == "WorkField"))
        {
            if (cameraMode.CurrentMode == CameraMode.Mode.ItemControlsUI)
            {
                if (handsInventory.Empty())
                {
                    initedObject.GetComponent<UsableObject>().Use();
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
            else if (initedObject.GetComponent<SystemObject>() != null)
            {
                cameraMode.doorSelected = controls.SelectedObject.GetComponent<SystemObject>();
                cameraMode.doorSelected.Use();
            }
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
            }
        }

        Close();
    }

    public void Combine()
    {
        handsInventory.OnCombineAction();

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
        player.usingOnMode = true;
        player.usingOnHand = initedObject == handsInventory.LeftHandObject;

        Close();
    }

    public void UseOnNoTarget()
    {
        if (initedObject == handsInventory.LeftHandObject)
        {
            handsInventory.LeftHandObject.GetComponent<PickableObject>().Use(true);
        }
        else
        {
            handsInventory.RightHandObject.GetComponent<PickableObject>().Use(false);
        }

        Close();
    }
}
