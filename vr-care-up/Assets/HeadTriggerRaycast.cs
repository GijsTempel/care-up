using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTriggerRaycast : MonoBehaviour
{
    private float lookAtTimer = 0f;
    int lookAtObjectId;
    const float MAX_RAY_TRIGGER_TIME = 1.0f;

    public float coneAngle = 15f;
    private ActionCollider[] actionColliders;

    public UnityEngine.UI.Text debugText;

    void Start()
    {
        // possible pitfall: are new ActionColliders being created in the middle of the scene?
        actionColliders = FindObjectsOfType<ActionCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForCollisions();
        UpdateConeRayCast();
    }

    /// <summary>
    /// Checks whether objects of type ActionCollider are within a certain look angle of the VR headset.
    /// Use DebugDrawCone() to visually see the cone angle.
    /// Supports collision only with a single object at a time.
    /// In case of multiple objects, priority takes the object that is met earlier in the list `actionColliders`.
    /// </summary>
    void UpdateConeRayCast()
    {
        string debug = "";                   // debug, to remove
        string closest = "";                 // debug, to remove
        float angleClosest = Mathf.Infinity; // debug, to remove

        bool collisionOccured = false;
        foreach (ActionCollider ac in actionColliders)
        {
            if (!ac.isRayTrigger) continue; // skip this object if it's not supporting RayTrigger

            Quaternion objectAngle = Quaternion.LookRotation(ac.transform.position - transform.position);
            float angleBetween = Quaternion.Angle(transform.rotation, objectAngle);

            if (angleBetween < angleClosest)
            {
                closest = ac.name;
                angleClosest = angleBetween;
            }

            if (angleBetween <= coneAngle)
            {
                collisionOccured = true;
                debug += "Current Object: " + ac.name + "\n"; // debug, to remove
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

        // debug, to remove (everything below)
        debug += "Closest: \"" + closest + "\" with angle " + Mathf.RoundToInt(angleClosest) + "\n";
        debug += "Timer: " + lookAtTimer + "\n";
        debugText.text = debug;
        //Debug.DrawLine(transform.position, closestPos, Color.green);
        //DebugDrawCone();
    }

    void DebugDrawCone(float distance = 3f)
    {
        const int RayAmount = 8;

        Vector3 mid = transform.position + transform.forward * distance;
        float radius = distance * Mathf.Tan(Mathf.PI * coneAngle / 180f);
        for (int i = 0; i < RayAmount; i++)
        {
            Vector3 end = mid + radius * (transform.up * Mathf.Sin(i * Mathf.PI / 4f) + transform.right * Mathf.Cos(i * Mathf.PI / 4f));
            Debug.DrawLine(transform.position, end);
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
