using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
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
        CheckTriggerConfirmation(currentTriggerHand, tAction);
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

    public void AttemptTrigger()
    {
        if (player == null || triggerName == "")
            return;
        

        
        if (CheckTriggerConfirmation())
        {
            GameObject target = transform.Find("CinematicTarget").gameObject;
            
            player.TriggerAction(triggerName, target);
        }
    }
  
}
