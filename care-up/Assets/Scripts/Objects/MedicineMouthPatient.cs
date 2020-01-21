using UnityEngine;

public class MedicineMouthPatient : PersonObject
{
    private Animator animator;
    private Animator playerAnimator;
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public Transform playerPositionTarget;

    protected override void Start()
    {
        base.Start();
        playerAnimator = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        animator = GetComponent<Animator>();
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
                    playerAnimator.SetTrigger("Player_TakeOffBlanket");
                    playerAnimator.SetTrigger("S Player_TakeOffBlanket");
                    break;
                case "MouthOpen":
                    playerAnimator.SetTrigger("OpenMouth");
                    playerAnimator.SetTrigger("S OpenMouth");
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
