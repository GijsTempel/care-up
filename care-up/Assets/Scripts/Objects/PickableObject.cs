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
    
    public Transform controlBone;

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

    void LateUpdate()
    {
        framePositions.Add(transform.position);

        if (framePositions.Count > 15)
        {
            framePositions.RemoveAt(0);
        }
    }

    /// <summary>
    /// Drops and object
    /// </summary>
    /// <param name="force">If true - forces load position instead of free dropping</param>
    public virtual bool Drop(bool force = false)
    {
        gameObject.layer = 0;
        GetComponent<Collider>().enabled = true;

        if (rigidBody != null)
        {
            rigidBody.useGravity = true;
            rigidBody.constraints = RigidbodyConstraints.None;
            if (Vector3.Distance(transform.position, savedPosition) < 3.0f || force)
            {
                LoadPosition();
                return true;
            }
            else
            {
                if (framePositions.Count > 0)
                {
                    Vector3 deltaPosition = framePositions[framePositions.Count - 1] - framePositions[0];
                    deltaPosition = deltaPosition * 3 / Time.fixedDeltaTime;
                    rigidBody.AddForce(deltaPosition);
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Handle using of an object on another one.
    /// </summary>
    /// <returns>True if used</returns>
    public virtual bool Use(bool hand)
    {
        string[] info = actionManager.CurrentUseOnInfo;
        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if ((name == "InjectionNeedle" || name == "AbsorptionNeedle"
                || name == "TestStrips" || name == "Lancet" || name == "NeedleHolderWithNeedle"
                || name == "ClothWithAmpouleTop")
                && controls.SelectedObject.name == "NeedleCup")
            {
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
    }
}
