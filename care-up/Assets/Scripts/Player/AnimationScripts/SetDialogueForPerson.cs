using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDialogueForPerson : StateMachineBehaviour {
    public bool next = false;

    public int actionFrame;
    public string ObjectName = "";
    public int DialogueNum = 0;


    protected float frame = 0f;
    protected float prevFrame = 0f;


    void setDialogue()
    {
        if (GameObject.Find(ObjectName) != null)
        {
            if (GameObject.Find(ObjectName).GetComponent<PersonObject>() != null) {
                if (next)
                {
                    GameObject.Find(ObjectName).GetComponent<PersonObject>().NextDialogue();
                }
                else
                {
                    if (DialogueNum > GameObject.Find(ObjectName).GetComponent<PersonObject>().currentDialogueIndex)
                        GameObject.Find(ObjectName).GetComponent<PersonObject>().SetDialogue(DialogueNum);
                }
            }
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame == 0)
            setDialogue();

    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0 && actionFrame != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
                setDialogue();

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0;
        prevFrame = 0;
    }
}
