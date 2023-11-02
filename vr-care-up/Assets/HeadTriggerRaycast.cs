using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UIElements;

public class HeadTriggerRaycast : MonoBehaviour
{
    [Range(0f, 120f)]
    public float coneAngle = 25f;
    [Range(0f, 2f)]
    public float rayCastDistance = 0.5f;
    WalkToGroupVR savedTeleportAnchor = null;


    PlayerScript player;
    public RaycastProgressBar progressBar;
    private List<ActionCollider> actionColliders = new List<ActionCollider>();

    private ActionManager actionManager = null;
    public GameObject teleportProgressCanvas;
    float teleportationWaitTime = 1.5f;
    const float tTimeStartValue = -0.05f;
    float teleportationTimeValue = tTimeStartValue;
    bool justTeleported = false;
    public UnityEngine.UI.Image teleportationProgressImage;
    public void RegisterActionCollider(ActionCollider col)
    {
        if (!actionColliders.Contains(col))
        {
            actionColliders.Add(col);
        }
    }

    public bool IsLookingAtTeleport()
    {
        return teleportationTimeValue > 0;
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
        const int layerMask = 0b01001001;

        //UI pointer ray
        const int pointerLayerMask = 0b100000000;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit phit, 5f, pointerLayerMask))
        {
            SelectDialogue selectDialogue = phit.collider.GetComponent<SelectDialogue>();
            if (selectDialogue != null)
                selectDialogue.PointerRayHit(phit.point);
            VRRaycastButton raycastButton = phit.collider.GetComponentInChildren<VRRaycastButton>();
            if (raycastButton != null)
                raycastButton.PointerRayHit(phit.point);
        }

        if (!player.IsInCopyAnimationState())
        {
            const int teleportLayerMask = 0b1111111111;
            teleportProgressCanvas.SetActive(false);
            WalkToGroupVR currentTeleportAnchor = null;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit thit, 10f, teleportLayerMask))
            {
                WalkToGroupVR teleportationAnchor = thit.collider.GetComponent<WalkToGroupVR>();
                if (teleportationAnchor != null)
                {
                    Debug.Log("TeleportRay:" + thit.collider.name + " " +
                        UnityEngine.Random.Range(0, 9999).ToString());
                    currentTeleportAnchor = teleportationAnchor;
                }
            }
            if (!justTeleported && currentTeleportAnchor != null && savedTeleportAnchor == currentTeleportAnchor)
            {
                teleportationTimeValue += Time.deltaTime;
                if (teleportationTimeValue > 0)
                {
                    teleportProgressCanvas.SetActive(true);
                    teleportationProgressImage.fillAmount =
                        Remap(teleportationTimeValue, 0f, teleportationWaitTime, 0f, 1f);
                }
                if (teleportationTimeValue > teleportationWaitTime)
                {
                    justTeleported = false;
                    teleportationTimeValue = tTimeStartValue;
                    player.TriggerTeleportation(currentTeleportAnchor.GetTeleportationAnchor(),
                        currentTeleportAnchor);
                }
            }
            else
            {
                justTeleported = false;
                teleportationTimeValue = tTimeStartValue;
            }
            savedTeleportAnchor = currentTeleportAnchor;
        }

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
            Vector3 rayDirection = (ac.transform.position - transform.position).normalized;
            // check whether direction is within an angle

            // if (Vector3.Angle(transform.forward, rayDirection) > coneAngle)
            //     continue;
            if (Vector3.Dot(transform.forward, rayDirection) < 0.9f)
                continue;

            if (actionType == ActionManager.ActionType.ObjectExamine)
            {
                if (!actionManager.CompareExamineAction(ac.ActionTriggerObjectNames[0]))
                    continue;
                // If actionType = examine, filter objects that are only in hands
                // might be changed later if we'll have objects outside hands to examine
                // PickableObject acPickable = ac.GetPickable();
                // if (acPickable == null || player.GetHandWithThisObject(acPickable.gameObject) == null)
                //     continue;

                if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, rayCastDistance, layerMask))
                {
                    // blocked by another collider
                    if (hit.collider.gameObject != ac.gameObject)
                        continue;

                    // check whether the object is facing camera
                    // if (Vector3.Angle(-hit.normal, rayDirection) > ac.rayTriggerAngle)
                    //     continue;
                    float objDirDot = Vector3.Dot(-hit.normal, rayDirection);
                    if (objDirDot < 0.85f)
                        continue;

                    collisionOccured = true;
                }
            }
            else //the only other allowed type is <SequenceStep>, wouldn't get here otherwise
            {
                // and fot the <SequenceStep> we don't need to check if it's in hands
                // or to check whether it's facing camera, because UI always spawns facing camera
                // or whether raycast hits, because object is in unique layer that drawn on top of everything
                // collisionOccured = true;
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

    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
        return targetFrom + (source-sourceFrom)*(targetTo-targetFrom)/(sourceTo-sourceFrom);
    }
}
