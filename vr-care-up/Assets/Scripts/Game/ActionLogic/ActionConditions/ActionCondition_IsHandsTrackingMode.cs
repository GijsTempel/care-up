using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCondition_IsHandsTrackingMode : MonoBehaviour
{
    public enum CheckingForMode
    {
        isTrackingMode,
        isControllerMode,
    };
    public CheckingForMode checkingForMode;

    public bool Check()
    {
        if (checkingForMode == CheckingForMode.isTrackingMode)
            return GameObject.FindObjectOfType<HTrackingHand>() != null;
        if (checkingForMode == CheckingForMode.isControllerMode)
            return GameObject.FindObjectOfType<HTrackingHand>() == null;
        return false;
    }
}
