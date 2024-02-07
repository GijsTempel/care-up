using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToTriggerAction : MonoBehaviour
{
    public void OnButtonClicked()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ActionModule_ActionTrigger>())
                transform.GetChild(i).GetComponent<ActionModule_ActionTrigger>().AttemptTrigger();
        }
    }

}
