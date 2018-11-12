using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateManager : MonoBehaviour {
    Animator animator;
    PlayerAnimationManager playerAnimationManager;
    public string lie_anim_name = "";
    public string hold_anim_name = "";
    float hold_anim_value = 0f;
    float lie_anim_value = 0f;

    // Use this for initialization

    void Start () {
        animator = GetComponent<Animator>();
        playerAnimationManager = GameObject.FindObjectOfType<PlayerAnimationManager>();
        animator.speed = 0;

    }

	void Update () {
        //print(transform.parent.name);
        string anim_name = lie_anim_name;
        float anim_value = lie_anim_value;

        if (transform.parent != null)
        {
            if (transform.parent.name == "toolHolder.L")
            {
                anim_name = hold_anim_name;
                anim_value = playerAnimationManager.leftModifier02;
            }
            else if (transform.parent.name == "toolHolder.R")
            {
                anim_name = hold_anim_name;
                anim_value = playerAnimationManager.rightModifier02;
            }

        }
        animator.Play(anim_name, 0, anim_value);
    }
}
