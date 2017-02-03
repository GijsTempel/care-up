using UnityEngine;
using System.Collections;

public class HandsInventory : MonoBehaviour {

    public bool tutorial_pickedLeft = false;
    public bool tutorial_pickedRight = false;
    public bool tutorial_droppedLeft = false;
    public bool tutorial_droppedRight = false;
    public bool tutorial_combined = false;
    public bool tutorial_itemUsedOn = false;

    // position in air 
    public float horisontalOffset = 0.5f;
    public float distanceFromCamera = 1.0f;

    private PickableObject leftHandObject;
    private PickableObject rightHandObject;

    private Vector3 leftHandPosition;
    private Vector3 rightHandPosition;
    private bool glovesOn = false;

    private CombinationManager combinationManager;
    private GameObject interactableObjects;
    private Controls controls;
    private CameraMode cameraMode;

    private ActionManager actionManager;

    public GameObject LeftHandObject
    {
        get { return leftHandObject ? leftHandObject.gameObject : null; }
    }

    public GameObject RightHandObject
    {
        get { return rightHandObject ? rightHandObject.gameObject : null; }
    }

    void Start()
    {
        combinationManager = GameObject.Find("GameLogic").GetComponent<CombinationManager>();
        if (combinationManager == null) Debug.LogError("No combination manager found.");

        interactableObjects = GameObject.Find("Interactable Objects");
        if (interactableObjects == null) Debug.LogError("No Interactable Objets object found");

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No Action Manager found.");

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        if (controls == null) Debug.LogError("No controls found");

        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (cameraMode == null) Debug.LogError("No camera mode found");

        glovesOn = false;
    }

    void Update() {

        leftHandPosition = Camera.main.transform.position +
                Camera.main.transform.forward * distanceFromCamera +
                Camera.main.transform.right * (-horisontalOffset);
        rightHandPosition = Camera.main.transform.position +
                Camera.main.transform.forward * distanceFromCamera +
                Camera.main.transform.right * horisontalOffset;

        if (leftHandObject)
        {
            leftHandObject.transform.position = leftHandPosition;
            leftHandObject.InHandUpdate(false);
        }

        if (rightHandObject)
        {
            rightHandObject.transform.position = rightHandPosition;
            rightHandObject.InHandUpdate(true);
        }

        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.keyPreferences.LeftDropKey.Pressed())
            {
                if (leftHandObject)
                {
                    tutorial_droppedLeft = true;
                    leftHandObject.Drop();
                    leftHandObject = null;
                }
            }

            if (controls.keyPreferences.RightDropKey.Pressed())
            {
                if (rightHandObject)
                {
                    tutorial_droppedRight = true;
                    rightHandObject.Drop();
                    rightHandObject = null;
                }
            }

            if (controls.keyPreferences.CombineKey.Pressed())
            {
                string leftName = leftHandObject ? leftHandObject.name : "";
                string rightName = rightHandObject ? rightHandObject.name : "";

                string[] currentObjects = actionManager.CurrentCombineObjects;
                bool combineAllowed = (currentObjects[0] == leftName && currentObjects[1] == rightName)
                    || (currentObjects[0] == rightName && currentObjects[1] == leftName);

                string leftResult, rightResult;
                bool combined = combinationManager.Combine(leftName, rightName, out leftResult, out rightResult);

                if (combined && combineAllowed)
                {
                    tutorial_combined = true;
                    if (leftName != leftResult)
                    {
                        Vector3 leftSavedPos = Vector3.zero;
                        Quaternion leftSavedRot = Quaternion.identity;

                        if (leftHandObject != null)
                        {
                            leftHandObject.GetSavesLocation(out leftSavedPos, out leftSavedRot);

                            Destroy(leftHandObject.gameObject);
                            leftHandObject = null;
                        }

                        if (leftResult != "")
                        {
                            GameObject leftObject = CreateObjectByName(leftResult, leftHandPosition);
                            leftHandObject = leftObject.GetComponent<PickableObject>();

                            leftHandObject.SavePosition(leftSavedPos, leftSavedRot);
                        }
                    }

                    if (rightName != rightResult)
                    {
                        Vector3 rightSavedPos = Vector3.zero;
                        Quaternion rightSavedRot = Quaternion.identity;

                        if (rightHandObject != null)
                        {
                            rightHandObject.GetSavesLocation(out rightSavedPos, out rightSavedRot);

                            Destroy(rightHandObject.gameObject);
                            rightHandObject = null;
                        }

                        if (rightResult != "")
                        {
                            GameObject rightObject = CreateObjectByName(rightResult, rightHandPosition);
                            rightHandObject = rightObject.GetComponent<PickableObject>();

                            rightHandObject.SavePosition(rightSavedPos, rightSavedRot);
                        }
                    }
                }
            }

