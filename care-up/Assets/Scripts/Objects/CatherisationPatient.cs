using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatherisationPatient : PersonObject {

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
                case "LayOnBed":
                    animator.SetTrigger("pants_down");

                    GameObject playerPosAtPatient = GameObject.Find("PlayerPositions/PatientPos/Target");
                    playerPosAtPatient.transform.position = playerPositionTarget.position;
                    playerPosAtPatient.transform.rotation = playerPositionTarget.rotation;
                    PlayerAnimator.SetTrigger("CloseCurtains");
                    PlayerAnimator.SetTrigger("S CloseCurtains");
                    break;
                case "HelpGetUp":
                    PlayerAnimationManager.PlayAnimation("helppatientgetup");
                    break;
                default:
                    break;
            }

            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }
}
