using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FraxiparineSequence : AnimationSequenceState
{
    public int takeSyringeFrame;
    public int takeOffCapFrame;
    public int dropCapFrame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inv.PutAllOnTable();

        inv.sequenceAborted = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takeSyringeFrame))
        {
            inv.ForcePickItem("Frexi_with_needle_cap", false);
            PlayerAnimationManager.SetHandItem(false, GameObject.Find("Frexi_with_needle_cap"));
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takeOffCapFrame))
        {
            inv.ReplaceHandObject(false, "Frexi");

            GameObject cap = inv.CreateObjectByName("SyringeInjectionCap", Vector3.zero);

            Vector3 savedPos = Vector3.zero;
            Quaternion savedRot = Quaternion.identity;
            inv.RightHandObject.GetComponent<PickableObject>().GetSavesLocation(out savedPos, out savedRot);
            float offset = inv.RightHandObject.GetComponent<MeshFilter>().mesh.bounds.size.z * inv.RightHandObject.transform.lossyScale.z +
                            cap.GetComponent<MeshFilter>().mesh.bounds.size.z * cap.transform.lossyScale.z;
            cap.GetComponent<PickableObject>().SavePosition(savedPos + new Vector3(0, 0, -3f * offset), savedRot);

            inv.ForcePickItem("SyringeInjectionCap", true);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropCapFrame))
        {
            inv.RemoveHandObject(true); // left
            inv.RightHandObject.GetComponent<FraxiparineSyringe>().updatePlunger = true;
        }

        if (keyFrame < keyFrames.Count)
        {
            if (animator.speed != 0)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, keyFrames[keyFrame]))
                {
                    PlayerAnimationManager.NextSequenceStep(true);
                    if (keyFrame == 0)
                    {
                        if (GameObject.Find("GameLogic") != null)
                        {
                            if (GameObject.Find("GameLogic").GetComponent<TutorialManager>() != null)
                            {
                                PlayerAnimationManager.SequenceTutorialLock(true);
                            }
                        }
                    }
                    animator.speed = 0f;
                    ++keyFrame;
                }
            }
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (inv.RightHandObject && inv.RightHandObject.GetComponent<FraxiparineSyringe>())
            inv.RightHandObject.GetComponent<FraxiparineSyringe>().updatePlunger = false;

        if (keyFrame >= 2 && inv.sequenceAborted)
        {
            if (keyFrame < keyFrames.Count)
            {
                if (inv.RightHandObject && inv.RightHandObject.GetComponent<FraxiparineSyringe>())
                {
                    inv.ReplaceHandObject(false, "Frexi_with_needle_cap");
                    inv.PutAllOnTable();
                    Destroy(GameObject.Find("SyringeInjectionCap"));
                }
            }
            else
            {
                if (inv.LeftHandObject && inv.LeftHandObject.GetComponent<FraxiparineSyringe>())
                {
                    inv.ReplaceHandObject(true, "Frexi_with_needle_cap");
                    inv.PutAllOnTable();
                    Destroy(GameObject.Find("SyringeInjectionCap"));
                }
            }
        }

        if (keyFrame >= keyFrames.Count && !inv.sequenceAborted)
        {
            GameObject.FindObjectOfType<InjectionPatient>().AfterSequenceDialogue();
            GameObject.FindObjectOfType<InjectionPatient>().GetComponent<Animator>().SetTrigger("ShirtDown");
        }
    }
}
