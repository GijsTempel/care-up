using UnityEngine;
using System.Collections;

/// <summary>
/// Handles things in hands.
/// </summary>
public class HandsInventory : MonoBehaviour {

    // tutorial special variables.
    [HideInInspector]
    public bool tutorial_pickedLeft = false;
    [HideInInspector]
    public bool tutorial_pickedRight = false;
    [HideInInspector]
    public bool tutorial_droppedLeft = false;
    [HideInInspector]
    public bool tutorial_droppedRight = false;
    [HideInInspector]
    public bool tutorial_combined = false;
    [HideInInspector]
    public bool tutorial_itemUsedOn = false;

    // position in air 
    public float horisontalOffset = 0.5f;
    public float distanceFromCamera = 1.0f;
    public bool dropPenalty = true;

    private Transform leftToolHolder;
    private Transform rightToolHolder;
    private Transform leftControlBone;
    private Transform rightControlBone;

    private PickableObject leftHandObject;
    private PickableObject rightHandObject;
    private bool leftHold = false;
    private bool rightHold = false;
    
    private bool combineDelayed = false;
    private string leftCombineResult;
    private string rightCombineResult;
    
    private Vector3 glovesPosition;
    private Quaternion glovesRotation;
    private bool glovesOn = false;

    private GameObject animationObject;

    private CombinationManager combinationManager;
    private GameObject interactableObjects;
    private Controls controls;
    private CameraMode cameraMode;

    private ActionManager actionManager;

    private TutorialManager tutorial;

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

