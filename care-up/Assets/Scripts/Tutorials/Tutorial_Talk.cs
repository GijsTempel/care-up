using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Talk : TutorialManager
{
    public enum TutorialStep
    {
        First,
        Welcome,
        MoveTo,
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
                    UItext.text = "Welkom. In deze leermodule zul je leren hoe je met mensen een gesprek kunt aangaan.";
                    SetUpTutorialNextButton();

                    GameObject.Find("DevHint").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Laten we naar de cliënt toe gaan door op hem te klikken.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        currentStep = TutorialStep.OpenOptions;
                        UItext.text = "Heel goed. Klik nu nogmaals op de cliënt om het een gesprek te starten. ";

                        patient.tutorial_used = false;
                    }
                    break;
                case TutorialStep.OpenOptions:
                    if (patient.tutorial_used)
                    {
                        patient.tutorial_used = false;

                        currentStep = TutorialStep.Talk;
                        UItext.text = "Als je een gesprek start kun je keuzes maken over wat je tegen de persoon wilt zeggen. Laten we de cliënt begroeten door de optie 'Goedemorgen' te kiezen.";

                        patient.tutorial_talked = false;
                    }
                    break;
                case TutorialStep.Talk:
                    if (patient.tutorial_talked)
                    {
                        patient.tutorial_talked = false;

                        currentStep = TutorialStep.Done;
                        UItext.text = "Goed gedaan. Je weet nu hoe je een gesprek kunt starten met personen!";
                    }
                    break;
                case TutorialStep.Done:
                    if (patient.tutorial_greetingEnded)
                    {
                        currentStep = TutorialStep.None;
                        endPanel.SetActive(true);
                        player.enabled = false;
                        GameObject.FindObjectOfType<RobotManager>().enabled = false;
                        foreach (InteractableObject o in GameObject.FindObjectsOfType<InteractableObject>())
                        {
                            o.Reset();
                            o.enabled = false;
                        }
                    }
                    break;
            }
        }
    }
}
