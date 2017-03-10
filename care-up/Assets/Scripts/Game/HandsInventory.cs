using UnityEngine;
using System.Collections;

/// <summary>
/// Handles things in hands.
/// </summary>
public class HandsInventory : MonoBehaviour {

    // tutorial special variables.
    // TODO: hide from unity editor.
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
    private Vector3 glovesPosition;
    private Quaternion glovesRotation;
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

        // calculate position of objects in hands
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

        // handle player actions in free mode
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            // drop left object
            if (controls.keyPreferences.LeftDropKey.Pressed())
            {
                if (leftHandObject)
                {
                    tutorial_droppedLeft = true;
                    leftHandObject.Drop();
                    leftHandObject = null;
                }
            }

            // drop right object
            if (controls.keyPreferences.RightDropKey.Pressed())
            {
                if (rightHandObject)
                {
                    tutorial_droppedRight = true;
                    rightHandObject.Drop();
                    rightHandObject = null;
                }
            }

            // combine key pressed, if combine performed - handle object changes
            if (controls.keyPreferences.CombineKey.Pressed())
            {
                string leftName = leftHandObject ? leftHandObject.name : "";
                string rightName = rightHandObject ? rightHandObject.name : "";

                string[] currentObjects = actionManager.CurrentCombineObjects;
                bool combineAllowed = (currentObjects[0] == leftName && currentObjects[1] == rightName)
                    || (currentObjects[0] == rightName && currentObjects[1] == leftName);

                string leftResult, rightResult;
                bool combined = combinationManager.Combine(leftName, rightName, out leftResult, out rightResult);

                // combine performed
                if (combined && combineAllowed)
                {
                    tutorial_combined = true;
                    
                    Vector3 leftSavedPos = Vector3.zero;
                    Quaternion leftSavedRot = Quaternion.identity;

                    if (leftHandObject != null)
                    {
                        leftHandObject.GetSavesLocation(out leftSavedPos, out leftSavedRot);
                    }

                    Vector3 rightSavedPos = Vector3.zero;
                    Quaternion rightSavedRot = Quaternion.identity;

                    if (rightHandObject != null)
                    {
                        rightHandObject.GetSavesLocation(out rightSavedPos, out rightSavedRot);
                    }

                    // object changed
                    if (leftName != leftResult)
                    {
                        if (leftHandObject != null)
                        {
                            Destroy(leftHandObject.gameObject);
                            leftHandObject = null;
                        }

                        if (leftResult != "")
                        {
                            GameObject leftObject = CreateObjectByName(leftResult, leftHandPosition);
                            leftHandObject = leftObject.GetComponent<PickableObject>();

                            if (leftSavedPos != Vector3.zero)
                            {
                                leftHandObject.SavePosition(leftSavedPos, leftSavedRot);
                            }
                            else if (rightSavedPos != Vector3.zero)
                            {
                                float offset = rightHandObject.GetComponent<Renderer>().bounds.size.x
                                    + leftHandObject.GetComponent<Renderer>().bounds.size.x; ;
                                leftHandObject.SavePosition(rightSavedPos + new Vector3(offset, 0), rightSavedRot);
                            }
                        }
                    }

                    // object changed
                    if (rightName != rightResult)
                    {
                        if (rightHandObject != null)
                        {
                            Destroy(rightHandObject.gameObject);
                            rightHandObject = null;
                        }

                        if (rightResult != "")
                        {
                            GameObject rightObject = CreateObjectByName(rightResult, rightHandPosition);
                            rightHandObject = rightObject.GetComponent<PickableObject>();

                            if (rightSavedPos != Vector3.zero)
                            {
                                rightHandObject.SavePosition(rightSavedPos, rightSavedRot);
                            }
                            else if (leftSavedPos != Vector3.zero)
                            {
                                float offset = leftHandObject.GetComponent<Renderer>().bounds.size.x
                                    + rightHandObject.GetComponent<Renderer>().bounds.size.x;
                                rightHandObject.SavePosition(leftSavedPos + new Vector3(offset, 0), leftSavedRot);
                            }
                        }
                    }
                }
            }

            // use left object
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

            // use right object
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

        // handle picking items
        if (controls.MouseClicked() && cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject != null && controls.CanInteract)
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
    
    /// <summary>
    /// Proper picking, left hand first, right second
    /// </summary>
    /// <param name="item">Object picked</param>
    /// <param name="hand">If certain hand forced</param>
    /// <returns></returns>
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

    /// <summary>
    /// After combining a new object can appear on the scene.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <param name="position">Position of the object</param>
    /// <returns>Object created.</returns>
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

    /// <summary>
    /// Checks if hands are empty
    /// </summary>
    /// <returns>True if empty</returns>
    public bool Empty()
    {
        return (leftHandObject == null) && (rightHandObject == null);
    }

    public bool OneHandEmpty()
    {
        return (leftHandObject == null) || (rightHandObject == null);
    }

    /// <summary>
    /// Puts on or takes of gloves.
    /// </summary>
    /// <param name="value">True - puts on, False - takes off</param>
    public void GlovesToggle(bool value)
    {
        if (value)
        {
            if ((leftHandObject && !rightHandObject)
                || (!leftHandObject && rightHandObject))
            {
                if (leftHandObject)
                {
                    leftHandObject.GetSavesLocation(out glovesPosition, out glovesRotation);
                    Destroy(leftHandObject.gameObject);
                }
                else if (rightHandObject)
                {
                    rightHandObject.GetSavesLocation(out glovesPosition, out glovesRotation);
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
            leftHandObject.SavePosition(glovesPosition, glovesRotation);
        }
    }

    /// <summary>
    /// Forces drop both objects. Used during animation sequences.
    /// </summary>
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

    /// <summary>
    /// Used during animation sequence to force pick.
    /// </summary>
    public void PickTestStrips()
    {
        leftHandObject = GameObject.Find("TestStrips").GetComponent<PickableObject>();
    }
}
