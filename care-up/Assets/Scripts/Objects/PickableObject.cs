using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Object that can be picked in hand
/// </summary>
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class PickableObject : InteractableObject {


    [HideInInspector]
    public bool tutorial_usedOn = false;

    [HideInInspector]
    public Transform leftControlBone;
    [HideInInspector]
    public Transform rightControlBone;

    public int holdAnimationID = 0;

    private List<Vector3> framePositions = new List<Vector3>();
    private Rigidbody rigidBody;

    [HideInInspector]
    public bool sihlouette = false;
    [HideInInspector]
    public PickableObject mainObject;
    [HideInInspector]
    public List<PickableObject> ghostObjects;

    [System.Serializable]
    public struct GhostPosition
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    public List<GhostPosition> ghostPositions = new List<GhostPosition>();

    protected override void Start()
    {
        base.Start();

        framePositions.Clear();

        rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Drops and object
    /// </summary>
    /// <param name="force">If true - forces load position instead of free dropping</param>
    public virtual bool Drop(bool force = false)
    {
        if (GetComponent<Rigidbody>() != null)
        {
            // stop falling mid frame?
            GetComponent<Rigidbody>().useGravity = true;
        }
        gameObject.layer = 0;
        GetComponent<Collider>().enabled = true;

        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        if (rigidBody != null)
        {
            rigidBody.isKinematic = true;
            rigidBody.constraints = RigidbodyConstraints.None;
            //if (Vector3.Distance(transform.position, savedPosition) < 3.0f || force)
            //{
                LoadPosition();
                return true;
            //}
            /*else
            {
                if (framePositions.Count > 0)
                {
                    Vector3 deltaPosition = framePositions[framePositions.Count - 1] - framePositions[0];
                    deltaPosition = deltaPosition * 3 / Time.fixedDeltaTime;
                    rigidBody.AddForce(deltaPosition);
                }
            }*/
        }

        return false;
    }

    /// <summary>
    /// Handle using of an object on another one.
    /// </summary>
    /// <returns>True if used</returns>
    public virtual bool Use(bool hand, bool noTarget = false)
    {
        tutorial_usedOn = true;

        if (controls.SelectedObject != null && controls.CanInteract)
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
            else if ((actionManager.CompareUseOnInfo("AbsorptionNeedleNoCap", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("InjectionNeedleNoCap", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("ClothWithAmpouleTop", "NeedleCup", this.name) ||
                actionManager.CompareUseOnInfo("InsulinNeedle", "NeedleCup", this.name))
                && controls.SelectedObject.name == "NeedleCup")
            {
                string animation = (hand ? "UseLeft " : "UseRight ") + name + " 3";
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
            else if (GameObject.FindObjectOfType<ObjectsIDController>() != null)
            {
      
                ObjectsIDController ObjectsID_Controller = GameObject.FindObjectOfType<ObjectsIDController>();
                string selectedName = controls.SelectedObject.transform.name;
                if (controls.SelectedObject.GetComponent<PersonObjectPart>() != null)
                {
                    selectedName = controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name;
                }
                if (ObjectsID_Controller.FindByName(transform.name) != -1 && ObjectsID_Controller.FindByName(selectedName) != -1)
                {
                    bool alloweUse = actionManager.CompareUseOnInfo(transform.name, selectedName);
                    if (ObjectsID_Controller != null)
                    {
                        if (ObjectsID_Controller.Cheat && Application.isEditor)
                        {
                            alloweUse = true;
                        }
                    }

                    if (alloweUse)
                    {
                        actionManager.OnUseOnAction(transform.name, selectedName);

                        ObjectsIDs objectID_Controller = ObjectsID_Controller.GetObject(ObjectsID_Controller.FindByName(transform.name));
                        ObjectsIDs selectedID_Controller = ObjectsID_Controller.GetObject(ObjectsID_Controller.FindByName(selectedName));

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

        actionManager.OnUseOnAction(name, controls.SelectedObject != null ? controls.SelectedObject.name : "");

        return (controls.SelectedObject != null && actionManager.CompareUseOnInfo(name, controls.SelectedObject.name));
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
        RobotUIMessageTab messageCenter = GameObject.FindObjectOfType<RobotUIMessageTab>();
        messageCenter.NewMessage("Actie kan niet worden uitgevoerd!", message, RobotUIMessageTab.Icon.Warning);
    }

    public void CreateGhostObject()
    {
        if (ghostPositions.Count == 0)
        {
            InstantiateGhostObject(this.SavedPosition, this.SavedRotation);
        }
        else
        {
            foreach(GhostPosition g in ghostPositions)
            {
                InstantiateGhostObject(g.position, 
                    Quaternion.Euler(g.rotation));
            }
        }
    }

    private void InstantiateGhostObject(Vector3 pos, Quaternion rot)
    {
        GameObject ghost = Instantiate(Resources.Load<GameObject>("Prefabs/" + this.name),
            pos, rot);
        ghost.layer = 9; // no collisions
        ghost.GetComponent<PickableObject>().mainObject = this;
        PickableObject ghostObject = ghost.GetComponent<PickableObject>();
        ghostObject.sihlouette = true;
        ghostObject.SetGhostShader();
        ghostObject.GetComponent<Rigidbody>().useGravity = false;
        ghostObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.ghostObjects.Add(ghostObject);
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
