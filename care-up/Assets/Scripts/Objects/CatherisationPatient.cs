using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatherisationPatient : PersonObject
{

    [HideInInspector] public bool startCheckingPlayersPosition;
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
        startCheckingPlayersPosition = false;
    }

    protected override void Update()
    {
        if (startCheckingPlayersPosition)
        {
            if (GameObject.Find("Player").transform.position != GameObject.Find("playerPositionTarget2").transform.position)
            {
                this.GetComponent<Animator>().SetTrigger("stand");
                Debug.Log("Player moved from patient.");
                startCheckingPlayersPosition = false;
            }
        }
    }

    public override void Talk(string topic = "", string audio = "")
    {
        if (ViewModeActive() || topic == "CM_Leave" || topic == "")
            return;

        if (actionManager.CompareTopic(topic))
        {
      
            switch (topic)
            {
                case "LayOnBed":
                    //animator.SetTrigger("pants_down");

                    /*PlayerAnimator.SetTrigger("Player_Dialog_AskToLay");
                    PlayerAnimator.SetTrigger("S Player_Dialog_AskToLay");*/
                    if (playerPositionTarget != null)
                    {
                        GameObject playerPosAtPatient = GameObject.Find("PlayerPositions/PatientPos/Target");
                        playerPosAtPatient.transform.position = playerPositionTarget.position;
                        playerPosAtPatient.transform.rotation = playerPositionTarget.rotation;
                    }
                    
                    if (GameObject.Find("w4_Empty") == null)
                    {
                        PlayerAnimator.SetTrigger("CloseCurtains");
                        PlayerAnimator.SetTrigger("S CloseCurtains");
                    }
                    if (GameObject.Find("w4_Empty") != null)
                    {
                        Animator w4_Animator = GameObject.Find("w4_Empty").GetComponent<Animator>();
                        int u = w4_Animator.GetInteger("GreetingIsDone");
                        if (w4_Animator.GetInteger("GreetingIsDone") == 1)
                        {
                            PlayerAnimator.SetTrigger("CloseCurtains");
                            PlayerAnimator.SetTrigger("S CloseCurtains");
                        }

                        if (w4_Animator.GetInteger("GreetingIsDone") == 0)
                        {
                            animator.SetTrigger("RemovingCardiganStarter");
                        }


                    }

                        break;
                case "HelpGetUp":
                    PlayerAnimationManager.PlayAnimation("helppatientgetup");
                    break;
                case "WashHands":
                    Animator PlayerAnim = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
                    PlayerAnim.SetTrigger("MoveToSide");
                    PlayerAnim.SetTrigger("S MoveToSide");

                    //animator.SetTrigger("patient_standup");
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
    
