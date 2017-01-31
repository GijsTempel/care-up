using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class PickableObject : InteractableObject {
    
    public Vector3 rotationInHand;

    private List<Vector3> framePositions = new List<Vector3>();
    private Rigidbody rigidBody;
    private static HandsInventory inventory;
  
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
    
    public void InHandUpdate(bool right)
    {
        Vector3 rotation = Camera.main.transform.eulerAngles;
        transform.localRotation = Quaternion.Euler(rotationInHand + rotation);
    }

    public void Drop(bool force = false)
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
            }
            else
            {
                Narrator.PlaySound("WrongAction");
                actionManager.Points--;

                if (framePositions.Count > 0)
                {
                    Vector3 deltaPosition = framePositions[framePositions.Count - 1] - framePositions[0];
                    deltaPosition = deltaPosition * 3 / Time.fixedDeltaTime;
                    rigidBody.AddForce(deltaPosition);
                }
            }
        }
    }

    public virtual bool Use()
    {
        string[] info = actionManager.CurrentUseOnInfo;
        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if ((name == "InjectionNeedle" || name == "AbsorptionNeedle"
                || name == "TestStrips" || name == "Lancet")
                && controls.SelectedObject.name == "NeedleCup")
            {
                Destroy(gameObject);
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
            else if (name == "PrickingPen" && controls.SelectedObject.name == "Finger")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "PrickingPen" && info[1] == "Finger")
                {
                    actionManager.OnUseOnAction("PrickingPen", "Finger");
                    AnimationSequence animationSequence = new AnimationSequence("MeasureBloodGlucose");
                    animationSequence.NextStep();
                    return true;
                }
            }
            else if (name == "SyringeWithInjectionNeedle"
                && controls.SelectedObject.name == "Hand")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "SyringeWithInjectionNeedle" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("SyringeWithInjectionNeedle", "Hand");
                    if (SceneManager.GetActiveScene().name == "Injection" ||
                        SceneManager.GetActiveScene().name == "Injection_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection_disolve")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("Injection");
                        animationSequence.NextStep();
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection Subcutaneous" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous_desolve")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("SubcutaneousInjection");
                        animationSequence.NextStep();
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection Subcutaneous v2" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous v2_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous v2_desolve")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("SubcutaneousInjection v2");
                        animationSequence.NextStep();
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection scene v2")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("Injection v2");
                        animationSequence.NextStep();
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
            else if (name == "Syringe"
                && controls.SelectedObject.name == "Person")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "Syringe" && info[1] == "Person")
                {
                    actionManager.OnUseOnAction("Syringe", "Person");
                    AnimationSequence animationSequence = new AnimationSequence("WingedNeedle");
                    animationSequence.NextStep();
                    return true;
                }
            }
            else if (name == "SyringeWithAbsorptionNeedle"
                && controls.SelectedObject.name == "Hand")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "SyringeWithAbsorptionNeedle" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("SyringeWithAbsorptionNeedle", "Hand");
                    return true; // for tutorial
                }
            }
            else if (name == "SyringeWithAbsorptionNeedle")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "SyringeWithAbsorptionNeedle" && info[1] == "")
                {
                    actionManager.OnUseOnAction("SyringeWithAbsorptionNeedle", "");
                    return true; // fix for venting syringe
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
}
