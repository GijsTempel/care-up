using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncWithObject : StateMachineBehaviour
{
    public string SyncObject;
    SyncAnim[] syncAnimations;
    GameObject syncer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        syncer = GameObject.Find(SyncObject);

        if (syncer != null)
        {
            syncAnimations = syncer.GetComponents<SyncAnim>();

            foreach (var anim in syncAnimations)
                anim.IsSyncing = true;
        }



        /*if (GameObject.Find(Syncer))
        {
            foreach (GameObject.Find(Syncer).GetComponent<SyncAnim>() in GameObject.Find(Syncer))
            {
                GameObject.Find(Syncer).GetComponent<SyncAnim>().IsSyncing = true;
            }

        }*/


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (syncer != null)
        {
            syncAnimations = syncer.GetComponents<SyncAnim>();

            foreach (var anim in syncAnimations)
                anim.IsSyncing = false;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
