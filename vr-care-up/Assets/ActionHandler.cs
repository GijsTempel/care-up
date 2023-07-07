using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    ActionManager actionManager;
    public void TryExecuteAction(ActionManager.ActionType actionType, string leftHandObjectName, string rightHandObjectName)
    {
        if (actionManager == null)
            actionManager = GameObject.FindObjectOfType<ActionManager>();
        if (actionManager == null)
            return;

        switch (actionType)
        {
            case ActionManager.ActionType.PersonTalk:
                actionManager.OnTalkAction(leftHandObjectName);
                break;
            case ActionManager.ActionType.ObjectUse:
                actionManager.OnUseAction(leftHandObjectName);
                break;
            case ActionManager.ActionType.ObjectCombine:
                actionManager.OnCombineAction(leftHandObjectName, rightHandObjectName);
                break;
            case ActionManager.ActionType.ObjectExamine:
                string expected = (rightHandObjectName == "") ? "good" : rightHandObjectName;
                actionManager.OnExamineAction(leftHandObjectName, expected);
                break;
            case ActionManager.ActionType.ObjectUseOn:
                actionManager.OnUseOnAction(leftHandObjectName, rightHandObjectName);
                break;
            case ActionManager.ActionType.SequenceStep:
                actionManager.OnSequenceStepAction(leftHandObjectName);
                break;
            default:
                Debug.LogWarning(actionType + " is not supported yet in ActionHandler. TODO");
                break;
        }
    }
    
}
