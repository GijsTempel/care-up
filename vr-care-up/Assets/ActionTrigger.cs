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

    public void AttemptTrigger()
    {
        if (player != null && triggerName != "")
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
            if (isActionConfirmed)
            {
                GameObject target = transform.Find("CinematicTarget").gameObject;
                
                player.TriggerAction(triggerName, target);
            }
        }
    }
  
}
