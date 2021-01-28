using UnityEngine;

public class InsulfonInsertationPatient : PersonObject {

   // private Animator PatientAnimator;

    private Animator PlayerAnimator;
    private Animator PatientAnimator;

    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public Transform playerPositionTarget;

    protected override void Start()
    {
        base.Start();
        PlayerAnimator = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        PatientAnimator = GetComponent<Animator>();
    }

    public override void Talk(string topic = "", string audio = "")
    {
        if (ViewModeActive() || topic == "CM_Leave" || topic == "")
            return;

        if (actionManager.CompareTopic(topic))
        {
            switch (topic)
            {
                case "show injection spot":
                    PlayerAnimator.SetTrigger("BlanketEmpty");
                    PlayerAnimator.SetTrigger("S BlanketEmpty");
                    //PlayerAnimator.SetTrigger("TakeOffBlanket");

                    //PatientAnimator.SetTrigger("Patient_Zeker");
                    break;

                case "SitInBed":
                    /*PlayerAnimator.SetTrigger("StartSittingInBedAnimation");
                    PlayerAnimator.SetTrigger("S StartSittingInBedAnimation");*/
                    PatientAnimator.SetTrigger("Patient_Oke");

                    break;

                default:
                    break;
                case "Hello":

                    PlayerAnimator.SetTrigger("Player_Dialog_Greeting");
                    PlayerAnimator.SetTrigger("S Player_Dialog_Greeting");

                    break;

                case "InstructSign":

                    PatientAnimator.SetTrigger("Patient_Ja");

                    break;
            }

            AttemptPlayAudioAfterTalk(audio);
            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }
}
