using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCollider : MonoBehaviour
{
    public ActionTrigger.TriggerHand triggerHand;
    public ActionTrigger.TriggerHandAction triggerHandAction;
    List<GameObject> handsInArea = new List<GameObject>();
    private ActionTrigger actionTrigger;

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

    public bool CheckConformity(ActionTrigger.TriggerHand currentTriggerHand = ActionTrigger.TriggerHand.None,
            ActionTrigger.TriggerHandAction currentTriggerHandAction = ActionTrigger.TriggerHandAction.None)
    {
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
        return true;
    }

    void RemoveHandFromArea(GameObject hand)
    {
        handsInArea.Remove(hand);
    }

    void Start()
    {
        actionTrigger = transform.parent.GetComponent<ActionTrigger>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (triggerHand == ActionTrigger.TriggerHand.Left && collision.gameObject.tag != "LeftHand")
            return;
        if (triggerHand == ActionTrigger.TriggerHand.Right && collision.gameObject.tag != "RightHand")
            return;
        AddHandToArea(collision.gameObject);
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
        Debug.Log("@" + name + "_detected:" + ss);

    }

}
