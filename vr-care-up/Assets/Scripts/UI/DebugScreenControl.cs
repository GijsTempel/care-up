using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScreenControl : MonoBehaviour
{
    public void UpdateScreenPositionToWTG(string walkToGroupName)
    {
       GameObject posObj = transform.Find("Positions/" + walkToGroupName).gameObject;
       if (posObj != null)
       {
            transform.Find("DebugScreen").localPosition = posObj.transform.localPosition;
       }
    }
}
