using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial : TutorialManager {

    public AudioClip Popup;
    public AudioClip Done;
    public AudioClip Robot1;
    public AudioClip Robot2;
    public AudioClip Robot3;
    public AudioClip RobotShort1;
    public AudioClip RobotShort2;
    AudioSource audioSource;

    public enum TutorialStep {
        First,
        Welcome,
        RobotIntro,
        DevHintsExpl,
        DevHintsMax,
        DevHintsExplMax,
        OpenExtraHints,
        CloseHints,
        ExplOptions,
        ExplQuit,
        PointsExpl,
        MoveToCliënt,
        OpenOptions,
        Talk,
        DoneTalking,
        MoveBack,
        MoveToTable,
        CleanHands,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    Cheat_CurrentAction hintsUI;

    private InjectionPatient patient;

    public string tabToOpen = "";
    public bool closeTab = false;
    public bool expectedRobotUIstate = false;
    public bool expectedHintsState = false;
    public bool biggerHintOpenend = false;

    private GameObject wfPos;
    private GameObject docPos;
    private GameObject cliëntPos;

    public GameObject Tweenbutton;

    protected override void Start () {
        base.Start ();

        patient = GameObject.FindObjectOfType<InjectionPatient> ();

        wfPos = GameObject.Find ("WorkFieldPos");
        docPos = GameObject.Find ("DoctorPos");
        cliëntPos = GameObject.Find ("PatientPos");

        wfPos.SetActive (false);
        docPos.SetActive (false);
        cliëntPos.SetActive (false);

        hintsUI = GameObject.FindObjectOfType<Cheat_CurrentAction> ();
    }

    protected override void Update () {
        base.Update ();

        if (!Paused ()) {

            audioSource = GetComponent<AudioSource> ();

            switch (currentStep) {
                case TutorialStep.First:

                audioSource.PlayOneShot (Popup, 0.1F);
                currentStep = TutorialStep.Welcome;
                hintsN.SetSize (788f, 524.9f);
                hintsN.LockTo ("UI(Clone)", new Vector3 (-393.80f, 214.70f, 0.00f));
                UItext.text = "Welkom, in deze oefening zullen wij je uitleggen hoe Care Up werkt.";
                SetUpTutorialNextButton ();
                break;
                case TutorialStep.Welcome:
                if (nextButtonClicked) {

                    audioSource.PlayOneShot (Popup, 0.1F);
                    currentStep = TutorialStep.RobotIntro;
                    hintsN.LockTo ("robot", new Vector3 (0.00f, -0.25f, 0.09f));
                    hintsN.SetIconPosition (1);
                    hintsN.SetSize (452f, 463.3f);
                    UItext.text = "Dit is Olivia. Olivia is jouw hulpje. Ze geeft aan wanneer een actie goed of fout is uitgevoerd. ";
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.RobotIntro:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    currentStep = TutorialStep.DevHintsExpl;

                    hintsN.SetIconPosition (0);
                    hintsN.LockTo ("DevHint", new Vector3 (375.39f, -84.72f, 0.00f));
                    UItext.text = "Hiernaast zien we de werkwijze die je moet doorlopen om het protocol succesvol af te ronden.";
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.DevHintsExpl:
                if (nextButtonClicked) {

                    audioSource.PlayOneShot (Popup, 0.1F);
                    currentStep = TutorialStep.DevHintsMax;
                    hintsN.ResetSize ();
                    //hintsBox.anchoredPosition = new Vector2(-560, -68);
                    hintsN.SetIconPosition (0);
                    hintsN.LockTo ("/DevHint/Fullscreen", new Vector3 (375.39f, -84.72f, 0.00f));

                    UItext.DOText ("Klik op de vergroot knop.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsUI.tutorial_devHintOpened = false;
                    biggerHintOpenend = true;
                }
                break;
                case TutorialStep.DevHintsMax:
                if (hintsUI.tutorial_devHintOpened) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    currentStep = TutorialStep.DevHintsExplMax;
                    hintsN.SetIconPosition (0);
                    hintsN.SetSize (323.8f, 674.1f);
                    hintsN.LockTo ("BiggerDevHint/Backbutton (1)", new Vector3 (50.00f, 0.00f, 0.00f));

                    UItext.DOText ("Hier kan je de hints vergroot zien, sluit de hint nu weer af door op het kruisje te klikken", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsUI.tutorial_devHintClosed = false;
                    biggerHintOpenend = false;
                }
                break;
                case TutorialStep.DevHintsExplMax:
                if (hintsUI.tutorial_devHintClosed) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    currentStep = TutorialStep.OpenExtraHints;
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    hintsN.SetSize (452f, 414.7f);
                    hintsN.LockTo ("ExtraButton", new Vector3 (81.50f, -22.50f, 0.00f));
                    UItext.DOText ("Als je niet weet hoe je een stap moet uitvoeren, klik dan op het informatie icoontje. ", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsUI.tutorial_extraOpened = false;
                    expectedHintsState = true;
                }
                break;
                case TutorialStep.OpenExtraHints:
                if (hintsUI.tutorial_extraOpened) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot1, 0.1F);
                    currentStep = TutorialStep.CloseHints;
                    hintsN.SetSize (323.8f, 674.1f);
                    hintsN.LockTo ("Extra", new Vector3 (523.70f, 426.20f, 0.00f));
                    UItext.DOText ("Hier komt tijdens het spelen van de handeling extra uitleg te staan over hoe je een stap kunt uitvoeren. Laten we het scherm weer sluiten door nogmaals op het informatie icoon te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsUI.tutorial_extraClosed = false;
                    expectedHintsState = false;
                }
                break;
                case TutorialStep.CloseHints:
                if (hintsUI.tutorial_extraClosed) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot1, 0.1F);
                    currentStep = TutorialStep.ExplOptions;
                    hintsN.SetSize (452f, 414.7f);
                    hintsN.LockTo ("Mute", new Vector3 (-400.00f, 550.00f, 0.00f));
                    UItext.DOText ("Door op het volume knopje te klikken kan je het geluid in de game uitzetten en weer aan zetten.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.ExplOptions:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot1, 0.1F);
                    currentStep = TutorialStep.ExplQuit;
                    hintsN.SetSize (452f, 414.7f);
                    hintsN.LockTo ("Mute", new Vector3 (-400.00f, 550.00f, 0.00f));
                    UItext.DOText ("Door op het huisje te klikken kan je het spel afsluiten terwijl je nog bezig bent met het protocol.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.ExplQuit:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot2, 0.1F);
                    currentStep = TutorialStep.PointsExpl;
                    hintsN.SetIconPosition (1);
                    hintsN.SetSize (685f, 486f);
                    hintsN.LockTo ("robot", new Vector3 (273.20f, 19.20f, -118.50f));
                    UItext.DOText ("Binnen Care Up kun je bewegen door te klikken op interessante objecten. Je ontdekt de objecten door er met de muis overheen te bewegen. Speel je op de tablet of telefoon? Dan zijn de namen van de objecten ook weergegeven. ", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.PointsExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    //hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                    //hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                    hintsN.LockTo ("WorkField", new Vector3 (0.80f, 0.78f, 0.40f));
                    hintsN.ResetSize ();
                    currentStep = TutorialStep.MoveToCliënt;
                    UItext.DOText ("Probeer nu naar de cliënt  te bewegen. Dit kun je doen door op het cliënt te klikken.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsN.SetIconPosition (2);
                    player.tutorial_movedTo = false;
                    cliëntPos.SetActive (true);
                    particleHint.SetActive (true);
                    particleHint.transform.position = GameObject.Find ("PatientPos").transform.position;
                }
                break;
                case TutorialStep.MoveToCliënt:
                if (player.tutorial_movedTo) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    currentStep = TutorialStep.OpenOptions;
                    hintsN.LockTo ("RightShoulder", new Vector3 (0.00f, 0.00f, 0.29f));
                    UItext.DOText ("Klik op de cliënt om een gesprek te starten. ", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsN.SetIconPosition (1);
                    patient.tutorial_used = false;
                    particleHint.SetActive (false);
                    GameObject.FindObjectOfType<InjectionPatient> ().allowToTalk = true;
                }
                break;
                case TutorialStep.OpenOptions:
                if (patient.tutorial_used) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    patient.tutorial_used = false;
                    hintsN.SetSize (452f, 159f);
                    currentStep = TutorialStep.Talk;

                    hintsN.LockTo ("SelectionDialogue(Clone)", new Vector3 (68.02f, 120.35f, 0.00f));
                    hintsN.SetIconPosition (1);
                    UItext.DOText ("Laten we de cliënt begroeten door de optie 'Goedemorgen' te kiezen.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    patient.tutorial_talked = false;
                }
                break;
                case TutorialStep.Talk:
                if (patient.tutorial_talked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    patient.tutorial_talked = false;
                    hintsN.SetIconPosition (0);
                    hintsN.LockTo ("SceneLoader 1", new Vector3 (262.50f, -69.10f, 0.00f));
                    currentStep = TutorialStep.DoneTalking;
                    UItext.DOText ("Goed gedaan. Je weet nu hoe je een gesprek kunt starten met personen! ", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.DoneTalking:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    cliëntPos.SetActive (false);
                    player.tutorial_movedTo = false;
                    hintsN.SetIconPosition (1);
                    //hintsBox.anchoredPosition = new Vector2(681f, 175f);
                    //hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                    hintsN.LockTo ("MoveBackButton", new Vector3 (-59.90f, -160.20f, 0.00f));
                    hintsN.SetSize (560f, 425f);
                    hintsN.SetIconPosition (1);
                    currentStep = TutorialStep.MoveBack;
                    UItext.DOText ("We zijn naar de cliënt verplaatst. Op ieder moment kun je terugkeren naar de beginpositie. Dit wordt het overzicht genoemd. Wil je terug? Klik dan rechtsboven in op de knop ‘Terug naar overzicht’. ", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    player.tutorial_movedBack = false;
                }
                break;
                case TutorialStep.MoveBack:
                if (player.tutorial_movedBack) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    //hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                    //hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                    hintsN.LockTo ("WorkField", new Vector3 (0.80f, 0.78f, 0.40f));
                    hintsN.ResetSize ();
                    currentStep = TutorialStep.MoveToTable;
                    UItext.DOText ("Probeer nu naar het werkveld te bewegen. Dit kun je doen door op het werkveld te klikken.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsN.SetIconPosition (2);
                    player.tutorial_movedTo = false;
                    wfPos.SetActive (true);
                    particleHint.SetActive (true);
                    particleHint.transform.position = GameObject.Find ("WorkField").transform.position;
                }
                break;
                case TutorialStep.MoveToTable:
                if (player.tutorial_movedTo) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    //hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                    //hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                    hintsN.LockTo ("WorkField", new Vector3 (0.80f, 0.78f, 0.40f));
                    hintsN.ResetSize ();
                    currentStep = TutorialStep.MoveToTable;
                    UItext.DOText ("Probeer nu naar het werkveld te bewegen. Dit kun je doen door op het werkveld te klikken.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsN.SetIconPosition (2);
                    player.tutorial_movedTo = false;
                    wfPos.SetActive (true);
                    particleHint.SetActive (true);
                    particleHint.transform.position = GameObject.Find ("WorkField").transform.position;
                }
                break;
                /*case TutorialStep.CloseHints:
                if (hintsUI.tutorial_extraClosed) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    currentStep = TutorialStep.Done;

                    hintsN.LockTo ("SceneLoader 1", new Vector3 (262.50f, -69.10f, 0.00f));
                    UItext.text = "Gefeliciteerd! Je hebt nu alles geleerd over Care Up. Veel plezier met het oefenen van de verpleegtechnische handelingen!";
                }
                break;
                case TutorialStep.Done:
                currentStep = TutorialStep.None;
                TutorialEnd ();
                break;*/
            }
        }
    }
}