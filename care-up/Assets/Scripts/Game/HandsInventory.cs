using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CareUp.Actions;
using System.Linq;
using AssetBundles;

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
    [HideInInspector]
    public bool tutorial_InspectItem = false;

    private Tutorial_UseOn tutorialUseOn;

    // position in air 
    public float horisontalOffset = 0.5f;
    public float distanceFromCamera = 1.0f;
    public bool dropPenalty = true;

    bool practiceMode = true;

	ObjectsIDsController ObjectsID_Controller;

    [System.Serializable]
    public struct ItemPosition
    {
        public string objectName;
        public Vector3 position;
        public Vector3 rotation;
    };

    public List<ItemPosition> customPositions = new List<ItemPosition>();

    [System.Serializable]
    public struct GhostPosition
    {
        public string objectName;
        public Vector3 position;
        public Vector3 rotation;
        public int id;
    };
    public List<GhostPosition> customGhostPositions = new List<GhostPosition>();

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
    
    private bool glovesOn = false;

    private GameObject animationObject;
    private GameObject animationObject2;

    private CombinationManager combinationManager;
    private GameObject interactableObjects;
    private Controls controls;
    private CameraMode cameraMode;

    private ActionManager actionManager;
    private PlayerPrefsManager prefs;

    //private TutorialManager tutorial;


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
        tutorialUseOn = GameObject.FindObjectOfType<Tutorial_UseOn> ();
        prefs = GameObject.FindObjectOfType<PlayerPrefsManager>();

        if (prefs != null)
            practiceMode = prefs.practiceMode;

        ObjectsID_Controller = GameObject.Find("GameLogic").GetComponent<ObjectsIDsController>();

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

        // never used
        //tutorial = GetComponent<TutorialManager>();

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
    
    public bool PickItem(PickableObject item, PlayerAnimationManager.Hand hand)
    {
        return PickItem(item, (hand == PlayerAnimationManager.Hand.Left) ? "left" : "right");
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
            if (rightHandObject == null)
            {
                rightHandObject = item;
                picked = true;
                tutorial_pickedRight = true;
                rightHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rightHandObject.GetComponent<Rigidbody>().isKinematic = false;
                //rightHandObject.GetComponent<Collider>().enabled = false;
                rightHandObject.leftControlBone = leftControlBone;
                rightHandObject.rightControlBone = rightControlBone;
                actionManager.OnPickUpAction(rightHandObject.name);
                PlayerAnimationManager.PlayAnimation("RightPick");
                PlayerScript.actionsLocked = true;
                PlayerAnimationManager.SetHandItem(false, item.gameObject);
                rightHold = false;
            }
            else if (leftHandObject == null)
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
                PlayerScript.actionsLocked = true;
                PlayerAnimationManager.SetHandItem(true, item.gameObject);
                leftHold = false;
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
                PlayerScript.actionsLocked = true;
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
                //-----------------------------------------------------------------------
                rightHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rightHandObject.GetComponent<Rigidbody>().isKinematic = false;
                //rightHandObject.GetComponent<Collider>().enabled = false;
                rightHandObject.leftControlBone = leftControlBone;
                rightHandObject.rightControlBone = rightControlBone;
                actionManager.OnPickUpAction(rightHandObject.name);
                PlayerAnimationManager.PlayAnimation("RightPick");
                PlayerScript.actionsLocked = true;
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

            //RobotUIMessageTab messageCenter = GameObject.FindObjectOfType<RobotUIMessageTab>();
            //messageCenter.NewMessage("Je hebt je handen vol!", message, RobotUIMessageTab.Icon.Warning);

            GameObject.FindObjectOfType<GameUI>().ShowBlockMessage("Je hebt je handen vol!", message);
        }

        ActionManager.UpdateRequirements();

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
            if (leftHandObject != null)
            {
                leftHandObject.DeleteGhostObject();
                Destroy(leftHandObject.gameObject);
                leftHandObject = null;
            }
        }
        else
        {
            if (rightHandObject != null)
            {
                rightHandObject.DeleteGhostObject();
                Destroy(rightHandObject.gameObject);
                rightHandObject = null;
            }
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

            leftHandObject.DeleteGhostObject();
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

            leftHandObject.CreateGhostObject();

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

            rightHandObject.DeleteGhostObject();
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
            rightHandObject.CreateGhostObject();

            if (rightHandObject.GetComponent<PickableObjectWithInfo>() != null && load)
            {
                rightHandObject.GetComponent<PickableObjectWithInfo>().LoadInfo(saveInfo1, saveInfo2);
            }
        }

        UpdateHoldAnimation();
    }

    Object FindFromBundles(string _name)
    {
        string FullPath = "assets/resources/prefabs/" + _name.ToLower() + ".prefab";
        Object bundleObject = AssetBundleManager.GetObjectFromLoaded(FullPath);
        return bundleObject;
    }

    
    /// <summary>
    /// After combining a new object can appear on the scene.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <param name="position">Position of the object</param>
    /// <returns>Object created.</returns>
    public GameObject CreateObjectByName(string name, Vector3 position)
    {
        Object bundleObject = FindFromBundles(name);
        bool from_bundle = bundleObject != null;
        if (bundleObject == null)
            bundleObject = Resources.Load<GameObject>("Prefabs\\" + name);

        GameObject newObject = Instantiate(bundleObject, position, Quaternion.identity) as GameObject;
        if (newObject != null)
        {
            newObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            newObject.GetComponent<Rigidbody>().useGravity = false;
            newObject.GetComponent<Rigidbody>().isKinematic = false;
            newObject.transform.parent = interactableObjects.transform;
            newObject.name = name;
            newObject.GetComponent<InteractableObject>().assetSource = InteractableObject.AssetSource.Resources;
            if (from_bundle)
                newObject.GetComponent<InteractableObject>().assetSource = InteractableObject.AssetSource.Bundle;
        }
        else
            print("!!!!!!! Object not available " + name);
        GameObject wf = GameObject.Find("WorkField");
        if (wf != null)
        {
            wf.GetComponent<WorkField>().objects.Add(newObject);
        }

        RefrashAssetDict();

        return newObject;
    }


    public GameObject CreateStaticObjectByName(string name, Vector3 position, Quaternion rotation)
    {

        Object bundleObject = FindFromBundles(name);
        bool from_bundle = bundleObject != null;
        if (bundleObject == null)
            bundleObject = Resources.Load<GameObject>("Prefabs\\" + name);
        GameObject newObject = Instantiate(bundleObject, position, rotation) as GameObject;
        newObject.name = name;
        if (from_bundle)
            newObject.GetComponent<InteractableObject>().assetSource = InteractableObject.AssetSource.Bundle;
        
        RefrashAssetDict();
        
        return newObject;
    }
    
    public void CreateAnimationObject(string name, PlayerAnimationManager.Hand hand)
    {
        CreateAnimationObject(name, hand == PlayerAnimationManager.Hand.Left);
    }

    public void CreateAnimationObject(string name, bool hand)
    {
        Object bundleObject = FindFromBundles(name);
        bool from_bundle = bundleObject != null;
        if (bundleObject == null)
            bundleObject = Resources.Load<GameObject>("Prefabs\\" + name);

        GameObject animationObject = Instantiate(bundleObject, Vector3.zero, Quaternion.identity) as GameObject;

        if (animationObject != null)
        {

            if (animationObject.GetComponent<Rigidbody>() != null)
            {
                animationObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                animationObject.GetComponent<Rigidbody>().useGravity = false;
            }

            if (animationObject.GetComponent<Collider>() != null)
            {
                animationObject.GetComponent<Collider>().enabled = false;
            }

            animationObject.name = name;
        
            animationObject.transform.parent = hand ? leftToolHolder : rightToolHolder;
            animationObject.transform.localPosition = Vector3.zero;
            animationObject.transform.localRotation = Quaternion.identity;

            animationObject.GetComponent<PickableObject>().Pick();
            animationObject.GetComponent<InteractableObject>().assetSource = InteractableObject.AssetSource.Resources;
            if (from_bundle)
                animationObject.GetComponent<InteractableObject>().assetSource = InteractableObject.AssetSource.Bundle;
        }
        else
            print("!!!!!!! Object not available " + name);

        RefrashAssetDict();
    }

    public void DeleteAnimationObject()
    {
        Destroy(animationObject);
        animationObject = null;
    }

    public void CreateAnimationObject2(string name, bool hand)
    {
        Object bundleObject = FindFromBundles(name);
        bool from_bundle = bundleObject != null;
        if (bundleObject == null)
            bundleObject = Resources.Load<GameObject>("Prefabs\\" + name);

        GameObject animationObject2 = Instantiate(bundleObject, Vector3.zero, Quaternion.identity) as GameObject;
                       
        animationObject2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        animationObject2.GetComponent<Rigidbody>().useGravity = false;
        animationObject2.GetComponent<Collider>().enabled = false;
        animationObject2.name = name;
                       
        animationObject2.transform.parent = hand ? leftToolHolder : rightToolHolder;
        animationObject2.transform.localPosition = Vector3.zero;
        animationObject2.transform.localRotation = Quaternion.identity;
                       
        animationObject2.GetComponent<PickableObject>().Pick();
        if (from_bundle)
                animationObject2.GetComponent<InteractableObject>().assetSource = InteractableObject.AssetSource.Bundle;
        

        RefrashAssetDict();
    }

    void RefrashAssetDict()
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        AssetDebugPanel adp = GameObject.FindObjectOfType<AssetDebugPanel>();
        if(adp != null)
            adp.RefrashAssetDict();
