using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class PickableObject : InteractableObject {
    
    public Vector3 rotationInHand;
    
    private Vector3 savedPosition;
    private Quaternion savedRotation;

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
        transform.localRotation = Quaternion.Euler(rotationInHand + Camera.main.transform.rotation.eulerAngles);
    }

    public void Drop()
    {
        gameObject.layer = 0;
        GetComponent<Collider>().enabled = true;
       
        if (rigidBody != null)
        {
            rigidBody.useGravity = true;
            rigidBody.constraints = RigidbodyConstraints.None;
            if (framePositions.Count > 0)
            {
                Vector3 deltaPosition = framePositions[framePositions.Count - 1] - framePositions[0];
                deltaPosition = deltaPosition * 3 / Time.fixedDeltaTime;
                rigidBody.AddForce(deltaPosition);
            }
        }
    }

    public virtual void Use()
    {
        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if ((name == "InjectionNeedle" || name == "AbsorptionNeedle")
                && controls.SelectedObject.name == "NeedleCup")
            {
                Destroy(gameObject);
            }
            else if (name == "BandAid" && controls.SelectedObject.name == "Hand")
            {
                string[] info = actionManager.CurrentUseOnInfo;
                if (info[0] == "BandAid" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("BandAid", "Hand");
                    AnimationSequence animationSequence = new AnimationSequence("BandAid");
                    animationSequence.NextStep();
                    Destroy(gameObject);
                    return;
                }
            }
            else if (name == "SyringeWithInjectionNeedle"
                && controls.SelectedObject.name == "Hand")
            {
                string[] info = actionManager.CurrentUseOnInfo;
                if (info[0] == "SyringeWithInjectionNeedle" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("SyringeWithInjectionNeedle", "Hand");
                    if (SceneManager.GetActiveScene().name == "Injection")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("Injection");
                        animationSequence.NextStep();
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection Subcutaneous")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("SubcutaneousInjection");
                        animationSequence.NextStep();
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection scene v2")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("Injection v2");
                        animationSequence.NextStep();
                    }
                    return;
                }
            }
            else if (name == "InsulinPenWithNeedle"
                && controls.SelectedObject.name == "Hand")
            {
                string[] info = actionManager.CurrentUseOnInfo;
                if (info[0] == "InsulinPenWithNeedle" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("InsulinPenWithNeedle", "Hand");
                    AnimationSequence animationSequence = new AnimationSequence("InsulinInjection");
                    animationSequence.NextStep();
                    return;
                }
            }
        }
        actionManager.OnUseOnAction(name, controls.SelectedObject != null && controls.CanInteract ? controls.SelectedObject.name : "");
    }
}
