using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_DropCondition : MonoBehaviour
{
    public bool invertResult = false;
    public bool Check()
    {
        bool result = true;
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ActionModule_ActionExpectant>() != null)
            {
                if (!transform.GetChild(i).GetComponent<ActionModule_ActionExpectant>().isCurrentAction)
                {
                    result = false;
                    break;
                }
            }
        }

        if (invertResult)
            return !result;
        return result;
    }
}
