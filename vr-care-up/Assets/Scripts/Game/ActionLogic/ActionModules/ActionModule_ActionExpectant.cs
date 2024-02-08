using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ActionModule_ActionExpectant : MonoBehaviour
{
    private ActionHandler actionHandler;
    public GameObject testObject;
    public bool isCurrentAction = false;
    private bool savedIsCurrentAction = false;
    public ActionManager.ActionType actionType = ActionManager.ActionType.None;
    public string leftActionManagerObject = "";
    public string rightActionManagerObject = "";
    public string walkToGroupName = "";
    bool noExtraConditions = true;
    private PlayerScript player;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        UpdateAction();
        for (int i = 0; i < transform.childCount; i ++)
        {

            if (transform.GetChild(i).GetComponent<ActionCondition_ItemInHand>() != null ||
                transform.GetChild(i).GetComponent<ActionCondition_IsHandsTrackingMode>() != null ||
                transform.GetChild(i).GetComponent<ActionCondition_PlayerWTG>() != null )
            {
                noExtraConditions = false;
                break;
            }
        }
        // if (walkToGroupName == "" && noExtraConditions)
        //     enabled = false;
    }

    public bool TryExecuteAction()
    {
        bool result = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ActionModule_ActionTrigger>() != null)
            {
                transform.GetChild(i).GetComponent<ActionModule_ActionTrigger>().AttemptTrigger();
                result = true;
            }
        }
        return result;
    }
    
    
    public void UpdateAction()
    {
        if (actionType == ActionManager.ActionType.None)
        {
            savedIsCurrentAction = true;
            UpdateIsCurrentActionValue(savedIsCurrentAction);
            return;
        }

        if (actionHandler == null)
        {
            if ((actionHandler = GameObject.FindObjectOfType<ActionHandler>()) == null)
            {
                // we're here if there's still no actionHandler after looking for one
                Debug.LogWarning("No actionHandler found, not a game scene?");
                return;
            }
        }

        savedIsCurrentAction = actionHandler.CheckAction(actionType, leftActionManagerObject, rightActionManagerObject);
        if (noExtraConditions && walkToGroupName != "")
            UpdateIsCurrentActionValue(savedIsCurrentAction);
    }

    void UpdateIsCurrentActionValue(bool value)
    {
        if(testObject != null)
            testObject.SetActive(value);
        isCurrentAction = value;
    }

    private void Update()
    {
        if (noExtraConditions && 
            actionType == ActionManager.ActionType.None && 
            walkToGroupName == "-" &&
            player.currentWTGName == "")
        {
            UpdateIsCurrentActionValue(true);
            return;
        }
        if (!savedIsCurrentAction)
        {
            UpdateIsCurrentActionValue(false);
            return;
        }
        
        if (walkToGroupName != "-")
            if (walkToGroupName != "" && player.currentWTGName != walkToGroupName)
            {
                UpdateIsCurrentActionValue(false);
                return;
            }
            
        if (!noExtraConditions)
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<ActionCondition_ItemInHand>() != null &&
                    !transform.GetChild(i).GetComponent<ActionCondition_ItemInHand>().Check())
                {
                    UpdateIsCurrentActionValue(false);
                    return;
                }
                if (transform.GetChild(i).GetComponent<ActionCondition_IsHandsTrackingMode>() != null &&
                    !transform.GetChild(i).GetComponent<ActionCondition_IsHandsTrackingMode>().Check())
                {
                    UpdateIsCurrentActionValue(false);
                    return;
                }
                if (transform.GetChild(i).GetComponent<ActionCondition_PlayerWTG>() != null &&
                    !transform.GetChild(i).GetComponent<ActionCondition_PlayerWTG>().Check())
                {
                    UpdateIsCurrentActionValue(false);
                    return;
                }
            }
        UpdateIsCurrentActionValue(true);
    }
}
