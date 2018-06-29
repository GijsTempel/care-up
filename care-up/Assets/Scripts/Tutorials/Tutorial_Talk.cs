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
                    GameObject.FindObjectOfType<InjectionPatient>().allowToTalk = false;

                    currentStep = TutorialStep.Welcome;
					hintsN.SetSize(788f, 524.9f);
                    hintsN.LockTo("UI(Clone)", new Vector3(-393.80f, 214.70f, 0.00f));
                    UItext.text = "Welkom. In deze leermodule zul je leren hoe je met mensen een gesprek kunt aangaan.";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
						hintsN.ResetSize();
						hintsN.LockTo("Patient", new Vector3(0.00f, 0.00f, 0.00f));
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Laten we naar de cliënt toe gaan door op hem te klikken.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        currentStep = TutorialStep.OpenOptions;
						hintsN.LockTo("RightShoulder", new Vector3(0.00f, 0.00f, 0.00f));
                        UItext.text = "Heel goed. Klik nu nogmaals op de cliënt om het een gesprek te starten. ";

                        patient.tutorial_used = false;
                        GameObject.FindObjectOfType<InjectionPatient>().allowToTalk = true;
                    }
                    break;
                case TutorialStep.OpenOptions:
                    if (patient.tutorial_used)
                    {
                        patient.tutorial_used = false;

                        currentStep = TutorialStep.Talk;
						hintsN.LockTo("SelectionDialogue(Clone)", new Vector3(168.40f, 124.90f, 0.00f));
						hintsN.SetIconPosition(1);
                        UItext.text = "Als je een gesprek start kun je keuzes maken over wat je tegen de persoon wilt zeggen. Laten we de cliënt begroeten door de optie 'Goedemorgen' te kiezen.";

                        patient.tutorial_talked = false;
                    }
                    break;
                case TutorialStep.Talk:
                    if (patient.tutorial_talked)
                    {
                        patient.tutorial_talked = false;
						hintsN.SetIconPosition(0);
						hintsN.LockTo("SceneLoader 1", new Vector3(356.74f, 39.10f, 0.00f));
                        currentStep = TutorialStep.Done;
                        UItext.text = "Goed gedaan. Je weet nu hoe je een gesprek kunt starten met personen!";
                    }
                    break;
                case TutorialStep.Done:
                    if (patient.tutorial_greetingEnded)
                    {
                        currentStep = TutorialStep.None;
                        TutorialEnd();
                    }
                    break;
            }
        }
    }
}
