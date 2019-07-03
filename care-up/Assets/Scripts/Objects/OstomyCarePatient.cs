using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstomyCarePatient : PersonObject {

    private Animator animator;
    private Animator PlayerAnimator;
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public Transform playerPositionTarget;

    protected override void Start()
    {
        base.Start();
        PlayerAnimator = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        animator = GetComponent<Animator>();
    }

    public override void Talk(string topic = "")
    {
        if (ViewModeActive() || topic == "CM_Leave" || topic == "")
            return;

        if (actionManager.CompareTopic(topic))
        {
            switch (topic)
            {
                case "LayInHalfPosOnBed":
                    //animator.SetTrigger("sit_up_in_bed");
                    PlayerAnimator.SetTrigger("AllowPatientToLay");
                    PlayerAnimator.SetTrigger("S AllowPatientToLay");
                    break;
                default:
                    break;
            }

            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }
}
