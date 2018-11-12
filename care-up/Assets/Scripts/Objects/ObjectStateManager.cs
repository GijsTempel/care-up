using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateManager : MonoBehaviour {
    Animator animator;
    PlayerAnimationManager playerAnimationManager;
    public string LieAnimName = "";
    public string HoldAnimName = "";
    float HoldAnimValue = 0f;
    float LieAnimValue = 0f;
    public bool LockHoldState = true;


    // Use this for initialization

    void Start () {
        animator = GetComponent<Animator>();
        playerAnimationManager = GameObject.FindObjectOfType<PlayerAnimationManager>();
        animator.speed = 0;

    }

	void Update () {
        //print(transform.parent.name);
        string anim_name = LieAnimName;
        float anim_value = LieAnimValue;

        if (transform.parent != null)
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

        }
        animator.Play(anim_name, 0, anim_value);
    }
}
