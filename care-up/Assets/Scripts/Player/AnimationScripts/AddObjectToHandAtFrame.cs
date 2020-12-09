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
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, addFrame))
            {
                AddObject();
            }    
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (addFrame / 60f > frame)
        {
            AddObject();
        }

        frame = 0f;
        obj = null;
    }

    private void AddObject()
    {
        inventory.CreateObjectByName(objectName, Vector3.zero, callback => obj = callback);
        inventory.ForcePickItem(obj, hand);
        PlayerAnimationManager.SetHandItem(hand == PlayerAnimationManager.Hand.Left, obj);

        if (GhostObjectTarget != "" && obj != null)
        {
            if (GameObject.Find(GhostObjectTarget) != null)
            {
                Transform targetObj = GameObject.Find(GhostObjectTarget).transform;
                obj.GetComponent<PickableObject>().InstantiateGhostObject(targetObj.position, targetObj.rotation, 0);

                bool isInList = false;
                HandsInventory.GhostPosition CGP = new HandsInventory.GhostPosition();
                CGP.position = targetObj.position;
                CGP.rotation = targetObj.rotation.eulerAngles;
                CGP.objectName = objectName;
                obj.GetComponent<PickableObject>().SavePosition(CGP.position, targetObj.rotation, true);
                if (inventory.customGhostPositions.Count > 0)
                {
                    for (int i = 0; i < inventory.customGhostPositions.Count; i++)
                    {
                        if (inventory.customGhostPositions[i].objectName == objectName)
                        {
                            isInList = true;
                            inventory.customGhostPositions[i] = CGP;
                        }
                    }
                }
                if (!isInList)
                {
                    inventory.customGhostPositions.Add(CGP);
                }

                if (destroyOnDrop != AddObjectToHandAtFrame.DestroyStates.None)
                {
                    bool dod = destroyOnDrop == AddObjectToHandAtFrame.DestroyStates.True;
                    obj.GetComponent<PickableObject>().destroyOnDrop = dod;
                }
            }
        }
    }
}
