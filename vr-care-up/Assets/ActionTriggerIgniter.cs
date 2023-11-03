using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerIgniter : MonoBehaviour
{
    public ActionTrigger actionTrigger;

    public void Execute()
    {
        actionTrigger.AttemptTrigger();
    }
}
