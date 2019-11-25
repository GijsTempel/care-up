using System.Collections.Generic;
using UnityEngine;

public class DisableComponent : StateMachineBehaviour
{
    [SerializeField]
    private int selectedFrame;

    [SerializeField]
    private List<string> componentsNames;

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
                foreach (string name in objectsNames)
                {
                    GameObject item = GameObject.Find(name);

                    if (item)
                    {
                        foreach (string componentName in componentsNames)
                        {
                            if (item.GetComponent(componentName))
                                item.GetComponent(componentName).gameObject.SetActive(enable);
                        }
                    }
                }

                prevFrame = frame;
                frame += Time.deltaTime;
            }
        }
    }
}
