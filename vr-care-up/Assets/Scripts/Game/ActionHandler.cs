using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    private ActionManager actionManager;
    private GameUIVR gameUIVR;
    private VRCollarHolder vrCollarHolder;
    
    void Start()
    {
        vrCollarHolder = GameObject.FindObjectOfType<VRCollarHolder>();
    }
  
    public bool TryExecuteAction(ActionManager.ActionType actionType, string leftHandObjectName, string rightHandObjectName)
    {
        if (actionManager == null)
            actionManager = GameObject.FindObjectOfType<ActionManager>();
        if (gameUIVR == null)
            gameUIVR = GameObject.FindObjectOfType<GameUIVR>();
        if (actionManager == null)
            return false;
        bool result = false;
        switch (actionType)
        {
            case ActionManager.ActionType.PersonTalk:
                if (result = actionManager.OnTalkAction(leftHandObjectName))
                { // ^intentionally assign action, not compare
                    //if (TalkingActionModule.latestCaller != null)
                    //{
                    //    TalkingActionModule.latestCaller.CompleteDialogueLoadNext();
                    //}
                }
                
                // BIG difference between sequence and talk action:
                // talk action closes selection on ANY option
                SelectDialogue talkDialogue = GameObject.FindObjectOfType<SelectDialogue>();
                if (talkDialogue != null)
                {
                    talkDialogue.gameObject.SetActive(false);
                }
                break;
            case ActionManager.ActionType.ObjectUse:
                result = actionManager.OnUseAction(leftHandObjectName);
                break;
            case ActionManager.ActionType.ObjectCombine:
                result = actionManager.OnCombineAction(leftHandObjectName, rightHandObjectName);
                break;
            case ActionManager.ActionType.ObjectExamine:
                string expected = (rightHandObjectName == "") ? "good" : rightHandObjectName;
                result = actionManager.OnExamineAction(leftHandObjectName, expected);
                break;
            case ActionManager.ActionType.ObjectUseOn:
                result = actionManager.OnUseOnAction(leftHandObjectName, rightHandObjectName);
                break;
            case ActionManager.ActionType.SequenceStep:
                if (result = actionManager.OnSequenceStepAction(leftHandObjectName))
                { // ^intentionally assign action, not compare
                    SelectDialogue dialogue = GameObject.FindObjectOfType<SelectDialogue>();
                    if (dialogue != null)
                    {
                        dialogue.gameObject.SetActive(false);
                    }
                }
                break;
            default:
                Debug.LogWarning(actionType + " is not supported yet in ActionHandler. TODO");
                break;
        }
        if (gameUIVR != null)
            gameUIVR.UpdateHelpHighlight();
        
        if (vrCollarHolder != null)
        {
            vrCollarHolder.CloseTutorialShelf();
        }
        return result;
    }

    public bool CheckAction(ActionManager.ActionType actionType, string leftHandObjectName, string rightHandObjectName)
    {
        string[] info = { leftHandObjectName, rightHandObjectName };
        if (actionManager == null)
            actionManager = GameObject.FindObjectOfType<ActionManager>();
        return actionManager.OnlyCheck(info, actionType);
    }
    
}
