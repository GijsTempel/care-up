using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTriggerRaycast : MonoBehaviour
{
    private float lookAtTimer = 0f;
    private int lookAtObjectId;
    const float MAX_RAY_TRIGGER_TIME = 1.0f;

    public float coneAngle = 15f;
    private List<ActionCollider> actionColliders = new List<ActionCollider>();

    public void RegisterActionCollider(ActionCollider col)
    {
        if (!actionColliders.Contains(col))
        {
            actionColliders.Add(col);
        }
    }

    void Update()
    {
        //CheckForCollisions();
        UpdateConeRayCast();
    }

    /// <summary>
    /// Checks whether objects of type ActionCollider are within a certain look angle of the VR headset.
    /// Supports collision only with a single object at a time.
    /// In case of multiple objects, priority takes the object that is met earlier in the list `actionColliders`.
    /// </summary>
    void UpdateConeRayCast()
    {
        bool collisionOccured = false;
        foreach (ActionCollider ac in actionColliders)
        {
            // skip this object if it's inactive
            // or if it's not supporting RayTrigger
            if (ac == null || !ac.gameObject.activeInHierarchy || !ac.isRayTrigger) continue; 

            // find the angle between where player is looking and the vector towards the object
            Quaternion objectAngle = Quaternion.LookRotation(ac.transform.position - transform.position);
            float angleBetween = Quaternion.Angle(transform.rotation, objectAngle);

            if (angleBetween <= coneAngle)
            {
                collisionOccured = true;
                int objectId = ac.GetInstanceID(); 
                if (objectId == lookAtObjectId)
                {
                    lookAtTimer += Time.deltaTime;
                    if (lookAtTimer > MAX_RAY_TRIGGER_TIME)
                    {
                        ac.RayTriggerAction();
                        lookAtTimer = 0f;
                    }
                }
                else
                {
                    lookAtTimer = 0f;
                }
                lookAtObjectId = objectId;
                break; // ensure we're handling only one object at a time to prevent flickering
            }
        }

        if (!collisionOccured)
        {
            lookAtObjectId = -1;
            lookAtTimer = 0f;
        }
    }

    void CheckForCollisions()
    {
        string ditObjName = "";
        int layerMask = 1 << 6;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.4f, layerMask))
        {
            ditObjName = hit.collider.name;
            int objectId = hit.collider.GetInstanceID();
            if (objectId == lookAtObjectId && hit.collider.GetComponent<ActionCollider>() != null &&
                hit.collider.GetComponent<ActionCollider>().isRayTrigger)
            {   

                lookAtTimer += Time.deltaTime;
                if (lookAtTimer > MAX_RAY_TRIGGER_TIME)
                {
                    hit.collider.GetComponent<ActionCollider>().RayTriggerAction();
                    lookAtTimer = 0f;
                }

            }
            else
            {
                lookAtTimer = 0f;
            }
            lookAtObjectId = objectId;
        }
        else
        {
            lookAtTimer = 0f;
        }
    }
}