            if (controls.keyPreferences.LeftUseKey.Pressed())
            {
                if (leftHandObject != null)
                {
                    if (leftHandObject.Use())
                    {
                        tutorial_itemUsedOn = true;
                    }
                }
                else if (glovesOn && rightHandObject == null)
                {
                    actionManager.OnUseOnAction("", "");
                    GlovesToggle(false);
                }
            }

            if (controls.keyPreferences.RightUseKey.Pressed())
            {
                if (rightHandObject)
                {
                    if (rightHandObject.Use())
                    {
                        tutorial_itemUsedOn = true;
                    }
                }
                else if (glovesOn && leftHandObject == null)
                {
                    actionManager.OnUseOnAction("", "");
                    GlovesToggle(false);
                }
            }
        }

        if (controls.MouseClicked() && cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject != null)
            {
                PickableObject item = controls.SelectedObject.GetComponent<PickableObject>();
                if (item != null)
                {
                    if (item.GetComponent<ExaminableObject>() == null)
                    {
                        PickItem(item);
                        controls.ResetObject();
                    }
                }
            }
        }
    }

    public bool PickItem(PickableObject item, string hand = "")
    {
        bool picked = false;
        if (hand == "")
        {
            if (leftHandObject == null)
            {
                leftHandObject = item;
                picked = true;
            }
            else if (rightHandObject == null)
            {
                rightHandObject = item;
                picked = true;
            }
        }
        else if (hand == "left")
        {
            if (leftHandObject == null)
            {
                leftHandObject = item;
                picked = true;
            }
        }
        else if (hand == "right")
        {
            if (rightHandObject == null)
            {
                rightHandObject = item;
                picked = true;
            }
        }

        if (picked)
        {
            item.SavePosition();

            if (leftHandObject)
            {
                tutorial_pickedLeft = true;
                leftHandObject.GetComponent<Rigidbody>().useGravity = false;
                leftHandObject.GetComponent<Collider>().enabled = false;
                actionManager.OnPickUpAction(leftHandObject.name);
            }
            if (rightHandObject)
            {
                tutorial_pickedRight = true;
                rightHandObject.GetComponent<Rigidbody>().useGravity = false;
                rightHandObject.GetComponent<Collider>().enabled = false;
                actionManager.OnPickUpAction(rightHandObject.name);
            }
        }

        return picked;
    }

    private GameObject CreateObjectByName(string name, Vector3 position)
    {
        GameObject newObject = Instantiate(Resources.Load<GameObject>("Prefabs\\" + name),
                            position, Quaternion.identity) as GameObject;

        newObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        newObject.GetComponent<Rigidbody>().useGravity = false;
        newObject.GetComponent<Collider>().enabled = false;
        newObject.transform.parent = interactableObjects.transform;
        newObject.name = name;

        GameObject wf = GameObject.Find("WorkField");
        if (wf != null)
        {
            wf.GetComponent<WorkField>().objects.Add(newObject);
        }
        return newObject;
    }

    public bool Empty()
    {
        return (leftHandObject == null) && (rightHandObject == null);
    }

    public void GlovesToggle(bool value)
    {
        if (value)
        {
            if ((leftHandObject && !rightHandObject)
                || (!leftHandObject && rightHandObject))
            {
                if (leftHandObject)
                {
                    Destroy(leftHandObject.gameObject);
                }
                else if (rightHandObject)
                {
                    Destroy(rightHandObject.gameObject);
                }
                glovesOn = value;
            }
        }
        else
        {
            glovesOn = value;
            GameObject leftObject = CreateObjectByName("Gloves", leftHandPosition);
            leftHandObject = leftObject.GetComponent<PickableObject>();
        }
    }

    public void PutAllOnTable()
    {
        if (leftHandObject)
        {
            leftHandObject.Drop(true);
            leftHandObject = null;
        }

        if (rightHandObject)
        {
            rightHandObject.Drop(true);
            rightHandObject = null;
        }
    }
}
