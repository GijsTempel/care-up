using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstomyCarePatient : PersonObject {

    private Animator patientAnimator;
    private Animator PlayerAnimator;
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public Transform playerPositionTarget;

    protected override void Start()
    {
        base.Start();
        PlayerAnimator = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        patientAnimator = GetComponent<Animator>();
    }

    public override void Talk(string topic = "", string audio = "")
    {
        if (ViewModeActive() || topic == "CM_Leave" || topic == "")
            return;

        if (actionManager.CompareTopic(topic))
        {
            switch (topic)
            {
                case "LayInHalfPosOnBed":
                    //animator.SetTrigger("sit_up_in_bed");
                    /* PlayerAnimator.SetTrigger("AllowPatientToLay");
                     PlayerAnimator.SetTrigger("S AllowPatientToLay");*/

                    patientAnimator.SetTrigger("Patient_Oke");
                    break;
              
                case "Hello":

                    PlayerAnimator.SetTrigger("Player_Dialog_Greeting");
                    PlayerAnimator.SetTrigger("S Player_Dialog_Greeting");

                    break;

                default:
                    break;
            }

            AttemptPlayAudioAfterTalk(audio);
            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }
}
