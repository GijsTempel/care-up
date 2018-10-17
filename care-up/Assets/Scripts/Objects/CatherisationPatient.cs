using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatherisationPatient : PersonObject {

    private Animator animator;

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
                    animator.SetTrigger("lie_down");
                    break;
                default:
                    break;
            }

            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }
}
