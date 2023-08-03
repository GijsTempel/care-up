using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTriggerRaycast : MonoBehaviour
{
    public float coneAngle = 15f;
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
    }

    void Update()
    {
        UpdateRayCast();
    }

    void UpdateRayCast()
    {
        bool collisionOccured = false;
        foreach (ActionCollider ac in actionColliders)
        {
            // skip this object if it's inactive
            if (ac == null || !ac.gameObject.activeInHierarchy) 
                continue;
            PickableObject acPickable = ac.GetPickable();
            if (acPickable == null)
                continue;
            if (player.GetHandWithThisObject(acPickable.gameObject) == null)
                continue;

            
            if (actionManager == null || 
                !(actionManager.CompareExamineAction(ac.ActionTriggerObjectNames[0]) || actionManager.IsNextActionSequenceStep())
                )
                continue;
            int layerMask = 0b01001000;
            Vector3 rayDirection = (ac.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, rayDirection) < 0.9f)
                break;
            Ray ray = new Ray(transform.position, rayDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, 0.5f, layerMask))
            {
                if (hit.collider.gameObject != ac.gameObject)
                    break;
                
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
