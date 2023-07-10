using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    private ActionHandler actionHandler;
    public int actionNumberLimit = -1;
    public enum TriggerHand {None, Any, Left, Right}
    public enum TriggerHandAction {None, Pinch, Grip}
    PlayerScript player;
    public string triggerName = "";
    public PickableObject pickable;
    public bool mirrorAnimation = false;

    
    [Header("Action Manager Data")]
    public ActionManager.ActionType actionType = ActionManager.ActionType.None;
    public string LeftActionManagerObject = "";
    public string RightActionManagerObject = "";
    List<ActionCollider> actionColliders = new List<ActionCollider>();
    // Start is called before the first frame update
    void Start()
    {
        actionHandler = GameObject.FindObjectOfType<ActionHandler>();
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
        if (isActionConfirmed && actionHandler != null && actionType != ActionManager.ActionType.None)
        {
            isActionConfirmed = actionHandler.TryExecuteAction(actionType, LeftActionManagerObject, RightActionManagerObject);
        }
        return isActionConfirmed;
    }

    private void EmitTrigger()
    {

        if (actionNumberLimit == 0)
            return;
        GameObject target = null;
        if (transform.Find("CinematicTarget") != null)
            target = transform.Find("CinematicTarget").gameObject;
        bool actionAccepted = player.TriggerAction(triggerName, target, mirrorAnimation);
        if (actionAccepted && actionNumberLimit > 0)
            actionNumberLimit--;
    }

    public void AttemptTrigger()
    {
        if (player == null || triggerName == "")
            return;
        if (CheckTriggerConfirmation())
        {
            EmitTrigger();
        }
    }
  
}
