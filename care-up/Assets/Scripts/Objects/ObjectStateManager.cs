using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateManager : MonoBehaviour {
    Animator animator;
    PlayerAnimationManager playerAnimationManager;
    public string anim_name = "";
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        playerAnimationManager = GameObject.FindObjectOfType<PlayerAnimationManager>();
        animator.speed = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //print(transform.parent.name);
        if (transform.parent != null)
        {
            if (transform.parent.name == "toolHolder.L")
            {
                animator.Play(anim_name, 0, playerAnimationManager.leftModifier02);
            }
            else if (transform.parent.name == "toolHolder.R")
            {
                animator.Play(anim_name, 0, playerAnimationManager.rightModifier02);
            }

        }
    }
}
