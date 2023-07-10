using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCollider : MonoBehaviour
{
    [Tooltip("Comma separated names. Use 'none' if empty hand required")]
    public string triggerObjectNames = "";
    private PlayerScript player;
    public ActionTrigger.TriggerHand triggerHand;
    public ActionTrigger.TriggerHandAction triggerHandAction;
    public ActionTrigger.TriggerHandAction requiredHandPose = ActionTrigger.TriggerHandAction.None;
    List<GameObject> handsInArea = new List<GameObject>();
    private ActionTrigger actionTrigger;
    public bool isRayTrigger = false;
    bool isRayTriggered = false;
    void AddHandToArea(GameObject hand)
    {
        if (!handsInArea.Contains(hand))
        {
            handsInArea.Add(hand);
        }
    }

    private bool CheckIfActionFromHandInArea(bool isLeftHand)
    {
        bool actionHandIsInArea = false;
        CleanUpHandsInArea();
        foreach(GameObject h in handsInArea)
        {
            if (isLeftHand && h.tag == "LeftHand")
            {
                actionHandIsInArea = true;
                break;
            }
            if (!isLeftHand && h.tag == "RightHand")
            {
                actionHandIsInArea = true;
                break;
            }

        }
        return actionHandIsInArea;
    }
    // Check if any of the hands in area of the collider, or hand that is specified to this area and is in area
    // has needed hand pose to activate the trigger
    private bool CheckIfPoseFromHandInArea()
    {
        CleanUpHandsInArea();
        if (requiredHandPose == ActionTrigger.TriggerHandAction.None)
            return true;
        if (player == null)
            return false;
        ActionTrigger.TriggerHandAction lPose = player.GetCurrentHandPose(true);
        ActionTrigger.TriggerHandAction rPose = player.GetCurrentHandPose(false);
        if (!(lPose == requiredHandPose || rPose == requiredHandPose))
            return false;
        foreach(GameObject h in handsInArea)
        {
            if (triggerHand == ActionTrigger.TriggerHand.None || triggerHand == ActionTrigger.TriggerHand.Any)
            {
                if (h.tag == "LeftHand" || lPose == requiredHandPose)
                    return true;
                if (h.tag == "RightHand" || rPose == requiredHandPose)
                    return true;
            }
            else
            {
                if (triggerHand == ActionTrigger.TriggerHand.Left &&
                    h.tag == "LeftHand" || lPose == requiredHandPose)
                    return true;
                else if (triggerHand == ActionTrigger.TriggerHand.Right &&
                    h.tag == "RightHand" || rPose == requiredHandPose)
                    return true;
            }
        }
        return false;
    }

    public bool CheckConformity(ActionTrigger.TriggerHand currentTriggerHand = ActionTrigger.TriggerHand.None,
            ActionTrigger.TriggerHandAction currentTriggerHandAction = ActionTrigger.TriggerHandAction.None)
    {
        Debug.Log("@)))) isRayTrigger:" + isRayTriggered);
        if (isRayTrigger)
        {
            return isRayTriggered;
        }
        if (handsInArea.Count == 0)
            return false;
        if (triggerHandAction != ActionTrigger.TriggerHandAction.None)
        {
            if (currentTriggerHand == ActionTrigger.TriggerHand.None || 
                currentTriggerHandAction == ActionTrigger.TriggerHandAction.None)
                return false;
            
            if (currentTriggerHandAction != triggerHandAction)
                return false;
            
            if (!CheckIfActionFromHandInArea(currentTriggerHand == ActionTrigger.TriggerHand.Left))
                return false;
        }
        if (requiredHandPose != ActionTrigger.TriggerHandAction.None)
        {
            if (!CheckIfPoseFromHandInArea())
                return false;
        }
        return true;
    }

    void RemoveHandFromArea(GameObject hand)
    {
        handsInArea.Remove(hand);
    }

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        actionTrigger = transform.parent.GetComponent<ActionTrigger>();
    }

    void Update()
    {
        
    }
    private void CleanUpHandsInArea()
    {
        if (actionTrigger.pickable != null)
        {
            HandPresence objectInThisHand = player.GetHandWithThisObject(actionTrigger.pickable.gameObject);
            if (objectInThisHand != null)
            {
                bool isLeft = objectInThisHand.IsLeftHand();
                foreach(GameObject h in handsInArea)
                {
                    if (isLeft && h.tag == "LeftHand")
                        RemoveHandFromArea(h);
                    if (!isLeft && h.tag == "RightHand")
                        RemoveHandFromArea(h);
                }
            }
        }
        DebugTrigger();
    }

    private void OnTriggerEnter(Collider collision)
    {
//If object with this action collider must to be in hand to trigger action
        if (actionTrigger.pickable != null)
        {
//if object is NOT in hand
            HandPresence objectInThisHand = player.GetHandWithThisObject(actionTrigger.pickable.gameObject);
            if (objectInThisHand == null)
            {
                return;
            }
// if object IS in hand
            else
            {
                bool isLeft = objectInThisHand.IsLeftHand();
                
                if (isLeft && collision.gameObject.tag == "LeftHand")
                    return;
                if (!isLeft && collision.gameObject.tag == "RightHand")
                    return;
            }
        }

        if (triggerObjectNames != "")
        {
//if hand has to be empty to trigger
            if (triggerObjectNames == "none")
            {
                if (player.GetObjectInHand(collision.gameObject.tag == "LeftHand") != null)
                {
                    return;
                }
            }
//if object in hand requred to trigger the action
            else
            {
                GameObject objectInCollisionHand = player.GetObjectInHand(collision.gameObject.tag == "LeftHand");
                if (objectInCollisionHand == null)
                {
                    return;
                }
                string[] triggerObjects = triggerObjectNames.Split(',');
                bool containsNeededObject = false;
                foreach(string oName in triggerObjects)
                {
                    if (oName == objectInCollisionHand.name)
                    {
                        containsNeededObject = true;
                        break;
                    }
                }
                if (!containsNeededObject)
                    return;
            }
        }

        if (triggerHand == ActionTrigger.TriggerHand.Left && collision.gameObject.tag != "LeftHand")
            return;
        if (triggerHand == ActionTrigger.TriggerHand.Right && collision.gameObject.tag != "RightHand")
            return;
        AddHandToArea(collision.gameObject);
        CleanUpHandsInArea();

        DebugTrigger();
        actionTrigger.AttemptTrigger();
    }

    private void OnTriggerExit(Collider collision)
    {
        RemoveHandFromArea(collision.gameObject);
        DebugTrigger();
    }

    void DebugTrigger()
    {
        string ss = "";
        foreach(GameObject h in handsInArea)
        {
            ss += h.name + " ";
        }
    }

    public void RayTriggerAction()
    {
        isRayTriggered = true;
        actionTrigger.AttemptTrigger();
        isRayTriggered = false;
    }

}
