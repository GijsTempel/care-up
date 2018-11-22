using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Sequence : TutorialManager
{

    public AudioClip Popup;
    AudioSource audioSource;

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
    [HideInInspector]
    public bool sequenceLock = true;
    [HideInInspector]
    public bool dialogueEnded = false;

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

            audioSource = GetComponent<AudioSource> ();

            switch (currentStep)
            {
                case TutorialStep.First:
                    audioSource.PlayOneShot (Popup, 0.1F);
                    GameObject.FindObjectOfType<InjectionPatient>().allowToTalk = false;
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetSize(788f, 524.9f);
                    hintsN.LockTo("UI(Clone)", new Vector3(-393.80f, 214.70f, 0.00f));
                    UItext.text = "Welkom, In deze tutorial zul je leren hoe je ingewikkelde handelingen, zoals injecteren, moet uitvoeren.";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        hintsN.ResetSize();
                        hintsN.LockTo("WorkField", new Vector3(0.00f, 0.51f, -0.73f));
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Laten we beginnen. Beweeg naar het werkveld door erop te klikken.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        player.tutorial_movedTo = false;
						hintsN.LockTo("SyringeWithInjectionNeedleCap", new Vector3(0.00f, 0.12f, 0.00f));
						hintsN.SetIconPosition(3);
                        currentStep = TutorialStep.PickSyringe;
                        UItext.text = "Pak de spuit + injectienaald + beschermdop op door op het object te klikken.";

                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "SyringeWithInjectionNeedleCap";
                    }
                    break;
                case TutorialStep.PickSyringe:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        handsInventory.tutorial_pickedLeft = false;
                        hintsN.LockTo("WorkField", new Vector3(0.13f, 0.93f, -1.17f));
                        hintsN.SetIconPosition(1);
                        currentStep = TutorialStep.MoveToPatient;
                        UItext.text = "Heel goed. Laten we naar de cliënt toe gaan. Dit kan door op door op de 'terug naar overzicht' knop te klikken en daarna op de cliënt of door de camera te draaien richting de cliënt op hem te klikken. ";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveToPatient:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        player.tutorial_movedTo = false;
						GameObject.FindObjectOfType<InjectionPatient>().allowToTalk = true;
                        hintsN.LockTo("RightShoulder", new Vector3(0.00f, 0.00f, 0.23f));
                        hintsN.SetIconPosition(1);
                        currentStep = TutorialStep.Talk;
                        UItext.text = "Vraag de cliënt om zijn mouw omhoog te doen door op de cliënt te klikken en te kiezen voor de eerste optie. ";

                        patient.tutorial_talked = false;
                    }
                    break;
                case TutorialStep.Talk:
                    if (patient.tutorial_talked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        patient.tutorial_talked = false;
                        hintsN.LockTo("RightArm", new Vector3(417.45f, -214.30f, -287.30f));
                        hintsN.SetIconPosition(3);
                        currentStep = TutorialStep.UseOnPatient;
                        UItext.text = "Gebruik nu de spuit + naald + dop met de cliënt. Doe dit door op de spuit + injectienaald + beschermdop te klikken, te kklikken op het + icoon en vervolgens te klikken op de cliënt.";

                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOnPatient:
                    if (handsInventory.tutorial_itemUsedOn)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        hintsN.LockTo("/UI(Clone)", new Vector3(-793.25f, 157.60f, 0.00f));
                        hintsN.SetIconPosition(0);
                        hintsN.SetSize(788f, 524.9f);
                        currentStep = TutorialStep.SequenceExplanation;
                        UItext.text = "Ingewikkelde handelingen die uit meerdere stappen bestaan openen het 'actie keuze menu'. De handeling start automatisch. Zodra er een belangrijke stap is aangebroken, pauzeert het spel. Er verschijnen keuzes in het beeld en het is aan jou om de juiste keuze te selecteren. Zodra je de goede keuze maakt zal de handeling verder worden uitgevoerd. Dit herhaalt zich tot de handeling is afgerond.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.SequenceExplanation:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        hintsN.LockTo("/UI(Clone)", new Vector3(135.20f, 288.50f, 0.00f));
                        hintsN.ResetSize();
                        hintsN.SetIconPosition(1);
                        currentStep = TutorialStep.CompleteSequence;
                        sequenceCompleted = sequenceLock = false;
                        PlayerAnimationManager.SequenceTutorialLock(false);
                        UItext.text = "In deze instructie zijn de juiste keuzes aangegeven in het groen. Doorloop nu de verschillende injectie stappen door de juiste keuzes te selecteren.";
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if (sequenceCompleted && dialogueEnded)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
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
