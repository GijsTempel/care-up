using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncWithObject : StateMachineBehaviour
{
    public string SyncObject;
    public string syncAnimationName = "";
    SyncAnim[] syncAnimations;
    GameObject syncer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        syncer = GameObject.Find(SyncObject);

        if (syncer != null)
        {
            syncAnimations = syncer.GetComponents<SyncAnim>();

            foreach (SyncAnim anim in syncAnimations)
            {

                if (syncAnimationName != "")
                    anim.AnimName = syncAnimationName;
                anim.IsSyncing = true;
            }
        }
        else
        {
            Debug.Log("Syncer error!: "+syncer.name + " not found");
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (syncer != null)
        {
            syncAnimations = syncer.GetComponents<SyncAnim>();

            foreach (var anim in syncAnimations)
                anim.IsSyncing = false;
        }
    }

}
