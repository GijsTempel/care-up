using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ActionExpectant : MonoBehaviour
{
    private ActionHandler actionHandler;
    public bool isCurrentAction = false;
    public ActionManager.ActionType actionType = ActionManager.ActionType.None;
    public string leftActionManagerObject = "";
    public string rightActionManagerObject = "";

    void Start()
    {
        UpdateAction();
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
        
        isCurrentAction = actionHandler.CheckAction(actionType, leftActionManagerObject, rightActionManagerObject);
    }
}
