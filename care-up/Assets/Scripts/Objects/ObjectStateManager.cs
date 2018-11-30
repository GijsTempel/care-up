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

    // Use this for initialization

    void Start () {
        animator = GetComponent<Animator>();
        playerAnimationManager = GameObject.FindObjectOfType<PlayerAnimationManager>();
        animator.speed = 0;

    }

	void Update () {
        string anim_name = LieAnimName;
        float anim_value = LieAnimValue;
        bool in_hands = false;
        if (transform.parent != null)
        {
            in_hands = (transform.parent.name == "toolHolder.L" || transform.parent.name == "toolHolder.R");
        }
         

        if (in_hands)
        {
            if (transform.parent.name == "toolHolder.L")
            {
                anim_name = HoldAnimName;
                if (!LockHoldState)
                {
                    HoldAnimValue = playerAnimationManager.leftModifier02;
                }
                anim_value = HoldAnimValue;
            }
            else if (transform.parent.name == "toolHolder.R")
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
}
