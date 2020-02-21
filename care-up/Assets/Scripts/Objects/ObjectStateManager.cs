using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateManager : MonoBehaviour {
    Animator animator;
    PlayerAnimationManager playerAnimationManager;
    public string LieAnimName = "";
    public string HoldAnimName = "";
    public float HoldAnimValue = 0f;
    float LieAnimValue = 0f;
    public bool LockHoldState = true;
    public bool follow_left = true;
    public bool isActive = true;
    public bool followHoldingHand = true;


    // Use this for initialization

    void Start () {
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
            if (isLieAnim && animName != LieAnimName)
            {
                LieAnimName = animName;
                if (!isInHands())
                {
                    animator.Play(LieAnimName, 0, LieAnimValue);
                }
            }
            else if (!isLieAnim && animName != HoldAnimName)
            {
                HoldAnimName = animName;
                print(animName);
                if (isInHands())
                {
                    animator.Play(HoldAnimName, 0, HoldAnimValue);
                }
            }
        }
    }


    bool isInHands()
    {
        if (transform.parent == null)
        {
            return false;
        }
        return (transform.parent.name == "toolHolder.L" || transform.parent.name == "toolHolder.R");
    }


	void Update () {
        string anim_name = LieAnimName;
        float anim_value = LieAnimValue;

        if (isActive)
        {
            animator.speed = 0;
            if (isInHands())
            {
                bool f_left = true;

                if (followHoldingHand)
                {
                    f_left = transform.parent.name == "toolHolder.L";
                }
                else
                {
                    f_left = follow_left;
                }



                if (f_left)
                {
                    anim_name = HoldAnimName;
                    if (!LockHoldState)
                    {
                        HoldAnimValue = playerAnimationManager.leftModifier02;
                    }
                    anim_value = HoldAnimValue;
                }
                else
                {
                    anim_name = HoldAnimName;
                    if (!LockHoldState)
                    {
                        HoldAnimValue = playerAnimationManager.rightModifier02;
                    }
                    anim_value = HoldAnimValue;
                }
                animator.Play(anim_name, 0, anim_value);
            }
            else if (!LockHoldState)
            {
                anim_value = playerAnimationManager.rightModifier02;
                LieAnimValue = anim_value;
                if (follow_left)
                {
                    anim_value = playerAnimationManager.leftModifier02;
                }
                animator.Play(anim_name, 0, anim_value);
            }
        }
        else
        {
            animator.speed = 1.0f;
        }

    }
}
