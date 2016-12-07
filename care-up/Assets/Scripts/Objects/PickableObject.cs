using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        if (controls.SelectedObject != null)
        {
            if ((name == "InjectionNeedle" || name == "AbsorptionNeedle")
                && controls.SelectedObject.name == "NeedleCup")
            {
                Destroy(gameObject);
            }
            else if (name == "SyringeWithInjectionNeedle"
                && controls.SelectedObject.name == "Hand")
            {
                string[] info = actionManager.CurrentUseOnInfo;
                if (info[0] == "SyringeWithInjectionNeedle" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("SyringeWithInjectionNeedle", "Hand");
                    AnimationSequence animationSequence = new AnimationSequence("Injection");
                    return;
                }
            }
        }
        actionManager.OnUseOnAction(name, controls.SelectedObject == null ? "" : controls.SelectedObject.name);
    }
}
