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

    public bool CheckConformity()
    {
        bool confirmed = true;
        if (handsInArea.Count == 0)
            confirmed = false;
        return confirmed;
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
        if (triggerHand == ActionTrigger.TriggerHand.Left && collision.name != "LeftActionTriggerCollider")
            return;
        if (triggerHand == ActionTrigger.TriggerHand.Right && collision.name != "RightActionTriggerCollider")
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
        Debug.Log("@" + transform.parent.name + "_detected:" + ss);

    }

}
