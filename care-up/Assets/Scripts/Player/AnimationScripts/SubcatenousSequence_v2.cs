using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubcatenousSequence_v2 : AnimationSequenceState
{
    public int dropClothFrame;
    public int takeSyringeFrame;
    public int takeOffCapFrame;
    public int dropCapFrame;
    public int takeClothAgain;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        inv.PutAllOnTable();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropClothFrame))
        {
            inv.DeleteAnimationObject();
            //inv.PutAllOnTable();
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takeSyringeFrame))
        {
            inv.ForcePickItem("SyringeWithInjectionSNeedleCap", false);
            PlayerAnimationManager.SetHandItem(false, GameObject.Find("SyringeWithInjectionSNeedleCap"));
            inv.RightHandObject.GetComponent<Syringe>().updatePlunger = true;
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takeClothAgain))
        {
            inv.CreateAnimationObject("Cloth", true);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takeOffCapFrame))
        {
            inv.ReplaceHandObject(false, "SyringeWithInjectionSNeedle");

            GameObject cap = inv.CreateObjectByName("SyringeInjectionSCap", Vector3.zero);

            Vector3 savedPos = Vector3.zero;
            Quaternion savedRot = Quaternion.identity;
            inv.RightHandObject.GetComponent<PickableObject>().GetSavesLocation(out savedPos, out savedRot);
            float offset = inv.RightHandObject.GetComponent<MeshFilter>().mesh.bounds.size.z * inv.RightHandObject.transform.lossyScale.z +
                            cap.GetComponent<MeshFilter>().mesh.bounds.size.z * cap.transform.lossyScale.z;
            cap.GetComponent<PickableObject>().SavePosition(savedPos + new Vector3(0, 0, -3f * offset), savedRot);

            inv.ForcePickItem("SyringeInjectionSCap", true);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropCapFrame))
        {
            inv.DropLeftObject();
        }

        if (keyFrame < keyFrames.Count)
        {
            if (animator.speed != 0)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, keyFrames[keyFrame]))
                {
                    if (keyFrame == 0)
                    {
                        inv.CreateAnimationObject("Cloth", false);
                        //inv.ForcePickItem("DesinfectionCloth", false);
                        //PlayerAnimationManager.SetHandItem(false, GameObject.Find("DesinfectionCloth"));
                    }

                    PlayerAnimationManager.NextSequenceStep(true);
                    animator.speed = 0f;
                    ++keyFrame;
                }
            }
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        inv.DeleteAnimationObject();

        if (inv.LeftHandObject && inv.LeftHandObject.GetComponent<Syringe>())
            inv.LeftHandObject.GetComponent<Syringe>().updatePlunger = false;

        GameObject.FindObjectOfType<InjectionPatient>().AfterSequenceDialogue();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
