using System.Collections.Generic;
using UnityEngine;

public class DisableBoxCollider : StateMachineBehaviour
{
    [SerializeField]
    private int selectedFrame;

    [SerializeField]
    private List<string> objectsNames;

    [SerializeField]
    private bool enable;

    protected float frame;
    protected float prevFrame;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, selectedFrame) && (objectsNames.Count > 0))
            {
                BoxCollider component;

                foreach (string name in objectsNames)
                {
                    component = GameObject.Find(name).GetComponent<BoxCollider>();
                    if (component)
                        component.enabled = false;
                }
            }

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }
}
