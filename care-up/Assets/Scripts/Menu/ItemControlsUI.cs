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

    private GameObject closeButton;

    private GameObject pickButton;
    private GameObject examineButton;
    private GameObject useButton;
    private GameObject talkButton;
    private GameObject useOnButton;
    private GameObject combineButton;

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
        combineButton = transform.Find("CombineButton").gameObject;

        closeButton = transform.Find("CloseButton").gameObject;
    }

    public void Init()
    {
        initedObject = controls.SelectedObject;

        if (initedObject != null && initedObject.GetComponent<InteractableObject>() != null)
        {
            pickButton.SetActive(initedObject.GetComponent<PickableObject>() != null 
                && initedObject.GetComponent<ExaminableObject>() == null);
            examineButton.SetActive(initedObject.GetComponent<ExaminableObject>() != null);
            useButton.SetActive(initedObject.GetComponent<UsableObject>() != null);
            talkButton.SetActive(initedObject.GetComponent<PersonObjectPart>() != null);

            useOnButton.SetActive(false);
            combineButton.SetActive(false);

            closeButton.SetActive(true);

            //transform.position = initedObject.transform.position +
            //    Camera.main.transform.right * 0.2f;
            //transform.rotation = Camera.main.transform.rotation;
            gameObject.SetActive(true);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Use()
    {
        if (actionManager.CurrentUseObject == initedObject.name ||
            (actionManager.CurrentUseObject == "HandCleaner" 
            && initedObject.name == "WorkField"))
        {
            if (cameraMode.CurrentMode == CameraMode.Mode.Free)
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
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
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
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (initedObject != null)
            {
                PickableObject item = initedObject.GetComponent<PickableObject>();
                if (item != null)
                {
                    if (item.GetComponent<ExaminableObject>() == null)
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
        }

        Close();
    }
}
