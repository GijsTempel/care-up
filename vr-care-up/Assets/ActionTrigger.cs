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
    [Tooltip("Check if action is correct, before executing")]
    public bool checkBeforeAct = false;
    List<ActionCollider> actionColliders = new List<ActionCollider>();
    public float triggerDelay = -1f;
    private bool triggered = false;

    [Header("Trigger Animation Sequence")]
    public string animationSequenceToTrigger;
    // Start is called before the first frame update
    void Start()
    {
        actionHandler = GameObject.FindObjectOfType<ActionHandler>();
        player = GameObject.FindObjectOfType<PlayerScript>();
        foreach(ActionCollider a in transform.GetComponentsInChildren<ActionCollider>())
        {
            actionColliders.Add(a);
        }
        enabled = false;
    }


    private void Update()
    {
        if (triggerDelay > 0 && triggered)
        {
            triggerDelay -= Time.deltaTime;
            if (triggerDelay < 0)
            {
                AttemptTrigger();
                triggered = false;
            }
        }
    }


    public void ReceveTriggerAction(bool isLeftHand, TriggerHandAction tAction)
    {
        if (actionColliders.Count == 0)
            return;
        TriggerHand currentTriggerHand = TriggerHand.Right;
        if (isLeftHand)
            currentTriggerHand = TriggerHand.Left;
        if (CheckTriggerConfirmation(currentTriggerHand, tAction))
            EmitTrigger();
    }

    private bool CheckTriggerConfirmation(TriggerHand currentTriggerHand = TriggerHand.None, TriggerHandAction currentTriggerHandAction = TriggerHandAction.None)
    {
        if (actionType == ActionManager.ActionType.None)
        {
            TriggerIncludedObjects();
            return true;
        }
        if (actionNumberLimit == 0)
            return false;
        bool isActionConfirmed = true;
        foreach(ActionCollider c in actionColliders)
        {
            //Debug.Log(name);
            if (!c.CheckConformity(currentTriggerHand, currentTriggerHandAction))
            {
                isActionConfirmed = false;
                break;
            }
        }
        if (isActionConfirmed && actionHandler != null && actionType != ActionManager.ActionType.None)
        {
            if (checkBeforeAct)
            {
                if (actionHandler.CheckAction(actionType, LeftActionManagerObject, RightActionManagerObject))
                    isActionConfirmed = actionHandler.TryExecuteAction(actionType, LeftActionManagerObject, RightActionManagerObject);
            }
            else
            {
                isActionConfirmed = actionHandler.TryExecuteAction(actionType, LeftActionManagerObject, RightActionManagerObject);
            }
        }
        if (isActionConfirmed)
            TriggerIncludedObjects();

        return isActionConfirmed;
    }

    private void TriggerIncludedObjects()
    {
        foreach (TriggerShowHideDeleteAction t in transform.GetComponentsInChildren<TriggerShowHideDeleteAction>())
            t.StartTimeout();
        foreach (ActionTrigger t in transform.GetComponentsInChildren<ActionTrigger>())
        {
            if (t != this)
                t.AttemptTrigger();
        }
    }

    private bool EmitTrigger()
    {
        if (actionNumberLimit == 0)
            return false;
        if (triggerName == "")
            return false;
        GameObject target = null;
        if (transform.Find("CinematicTarget") != null)
            target = transform.Find("CinematicTarget").gameObject;
        bool actionAccepted = player.TriggerAction(triggerName, target, mirrorAnimation);
        if (actionAccepted)
        {
            if (animationSequenceToTrigger != "")
            {
                ActionManager.TriggerAnimationSequence(animationSequenceToTrigger);
            }
            actionNumberLimit--;
        }
        return actionAccepted;
    }


    public bool AttemptTrigger()
    {
        if (triggerDelay > 0)
        {
            enabled = true;
            triggered = true;
            return false;
        }
        if (actionNumberLimit == 0)
            return false;
        if (player == null)
            return false;
        if (CheckTriggerConfirmation())
        {
            EmitTrigger();
            return true;
        }
        return false;
    }
  
}
