using UnityEngine;

public class MedicineMouthPatient : PersonObject
{
    private Animator patientAnimator;
    private Animator playerAnimator;
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public Transform playerPositionTarget;
    

    protected override void Start()
    {
        base.Start();
        playerAnimator = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
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
                case "Sitstraight":
                    /*playerAnimator.SetTrigger("Player_TakeOffBlanket");
                    playerAnimator.SetTrigger("S Player_TakeOffBlanket");*/

                    patientAnimator.SetTrigger("Patient_JaHoor");

                    break;

                case "MouthOpen":
                    playerAnimator.SetTrigger("OpenMouth");
                    playerAnimator.SetTrigger("S OpenMouth");

                    
                    break;

                case "Hello":

                    playerAnimator.SetTrigger("Player_Dialog_Greeting");
                    playerAnimator.SetTrigger("S Player_Dialog_Greeting");

                    break;

                case "NoSwallow":

                    patientAnimator.SetTrigger("Patient_Oke2");

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
