using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [HideInInspector]
    public bool sequenceAborted = false;

    // position in air 
    public float horisontalOffset = 0.5f;
    public float distanceFromCamera = 1.0f;
    public bool dropPenalty = true;


	ObjectsIDController ObjectsID_Controller;

    [System.Serializable]
    public struct ItemPosition
    {
        public string objectName;
        public Vector3 position;
        public Vector3 rotation;
    };

    public List<ItemPosition> customPositions = new List<ItemPosition>();

    private Transform leftToolHolder;
    private Transform rightToolHolder;
    private Transform leftControlBone;
    private Transform rightControlBone;
    
    public PickableObject leftHandObject;
    public PickableObject rightHandObject;
    private bool leftHold = false;
    private bool rightHold = false;
    
    private bool combineDelayed = false;
    private string leftCombineResult;
    private string rightCombineResult;
    
    private Vector3 glovesPosition;
    private Quaternion glovesRotation;
    private bool glovesOn = false;

    private GameObject animationObject;
    private GameObject animationObject2;

    private CombinationManager combinationManager;
    private GameObject interactableObjects;
    private Controls controls;
    private CameraMode cameraMode;

    private ActionManager actionManager;
    //private PlayerPrefsManager prefsManager;

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
		ObjectsID_Controller = GameObject.Find("GameLogic").GetComponent<ObjectsIDController>();

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

        /* never used?
        if (GameObject.Find("Preferences"))
        {
            prefsManager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        } */

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

        Physics.IgnoreLayerCollision(9, 9);
        Physics.IgnoreLayerCollision(0, 9);
    }

    void Update() {

        if (leftHandObject && leftHold)
            leftHandObject.transform.localPosition = Vector3.zero;

        if (rightHandObject && rightHold)
            rightHandObject.transform.localPosition = Vector3.zero;
        
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
                leftHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                leftHandObject.GetComponent<Rigidbody>().isKinematic = false; 
                //leftHandObject.GetComponent<Collider>().enabled = false;
                leftHandObject.leftControlBone = leftControlBone;
                leftHandObject.rightControlBone = rightControlBone;
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
                leftHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rightHandObject.GetComponent<Rigidbody>().isKinematic = false;
                //rightHandObject.GetComponent<Collider>().enabled = false;
                rightHandObject.leftControlBone = leftControlBone;
                rightHandObject.rightControlBone = rightControlBone;
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
                leftHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                leftHandObject.GetComponent<Rigidbody>().isKinematic = false;
                //leftHandObject.GetComponent<Collider>().enabled = false;
                leftHandObject.leftControlBone = leftControlBone;
                leftHandObject.rightControlBone = rightControlBone;
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
                leftHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rightHandObject.GetComponent<Rigidbody>().isKinematic = false;
                //rightHandObject.GetComponent<Collider>().enabled = false;
                rightHandObject.leftControlBone = leftControlBone;
                rightHandObject.rightControlBone = rightControlBone;
                actionManager.OnPickUpAction(rightHandObject.name);
                PlayerAnimationManager.PlayAnimation("RightPick");
                PlayerAnimationManager.SetHandItem(false, item.gameObject);
            }
        }

        if (picked)
        {
            item.SavePosition();
        }
        else
        {
            string message = "Je hebt je handen vol. Leg objecten terug om je handen vrij te maken.";
            RobotUIMessageTab messageCenter = GameObject.FindObjectOfType<RobotUIMessageTab>();
            messageCenter.NewMessage("Je hebt je handen vol!", message, RobotUIMessageTab.Icon.Warning);
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
        if (hand)
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
            
            Vector3 saveInfo1 = new Vector3();
            Vector3 saveInfo2 = new Vector3();
            bool load = false;

            if (leftHandObject.GetComponent<PickableObjectWithInfo>() != null)
            {
                leftHandObject.GetComponent<PickableObjectWithInfo>().SaveInfo(ref saveInfo1, ref saveInfo2);
                load = true;
            }

            Destroy(leftHandObject.gameObject);
            leftHandObject = null;
            
            GameObject leftObject = CreateObjectByName(name, Vector3.zero);
            leftHandObject = leftObject.GetComponent<PickableObject>();
            leftHandObject.leftControlBone = leftControlBone;
            leftHandObject.rightControlBone = rightControlBone;
            SetHold(true);

            PlayerAnimationManager.SetHandItem(true, leftObject);

            if (leftSavedPos != Vector3.zero)
            {
                leftHandObject.SavePosition(leftSavedPos, leftSavedRot);
            }

            if (leftHandObject.GetComponent<PickableObjectWithInfo>() != null && load)
            {
                leftHandObject.GetComponent<PickableObjectWithInfo>().LoadInfo(saveInfo1, saveInfo2);
            }
        }
        else
        {
            Vector3 rightSavedPos = Vector3.zero;
            Quaternion rightSavedRot = Quaternion.identity;
            rightHandObject.GetSavesLocation(out rightSavedPos, out rightSavedRot);

            Vector3 saveInfo1 = new Vector3();
            Vector3 saveInfo2 = new Vector3();
            bool load = false;

            if (rightHandObject.GetComponent<PickableObjectWithInfo>() != null)
            {
                rightHandObject.GetComponent<PickableObjectWithInfo>().SaveInfo(ref saveInfo1, ref saveInfo2);
                load = true;
            }

            Destroy(rightHandObject.gameObject);
            rightHandObject = null;

            GameObject rightObject = CreateObjectByName(name, Vector3.zero);
            rightHandObject = rightObject.GetComponent<PickableObject>();
            rightHandObject.leftControlBone = leftControlBone;
            rightHandObject.rightControlBone = rightControlBone;
            SetHold(false);

            PlayerAnimationManager.SetHandItem(false, rightObject);

            if (rightSavedPos != Vector3.zero)
            {
                rightHandObject.SavePosition(rightSavedPos, rightSavedRot);
            }

            if (rightHandObject.GetComponent<PickableObjectWithInfo>() != null && load)
            {
                rightHandObject.GetComponent<PickableObjectWithInfo>().LoadInfo(saveInfo1, saveInfo2);
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

        newObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        newObject.GetComponent<Rigidbody>().useGravity = false;
        newObject.GetComponent<Rigidbody>().isKinematic = false;
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

        animationObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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

    public void CreateAnimationObject2(string name, bool hand)
    {
        animationObject2 = Instantiate(Resources.Load<GameObject>("Prefabs\\" + name),
                            Vector3.zero, Quaternion.identity) as GameObject;
                       
        animationObject2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        animationObject2.GetComponent<Rigidbody>().useGravity = false;
        animationObject2.GetComponent<Collider>().enabled = false;
        animationObject2.name = name;
                       
        animationObject2.transform.parent = hand ? leftToolHolder : rightToolHolder;
        animationObject2.transform.localPosition = Vector3.zero;
        animationObject2.transform.localRotation = Quaternion.identity;
                       
        animationObject2.GetComponent<PickableObject>().Pick();
    }

    public void DeleteAnimationObject2()
    {
        Destroy(animationObject2);
        animationObject2 = null;
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

    public void FreezeObject(bool hand)
    {
        if (hand)
        {
            if (leftHandObject)
            {
                leftHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
                leftHandObject.enabled = false;
                leftHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                leftHandObject = null;
                leftHold = false;
                PlayerAnimationManager.SetHandItem(true, null);
            }
        }
        else
        {
            if (rightHandObject)
            {
                rightHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
                rightHandObject.enabled = false;
                rightHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rightHandObject = null;
                rightHold = false;
                PlayerAnimationManager.SetHandItem(false, null);
            }
        }
    }

    public void ForcePickItem(string name, bool hand)
    {
        if (hand)
        { 
            leftHandObject = GameObject.Find(name).GetComponent<PickableObject>();
            leftHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftHandObject.GetComponent<Rigidbody>().isKinematic = false;
            leftHandObject.leftControlBone = leftControlBone;
            leftHandObject.rightControlBone = rightControlBone;
            leftHandObject.SavePosition();
        }
        else
        {
            rightHandObject = GameObject.Find(name).GetComponent<PickableObject>();
            rightHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightHandObject.GetComponent<Rigidbody>().isKinematic = false;
            rightHandObject.leftControlBone = leftControlBone;
            rightHandObject.rightControlBone = rightControlBone;
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

            Vector3 saveInfo1 = new Vector3();
            Vector3 saveInfo2 = new Vector3();
            bool load = false;

            // object changed
            if (leftName != leftCombineResult)
            {
                if (leftHandObject != null)
                {
                    if (leftHandObject.GetComponent<PickableObjectWithInfo>() != null)
                    {
                        leftHandObject.GetComponent<PickableObjectWithInfo>().SaveInfo(ref saveInfo1, ref saveInfo2);
                        load = true;
                    }

                    Destroy(leftHandObject.gameObject);
                    leftHandObject = null;
                }

                if (leftCombineResult != "")
                {
                    GameObject leftObject = CreateObjectByName(leftCombineResult, Vector3.zero);
                    leftHandObject = leftObject.GetComponent<PickableObject>();
                    leftHandObject.leftControlBone = leftControlBone;
                    leftHandObject.rightControlBone = rightControlBone;
                    SetHold(true);

                    PlayerAnimationManager.SetHandItem(true, leftObject);

                    if (leftSavedPos != Vector3.zero)
                    {
                        leftHandObject.SavePosition(leftSavedPos, leftSavedRot);
                    }
                    else if (rightSavedPos != Vector3.zero)
                    {
                        leftHandObject.SavePosition(rightSavedPos + GetOffset(rightHandObject, leftHandObject, rightSavedRot), rightSavedRot);
                    }

                    // whatever, override with custom in all cases
                    if (customPositions.Exists(x => x.objectName == leftCombineResult))
                    {
                        ItemPosition custom = customPositions.Find(x => x.objectName == leftCombineResult);
                        leftHandObject.SavePosition(custom.position, Quaternion.Euler(custom.rotation), true);
                    }

                    if (leftHandObject.GetComponent<PickableObjectWithInfo>() != null && load)
                    {
                        leftHandObject.GetComponent<PickableObjectWithInfo>().LoadInfo(saveInfo1, saveInfo2);
                    }
                }
                else
                {
                    PlayerAnimationManager.SetHandItem(true, null);
                }
            }

            load = false;
            // object changed
            if (rightName != rightCombineResult)
            {
                if (rightHandObject != null)
                {
                    if (rightHandObject.GetComponent<PickableObjectWithInfo>() != null)
                    {
                        rightHandObject.GetComponent<PickableObjectWithInfo>().SaveInfo(ref saveInfo1, ref saveInfo2);
                    }
                    Destroy(rightHandObject.gameObject);
                    rightHandObject = null;
                }

                if (rightCombineResult != "")
                {
                    GameObject rightObject = CreateObjectByName(rightCombineResult, Vector3.zero);
                    rightHandObject = rightObject.GetComponent<PickableObject>();
                    rightHandObject.leftControlBone = leftControlBone;
                    rightHandObject.rightControlBone = rightControlBone;
                    SetHold(false);

                    PlayerAnimationManager.SetHandItem(false, rightObject);

                    if (rightSavedPos != Vector3.zero)
                    {
                        rightHandObject.SavePosition(rightSavedPos, rightSavedRot);
                    }
                    else if (leftSavedPos != Vector3.zero)
                    {
                        rightHandObject.SavePosition(leftSavedPos + GetOffset(leftHandObject, rightHandObject, leftSavedRot), leftSavedRot);
                    }

                    // whatever, override with custom in all cases
                    if (customPositions.Exists(x => x.objectName == rightCombineResult))
                    {
                        ItemPosition custom = customPositions.Find(x => x.objectName == rightCombineResult);
                        rightHandObject.SavePosition(custom.position, Quaternion.Euler(custom.rotation), true);
                    }

                    if (rightHandObject.GetComponent<PickableObjectWithInfo>() != null && load)
                    {
                        rightHandObject.GetComponent<PickableObjectWithInfo>().LoadInfo(saveInfo1, saveInfo2);
                    }
                }
                else
                {
                    PlayerAnimationManager.SetHandItem(false, null);
                }
            }
        }
    }

    private Vector3 GetOffset(PickableObject origin, PickableObject target, Quaternion rotation)
    {
        Vector3 meshBoundsOrigin = origin.GetComponent<MeshFilter>().mesh.bounds.size;
        meshBoundsOrigin.Scale(origin.transform.lossyScale);
        Vector3 originVector = rotation * meshBoundsOrigin;

        Vector3 meshBoundsTarget = target.GetComponent<MeshFilter>().mesh.bounds.size;
        meshBoundsTarget.Scale(target.transform.lossyScale);
        Vector3 targetVector = rotation * meshBoundsTarget;

        float offset = -1.5f * Mathf.Abs(originVector.z + targetVector.z);
        
        return new Vector3(0.0f, 0.0f, offset);
    }

    public bool IsInHand(GameObject target)
    {
        GameObject left = (leftHandObject != null) ? leftHandObject.gameObject : null;
        GameObject right = (rightHandObject != null) ? rightHandObject.gameObject : null;
        return (target == left || target == right) && target != null;
    }

    public void OnCombineAction()
    {
        string leftName = leftHandObject ? leftHandObject.name : "";
        string rightName = rightHandObject ? rightHandObject.name : "";
        
        bool combineAllowed = actionManager.CompareCombineObjects(leftName, rightName);

        bool combined = combinationManager.Combine(leftName, rightName, out leftCombineResult, out rightCombineResult);


        // combine performed
        if ((combined && combineAllowed) || (ObjectsID_Controller.Cheat && Application.isEditor))
        {
            tutorial_combined = true;
            string combineAnimation = "Combine " +
                (leftHandObject ? leftHandObject.name : "_") + " " +
                (rightHandObject ? rightHandObject.name : "_");
			//--------------------------------------------------------------------
			if (ObjectsID_Controller.FindByName(leftName) != -1 || ObjectsID_Controller.FindByName(rightName) != -1)
			{
				ObjectsIDs l = ObjectsID_Controller.GetObject(ObjectsID_Controller.FindByName(leftName));
				ObjectsIDs r = ObjectsID_Controller.GetObject(ObjectsID_Controller.FindByName(rightName));

				PlayerAnimationManager.PlayCombineAnimation(l, r);
			}
			else
			{

				PlayerAnimationManager.PlayAnimation(combineAnimation);
			}

            combineDelayed = true;
            ToggleControls(true);
        }
    }

    public void DropLeft()
    {
        if (leftHandObject)
        {
            if (tutorial == null || (tutorial != null &&
            (tutorial.itemToDrop == leftHandObject.name ||
            tutorial.itemToDrop2 == leftHandObject.name)))
            {
                leftHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
                tutorial_droppedLeft = true;
                if (!leftHandObject.Drop())
                {
                    if (dropPenalty)
                    {
                        ActionManager.WrongAction();
                        actionManager.UpdatePoints(-1);
                    }
                }
                leftHandObject = null;
                leftHold = false;
                PlayerAnimationManager.SetHandItem(true, null);
            }
        }
    }

    public void DropRight()
    {
        if (rightHandObject)
        {
            if (tutorial == null || (tutorial != null &&
            (tutorial.itemToDrop == rightHandObject.name ||
            tutorial.itemToDrop2 == rightHandObject.name)))
            {
                rightHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
                tutorial_droppedRight = true;
                if (!rightHandObject.Drop())
                {
                    if (dropPenalty)
                    {
                        ActionManager.WrongAction();
                        actionManager.UpdatePoints(-1);
                    }
                }
                rightHandObject = null;
                rightHold = false;
                PlayerAnimationManager.SetHandItem(false, null);
            }
        }
    }

    public void LeftHandUse()
    {
        if (leftHandObject != null)
        {
            if (controls.SelectedObject == RightHandObject)
            {
                OnCombineAction();
            }
            else
            {
                if (leftHandObject.Use(true))
                {
                    tutorial_itemUsedOn = true;
                }
            }
        }
        else if (glovesOn && rightHandObject == null)
        {
            actionManager.OnUseOnAction("", "");
            GlovesToggle(false);
        }
    }

    public void RightHandUse()
    {
        if (rightHandObject)
        {
            if (controls.SelectedObject == LeftHandObject)
            {
                OnCombineAction();
            }
            else
            {
                if (rightHandObject.Use(false))
                {
                    tutorial_itemUsedOn = true;
                }
            }
        }
        else if (glovesOn && leftHandObject == null)
        {
            actionManager.OnUseOnAction("", "");
            GlovesToggle(false);
        }
    }
}
