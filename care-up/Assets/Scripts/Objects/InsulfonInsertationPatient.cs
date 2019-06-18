using UnityEngine;

public class InsulfonInsertationPatient : PersonObject {

   // private Animator PatientAnimator;

    private Animator PlayerAnimator;

    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public Transform playerPositionTarget;

    protected override void Start()
    {
        base.Start();
        PlayerAnimator = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
      //  PatientAnimator = GetComponent<Animator>();
    }

    public override void Talk(string topic = "")
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
                    PlayerAnimator.SetTrigger("TakeOffBlanket");
                    break;

                case "SitInBed":
                    PlayerAnimator.SetTrigger("StartSittingInBedAnimation");
                    PlayerAnimator.SetTrigger("S StartSittingInBedAnimation");
                    break;

                default:
                    break;
            }

            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }
}
