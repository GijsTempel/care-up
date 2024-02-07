using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ActionCondition_ItemInHand : MonoBehaviour
{
    public ActionModule_ActionTrigger.TriggerHand handSide;
    [Tooltip("If empty check if hand is empty")]
    public string objectInHand = "";
    public bool invert = false;

    public bool Check()
    {
        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
        if (handSide == ActionModule_ActionTrigger.TriggerHand.None)
        {
            if (player.GetObjectInHand(true) != null && player.GetObjectInHand(false) == true)
                return true;
        }
        if (handSide == ActionModule_ActionTrigger.TriggerHand.Any)
        {
            if (!invert && objectInHand == "" &&
                player.GetObjectInHand(true) == null &&
                player.GetObjectInHand(false) == null)
                    return true;

            if ((player.GetObjectInHand(true) != null && 
                player.GetObjectInHand(true).name == objectInHand) ||
                (player.GetObjectInHand(false) != null &&
                player.GetObjectInHand(false).name == objectInHand))
            {
                if (invert)
                    return false;
                return true;
            }
        }
        if (handSide == ActionModule_ActionTrigger.TriggerHand.Left)
        {
            if (!invert && objectInHand == "" &&
                player.GetObjectInHand(true) == null)
                    return true;
            if (player.GetObjectInHand(true) != null && 
                player.GetObjectInHand(true).name == objectInHand)
                {
                    if (invert)
                        return false;
                    return true;
                }
        }
        if (handSide == ActionModule_ActionTrigger.TriggerHand.Right)
        {
            if (!invert && objectInHand == "" &&
                player.GetObjectInHand(false) == null)
                    return true;
            if (player.GetObjectInHand(false) != null && 
                player.GetObjectInHand(false).name == objectInHand)
                {
                    if (invert)
                        return false;
                    return true;
                }
        }
        if (invert)
            return true;
        return false;
    }
}
