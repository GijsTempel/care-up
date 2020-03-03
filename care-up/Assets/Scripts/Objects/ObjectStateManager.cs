using UnityEngine;

public class ObjectStateManager : MonoBehaviour
{
    public string lieAnimName = "";
    public string holdAnimName = "";
    public float holdAnimValue = 0f;
    public bool lockHoldState = true;
    public bool followLeft = true;
    public bool isActive = true;
    public bool followHoldingHand = true;

    private float lieAnimValue = 0f;
    private Animator animator;
    private PlayerAnimationManager playerAnimationManager;

    private bool IsInHands
    {
        get
        {
            if (transform.parent == null)
            {
                return false;
            }
            return (transform.parent.name == "toolHolder.L" || transform.parent.name == "toolHolder.R");
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAnimationManager = GameObject.FindObjectOfType<PlayerAnimationManager>();

        if (isActive)
        {
            animator.speed = 0;
        }
    }

    public void SetAnimation(bool isLieAnim = true, string animName = "")
    {
        if (animName != "")
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            if (isLieAnim && animName != lieAnimName)
            {
                lieAnimName = animName;

                if (!IsInHands)
                {
                    animator.Play(lieAnimName, 0, lieAnimValue);
                }
            }
            else if (!isLieAnim && animName != holdAnimName)
            {
                holdAnimName = animName;
                print(animName);

                if (IsInHands)
                {
                    animator.Play(holdAnimName, 0, holdAnimValue);
                }
            }
        }
    }

    void Update()
    {
        string animationName = lieAnimName;
        float animationValue = lieAnimValue;

        if (isActive)
        {
            animator.speed = 0;

            if (IsInHands)
            {
                bool f_left = true;

                if (followHoldingHand)
                {
                    f_left = transform.parent.name == "toolHolder.L";
                }
                else
                {
                    f_left = followLeft;
                }

                if (f_left)
                {
                    animationName = holdAnimName;
                    if (!lockHoldState)
                    {
                        holdAnimValue = playerAnimationManager.leftModifier02;
                    }
                    animationValue = holdAnimValue;
                }
                else
                {
                    animationName = holdAnimName;

                    if (!lockHoldState)
                    {
                        holdAnimValue = playerAnimationManager.rightModifier02;
                    }
                    animationValue = holdAnimValue;
                }
                animator.Play(animationName, 0, animationValue);
            }
            else if (!lockHoldState)
            {
                animationValue = playerAnimationManager.rightModifier02;
                lieAnimValue = animationValue;

                if (followLeft)
                {
                    animationValue = playerAnimationManager.leftModifier02;
                }
                animator.Play(animationName, 0, animationValue);
            }
        }
        else
        {
            animator.speed = 1.0f;
        }
    }
}
