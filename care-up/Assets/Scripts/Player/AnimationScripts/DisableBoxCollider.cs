using System.Collections.Generic;
using UnityEngine;

public class DisableBoxCollider : StateMachineBehaviour
{
    [SerializeField]
    private int selectedFrame = default(int);

    [SerializeField]
    private List<string> objectsNames = default(List<string>);

    [SerializeField]
    private bool enable;

    protected float frame;
    protected float prevFrame;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, selectedFrame) && (objectsNames.Count > 0))
            {
                BoxCollider boxCollider;
                CapsuleCollider capsuleCollider;

                foreach (string name in objectsNames)
                {
                    boxCollider = GameObject.Find(name).GetComponent<BoxCollider>();
                    capsuleCollider = GameObject.Find(name).GetComponent<CapsuleCollider>();

                    if (boxCollider)
                        boxCollider.enabled = false;
                    if (capsuleCollider)
                        capsuleCollider.enabled = false;
                }
            }        
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (selectedFrame / 60f > frame)
        {
            BoxCollider boxCollider;
            CapsuleCollider capsuleCollider;

            foreach (string name in objectsNames)
            {
                boxCollider = GameObject.Find(name).GetComponent<BoxCollider>();
                capsuleCollider = GameObject.Find(name).GetComponent<CapsuleCollider>();

                if (boxCollider)
                    boxCollider.enabled = false;
                if (capsuleCollider)
                    capsuleCollider.enabled = false;
            }
        }
    }
}
