using UnityEngine;

public class InsulinSequence : AnimationSequenceState
{
    public int takePenFrame;
    public int startButtonFrame;
    public int endButtonFrame;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        inv.PutAllOnTable();
        inv.sequenceAborted = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takePenFrame))
            {
                inv.ForcePickItem("InsulinPenWithNeedle", false, true);

                PlayerAnimationManager.SetHandItem(false, GameObject.Find("InsulinPenWithNeedle"));
            }
            else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, startButtonFrame))
            {
                inv.RightHandObject.GetComponent<InsulinPen>().animateButton = true;
            }
            else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, endButtonFrame))
            {
                inv.RightHandObject.GetComponent<InsulinPen>().animateButton = false;
            }

            if (keyFrame < keyFrames.Count)
            {
                if (animator.speed != 0)
                {
                    if (PlayerAnimationManager.CompareFrames(frame, prevFrame, keyFrames[keyFrame]))
                    {
                        PlayerAnimationManager.NextSequenceStep(true);
                        animator.speed = 0f;
                        ++keyFrame;
                    }
                }
            }


            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (inv.LeftHandObject && inv.LeftHandObject.GetComponent<InsulinPen>())
            inv.LeftHandObject.GetComponent<InsulinPen>().animateButton = false;

        if (keyFrame >= keyFrames.Count && !inv.sequenceAborted)
        {
            InjectionPatient patient_v1 = GameObject.FindObjectOfType<InjectionPatient>();
            if (patient_v1 != null)
            {
                patient_v1.AfterSequenceDialogue();
                patient_v1.GetComponent<Animator>().SetTrigger("ShirtDown");
            }
            else
            {
                InjectionPatient_v2 patient_v2 = GameObject.FindObjectOfType<InjectionPatient_v2>();
                if (patient_v2 != null)
                {
                    patient_v2.AfterSequenceDialogue();
                    patient_v2.GetComponent<Animator>().SetTrigger("ShirtDown");
                }
                else
                {
                    Debug.LogWarning("Attempted to use InjectionPatient, but none found");
                }
            }
        }
    }
}
