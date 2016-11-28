using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class PickableObject : InteractableObject {
    
    public Vector3 rotationInHand;

    private bool viewMode = false;
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
        viewMode = false;
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
        actionManager.OnUseOnAction(name, controls.SelectedObject == null ? "" : controls.SelectedObject.name);

        if (controls.SelectedObject != null)
        {
            if ((name == "InjectionNeedle" || name == "AbsorptionNeedle")
                && controls.SelectedObject.name == "NeedleCup")
            {
                Destroy(gameObject);
            }
        }
    }
}