#endif 
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
    /// Puts on or takes off gloves.
    /// </summary>
    /// <param name="value">True - puts on, False - takes off</param>
    public void GlovesToggle(bool value)
    {
        glovesOn = value;

        if (glovesOn)
        {
            // just so it shuts up with never used, we will 
        }

        Renderer hands = GameObject.FindObjectOfType<PlayerScript>().transform.
            Find("CinematicControl/Arms/FPArms_Female").GetComponent<Renderer>();

        if (value)
        {
            hands.material = Resources.Load<Material>("Materials/FPArms_Female-Glow");
        }
        else
        {
            hands.material = Resources.Load<Material>("FPArms_Female-Light_COL");
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

        ActionManager.UpdateRequirements();
    }

    public void DropLeftObject()
    {
        if (leftHandObject)
        {
            leftHandObject.DeleteGhostObject();
            leftHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
            leftHandObject.Drop(true);
            leftHandObject = null;
            leftHold = false;
            PlayerAnimationManager.SetHandItem(true, null);
            ActionManager.UpdateRequirements();
        }
    }

    public void DropRightObject()
    {
        if (rightHandObject)
        {
            rightHandObject.DeleteGhostObject();
            rightHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
            rightHandObject.Drop(true);
            rightHandObject = null;
            rightHold = false;
            PlayerAnimationManager.SetHandItem(false, null);
            ActionManager.UpdateRequirements();
        }
    }

    public void FreezeObject(bool hand)
    {
        if (hand)
        {
            if (leftHandObject)
            {
                leftHandObject.DeleteGhostObject();
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
                rightHandObject.DeleteGhostObject();
                rightHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
                rightHandObject.enabled = false;
                rightHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rightHandObject = null;
                rightHold = false;
                PlayerAnimationManager.SetHandItem(false, null);
            }
        }
    }

    public void ForcePickItem(string name, PlayerAnimationManager.Hand hand, bool createGhost = false)
    {
        ForcePickItem(name, hand == PlayerAnimationManager.Hand.Left, createGhost);
    }

    public void ForcePickItem(string name, bool hand, bool createGhost = false)
    {
        ForcePickItem(GameObject.Find(name).gameObject,
            (hand ? PlayerAnimationManager.Hand.Left : PlayerAnimationManager.Hand.Right)
            , createGhost);
    }

    public void ForcePickItem(GameObject obj, bool hand, bool createGhost = false)
    {
        ForcePickItem(obj,
            (hand ? PlayerAnimationManager.Hand.Left : PlayerAnimationManager.Hand.Right)
            , createGhost);
    }

    public void ForcePickItem(GameObject obj, PlayerAnimationManager.Hand hand, bool createGhost = false)
    {
        PickableObject item = obj.GetComponent<PickableObject>();
        if (item != null && item.prefabInHands != "")
        {
            item.SavePosition();
            GameObject replaced = CreateObjectByName(item.prefabInHands, Vector3.zero);
            replaced.GetComponent<PickableObject>().SavePosition(item.SavedPosition, item.SavedRotation, true);
            item.DeleteGhostObject();
            Destroy(item.gameObject);
            item = replaced.GetComponent<PickableObject>();
        }

        if (hand == PlayerAnimationManager.Hand.Left)
        {
            leftHandObject = item;
            leftHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftHandObject.GetComponent<Rigidbody>().isKinematic = false;
            leftHandObject.leftControlBone = leftControlBone;
            leftHandObject.rightControlBone = rightControlBone;
            leftHandObject.SavePosition();
        }
        else
        {
            rightHandObject = item;
            rightHandObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightHandObject.GetComponent<Rigidbody>().isKinematic = false;
            rightHandObject.leftControlBone = leftControlBone;
            rightHandObject.rightControlBone = rightControlBone;
            rightHandObject.SavePosition();
        }

        if (createGhost)
        {
            item.CreateGhostObject();
        }
        
        SetHold(hand == PlayerAnimationManager.Hand.Left);
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

                    leftHandObject.DeleteGhostObject();
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

                    leftHandObject.CreateGhostObject();
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

                    rightHandObject.DeleteGhostObject();
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

                    rightHandObject.CreateGhostObject();
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

    public class HandObject
    {
        public GameObject left;
        public GameObject right;
    }

    public HandObject IsInOneOfHands()
    {
        HandObject handObject = new HandObject();
        handObject.left = (leftHandObject != null) ? leftHandObject.gameObject : null;
        handObject.right = (rightHandObject != null) ? rightHandObject.gameObject : null;
        print(handObject.left);
        return handObject;
    }

    public void OnCombineAction()
    {
        if (tutorialUseOn != null && !tutorialUseOn.decombiningAllowed) {
            return;
        }

        string leftName = leftHandObject ? leftHandObject.name : "";
        string rightName = rightHandObject ? rightHandObject.name : "";

        bool combineAllowed = actionManager.CompareCombineObjects(leftName, rightName);
        //bool practice = (GameObject.FindObjectOfType<PlayerPrefsManager>() != null ? GameObject.FindObjectOfType<PlayerPrefsManager>().practiceMode : true);
        //combineAllowed = combineAllowed || practice;

        bool combined = combinationManager.Combine(leftName, rightName, out leftCombineResult, out rightCombineResult);

        bool allowMultiple = false;
        if (!practiceMode && combined)
            allowMultiple = combinationManager.CombineMultiple(leftName, rightName);


        // combine performed
        bool alloweCombine = (combined && (combineAllowed || allowMultiple));

        actionManager.OnCombineAction(leftName, rightName, allowMultiple);

        if (alloweCombine) HandleCombineQuizTriggers(leftName, rightName);

        if (ObjectsID_Controller != null)
		{
			if (ObjectsID_Controller.cheat && Application.isEditor)
			{
				alloweCombine = true;
			}
		}
		if (alloweCombine)
        {
            tutorial_combined = true; 
			bool idModeAllow = false;

			if (GameObject.Find("GameLogic").GetComponent<ObjectsIDsController>() != null)
			{
				if (ObjectsID_Controller.FindByName(leftName) != -1 || ObjectsID_Controller.FindByName(rightName) != -1)
				{
					idModeAllow = true;
                    
				}
			}

			if (idModeAllow)
			{
		        int leftID = ObjectsID_Controller.GetIDByName(leftName);
                int rightID = ObjectsID_Controller.GetIDByName(rightName);
                PlayerAnimationManager.PlayCombineAnimation(leftID, rightID);
			}
			else
			{
				string combineAnimation = "Combine " +
                 (leftHandObject ? leftHandObject.name : "_") + " " +
                 (rightHandObject ? rightHandObject.name : "_");
				
				PlayerAnimationManager.PlayAnimation(combineAnimation);
			}

            combineDelayed = true;
            ToggleControls(true);
        }
    }

    public void HandleCombineQuizTriggers(string left, string right)
    {
        if ((left == "SyringeWithAbsorptionNeedleCap" && right == "") ||
            (left == "" && right == "SyringeWithAbsorptionNeedleCap"))
        {
            PlayerScript.TriggerQuizQuestion(5.3f);
        }
        else if ((left == "InsulinOpenedNeedlePackage" && right == "InsulinPen") ||
                 (left == "InsulinPen" && right == "InsulinOpenedNeedlePackage"))
        {
            PlayerScript.TriggerQuizQuestion(7.3f);
        }
        else if ((left == "fraxi_pakage" && right == "") ||
                 (left == "" && right == "fraxi_pakage"))
        {
            PlayerScript.TriggerQuizQuestion(8.3f);
        }
    }

    public void DropLeft(GameObject ghost = null)
    {
        if (leftHandObject)
        {
            leftHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
            tutorial_droppedLeft = true;

            int posID = 0;
            if (ghost != null)
            {
                leftHandObject.SavePosition(ghost.transform.position,
                    ghost.transform.rotation, true);
                posID = ghost.GetComponent<PickableObject>().positionID;
            }

            if (!leftHandObject.Drop(posID)) 
            {
                if (dropPenalty)
                {
                    ActionManager.WrongAction();
                    actionManager.UpdatePoints(-1);
                }
            }

            actionManager.OnDropDownAction(leftHandObject.name, posID);
            
            leftHandObject = null;
            leftHold = false;
            PlayerAnimationManager.SetHandItem(true, null);
            ActionManager.UpdateRequirements();
        }
    }

    public void DropRight(GameObject ghost = null)
    {
        if (rightHandObject)
        {
            rightHandObject.transform.parent = GameObject.Find("Interactable Objects").transform;
            tutorial_droppedRight = true;

            int posID = 0;
            if (ghost != null)
            {
                rightHandObject.SavePosition(ghost.transform.position,
                    ghost.transform.rotation, true);
                posID = ghost.GetComponent<PickableObject>().positionID;
            }

            if (!rightHandObject.Drop(posID))
            {
                if (dropPenalty)
                {
                    ActionManager.WrongAction();
                    actionManager.UpdatePoints(-1);
                }
            }

            actionManager.OnDropDownAction(rightHandObject.name, posID);
            
            rightHandObject = null;
            rightHold = false;
            PlayerAnimationManager.SetHandItem(false, null);
            ActionManager.UpdateRequirements();
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
    }
}
