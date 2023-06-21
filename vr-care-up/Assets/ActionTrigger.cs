using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    public int actionNumberLimit = -1;
    public enum TriggerHand {None, Any, Left, Right}
    public enum TriggerHandAction {None, Pinch, Grip}
    PlayerScript player;
    public string triggerName = "";

    List<ActionCollider> actionColliders = new List<ActionCollider>();
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        foreach(ActionCollider a in transform.GetComponentsInChildren<ActionCollider>())
        {
            actionColliders.Add(a);
        }
    }

    public void ReceveTriggerAction(bool isLeftHand, TriggerHandAction tAction)
    {
        TriggerHand currentTriggerHand = TriggerHand.Right;
        if (isLeftHand)
            currentTriggerHand = TriggerHand.Left;
        if (CheckTriggerConfirmation(currentTriggerHand, tAction))
            EmitTrigger();
    }

    private bool CheckTriggerConfirmation(TriggerHand currentTriggerHand = TriggerHand.None, TriggerHandAction currentTriggerHandAction = TriggerHandAction.None)
    {
        bool isActionConfirmed = true;
        foreach(ActionCollider c in actionColliders)
        {
            if (!c.CheckConformity(currentTriggerHand, currentTriggerHandAction))
            {
                isActionConfirmed = false;
                break;
            }
          
        }
        return isActionConfirmed;
    }

    private void EmitTrigger()
    {
        if (actionNumberLimit == 0)
            return;
        GameObject target = transform.Find("CinematicTarget").gameObject;
        bool actionAccepted = player.TriggerAction(triggerName, target);
        if (actionAccepted && actionNumberLimit > 0)
            actionNumberLimit--;
    }

    public void AttemptTrigger()
    {
        if (player == null || triggerName == "")
            return;
        if (CheckTriggerConfirmation())
        {
            Debug.Log("@" + name + "aTrigger:" + triggerName);
            EmitTrigger();
        }
    }
  
}
