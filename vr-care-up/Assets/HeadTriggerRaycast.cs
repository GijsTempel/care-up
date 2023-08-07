using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;

public class HeadTriggerRaycast : MonoBehaviour
{
    [Range(0f, 120f)]
    public float coneAngle = 15f;
    [Range(0f, 2f)]
    public float rayCastDistance = 0.5f;

    PlayerScript player;
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
        player = GameObject.FindObjectOfType<PlayerScript> ();

        if (progressBar == null)
        {
            if (!(progressBar = GameObject.FindObjectOfType<RaycastProgressBar>(true)))
            { // ^intentionally assign instead of comparison
                progressBar = (Object.Instantiate(Resources.Load<GameObject>("RaycastProgressBar")) 
                    as GameObject).GetComponent<RaycastProgressBar>();
            }
        }
    }

    void Update()
    {
        UpdateRayCast();
    }

    void UpdateRayCast()
    {
        if (actionManager == null)
        {
            Debug.LogWarning("UpdateRayCast() failed, actionManager is null");
            return;
        }

        ActionManager.ActionType actionType = ActionManager.ActionType.None;
        actionType = actionManager.IsNextActionExamineAction() ? ActionManager.ActionType.ObjectExamine : actionType;
        actionType = actionManager.IsNextActionSequenceStep() ? ActionManager.ActionType.SequenceStep : actionType;
        // check whether the ActionType is either of these two
        // aka we don't need to raycast if action type is wrong
        if (GameObject.Find("SelectionDialogue") != null)
            actionType = ActionManager.ActionType.SequenceStep;
            
        if (actionType == ActionManager.ActionType.None)
            return;
        bool collisionOccured = false;
        foreach (ActionCollider ac in actionColliders)
        {
            // skip this object if it doesn't exist or inactive
            if (ac == null || !ac.gameObject.activeInHierarchy) 
                continue;

            Vector3 rayDirection = ac.transform.position - transform.position;
            // check whether direction is within an angle
            if (Vector3.Angle(transform.forward, rayDirection) > coneAngle)
                continue;

            if (actionType == ActionManager.ActionType.ObjectExamine)
            {
                // If actionType = examine, filter objects that are only in hands
                // might be changed later if we'll have objects outside hands to examine
                PickableObject acPickable = ac.GetPickable();
                if (acPickable == null || player.GetHandWithThisObject(acPickable.gameObject) == null)
                    continue;

                const int layerMask = 0b01001001;
                if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, rayCastDistance, layerMask))
                {
                    // blocked by another collider
                    if (hit.collider.gameObject != ac.gameObject)
                        continue;

                    // check whether the object is facing camera
                    if (Vector3.Angle(-hit.normal, rayDirection) > ac.rayTriggerAngle)
                        continue;

                    collisionOccured = true;
                }
            }
            else //the only other allowed type is <SequenceStep>, wouldn't get here otherwise
            {
                // and fot the <SequenceStep> we don't need to check if it's in hands
                // or to check whether it's facing camera, because UI always spawns facing camera
                // or whether raycast hits, because object is in unique layer that drawn on top of everything
                collisionOccured = true;
            }

            if (collisionOccured)
            {
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

                Debug.DrawLine(transform.position, targetObj.transform.position);

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
