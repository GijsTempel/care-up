using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ActionExpectant : MonoBehaviour
{
    private ActionHandler actionHandler;
    public bool isCurrentAction = false;
    private bool savedIsCurrentAction = false;
    public ActionManager.ActionType actionType = ActionManager.ActionType.None;
    public string leftActionManagerObject = "";
    public string rightActionManagerObject = "";
    public string walkToGroupName = "";
    bool noExtraConditions = true;
    private PlayerScript player;
    public bool debugElement = false;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        UpdateAction();
        for (int i = 0; i < transform.childCount; i ++)
        {
            if (transform.GetChild(i).GetComponent<ItemInHandCheck>() != null)
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
            if (transform.GetChild(i).GetComponent<ActionTrigger>() != null)
            {
                transform.GetChild(i).GetComponent<ActionTrigger>().AttemptTrigger();
                result = true;
            }
        }
        return result;
    }
    
    
    public void UpdateAction()
    {
        if (actionHandler == null)
            actionHandler = GameObject.FindObjectOfType<ActionHandler>();
        
        if (debugElement)
        {
            Debug.Log("ddd");
        }
        savedIsCurrentAction = actionHandler.CheckAction(actionType, leftActionManagerObject, rightActionManagerObject);
        if (noExtraConditions && walkToGroupName != "")
            isCurrentAction = savedIsCurrentAction;
    }

    private void Update()
    {
        if (noExtraConditions && 
            actionType == ActionManager.ActionType.None && 
            walkToGroupName == "-" &&
            player.currentWTGName == "")
        {
            isCurrentAction = true;
            return;
        }
        if (!savedIsCurrentAction)
        {
            isCurrentAction = false;
            return;
        }
        
        if ((walkToGroupName != "" && player.currentWTGName != walkToGroupName))
        {
            isCurrentAction = false;
            return;
        }
        if (!noExtraConditions)
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<ItemInHandCheck>() != null)
                {
                    if (!transform.GetChild(i).GetComponent<ItemInHandCheck>().Check())
                    {
                        isCurrentAction = false;
                        return;
                    }
                }
            }
        isCurrentAction = true;
    }
}
