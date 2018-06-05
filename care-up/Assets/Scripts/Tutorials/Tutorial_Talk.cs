using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Talk : TutorialManager
{
    public enum TutorialStep
    {
        First,
        Welcome,
        OpenOptions,
        Talk,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    private InjectionPatient patient;

    protected override void Start()
    {
        base.Start();

        patient = GameObject.FindObjectOfType<InjectionPatient>();
    }

    protected override void Update()
    {
        base.Update();

        if (!Paused())
        {
            switch (currentStep)
            {
                case TutorialStep.First:
                    currentStep = TutorialStep.Welcome;
                    UItext.text = "Welcome. This tutorial will teach you how to use items on other items that are on the table.";
                    SetUpTutorialNextButton();

                    GameObject.Find("DevHint").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenOptions;
                        UItext.text = "Come close to patient. Click on patient.";

                        patient.tutorial_used = false;
                    }
                    break;
                case TutorialStep.OpenOptions:
                    if (patient.tutorial_used)
                    {
                        patient.tutorial_used = false;

                        currentStep = TutorialStep.Talk;
                        UItext.text = "Select first option";

                        patient.tutorial_talked = false;
                    }
                    break;
                case TutorialStep.Talk:
                    if (patient.tutorial_talked)
                    {
                        patient.tutorial_talked = false;

                        currentStep = TutorialStep.Done;
                        UItext.text = "Great work! This concludes talking tutorial.";
                    }
                    break;
                case TutorialStep.Done:
                    currentStep = TutorialStep.None;
                    endPanel.SetActive(true);
                    player.enabled = false;
                    GameObject.FindObjectOfType<RobotManager>().enabled = false;
                    foreach (InteractableObject o in GameObject.FindObjectsOfType<InteractableObject>())
                    {
                        o.Reset();
                        o.enabled = false;
                    }
                    break;
            }
        }
    }
}
