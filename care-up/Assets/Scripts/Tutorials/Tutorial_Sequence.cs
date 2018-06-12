using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Sequence : TutorialManager
{
    public enum TutorialStep
    {
        First,
        Welcome,
        MoveTo,
        PickSyringe,
        MoveToPatient,
        Talk,
        UseOnPatient,
        SequenceExplanation,
        CompleteSequence,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;
    
    [HideInInspector]
    public bool sequenceCompleted = false;
    
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
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Laten we beginnen. Beweeg naar het werkveld door erop te klikken.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;

                        currentStep = TutorialStep.PickSyringe;
                        UItext.text = "Pick up syringe";

                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "SyringeWithInjectionNeedleCap";
                    }
                    break;
                case TutorialStep.PickSyringe:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        handsInventory.tutorial_pickedLeft = false;

                        currentStep = TutorialStep.MoveToPatient;
                        UItext.text = "Laten we beginnen. Beweeg naar het werkveld door erop te klikken.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveToPatient:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;

                        currentStep = TutorialStep.Talk;
                        UItext.text = "ask patient to pull up sleeves";

                        patient.tutorial_talked = false;
                    }
                    break;
                case TutorialStep.Talk:
                    if (patient.tutorial_talked)
                    {
                        patient.tutorial_talked = false;

                        currentStep = TutorialStep.UseOnPatient;
                        UItext.text = "Use syringe on patient";

                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOnPatient:
                    if (handsInventory.tutorial_itemUsedOn)
                    {
                        currentStep = TutorialStep.SequenceExplanation;
                        UItext.text = "Ingewikkelde handelingen die uit meerdere stappen bestaan openen het 'actie keuze menu'. De handeling start automatisch. Zodra er een belangrijke stap is aangebroken, pauzeert het spel. Er verschijnen keuzes in het beeld en het is aan jou om de juiste keuze te selecteren. Zodra je de goede keuze maakt zal de handeling verder worden uitgevoerd. Dit herhaalt zich tot de handeling is afgerond.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.SequenceExplanation:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.CompleteSequence;
                        sequenceCompleted = false;
                        PlayerAnimationManager.SequenceTutorialLock(false);
                        UItext.text = "In deze instructie zijn de juiste keuzes aangegeven in het groen. Doorloop nu de verschillende stappen om het injecteren uit te voeren door de juiste keuzes te selecteren.";
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if (sequenceCompleted)
                    {
                        currentStep = TutorialStep.Done;
                        UItext.text = "Congratulations, this concludes sequence tutorial.";
                    }
                    break;
                case TutorialStep.Done:
                    currentStep = TutorialStep.None;
                    TutorialEnd();
                    break;
            }
        }
    }
}
