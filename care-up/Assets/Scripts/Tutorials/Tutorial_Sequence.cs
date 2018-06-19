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
                    hintsBox.anchoredPosition = new Vector2(-14.8f, 9.9f);
                    hintsBox.sizeDelta = new Vector2(788f, 524.9f);
                    UItext.text = "Welkom, In deze tutorial zul je leren hoe je ingewikkelde handelingen, zoals injecteren, moet uitvoeren.";
                    SetUpTutorialNextButton();

     //               GameObject.Find("DevHint").SetActive(false);
					//GameObject.Find("Extra").SetActive(false);
                    //GameObject.Find("ExtraButton").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Laten we beginnen. Beweeg naar het werkveld door erop te klikken.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;
                        hintsBox.anchoredPosition = new Vector2(421f, -284f);
                     
                        currentStep = TutorialStep.PickSyringe;
                        UItext.text = "Pak de spuit + injectienaald + beschermdop op door op het object te klikken.";

                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "SyringeWithInjectionNeedleCap";
                    }
                    break;
                case TutorialStep.PickSyringe:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        handsInventory.tutorial_pickedLeft = false;
                        hintsBox.anchoredPosition = new Vector2(575f, -235.3f);
                        hintsBox.sizeDelta = new Vector2(626.4f, 396.3f);
                        currentStep = TutorialStep.MoveToPatient;
                        UItext.text = "Heel goed. Laten we naar de cliënt toe gaan. Dit kan door op door op de 'terug naar overzicht' knop te klikken en daarna op de cliënt of door de camera te draaien richting de cliënt op hem te klikken. ";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveToPatient:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;
                        hintsBox.anchoredPosition = new Vector2(-490.9f, -214.1f);
                        hintsBox.sizeDelta = new Vector2(592.3f, 374.7f);
                        currentStep = TutorialStep.Talk;
                        UItext.text = "Vraag de cliënt om zijn mouw omhoog te doen door op de cliënt te klikken en te kiezen voor de eerste optie. ";

                        patient.tutorial_talked = false;
                    }
                    break;
                case TutorialStep.Talk:
                    if (patient.tutorial_talked)
                    {
                        patient.tutorial_talked = false;
                        hintsBox.anchoredPosition = new Vector2(686f, -228f);
                        hintsBox.sizeDelta = new Vector2(520f, 322.5f);
                        currentStep = TutorialStep.UseOnPatient;
                        UItext.text = "Gebruik nu de spuit + naald + dop met de cliënt. Doe dit door op de spuit + injectienaald + beschermdop te klikken, te kiezen voor de optie 'Gebruiken met...' en vervolgens te klikken op de cliënt.";

                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOnPatient:
                    if (handsInventory.tutorial_itemUsedOn)
                    {
                        hintsBox.anchoredPosition = new Vector2(-14.8f, 9.9f);
                        hintsBox.sizeDelta = new Vector2(788f, 524.9f);
                        currentStep = TutorialStep.SequenceExplanation;
                        UItext.text = "Ingewikkelde handelingen die uit meerdere stappen bestaan openen het 'actie keuze menu'. De handeling start automatisch. Zodra er een belangrijke stap is aangebroken, pauzeert het spel. Er verschijnen keuzes in het beeld en het is aan jou om de juiste keuze te selecteren. Zodra je de goede keuze maakt zal de handeling verder worden uitgevoerd. Dit herhaalt zich tot de handeling is afgerond.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.SequenceExplanation:
                    if (nextButtonClicked)
                    {
                        hintsBox.anchoredPosition = new Vector2(-682f, 259.12f);
                        hintsBox.sizeDelta = new Vector2(521.9f, 347.6f);
                        currentStep = TutorialStep.CompleteSequence;
                        sequenceCompleted = false;
                        PlayerAnimationManager.SequenceTutorialLock(false);
                        UItext.text = "In deze instructie zijn de juiste keuzes aangegeven in het groen. Doorloop nu de verschillende injectie stappen door de juiste keuzes te selecteren.";
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if (sequenceCompleted)
                    {
                        hintsBox.anchoredPosition = new Vector2(519f, -347f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        currentStep = TutorialStep.Done;
                        UItext.text = "Gefeliciteerd! Je weet nu hoe je ingewikkelde handelingen succesvol kunt uitvoeren.";
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
