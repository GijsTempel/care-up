using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_ActionTriggerIgniter : MonoBehaviour
{
    public ActionModule_ActionTrigger actionTrigger;

    public void Execute()
    {
        actionTrigger.AttemptTrigger();
    }
}
