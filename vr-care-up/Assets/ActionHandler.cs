using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    ActionManager actionManager;
    public void TryExecuteAction(ActionManager.ActionType actionType, string leftHandObjectName, string rightHandObjectName)
    {
        if (actionManager = null)
            actionManager = GameObject.FindObjectOfType<ActionManager>();
        if (actionManager == null)
            return;
        if (actionType == ActionManager.ActionType.ObjectUse)
        {
            actionManager.CompareUseObject(leftHandObjectName);
        }

    }
    
}
