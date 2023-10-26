using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerButton : MonoBehaviour
{
    public void OnButtonClicked()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ActionTrigger>())
                transform.GetChild(i).GetComponent<ActionTrigger>().AttemptTrigger();
        }
    }

}
