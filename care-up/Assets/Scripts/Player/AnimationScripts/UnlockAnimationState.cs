using UnityEngine;

public class UnlockAnimationState : StateMachineBehaviour
{
    public string objectName = "";
    public int unlockFrame;
    public int lockFrame;
    public bool toActivate = false;

    protected float frame = 0f;
    protected float prevFrame = 0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        if (unlockFrame == 0)
        {
            Lock(false);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, unlockFrame))
            {
                Lock(false);
            }
            if (lockFrame > unlockFrame)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, lockFrame))
                {
                    Lock(true);
                }
            }
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    private void Lock(bool value)
    {
        if (GameObject.Find(objectName) != null)
        {
            foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (gameObj.name == objectName)
                {
                    bool isSihlouette = false;
                    if (gameObj.GetComponent<PickableObject>() != null)
                    {
                        if (gameObj.GetComponent<PickableObject>().sihlouette)
                        {
                            isSihlouette = false;
                        }
                    }

                    if (!isSihlouette)
                    {
                        if (gameObj.GetComponent<ObjectStateManager>() != null)
                        {
                            gameObj.GetComponent<ObjectStateManager>().lockHoldState = value;
                            if (toActivate)
                            {
                                gameObj.GetComponent<ObjectStateManager>().isActive = !value;
                                //Debug.Log(gameObj.GetComponent<ObjectStateManager>().isActive);
                            }
                        }
                        else if (gameObj.GetComponent<Syringe>() != null)
                        {
                            gameObj.GetComponent<Syringe>().updatePlunger = !value;
                        }
                    }
                }
            }
        }
    }
}
