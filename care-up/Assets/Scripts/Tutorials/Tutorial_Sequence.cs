using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Tutorial_Sequence : TutorialManager
{

    public AudioClip Popup;
    public AudioClip Done;
    public AudioClip Robot1;
    public AudioClip Robot2;
    public AudioClip Robot3;
    public AudioClip RobotShort1;
    public AudioClip RobotShort2;
    AudioSource audioSource;
    private MBS.WUADisplay achievements;

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

        achievements = GameObject.Find ("AchievementsDisplayPrefab").GetComponent<MBS.WUADisplay> ();
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
                    audioSource.PlayOneShot(RobotShort1, 0.1F);
                    GameObject.FindObjectOfType<InjectionPatient>().allowToTalk = false;
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetSize(551.6f, 400f);
                    hintsN.SetIconPosition(1);
                    hintsN.LockTo("robot", new Vector3(-704.81f, 3.20f, 317.20f));
                    UItext.DOText("In deze training leer je ingewikkelde handelingen uit te voeren.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        hintsN.SetIconPosition(0);
                        hintsN.SetSize(452f, 157.4f);
                        hintsN.LockTo("WorkField", new Vector3(0.00f, 0.51f, -1.33f));
                        currentStep = TutorialStep.MoveTo;
                        UItext.DOText("Klik op het werkveld om te beginnen.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        player.tutorial_movedTo = false;
						hintsN.LockTo("SyringeWithInjectionNeedleCap", new Vector3(0.00f, 0.12f, 0.00f));
						hintsN.SetIconPosition(3);
                        currentStep = TutorialStep.PickSyringe;
                        UItext.DOText("Klik op de spuit met injectienaald om deze op te pakken.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "SyringeWithInjectionNeedleCap";
                    }
                    break;
                case TutorialStep.PickSyringe:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        handsInventory.tutorial_pickedLeft = false;
                        hintsN.LockTo("WorkField", new Vector3(0.13f, 0.93f, -1.17f));
                        hintsN.SetIconPosition(1);
                        hintsN.SetSize(452f, 60f);
                        currentStep = TutorialStep.MoveToPatient;
                        UItext.DOText("Beweeg naar de cliënt toe.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveToPatient:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        player.tutorial_movedTo = false;
						GameObject.FindObjectOfType<InjectionPatient>().allowToTalk = true;
                        hintsN.LockTo("/Patient/pArmature/Hips/Spine/Spine1/Spine2/RightShoulder", new Vector3(0.00f, 0.00f, 0.32f));
                        hintsN.SetSize(489f, 251.5f);
                        hintsN.SetIconPosition(1);
                        currentStep = TutorialStep.Talk;
                        UItext.DOText("Vraag de cliënt om zijn mouw omhoog te doen door op de cliënt te klikken en te kiezen voor de eerste optie. ", 1f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        patient.tutorial_talked = false;
                    }
                    break;
                case TutorialStep.Talk:
                    if (patient.tutorial_talked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        patient.tutorial_talked = false;
                        hintsN.LockTo("RightArm", new Vector3(417.45f, -214.30f, -287.30f));
                        hintsN.SetIconPosition(3);
                        currentStep = TutorialStep.UseOnPatient;
                        UItext.text = "";
                        UItext.DOText("Gebruik de spuit door op het object te klikken. Klik daarna op het +icoon en vervolgens op de cliënt.", 1f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOnPatient:
                    if (handsInventory.tutorial_itemUsedOn)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);
                        hintsN.LockTo("SelectionDialogue", new Vector3(136.90f, 94.20f, -287.30f));
                        hintsN.SetIconPosition(1);
                        hintsN.SetSize(611.5f, 372.4f);
                        currentStep = TutorialStep.SequenceExplanation;
                        UItext.DOText("Ingewikkelde handelingen starten automatisch. Er verschijnen keuzes in het beeld en het is aan jou om de juiste keuze te selecteren.", 1f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.SequenceExplanation:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        hintsN.LockTo("SelectionDialogue", new Vector3(584.80f, 435.86f, -46.70f));
                        hintsN.SetSize(1079f, 105f);
                        hintsN.SetIconPosition(1);
                        currentStep = TutorialStep.CompleteSequence;
                        sequenceCompleted = sequenceLock = false;
                        PlayerAnimationManager.SequenceTutorialLock(false);
                        UItext.DOText("Probeer nu te injecteren door de juiste keuzes te selecteren.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                    }
                    break;
                case TutorialStep.CompleteSequence:
                    if (sequenceCompleted && dialogueEnded)
                    {
                        audioSource.PlayOneShot(Done, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        hintsN.ResetSize();
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        currentStep = TutorialStep.Done;
                        UItext.DOText("Gefeliciteerd! Je hebt de laatste training afgerond. Je bent klaar om verpleegtehcnische handelingen te oefenen en te toesten. ", 1f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        achievements.UpdateKeys ("FinishedTutorial", 1);
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
