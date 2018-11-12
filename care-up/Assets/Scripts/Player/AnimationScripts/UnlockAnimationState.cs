using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAnimationState : StateMachineBehaviour
{

    public string ObjectName = "";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lock_function(false);
    }

    void lock_function(bool value)
    {
       
        if (GameObject.Find(ObjectName) != null)
        {
            if (GameObject.Find(ObjectName).GetComponent<ObjectStateManager>() != null)
            {
                GameObject.Find(ObjectName).GetComponent<ObjectStateManager>().LockHoldState = value;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lock_function(true);
    }

}
