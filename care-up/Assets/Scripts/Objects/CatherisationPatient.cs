using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatherisationPatient : PersonObject {

    private Animator animator;

    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public Transform playerPositionTarget;

    protected override void Start()
    {
        base.Start();
        
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
                    break;
                case "HelpGetUp":
                    PlayerAnimationManager.PlayAnimation("helppatientgetup");
                    break;
                case "WashHands":
                    Animator PlayerAnim = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
                    PlayerAnim.SetTrigger("MoveToSide");
                    PlayerAnim.SetTrigger("S MoveToSide");

                    animator.SetTrigger("patient_standup");
                    break;

                    
                default:
                    break;
            }

            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }
}
