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
                player.BlockPlayerActions(false);
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
        for (int i = 0; i < transform.childCount; i ++)
        {
            if (transform.GetChild(i).GetComponent<ItemInHandCheck>() != null)
                if (!transform.GetChild(i).GetComponent<ItemInHandCheck>().Check())
                    return false;
            if (transform.GetChild(i).GetComponent<ActionModule_ObjectDetector>() != null)
                if (!transform.GetChild(i).GetComponent<ActionModule_ObjectDetector>().Check())
                    return false;
        }
        // if (actionType == ActionManager.ActionType.None)
        // {
        //     TriggerIncludedObjects();
        //     return true;
        // }


        if (actionNumberLimit == 0)
            return false;
        bool isActionConfirmed = true;
        foreach(ActionCollider c in actionColliders)
        {
            if (!c.CheckConformity(currentTriggerHand, currentTriggerHandAction))
            {
                isActionConfirmed = false;
                break;
            }
        }
        // if (isActionConfirmed)
        //     Debug.Log("@ CheckConformity " + name + ":" + currentTriggerHand.ToString() + " " + currentTriggerHandAction.ToString());

        if (isActionConfirmed && actionHandler != null)
        {
            if (checkBeforeAct)
            {
                if (actionHandler.CheckAction(actionType, LeftActionManagerObject, RightActionManagerObject))
                    isActionConfirmed = actionHandler.TryExecuteAction(actionType, LeftActionManagerObject, RightActionManagerObject);
            }
            else
            {
                if (actionType != ActionManager.ActionType.None)
                    isActionConfirmed = actionHandler.TryExecuteAction(actionType, LeftActionManagerObject, RightActionManagerObject);
                else
                    isActionConfirmed = true;
            }
        }
        if (isActionConfirmed)
            TriggerIncludedObjects();

        return isActionConfirmed;
    }

    private void TriggerIncludedObjects()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ActionModule_ShowHideDelete>() != null)
                transform.GetChild(i).GetComponent<ActionModule_ShowHideDelete>().StartTimeout();
            if (transform.GetChild(i).GetComponent<ActionTrigger>() != null)
                transform.GetChild(i).GetComponent<ActionTrigger>().AttemptTrigger();
            if (transform.GetChild(i).GetComponent<ActionModule_AnimationTrigger>() != null)
                transform.GetChild(i).GetComponent<ActionModule_AnimationTrigger>().Execute();
            if (transform.GetChild(i).GetComponent<ActionModule_AudioTrigger>() != null)
                transform.GetChild(i).GetComponent<ActionModule_AudioTrigger>().Execute();
            if (transform.GetChild(i).GetComponent<ActionModule_TutorialTigger>() != null)
                transform.GetChild(i).GetComponent<ActionModule_TutorialTigger>().Execute();
            if (transform.GetChild(i).GetComponent<ActionModule_ActionTriggerIgniter>() != null)
                transform.GetChild(i).GetComponent<ActionModule_ActionTriggerIgniter>().Execute();
            if (transform.GetChild(i).GetComponent<ActionModule_ChangeHandPose>() != null)
                transform.GetChild(i).GetComponent<ActionModule_ChangeHandPose>().Execute();
            if (transform.GetChild(i).GetComponent<ActionModule_AddObjectToHand>() != null)
                transform.GetChild(i).GetComponent<ActionModule_AddObjectToHand>().Execute();
            if (transform.GetChild(i).GetComponent<ActionModule_Counter>() != null)
                transform.GetChild(i).GetComponent<ActionModule_Counter>().Execute();
            if (transform.GetChild(i).GetComponent<ActionModule_Gloves>() != null)
                transform.GetChild(i).GetComponent<ActionModule_Gloves>().Execute();
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
            player.BlockPlayerActions(true);
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
