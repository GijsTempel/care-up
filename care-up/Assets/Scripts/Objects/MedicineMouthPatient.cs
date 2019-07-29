using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineMouthPatient : PersonObject {

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
                case "Sitstraight":
                    PlayerAnimator.SetTrigger("Player_TakeOffBlanket");
                    PlayerAnimator.SetTrigger("S Player_TakeOffBlanket");
                    break;
                case "MouthOpen":
                    PlayerAnimator.SetTrigger("StartCheekPouch_SQ2");
                    PlayerAnimator.SetTrigger("S StartCheekPouch_SQ2");
                    break;
                default:
                    break;
            }

            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }
}
