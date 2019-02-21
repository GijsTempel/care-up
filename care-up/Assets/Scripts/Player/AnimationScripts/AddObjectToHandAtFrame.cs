using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToHandAtFrame : StateMachineBehaviour
{
    public int addFrame;
    public string objectName;
    public PlayerAnimationManager.Hand hand;

    protected float frame = 0f;
    protected float prevFrame;

    HandsInventory inventory;
    GameObject obj = null;
    public string GhostObjectTarget = "";
    public DestroyStates destroyOnDrop;

    public enum DestroyStates
    {
        None,
        True,
        False
    };


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inventory = GameObject.FindObjectOfType<HandsInventory>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(frame);
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, addFrame))
        {
            obj = inventory.CreateObjectByName(objectName, Vector3.zero);
            inventory.ForcePickItem(objectName, hand);
            PlayerAnimationManager.SetHandItem(hand == PlayerAnimationManager.Hand.Left, obj);
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GhostObjectTarget != "" && obj != null)
        {
            if (GameObject.Find(GhostObjectTarget) != null)
            {
                Transform targetObj = GameObject.Find(GhostObjectTarget).transform;
                obj.GetComponent<PickableObject>().InstantiateGhostObject(targetObj.position, targetObj.rotation, 0);
                if (destroyOnDrop != AddObjectToHandAtFrame.DestroyStates.None)
                {
                    bool dod = destroyOnDrop == AddObjectToHandAtFrame.DestroyStates.True;
                    obj.GetComponent<PickableObject>().destroyOnDrop = dod;
                }
            }
        }
        frame = 0f;
        obj = null;

    }

}
