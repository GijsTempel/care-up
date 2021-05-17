using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Object that can be picked in hand
/// </summary>
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class PickableObject : InteractableObject
{
    [HideInInspector]
    public bool tutorial_usedOn = false;
    [HideInInspector]
    public bool depoistNeedle = false;
    public bool useOriginalParent = false;
    Transform originalParent;
    [HideInInspector]
    public Transform leftControlBone;
    [HideInInspector]
    public Transform rightControlBone;

    public int holdAnimationID = 0;

    private List<Vector3> framePositions = new List<Vector3>();
    private Rigidbody rigidBody;

    [HideInInspector]
    public bool sihlouette = false;
    //[HideInInspector]
    public int positionID = 0;
    [HideInInspector]
    public PickableObject mainObject;
    [HideInInspector]
    public List<PickableObject> ghostObjects;

    public string prefabInHands = "";
    public string prefabOutOfHands = "";

    public bool destroyOnDrop = false;
    public GameObject customGhost;
    bool gravityUsed = false;
    
    public Transform GetOriginalParent()
    {
        return originalParent;
    }

    protected override void Start()
    {
        base.Start();

        if (useOriginalParent)
        {
            originalParent = transform.parent;
        }
        framePositions.Clear();

        rigidBody = GetComponent<Rigidbody>();
        gravityUsed = GetComponent<Rigidbody>().useGravity;
    }

    /// <summary>
    /// Drops and object
    /// </summary>
    /// <param name="force">If true - forces load position instead of free dropping</param>
    public virtual bool Drop(bool force = false)
    {
        if (destroyOnDrop)
        {
            Destroy(gameObject);
            return true;
        }

        if (GetComponent<Rigidbody>() != null)
        {
            // stop falling mid frame?
            GetComponent<Rigidbody>().useGravity = gravityUsed;
        }
        gameObject.layer = 0;
        GetComponent<Collider>().enabled = true;

        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
        DeleteGhostObject();
        if (rigidBody != null)
        {
            rigidBody.isKinematic = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;

            LoadPosition();
            return true;
        }
        
        return false;
    }

    public override void LoadPosition()
    {
        if (prefabOutOfHands != "")
        {
            GameObject replaced = null;
            inventory.CreateObjectByName(prefabOutOfHands, savedPosition, callback => replaced = callback);
            if (replaced != null)
            {
                replaced.GetComponent<PickableObject>().SavePosition(savedPosition, savedRotation, true);
                replaced.GetComponent<PickableObject>().BaseLoadPosition();
                Destroy(gameObject);
            }
        }
        else
        {
            base.LoadPosition();
        }
    }

    public virtual bool Drop(int posID)
    {
        bool res = Drop();

        if (res)
        {
            switch (name)
            {
                case "GauzeTrayWet":
                    if (posID == 2 && actionManager.CompareDropPos(name, 2))
                    {
                        inventory.CreateStaticObjectByName("GauzeTrayWet_placed", transform.position, transform.rotation);
                        Destroy(gameObject);
                    }
                    break;
                case "catheter_Supertubular_InHands":
                    if (posID == 0 && actionManager.CompareDropPos(name, 0))
                    {
                        // print("posID == 0 && actionManager.CompareDropPos(name, 0)");
                        // PlayerAnimationManager.PlayAnimation("SQ1Sequence");
                        // "SQ1Sequence"
                        Invoke("SQ1Sequence", 0.5f);
                    }
                    break;
                default:
                    break;
            }
        }

        return res;
    }

    void SQ1Sequence()
    {
        /*Transform _target = GameObject.Find("_closeKochertarget").transform;
        PlayerAnimationManager.PlayAnimation("SQ1Sequence", _target);*/
        PlayerAnimationManager.SetTrigger("SQ1Sequence");
        PlayerAnimationManager.SetTrigger("S SQ1Sequence");
    }

    /// <summary>
    /// Handle using of an object on another one.
    /// </summary>
    /// <returns>True if used</returns>
    public virtual bool Use(bool hand, bool noTarget = false)
    {
        tutorial_usedOn = true;

        Debug.Log (depoistNeedle);

        if(depoistNeedle == true) {
            return false;
        }

        if (controls.SelectedObject != null && controls.CanInteract && !noTarget)
        {
            if ((actionManager.CompareUseOnInfo("InjectionNeedle", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("AbsorptionNeedle", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("InjectionSNeedle", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("TestStrips", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("Lancet", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("NeedleHolderWithNeedle", "NeedleCup", this.name))
                && controls.SelectedObject.name == "NeedleCup")
            {
                if (GameObject.Find("GameLogic") != null)
                {
                    if (GameObject.Find("GameLogic").GetComponent<TutorialManager>() != null)
                    {
                        //GameObject.Find("GameLogic").GetComponent<TutorialManager>().needleTrashed = true;
                    }
                }
                inventory.RemoveHandObject(hand);
            }
            else if ((//actionManager.CompareUseOnInfo("AbsorptionNeedleNoCap", "NeedleCup", this.name) ||
                //actionManager.CompareUseOnInfo("InjectionNeedleNoCap", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("ClothWithAmpouleTop", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("InsulinNeedle", "NeedleCup", this.name))
                && controls.SelectedObject.name == "NeedleCup")
            {
                string animation = (hand ? "UseLeft " : "UseRight ") + name + " NeedleCup";
                PlayerAnimationManager.PlayAnimation(animation, GameObject.Find("NeedleCup").transform);
                actionManager.OnUseOnAction(name, "NeedleCup");
                return true;
            }
            else if ((name == "Pad" || name == "Tourniquet") && 
                controls.SelectedObject.name == "Person")
            {
                inventory.RemoveHandObject(hand);
            }
            else if (name == "BandAid" && controls.SelectedObject.name == "Hand")
            {
                if (actionManager.CompareUseOnInfo("BandAid", "Hand"))
                {
                    actionManager.OnUseOnAction("BandAid", "Hand");
                    AnimationSequence animationSequence = new AnimationSequence("BandAid");
                    animationSequence.NextStep();
                    Destroy(gameObject);
                    return true;
                }
            }
            else if (name == "PrickingPen" && controls.SelectedObject.GetComponent<PersonObjectPart>() != null
                && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (actionManager.CompareUseOnInfo("PrickingPen", "Patient"))
                {
                    actionManager.OnUseOnAction("PrickingPen", "Patient");
                    if ( SceneManager.GetActiveScene().name == "Measuring Blood Glucose(Haemogluco)")
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("MeasureBloodGlucose(Haemogluco)", target);
                    }
                    else
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("MeasureBloodGlucose", target);
                    }
                    return true;
                }
            }
            else if (name == "InsulinPenWithNeedle"
                && controls.SelectedObject.name == "Hand")
            {
                if (actionManager.CompareUseOnInfo("InsulinPenWithNeedle", "Hand"))
                {
                    actionManager.OnUseOnAction("InsulinPenWithNeedle", "Hand");
                    AnimationSequence animationSequence = new AnimationSequence("InsulinInjection");
                    animationSequence.NextStep();
                    return true;
                }
            }
            else if (name == "NeedleHolderWithNeedle"
                && controls.SelectedObject.name == "Hand")
            {
                if (actionManager.CompareUseOnInfo("NeedleHolderWithNeedle", "Hand"))
                {
                    actionManager.OnUseOnAction("NeedleHolderWithNeedle", "Hand");
                    AnimationSequence animationSequence = new AnimationSequence("Venapunction");
                    animationSequence.NextStep();
                    return true;
                }
            }
            else if (name == "cotton_ball"
                && controls.SelectedObject.GetComponent<PersonObjectPart>() != null
                && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "w0_A")
            {
                if (actionManager.CompareUseOnInfo("cotton_ball", "w0_A"))
                {
                    actionManager.OnUseOnAction("cotton_ball", "w0_A");
                    Transform target = GameObject.Find("w_clean_target").transform;
                    controls.ResetObject();
                    PlayerAnimationManager.PlayAnimationSequence("CatherisationWoman", target);

                    return true;
                }
            }
            else if (GameObject.FindObjectOfType<ObjectsIDsController>() != null)
            {
      
                ObjectsIDsController ObjectsID_Controller = GameObject.FindObjectOfType<ObjectsIDsController>();
                string selectedName = controls.SelectedObject.transform.name;
                //print("=============" + name + " selected = " + selectedName);
                if (controls.SelectedObject.GetComponent<PersonObjectPart>() != null)
                {
                    selectedName = controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name;
                }
                if (ObjectsID_Controller.FindByName(transform.name) != -1 && ObjectsID_Controller.FindByName(selectedName) != -1)
                {
                    bool alloweUse = actionManager.CompareUseOnInfo(transform.name, selectedName);
                    if (ObjectsID_Controller != null)
                    {
                        if (ObjectsID_Controller.cheat && Application.isEditor)
                        {
                            alloweUse = true;
                        }
                    }

                    if (alloweUse)
                    {
                        actionManager.OnUseOnAction(transform.name, selectedName);

                        // never used
                        //ObjectsIDs objectID_Controller = ObjectsID_Controller.GetObject(ObjectsID_Controller.FindByName(transform.name));
                        //ObjectsIDs selectedID_Controller = ObjectsID_Controller.GetObject(ObjectsID_Controller.FindByName(selectedName));

                        int oId = ObjectsID_Controller.GetIDByName(transform.name);
                        int sId = ObjectsID_Controller.GetIDByName(selectedName);
                       
                        Transform t = null;
                        if (transform.name == "cloth_02_folded" && selectedName == "w0_A")
                        {
                            //t = GameObject.Find("bed_w0").transform;
                        }
                        else if (controls.SelectedObject.transform.Find("CinematicTarget"))
                        {
                            t = controls.SelectedObject.transform;
                        }

                        if (hand)
                        {
                            PlayerAnimationManager.PlayUseAnimation(oId, sId, t);
                        }
                        else
                        {
                            PlayerAnimationManager.PlayUseAnimation(sId, oId, t);
                        }
                        return true;
                    }
                }
            }
                
        }
        else if (noTarget)
        {
            if (actionManager.CompareUseOnInfo(name, ""))
            {
                //-------------------------------------------------------------------------------
                int objectID = -1;

                if (GameObject.Find("GameLogic").GetComponent<ObjectsIDsController>() != null)
                {
                    ObjectsIDsController ObjectsID_Controller = GameObject.Find("GameLogic").GetComponent<ObjectsIDsController>();
                    if (ObjectsID_Controller.FindByName(name) != -1)
                        objectID = ObjectsID_Controller.GetIDByName(name);
                }
                
                if (inventory.rightHandObject == GetComponent<PickableObject>())
                {
                    if (objectID == -1)
                    {
                        PlayerAnimationManager.PlayAnimation("UseRight " + name);
                        actionManager.OnUseOnAction(name, "");
                        return true;
                    }
                    else
                    {
                        PlayerAnimationManager.PlayUseOnIDAnimation(objectID, false);
                    }
                }
                else if (inventory.leftHandObject == GetComponent<PickableObject>())
                {
                    if (objectID == -1)
                    {
                        PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                        actionManager.OnUseOnAction(name, "");
                        return true;
                    }
                    else
                    {
                        PlayerAnimationManager.PlayUseOnIDAnimation(objectID, true);
                    }

                }
            }
        }

        // fix for person objects
        // normal assignment
        string targetObject = controls.SelectedObject != null ? controls.SelectedObject.name : "";
        // check if target is person object part
        if (controls.SelectedObject != null && controls.SelectedObject.GetComponent<PersonObjectPart>() != null)
        {
            // reassign to the main person object part for correct name
            targetObject = controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name;
        }
        if (noTarget)
            targetObject = "";
        actionManager.OnUseOnAction(name, targetObject);

        return (controls.SelectedObject != null && actionManager.CompareUseOnInfo(name, targetObject));
    }
    

    public virtual void Pick()
    {
        // callback for handling different OnPick mechanics
        gameObject.layer = 9; // no collisions
        if (GetComponent<Rigidbody>() != null)
        {
            // stop falling mid frame stupid unity
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public void EmptyHandsWarning()
    {
        string message = "Je hebt geen vrije hand beschikbaar om de actie uit te voeren. Zorg voor een vrije hand door een object terug te leggen.";

        GameObject.FindObjectOfType<GameUI>().ShowBlockMessage("Actie kan niet worden uitgevoerd!", message);
    }

    public void CreateGhostObject(bool trash = false)
    {
        List<HandsInventory.GhostPosition> list =
            inventory.customGhostPositions.Where(x => x.objectName == name).ToList();

        if (list.Count == 0 || trash)
        {
            if (trash)
            {
                string[] trashArray = { "TrashBucket", "PlasticTrashbucket", "Bin" };
                Transform trashObj = null;

                foreach (string ta in trashArray)
                {
                    GameObject find = GameObject.Find(ta);
                    if (find != null)
                    {
                        trashObj = find.transform;
                        break;
                    }
                }

                if (trashObj != null)
                {
                    Vector3 trashPos = trashObj.position;
                    trashPos += new Vector3(0.0f, 0.2f, 0.0f); // a bit higher
                    SavePosition(trashPos, trashObj.rotation, true);
                }
                else
                {
                    Debug.LogWarning("No trashbucket object found");
                }
            }

            InstantiateGhostObject(this.SavedPosition, this.SavedRotation);
        }
        else
        {
            foreach(HandsInventory.GhostPosition g in list)
            {
                InstantiateGhostObject(g.position, 
                    Quaternion.Euler(g.rotation),
                    g.id);
            }
        }
    }

    GameObject SpawnObject(string _name)
    {
        GameObject newInstance = null;
        PrefabHolder prefabHolder = GameObject.FindObjectOfType<PrefabHolder>();
        if (prefabHolder != null)
        {
            newInstance = prefabHolder.GetPrefab(_name);
        }
        else
        {
            Debug.Log("!!!Object  " + _name + " not found");
        }
        return newInstance;
    }

    public void InstantiateGhostObject(Vector3 pos, Quaternion rot, int posID = 0)
    {

        GameObject bundleObject = SpawnObject(name);

        GameObject ghost = null;

        if (bundleObject != null)
        {
            bundleObject.SetActive(true);
            ghost = Instantiate(bundleObject, pos, rot) as GameObject;
           
            ghost.layer = 9; // no collisions
            ghost.GetComponent<PickableObject>().mainObject = this;
            PickableObject ghostObject = ghost.GetComponent<PickableObject>();
            ghostObject.sihlouette = true;
            ghostObject.positionID = posID;
            ghostObject.SetGhostShader();
            ghostObject.GetComponent<Rigidbody>().useGravity = false;
            ghostObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ghostObject.name = this.name;
            ghostObject.assetSource = InteractableObject.AssetSource.Resources;

            this.ghostObjects.Add(ghostObject);
        }
    }

    public void DeleteGhostObject()
    {
        for (int i = ghostObjects.Count - 1; i >= 0; --i)
        {
            GameObject ghost = ghostObjects[i].gameObject;
            ghostObjects.RemoveAt(i);
            Destroy(ghost);
        }
    }
}
