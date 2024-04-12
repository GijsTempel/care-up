using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncAnim : MonoBehaviour
{

    // protected Animator PlayerAnimator;
    public Animator MasterObject;
    public Animator SlaveObject;
    public string AnimName;
    public bool IsSyncing;

    // Update is called once per frame
    void Update()
    {
        if(IsSyncing)
        {
            if (!MasterObject.IsInTransition(0) && SlaveObject != null)
            {
                float nTime = Mathf.Repeat(MasterObject.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);
                if (MasterObject.GetAnimatorTransitionInfo(0).normalizedTime == 0f)
                    SlaveObject.Play(AnimName, -1, nTime);
            }
        }
    }
}