        tutorial = GetComponent<TutorialManager>();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("playerBone"))
        {
            switch (obj.name)
            {
                case "prop.L":
                    leftControlBone = obj.transform;
                    break;
                case "prop.R":
                    rightControlBone = obj.transform;
                    break;
                case "toolHolder.L":
                    leftToolHolder = obj.transform;
                    break;
                case "toolHolder.R":
                    rightToolHolder = obj.transform;
                    break;
            }
        }

        glovesOn = false;
    }

    void Update() {

        if (leftHandObject && leftHold)
            leftHandObject.transform.localPosition = Vector3.zero;

        if (rightHandObject && rightHold)
            rightHandObject.transform.localPosition = Vector3.zero;

        // handle player actions in free mode
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            // drop left object
            if (controls.keyPreferences.LeftDropKey.Pressed())
            {
                if (leftHandObject)
                {
                    if (tutorial == null || (tutorial != null &&
                    (tutorial.itemToDrop == leftHandObject.name ||
                    tutorial.itemToDrop2 == LeftHandObject.name)))
                    {
                        leftHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
                        tutorial_droppedLeft = true;
                        if (!leftHandObject.Drop() && dropPenalty)
                        {
                            Narrator.PlaySound("WrongAction");
                            actionManager.Points--;
                        }
                        leftHandObject = null;
                        leftHold = false;
                        PlayerAnimationManager.SetHandItem(true, null);
                    }
                }
            }

            // drop right object
            if (controls.keyPreferences.RightDropKey.Pressed())
            {
                if (rightHandObject)
                {
                    if (tutorial == null || (tutorial != null &&
                    (tutorial.itemToDrop == rightHandObject.name ||
                    tutorial.itemToDrop2 == rightHandObject.name)))
                    {
                        rightHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
                        tutorial_droppedRight = true;
                        if (!rightHandObject.Drop() && dropPenalty)
                        {
                            Narrator.PlaySound("WrongAction");
                            actionManager.Points--;
                        }
                        rightHandObject = null;
                        rightHold = false;
                        PlayerAnimationManager.SetHandItem(false, null);
                    }
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
                
                bool combined = combinationManager.Combine(leftName, rightName, out leftCombineResult, out rightCombineResult);

                // combine performed
                if (combined && combineAllowed)
                {
                    tutorial_combined = true;
                    
                    string combineAnimation = "Combine " +
                        (leftHandObject ? leftHandObject.name : "_") + " " +
                        (rightHandObject ? rightHandObject.name : "_");
                    PlayerAnimationManager.PlayAnimation(combineAnimation);

                    combineDelayed = true;
                    ToggleControls(true);
                }
            }

            // use left object
            if (controls.keyPreferences.LeftUseKey.Pressed())
            {
                if (leftHandObject != null)
                {
                    if (leftHandObject.Use(false))
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
                    if (rightHandObject.Use(true))
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
                        if (tutorial == null || 
                            (tutorial != null && 
                                (item.name == tutorial.itemToPick || 
                                item.name == tutorial.itemToPick2)))
                        {
                            PickItem(item);
                        }

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
                tutorial_pickedLeft = true;
                leftHandObject.GetComponent<Rigidbody>().useGravity = false; 
                leftHandObject.GetComponent<Collider>().enabled = false;
                leftHandObject.controlBone = leftControlBone;
                actionManager.OnPickUpAction(leftHandObject.name);
                PlayerAnimationManager.PlayAnimation("LeftPick");
                PlayerAnimationManager.SetHandItem(true, item.gameObject);
                leftHold = false;
            }
            else if (rightHandObject == null)
            {
                rightHandObject = item;
                picked = true;
                tutorial_pickedRight = true;
                rightHandObject.GetComponent<Rigidbody>().useGravity = false;
                rightHandObject.GetComponent<Collider>().enabled = false;
                rightHandObject.controlBone = leftControlBone; // TODO
                actionManager.OnPickUpAction(rightHandObject.name);
                PlayerAnimationManager.PlayAnimation("RightPick");
                PlayerAnimationManager.SetHandItem(false, item.gameObject);
                rightHold = false;
            }
        }
        else if (hand == "left")
        {
            if (leftHandObject == null)
            {
                leftHandObject = item;
                picked = true;
                tutorial_pickedLeft = true;
                leftHandObject.GetComponent<Rigidbody>().useGravity = false;
                leftHandObject.GetComponent<Collider>().enabled = false;
                leftHandObject.controlBone = leftControlBone;
                actionManager.OnPickUpAction(leftHandObject.name);
                PlayerAnimationManager.PlayAnimation("LeftPick");
                PlayerAnimationManager.SetHandItem(true, item.gameObject);
            }
        }
        else if (hand == "right")
        {
            if (rightHandObject == null)
            {
                rightHandObject = item;
                picked = true;
                tutorial_pickedRight = true;
                rightHandObject.GetComponent<Rigidbody>().useGravity = false;
                rightHandObject.GetComponent<Collider>().enabled = false;
                rightHandObject.controlBone = leftControlBone; // TODO
                actionManager.OnPickUpAction(rightHandObject.name);
                PlayerAnimationManager.PlayAnimation("RightPick");
                PlayerAnimationManager.SetHandItem(false, item.gameObject);
            }
        }

        if (picked)
        {
            item.SavePosition();
        }

        return picked;
    }

    public void SwapHands()
    {
        PickableObject t = leftHandObject;
        leftHandObject = rightHandObject;
        rightHandObject = t;

        if (leftHandObject)
            SetHold(true);
        if (rightHandObject)
            SetHold(false);

        UpdateHoldAnimation();
    }

    public void UpdateHoldAnimation()
    {
        PlayerAnimationManager.SetHandItem(true, leftHandObject == null ? null : leftHandObject.gameObject);
        PlayerAnimationManager.SetHandItem(false, rightHandObject == null ? null : rightHandObject.gameObject);
    }

    public void RemoveHandObject(bool hand)
    {
        if (!hand)
        {
            Destroy(leftHandObject.gameObject);
            leftHandObject = null;
        }
        else
        {
            Destroy(rightHandObject.gameObject);
            rightHandObject = null;
        }
        UpdateHoldAnimation();
    }

    public void ReplaceHandObject(bool hand, string name)
    {
        if (hand)
        {
            Vector3 leftSavedPos = Vector3.zero;
            Quaternion leftSavedRot = Quaternion.identity;
            leftHandObject.GetSavesLocation(out leftSavedPos, out leftSavedRot);
            
            Vector3 plungerPosition = new Vector3();
            bool updatePlunger = false;
            if (leftHandObject.GetComponent<Syringe>() != null)
            {
                plungerPosition = leftHandObject.GetComponent<Syringe>().PlungerPosition;
                updatePlunger = leftHandObject.GetComponent<Syringe>().updatePlunger;
            }

            Destroy(leftHandObject.gameObject);
            leftHandObject = null;
            
            GameObject leftObject = CreateObjectByName(name, Vector3.zero);
            leftHandObject = leftObject.GetComponent<PickableObject>();
            leftHandObject.controlBone = leftControlBone;
            SetHold(true);

            PlayerAnimationManager.SetHandItem(true, leftObject);

            if (leftSavedPos != Vector3.zero)
            {
                leftHandObject.SavePosition(leftSavedPos, leftSavedRot);
            }

            if (leftHandObject.GetComponent<Syringe>() != null)
            {
                leftHandObject.GetComponent<Syringe>().PlungerPosition = plungerPosition;
                leftHandObject.GetComponent<Syringe>().updatePlunger = updatePlunger;
            }
        }
        else
        {
            Vector3 rightSavedPos = Vector3.zero;
            Quaternion rightSavedRot = Quaternion.identity;
            rightHandObject.GetSavesLocation(out rightSavedPos, out rightSavedRot);

            Vector3 plungerPosition = new Vector3();
            bool updatePlunger = false;
            if (rightHandObject.GetComponent<Syringe>() != null)
            {
                plungerPosition = rightHandObject.GetComponent<Syringe>().PlungerPosition;
                updatePlunger = rightHandObject.GetComponent<Syringe>().updatePlunger;
            }

            Destroy(rightHandObject.gameObject);
            rightHandObject = null;

            GameObject rightObject = CreateObjectByName(name, Vector3.zero);
            rightHandObject = rightObject.GetComponent<PickableObject>();
            rightHandObject.controlBone = leftControlBone;
            SetHold(false);

            PlayerAnimationManager.SetHandItem(false, rightObject);

            if (rightSavedPos != Vector3.zero)
            {
                rightHandObject.SavePosition(rightSavedPos, rightSavedRot);
            }

            if (rightHandObject.GetComponent<Syringe>() != null)
            {
                rightHandObject.GetComponent<Syringe>().PlungerPosition = plungerPosition;
                rightHandObject.GetComponent<Syringe>().updatePlunger = updatePlunger;
            }
        }

        UpdateHoldAnimation();
    }

    /// <summary>
    /// After combining a new object can appear on the scene.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <param name="position">Position of the object</param>
    /// <returns>Object created.</returns>
    public GameObject CreateObjectByName(string name, Vector3 position)
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

    public void CreateAnimationObject(string name, bool hand)
    {
        animationObject = Instantiate(Resources.Load<GameObject>("Prefabs\\" + name),
                            Vector3.zero, Quaternion.identity) as GameObject;

        animationObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        animationObject.GetComponent<Rigidbody>().useGravity = false;
        animationObject.GetComponent<Collider>().enabled = false;
        animationObject.name = name;
        
        animationObject.transform.parent = hand ? leftToolHolder : rightToolHolder;
        animationObject.transform.localPosition = Vector3.zero;
        animationObject.transform.localRotation = Quaternion.identity;

        animationObject.GetComponent<PickableObject>().Pick();
    }

    public void DeleteAnimationObject()
    {
        Destroy(animationObject);
        animationObject = null;
    }

    /// <summary>
    /// Checks if hands are empty
    /// </summary>
    /// <returns>True if empty</returns>
    public bool Empty()
    {
        return (leftHandObject == null) && (rightHandObject == null);
    }

    public bool LeftHandEmpty()
    {
        return (leftHandObject == null);
    }

    public bool RightHandEmpty()
    {
        return (rightHandObject == null);
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
            GameObject leftObject = CreateObjectByName("Gloves", Vector3.zero);
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
            leftHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
            leftHandObject.Drop(true);
            leftHandObject = null;
            leftHold = false;
            PlayerAnimationManager.SetHandItem(true, null);
        }

        if (rightHandObject)
        {
            rightHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
            rightHandObject.Drop(true);
            rightHandObject = null;
            rightHold = false;
            PlayerAnimationManager.SetHandItem(false, null);
        }
    }

    public void DropLeftObject()
    {
        if (leftHandObject)
        {
            leftHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
            leftHandObject.Drop(true);
            leftHandObject = null;
            leftHold = false;
            PlayerAnimationManager.SetHandItem(true, null);
        }
    }

    public void DropRightObject()
    {
        if (rightHandObject)
        {
            rightHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
            rightHandObject.Drop(true);
            rightHandObject = null;
            rightHold = false;
            PlayerAnimationManager.SetHandItem(false, null);
        }
    }

    public void ForcePickItem(string name, bool hand)
    {
        if (hand)
        {
            leftHandObject = GameObject.Find(name).GetComponent<PickableObject>();
            leftHandObject.GetComponent<Rigidbody>().useGravity = false;
            leftHandObject.GetComponent<Collider>().enabled = false;
            leftHandObject.controlBone = leftControlBone;
            leftHandObject.SavePosition();
        }
        else
        {
            rightHandObject = GameObject.Find(name).GetComponent<PickableObject>();
            rightHandObject.GetComponent<Rigidbody>().useGravity = false;
            rightHandObject.GetComponent<Collider>().enabled = false;
            rightHandObject.controlBone = leftControlBone;
            rightHandObject.SavePosition();
        }
        SetHold(hand);
    }

    /// <summary>
    /// Hold object now (from animation behaviour) at certain frame.
    /// </summary>
    /// <param name="hand">True = left, false = right</param>
    public void SetHold(bool hand)
    {
        if (hand)
        {
            if (leftToolHolder == null)
            {
                Debug.LogWarning("LeftToolHolder not set, can cause problems");
            }

            leftHold = true;
            leftHandObject.transform.parent = (leftToolHolder == null) ?
                GameObject.Find("toolHolder.L").transform : leftToolHolder;
            leftHandObject.transform.localPosition = Vector3.zero;
            leftHandObject.transform.localRotation = Quaternion.identity;
            leftHandObject.Pick();
        }
        else
        {
            if (rightToolHolder == null)
            {
                Debug.LogWarning("RightToolHolder not set, can cause problems");
            }

            rightHold = true;
            rightHandObject.transform.parent = (rightToolHolder == null) ?
                GameObject.Find("toolHolder.R").transform : rightToolHolder;
            rightHandObject.transform.localPosition = Vector3.zero;
            rightHandObject.transform.localRotation = Quaternion.identity;
            rightHandObject.Pick();
        }
    }
    
    public void ToggleControls(bool value)
    {
        TutorialManager manager = GameObject.Find("GameLogic").GetComponent<TutorialManager>();
        if (manager != null && !value)
        {
            controls.keyPreferences.mouseClickLocked =
                controls.keyPreferences.mouseClickKey.locked = manager.mouseClickLocked;
            controls.keyPreferences.closeObjectView.locked = manager.closeObjectViewLocked;
            controls.keyPreferences.LeftDropKey.locked = manager.leftDropKeyLocked;
            controls.keyPreferences.RightDropKey.locked = manager.rightDropKeyLocked;
            controls.keyPreferences.CombineKey.locked = manager.combineKeyLocked;
            controls.keyPreferences.GetHintKey.locked = manager.getHintKeyLocked;
            controls.keyPreferences.pickObjectView.locked = manager.pickObjectViewKeyLocked;
            controls.keyPreferences.LeftUseKey.locked = manager.leftUseKeyLocked;
            controls.keyPreferences.RightUseKey.locked = manager.rightUseKeyLocked;
        }
        else
        {
            controls.keyPreferences.SetAllLocked(value);
            //controls.keyPreferences.mouseClickLocked = false;
            //controls.keyPreferences.mouseClickKey.locked = false;
        }
    }
    
    public void ExecuteDelayedCombination()
    {
        if (combineDelayed)
        {
            combineDelayed = false;

            string leftName = leftHandObject ? leftHandObject.name : "";
            string rightName = rightHandObject ? rightHandObject.name : "";

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

            Vector3 plungerPosition = new Vector3();
            bool updatePlunger = false;

            // object changed
            if (leftName != leftCombineResult)
            {
                if (leftHandObject != null)
                {
                    if (leftHandObject.GetComponent<Syringe>() != null)
                    {
                        plungerPosition = leftHandObject.GetComponent<Syringe>().PlungerPosition;
                        updatePlunger = leftHandObject.GetComponent<Syringe>().updatePlunger;
                    }

                    Destroy(leftHandObject.gameObject);
                    leftHandObject = null;
                }

                if (leftCombineResult != "")
                {
                    GameObject leftObject = CreateObjectByName(leftCombineResult, Vector3.zero);
                    leftHandObject = leftObject.GetComponent<PickableObject>();
                    leftHandObject.controlBone = leftControlBone;
                    SetHold(true);

                    PlayerAnimationManager.SetHandItem(true, leftObject);

                    if (leftSavedPos != Vector3.zero)
                    {
                        leftHandObject.SavePosition(leftSavedPos, leftSavedRot);
                    }
                    else if (rightSavedPos != Vector3.zero)
                    {
                        float offset = rightHandObject.GetComponent<MeshFilter>().mesh.bounds.size.z * rightHandObject.transform.lossyScale.z +
                            leftHandObject.GetComponent<MeshFilter>().mesh.bounds.size.z * leftHandObject.transform.lossyScale.z;
                        leftHandObject.SavePosition(rightSavedPos + new Vector3(0, 0, -3f * offset), rightSavedRot);
                    }

                    if (leftHandObject.GetComponent<Syringe>() != null)
                    {
                        leftHandObject.GetComponent<Syringe>().PlungerPosition = plungerPosition;
                        leftHandObject.GetComponent<Syringe>().updatePlunger = updatePlunger;
                    }
                }
                else
                {
                    PlayerAnimationManager.SetHandItem(true, null);
                }
            }

            // object changed
            if (rightName != rightCombineResult)
            {
                if (rightHandObject != null)
                {
                    if (rightHandObject.GetComponent<Syringe>() != null)
                    {
                        plungerPosition = rightHandObject.GetComponent<Syringe>().PlungerPosition;
                        updatePlunger = rightHandObject.GetComponent<Syringe>().updatePlunger;
                    }
                    Destroy(rightHandObject.gameObject);
                    rightHandObject = null;
                }

                if (rightCombineResult != "")
                {
                    GameObject rightObject = CreateObjectByName(rightCombineResult, Vector3.zero);
                    rightHandObject = rightObject.GetComponent<PickableObject>();
                    rightHandObject.controlBone = leftControlBone; // TODO
                    SetHold(false);

                    PlayerAnimationManager.SetHandItem(false, rightObject);

                    if (rightSavedPos != Vector3.zero)
                    {
                        rightHandObject.SavePosition(rightSavedPos, rightSavedRot);
                    }
                    else if (leftSavedPos != Vector3.zero)
                    {
                        float offset = rightHandObject.GetComponent<MeshFilter>().mesh.bounds.size.z * rightHandObject.transform.lossyScale.z +
                            leftHandObject.GetComponent<MeshFilter>().mesh.bounds.size.z * leftHandObject.transform.lossyScale.z;
                        rightHandObject.SavePosition(leftSavedPos + new Vector3(0, 0, -3f * offset), leftSavedRot);
                    }

                    if (rightHandObject.GetComponent<Syringe>() != null)
                    {
                        rightHandObject.GetComponent<Syringe>().PlungerPosition = plungerPosition;
                        rightHandObject.GetComponent<Syringe>().updatePlunger = updatePlunger;
                    }
                }
                else
                {
                    PlayerAnimationManager.SetHandItem(false, null);
                }
            }
        }
    }
}
