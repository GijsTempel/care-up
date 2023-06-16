using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCollider : MonoBehaviour
{
    public ActionTrigger.TriggerHand triggerHand;
    public ActionTrigger.TriggerHandAction triggerHandAction;
    List<GameObject> handsInArea = new List<GameObject>();

    void AddHandToArea(GameObject hand)
    {
        if (!handsInArea.Contains(hand))
        {
            handsInArea.Add(hand);
        }
    }

    void RemoveHandFromArea(GameObject hand)
    {
        handsInArea.Remove(hand);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        AddHandToArea(collision.gameObject);
        DebugTrigger();
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
