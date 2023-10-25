using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ItemInHandCheck : MonoBehaviour
{
    public ActionTrigger.TriggerHand handSide;
    public string objectInHand = "";
    public bool invert = false;

    public bool Check()
    {
        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
        string ss = "@Objects In Hands: L|";
        if (player.GetObjectInHand(true) != null)
            ss += (player.GetObjectInHand(true).name + " ");
        ss += "R|";
        if (player.GetObjectInHand(false) != null)
            ss += (player.GetObjectInHand(false).name + " ");
        
        Debug.Log(ss);
        if (handSide == ActionTrigger.TriggerHand.None)
        {
            if (player.GetObjectInHand(true) != null && player.GetObjectInHand(false) == true)
                return true;
        }
        if (handSide == ActionTrigger.TriggerHand.Any)
        {
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
        if (handSide == ActionTrigger.TriggerHand.Left)
        {
            if (player.GetObjectInHand(true) != null && 
                player.GetObjectInHand(true).name == objectInHand)
                {
                    if (invert)
                        return false;
                    return true;
                }
        }
        if (handSide == ActionTrigger.TriggerHand.Right)
        {
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
