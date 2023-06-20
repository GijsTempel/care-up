using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    public enum TriggerHand {Any, Left, Right }
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

    private bool CheckTriggerConfirmation()
    {
        bool isActionConfirmed = true;
        foreach(ActionCollider c in actionColliders)
        {
            if (!c.CheckConformity())
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
