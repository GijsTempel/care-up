using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTriggerRaycast : MonoBehaviour
{
    public float coneAngle = 15f;
    public RaycastProgressBar progressBar;
    private List<ActionCollider> actionColliders = new List<ActionCollider>();

    private ActionManager actionManager = null;

    public void RegisterActionCollider(ActionCollider col)
    {
        if (!actionColliders.Contains(col))
        {
            actionColliders.Add(col);
        }
    }

    private void Start()
    {
        actionManager = FindObjectOfType<ActionManager>();
    }

    void Update()
    {
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

            // also if target object is not the correct object in the next examine step, skip it
            if (actionManager == null || !actionManager.CompareExamineAction(ac.ActionTriggerObjectNames[0]))
            {
                continue;
            }

            // find the angle between where player is looking and the vector towards the object
            Quaternion objectAngle = Quaternion.LookRotation(ac.transform.position - (transform.position - transform.forward));
            float angleBetween = Quaternion.Angle(transform.rotation, objectAngle);
            float directionalAngle = Quaternion.Angle(transform.rotation, ac.transform.rotation);

            if (angleBetween <= coneAngle && (180-directionalAngle) < ac.rayTriggerAngle)
            {
                collisionOccured = true;
                ActionCollider targetObj = ac;
                if (targetObj == progressBar.target)
                {
                    progressBar.currentProgress += Time.deltaTime;
                    if (progressBar.currentProgress > progressBar.MaxRayTriggerTime)
                    {
                        ac.RayTriggerAction();
                        progressBar.currentProgress = 0f;
                    }
                }
                else
                {
                    progressBar.currentProgress = 0f;
                }
                progressBar.target = targetObj;
                break; // ensure we're handling only one object at a time to prevent flickering
            }
        }

        if (!collisionOccured)
        {
            progressBar.target = null;
            progressBar.currentProgress = 0f;
        }
    }
}
