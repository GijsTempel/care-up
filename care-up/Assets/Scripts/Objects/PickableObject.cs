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

    public Transform leftControlBone;
    public Transform rightControlBone;

    public int holdAnimationID = 0;
    
    protected static HandsInventory inventory;

    private List<Vector3> framePositions = new List<Vector3>();
    private Rigidbody rigidBody;
  
    protected override void Start()
    {
        base.Start();
        framePositions.Clear();

        rigidBody = GetComponent<Rigidbody>();

        if (inventory == null)
        {
            inventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
            if (inventory == null) Debug.LogError("No invenrity found");
        }
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
    public virtual bool Use(bool hand)
    {
        tutorial_usedOn = true;
        string[] info = actionManager.CurrentUseOnInfo;
        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if ((name == "InjectionNeedle" || name == "AbsorptionNeedle"
                || name == "InjectionSNeedle" || name == "ClothWithAmpouleTop"
                || name == "InsulinNeedle"
                || name == "TestStrips" || name == "Lancet" || name == "NeedleHolderWithNeedle")
                && info[1] == "NeedleCup"
                && controls.SelectedObject.name == "NeedleCup")
            {
                if (GameObject.Find("GameLogic") != null)
                {
                    if (GameObject.Find("GameLogic").GetComponent<TutorialManager>() != null)
                    {
                        GameObject.Find("GameLogic").GetComponent<TutorialManager>().needleTrashed = true;
                    }
                }
                inventory.RemoveHandObject(hand);
            }
            else if ((name == "Pad" || name == "Tourniquet") && 
                controls.SelectedObject.name == "Person")
            {
                inventory.RemoveHandObject(hand);
            }
            else if (name == "BandAid" && controls.SelectedObject.name == "Hand")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "BandAid" && info[1] == "Hand")
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
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "PrickingPen" && info[1] == "Patient")
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
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "InsulinPenWithNeedle" && info[1] == "Hand")
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
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "NeedleHolderWithNeedle" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("NeedleHolderWithNeedle", "Hand");
                    AnimationSequence animationSequence = new AnimationSequence("Venapunction");
                    animationSequence.NextStep();
                    return true;
                }
            }
        }
        else // cannot interact or target == ""
        {
            if ( name == "Gloves" )
            {
                GameObject.Find("GameLogic").GetComponent<HandsInventory>().GlovesToggle(true);
            }
        }
        actionManager.OnUseOnAction(name, controls.SelectedObject != null && controls.CanInteract ? controls.SelectedObject.name : "");

        return (info[0] == name && controls.SelectedObject != null && info[1] == controls.SelectedObject.name);
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
}
