using UnityEngine;

public class FraxiparineTubeAnimation : AnimationUseOn
{
    public bool hand;

    public int tubeStart;
    public int tubeEnd;

    private float frame = 0f;
    private float prevFrame = 0f;

    private FraxiparineSyringe syringe;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        syringe = hand ? inv.LeftHandObject.GetComponent<FraxiparineSyringe>() : inv.RightHandObject.GetComponent<FraxiparineSyringe>();

        frame = 0f;
        prevFrame = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, tubeStart))
        {
            syringe.updateTube = true;
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, tubeEnd))
        {
            syringe.updateTube = false;
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        syringe.updateTube = false;
    }
}