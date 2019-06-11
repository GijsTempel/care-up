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
        CleanTable,
        OpenRobotUI,
        StopWatchExpl,
        TimeExpl,
        PercentExpl,
        TabsExpl,
        PressGeneral,
        GeneralExpl,
        PatientRecordsOpen, // new
        PatientRecordsExpl,
        PatientRecordsBack,
        PatientPrescriptionOpen,
        PatientPrescriptionExpl,
        WritingDownExpl,
        PrescriptionBack,   // end of new
        GeneralBack,
        PressChecklist,
        ChecklistExpl,
        ChecklistBack,
        PressMessageCenter,
        MessageCenterExpl,
        OpenMessage,
        CloseMessageCenter,
        CloseRobotUI,
        PickOne,
        CheckMedicine,
        MedicineChecked,
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
    public bool openMailMessage = false;

    private GameObject wfPos;
    private GameObject docPos;
    private GameObject cliëntPos;
    [SerializeField]private GameObject robotUI = null;

    private UsableObject handCleaner;
    private WorkField workfield;

    public GameObject Tweenbutton;

    protected override void Start () {
        base.Start ();

        patient = GameObject.FindObjectOfType<InjectionPatient> ();

        wfPos = GameObject.Find ("WorkFieldPos");
        docPos = GameObject.Find ("DoctorPos");
        cliëntPos = GameObject.Find ("PatientPos");

        handCleaner = GameObject.Find ("HandCleaner").GetComponent<UsableObject> ();
        workfield = GameObject.Find ("WorkField").GetComponent<WorkField> ();

        wfPos.SetActive (false);
        docPos.SetActive (false);
        cliëntPos.SetActive (false);
        robotUI.SetActive (true);

        hintsUI = GameObject.FindObjectOfType<Cheat_CurrentAction> ();
    }

    protected override void Update () {
        base.Update ();

        if (!Paused ()) {

            audioSource = GetComponent<AudioSource> ();

            switch (currentStep) {
                case TutorialStep.First:

                Tweenbutton.transform.DOScale (1f, 0.4f).SetLoops (-1, LoopType.Yoyo);
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
                    robotUI.SetActive (true);
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
                    currentStep = TutorialStep.CleanHands;
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    hintsN.SetSize (452f, 414.7f);
                    hintsN.LockTo ("ExtraButton", new Vector3 (450.00f, -550.00f, 0.00f));
                    UItext.DOText ("Nu je bij het werkveld bent is het belangrijk dat je jouw handen goed wast dus klik op de hand hygiene pomp.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    particleHint.SetActive (true);
                    particleHint.transform.position = GameObject.Find ("HandCleaner").transform.position;
                }
                break;
                case TutorialStep.CleanHands:
                if (handCleaner.handsCleaned) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    currentStep = TutorialStep.CleanTable;
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    hintsN.SetSize (452f, 414.7f);
                    hintsN.LockTo ("ExtraButton", new Vector3 (450.00f, -550.00f, 0.00f));
                    UItext.DOText ("Nu heb je jou handen gewassen en dus kan je het werkveld schoonmaken dat doe je door erop te klikken doe dit nu maar.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    particleHint.SetActive (true);
                    particleHint.transform.position = GameObject.Find ("WorkField").transform.position;
                }
                break;
                case TutorialStep.CleanTable:
                if (workfield.tableCleaned) {
                    hintsN.SetSize (570f, 226.6f);
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    currentStep = TutorialStep.OpenRobotUI;
                    hintsN.SetIconPosition (0);
                    hintsN.LockTo ("RobotUITrigger", new Vector3 (53.24f, -101.46f, 0.00f));
                    robotUI.SetActive(true);
                    UItext.DOText ("Klik op het tableticoontje om de tablet te openen. Op de tablet kun je veel informatie vinden. Neem maar eens een kijkje!", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    expectedRobotUIstate = true;
                }
                break;
                case TutorialStep.OpenRobotUI:
                if (player.tutorial_robotUI_opened) {
                    hintsN.ResetSize ();
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    currentStep = TutorialStep.StopWatchExpl;
                    hintsN.LockTo ("Stopwatch", new Vector3 (107.20f, -143.10f, 0.00f));
                    UItext.DOText ("Het klokje geeft aan hoe lang je bezig bent met de handeling. ", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.StopWatchExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot2, 0.1F);
                    currentStep = TutorialStep.TimeExpl;

                    hintsN.LockTo ("Stopwatch", new Vector3 (1297.10f, -149.20f, 0.00f));
                    UItext.DOText ("Hier zie je het aantal punten dat je behaald hebt. ", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.TimeExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot1, 0.1F);
                    currentStep = TutorialStep.PercentExpl;

                    hintsN.LockTo ("Stopwatch", new Vector3 (2678.00f, -149.20f, 0.00f));
                    UItext.DOText ("Bekijk hier je voortgang.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.PercentExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    currentStep = TutorialStep.TabsExpl;

                    hintsN.LockTo ("RobotUI", new Vector3 (-428.00f, -213.19f, 0.00f));
                    UItext.DOText ("Klik op de apps om ze te openen.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.TabsExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    currentStep = TutorialStep.PressGeneral;
                    hintsN.SetSize (519f, 70.4f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-990.90f, -49.30f, -99.80f));
                    UItext.DOText ("Klik op het icoontje ‘Gegevens’", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    RobotUITabs.tutorial_generalOpened = false;
                    tabToOpen = "GeneralTab";
                }
                break;
                case TutorialStep.PressGeneral:
                if (RobotUITabs.tutorial_generalOpened) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot1, 0.1F);
                    currentStep = TutorialStep.GeneralExpl;
                    hintsN.SetSize (788f, 318.3f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1301.16f, -377.96f, 0.00f));
                    UItext.DOText ("Hier vind je cliëntgegevens en de toedienlijst. Gebruik dit om gegevens te controleren, bekijken en de toedienlijst af te tekenen. ", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.GeneralExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    currentStep = TutorialStep.PatientRecordsOpen;
                    UItext.text = "Bekijk de cliëntgegevens door op het icoontje te klikken. ";
                    UItext.DOText ("Bekijk de cliëntgegevens door op het icoontje te klikken.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsN.SetSize (452f, 164.8f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1006.70f, 38.00f, -99.80f));
                    RobotUITabs.tutorial_recordsOpened = false;
                    tabToOpen = "RecordsTab";
                }
                break;
                case TutorialStep.PatientRecordsOpen:
                if (RobotUITabs.tutorial_recordsOpened) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot2, 0.1F);
                    RobotUITabs.tutorial_recordsOpened = false;
                    hintsN.SetSize (788f, 524.9f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1301.16f, 958.30f, 0.00f));
                    currentStep = TutorialStep.PatientRecordsExpl;
                    UItext.DOText ("Bekijk en controleer hier de cliëntgegevens. De controle vindt automatisch plaats wanneer je de cliëntgegevens opent.  ", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.PatientRecordsExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    currentStep = TutorialStep.PatientRecordsBack;
                    hintsN.SetSize (452f, 162.1f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1726.40f, 1017.60f, 0.00f));
                    UItext.DOText ("Sluit de cliëntgegevens door op het pijltje te klikken. ", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    closeTab = true;
                    tabToOpen = "GeneralTab";
                    RobotUITabs.tutorial_back = false;
                }
                break;
                case TutorialStep.PatientRecordsBack:
                if (RobotUITabs.tutorial_back) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    currentStep = TutorialStep.PatientPrescriptionOpen;
                    hintsN.LockTo ("RobotUI", new Vector3 (268.00f, 111.00f, 0.00f));
                    UItext.DOText ("Open de toedienlijst door op het icoon te klikken. ", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    RobotUITabs.tutorial_prescriptionOpened = false;
                    tabToOpen = "PrescriptionTab";
                }
                break;
                case TutorialStep.PatientPrescriptionOpen:
                if (RobotUITabs.tutorial_prescriptionOpened) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    RobotUITabs.tutorial_prescriptionOpened = false;
                    hintsN.SetSize (788f, 524.9f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1301.16f, 958.30f, 0.00f));
                    currentStep = TutorialStep.WritingDownExpl;
                    UItext.DOText ("Bekijk en controleer hier de toedienlijst. De controle vindt automatisch plaats wanneer je de toedienlijst opent.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.WritingDownExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    currentStep = TutorialStep.PatientPrescriptionExpl;
                    hintsN.SetIconPosition (0);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1650.00f, 1017.60f, 0.00f));
                    hintsN.SetSize (452f, 162.1f);
                    UItext.DOText ("Sluit de toedienlijst door op het pijltje te klikken.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    closeTab = true;
                    tabToOpen = "GeneralTab";
                    RobotUITabs.tutorial_back = false;
                }
                break;
                case TutorialStep.PatientPrescriptionExpl:
                if (RobotUITabs.tutorial_back) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot1, 0.1F);
                    currentStep = TutorialStep.PrescriptionBack;
                    hintsN.LockTo ("RobotUI", new Vector3 (845.00f, -105.00f, 0.00f));
                    hintsN.SetIconPosition (1);
                    hintsN.SetSize (465f, 408f);
                    UItext.DOText ("Door op het aftekenicoon te klikken, kun je een handeling aftekenen. Let op: hiermee rond je de handeling ook af!", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.PrescriptionBack:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    currentStep = TutorialStep.GeneralBack;
                    hintsN.SetSize (452f, 162.1f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1650.00f, 936.80f, -99.80f));
                    hintsN.SetIconPosition (0);
                    UItext.DOText ("Sluit de ‘Gegevens’ app door op het pijltje te klikken.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    RobotUITabs.tutorial_back = false;
                }
                break;
                case TutorialStep.ChecklistExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    currentStep = TutorialStep.ChecklistBack;
                    hintsN.SetSize (452f, 162.1f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1650.00f, 958.20f, -34.70f));
                    UItext.DOText ("Klik op het pijltje om terug te gaan.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    closeTab = true;
                }
                break;
                case TutorialStep.ChecklistBack:
                if (RobotUITabs.tutorial_back) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    closeTab = RobotUITabs.tutorial_back = false;
                    currentStep = TutorialStep.PressMessageCenter;
                    UItext.DOText ("Je hebt een bericht! Klik op ‘Berichten’ om het bericht te openen. ", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    hintsN.LockTo ("RobotUI", new Vector3 (849.00f, -244.00f, -34.70f));
                    hintsN.SetSize (452f, 164.8f);
                    hintsN.SetIconPosition (1);
                    GameObject.FindObjectOfType<RobotUIMessageTab> ().NewMessage ("Leren Spelen",
                        "De inhoud van de berichten verschijnt in dit venster. Klik op het pijltje linksboven in je scherm om terug te gaan. ",
                        RobotUIMessageTab.Icon.Info);
                    RobotUITabs.tutorial_messageCenterOpened = false;
                    tabToOpen = "MessageCenter";
                }
                break;
                case TutorialStep.PressMessageCenter:
                if (RobotUITabs.tutorial_messageCenterOpened) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    tabToOpen = "";
                    currentStep = TutorialStep.MessageCenterExpl;

                    hintsN.SetIconPosition (0);
                    hintsN.SetSize (512f, 332.5f);
                    audioSource.PlayOneShot (Robot1, 0.1F);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1305.70f, 937.90f, 0.00f));
                    UItext.DOText ("Tijdens het oefenen zullen wij jou berichten sturen om je te helpen en te informeren.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.MessageCenterExpl:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    currentStep = TutorialStep.OpenMessage;
                    hintsN.SetSize (452f, 73.6f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-1113.50f, 683.00f, 0.00f));
                    UItext.DOText ("Klik op een bericht", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    RobotUIMessageTab.tutorial_messageOpened = false;
                    openMailMessage = true;
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.OpenMessage:
                //if (RobotUIMessageTab.tutorial_messageOpened) {
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot2, 0.1F);
                    currentStep = TutorialStep.CloseMessageCenter;
                    hintsN.SetIconPosition (1);
                    hintsN.SetSize (452f, 199.5f);
                    hintsN.LockTo ("RobotUI", new Vector3 (-557.30f, 809.80f, 0.00f));
                    UItext.DOText ("De inhoud van de berichten verschijnt in dit venster. Klik op het pijltje linksboven in je scherm om terug te gaan. ", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    RobotUITabs.tutorial_back = false;
                    closeTab = true;
                }
                break;
                case TutorialStep.CloseMessageCenter:
                if (RobotUITabs.tutorial_back) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    closeTab = RobotUITabs.tutorial_back = false;
                    currentStep = TutorialStep.CloseRobotUI;
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    hintsN.SetSize (452f, 300f);
                    hintsN.LockTo ("RobotUI", new Vector3 (1880.40f, 1109.00f, 0.00f));
                    hintsN.SetIconPosition (1);
                    UItext.DOText ("Sluit de tablet af door op het kruisje te klikken. Je kunt de tablet altijd weer openen door op het tableticoontje te klikken. ", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    expectedRobotUIstate = false;
                }
                break;
                case TutorialStep.PickOne:
                if (player.tutorial_robotUI_closed) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot1, 0.1F);
                    currentStep = TutorialStep.DevHintsExpl;
                    hintsN.SetSize (452f, 414.7f);
                    hintsN.SetIconPosition (0);
                    hintsN.LockTo ("DevHint", new Vector3 (375.39f, -284.23f, 0.00f));
                    UItext.DOText ("Objecten oppakken kun je doen door erop te klikken. Probeer nu het medicijn op te pakken door erop te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    itemToPick = "Medicine";
                    handsInventory.tutorial_pickedRight = false;

                    particleHint.transform.position = GameObject.Find ("Medicine").transform.position;
                }
                break; 
                case TutorialStep.CheckMedicine:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    hintsN.SetSize (452f, 350f);
                    currentStep = TutorialStep.CheckMedicine;
                    hintsN.LockTo ("RobotUITrigger", new Vector3 (0.00f, 0.16f, 0.00f));
                    UItext.DOText ("Het medicijn moet gecheckt worden dat doe je door op het medicijn in je hand te klikken en daarna op het vergrootglas, klik daar nu op.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    itemToPick = "Medicine";

                    particleHint.transform.position = GameObject.Find ("Medicine").transform.position;
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