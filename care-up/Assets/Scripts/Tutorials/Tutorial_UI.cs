using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_UI : TutorialManager
{

    public AudioClip Popup;
    AudioSource audioSource;

    public enum TutorialStep
    {
        First,
        Welcome,
        RobotIntro,
        OpenRobotUI,
        TimeExpl,
        PointsExpl,
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
        DevHintsExpl,
        OpenExtraHints,
        CloseHints,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    Cheat_CurrentAction hintsUI;

    public string tabToOpen = "";
    public bool closeTab = false;
    public bool expectedRobotUIstate = false;
    public bool expectedHintsState = false;
    public bool openMailMessage = false;

    protected override void Start()
    {
        base.Start();

        hintsUI = GameObject.FindObjectOfType<Cheat_CurrentAction>();
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
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetSize(570f, 367f);
                    hintsN.LockTo("robot", new Vector3(273.20f, 19.20f, 57.40f));
                    UItext.text = "Welkom! Voordat je begint, willen we je graag nog een aantal dingen uitleggen. Begin de uitleg door te klikken op de knop hieronder.";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {

                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.RobotIntro;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.25f, 0.09f));
                        hintsN.SetIconPosition(1);
               
                        UItext.text = "Ik ben Olivia en zal je op weg helpen om de besturing van Care Up te leren. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.RobotIntro:
                    if (nextButtonClicked)
                    {
                        hintsN.SetSize(570f, 226.6f);
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.OpenRobotUI;          
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("/UI/RobotUITrigger", new Vector3(53.24f, -48.91f, 0.00f));
                        UItext.text = "Klik op het tableticoontje om de tablet te openen. Op de tablet kun je veel informatie vinden. Neem maar eens een kijkje!";
                        expectedRobotUIstate = true;
                    }
                    break;
                case TutorialStep.OpenRobotUI:
                    if (player.tutorial_robotUI_opened)
                    {
                        hintsN.ResetSize();
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.TimeExpl;
                        hintsN.LockTo("Stopwatch", new Vector3(107.20f, -143.10f, 0.00f));
                        UItext.text = "Het klokje geeft aan hoe lang je bezig bent met het protocol. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TimeExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PointsExpl;

                        hintsN.LockTo("Stopwatch", new Vector3(1297.10f, -149.20f, 0.00f));
                        UItext.text = "Hier zie je het aantal punten dat je behaald hebt. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PointsExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PercentExpl;

                        hintsN.LockTo("Stopwatch", new Vector3(2678.00f, -149.20f, 0.00f));
                        UItext.text = "Bekijk hier je voortgang.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PercentExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.TabsExpl;

                        hintsN.LockTo("RobotUI", new Vector3(-428.00f, -213.19f, 0.00f));
                        UItext.text = "Klik op de apps om ze te openen.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TabsExpl:                        
                    if (nextButtonClicked)                         
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PressGeneral;
                        hintsN.SetSize(519f, 70.4f);
                        hintsN.LockTo("RobotUI", new Vector3(-990.90f, -49.30f, -99.80f)); 
                        UItext.text = "Klik op het icoontje ‘Gegevens’.";
                        RobotUITabs.tutorial_generalOpened = false;
                        tabToOpen = "GeneralTab";
                    }
                    break;
                case TutorialStep.PressGeneral:
                    if (RobotUITabs.tutorial_generalOpened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.GeneralExpl;
                        hintsN.SetSize(788f, 318.3f);
                        hintsN.LockTo("RobotUI", new Vector3(-1301.16f, -377.96f, 0.00f));
                        UItext.text = "Hier vind je cliëntgegevens en de toedienlijst. Gebruik dit om gegevens te controleren, bekijken en de toedienlijst af te tekenen. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.GeneralExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PatientRecordsOpen;
                        UItext.text = "Bekijk de cliëntgegevens door op het icoontje te klikken. ";
                        hintsN.SetSize(452f, 164.8f);
                        hintsN.LockTo("RobotUI", new Vector3(-1006.70f, 38.00f, -99.80f));
                        RobotUITabs.tutorial_recordsOpened = false;
                        tabToOpen = "RecordsTab";
                    }
                    break;
                case TutorialStep.PatientRecordsOpen:
                    if (RobotUITabs.tutorial_recordsOpened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        RobotUITabs.tutorial_recordsOpened = false;
                        hintsN.SetSize(788f, 524.9f);
                        hintsN.LockTo("RobotUI", new Vector3(-1301.16f, 958.30f, 0.00f));
                        currentStep = TutorialStep.PatientRecordsExpl;
                        UItext.text = "Bekijk en controleer hier de cliëntgegevens. De controle vindt automatisch plaats wanneer je de cliëntgegevens opent. ";

                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PatientRecordsExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PatientRecordsBack;
                        hintsN.SetSize(452f, 162.1f);
                        hintsN.LockTo("RobotUI", new Vector3(-1726.40f, 1017.60f, 0.00f));
                        UItext.text = "Sluit de cliëntgegevens door op het pijltje te klikken. ";
                        
                        closeTab = true;
                        tabToOpen = "GeneralTab";
                        RobotUITabs.tutorial_back = false;
                    }
                    break;
                case TutorialStep.PatientRecordsBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PatientPrescriptionOpen;
                        hintsN.LockTo("RobotUI", new Vector3(268.00f, 111.00f, 0.00f));
                        UItext.text = "Open de toedienlijst door op het icoon te klikken. ";

                        RobotUITabs.tutorial_prescriptionOpened = false;
                        tabToOpen = "PrescriptionTab";
                    }
                    break;
                case TutorialStep.PatientPrescriptionOpen:
                    if (RobotUITabs.tutorial_prescriptionOpened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        RobotUITabs.tutorial_prescriptionOpened = false;
                        hintsN.SetSize(788f, 524.9f);
                        hintsN.LockTo("RobotUI", new Vector3(-1301.16f, 958.30f, 0.00f));
                        currentStep = TutorialStep.WritingDownExpl;
                        UItext.text = "Bekijk en controleer hier de toedienlijst. De controle vindt automatisch plaats wanneer je de toedienlijst opent. ";

                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.WritingDownExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PatientPrescriptionExpl;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("RobotUI", new Vector3(-1650.00f, 1017.60f, 0.00f));
                        hintsN.SetSize(452f, 162.1f);
                        UItext.text = "Sluit de toedienlijst door op het pijltje te klikken. ";

                        closeTab = true;
                        tabToOpen = "GeneralTab";
                        RobotUITabs.tutorial_back = false;
                    }
                    break;
                case TutorialStep.PatientPrescriptionExpl:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PrescriptionBack;
                        hintsN.LockTo("RobotUI", new Vector3(845.00f, -105.00f, 0.00f));
                        hintsN.SetIconPosition(1);
                        hintsN.SetSize(465f, 408f);
                        UItext.text = "Door op het aftekenicoon te klikken, kun je een handeling aftekenen. Let op: hiermee rond je de handeling ook af!";

                        SetUpTutorialNextButton();
                    }
                    break;  
                case TutorialStep.PrescriptionBack:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.GeneralBack;
                        hintsN.SetSize(452f, 162.1f);
                        hintsN.LockTo("RobotUI", new Vector3(-1650.00f, 936.80f, -99.80f));
                        hintsN.SetIconPosition(0);
                        UItext.text = "Sluit de aftekenlijst door op het pijltje te klikken.";
                        
                        RobotUITabs.tutorial_back = false;
                    }
                    break;
                case TutorialStep.GeneralBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.PressChecklist;
                      
						hintsN.LockTo("RobotUI", new Vector3(153.20f, 22.10f, 0.00f));
                        UItext.text = "Open de checklist door op de checklist te klikken. ";
                        RobotUITabs.tutorial_checkListOpened = false;
                        tabToOpen = "CheckListTab";
                    }
                    break;
                case TutorialStep.PressChecklist:
                    if (RobotUITabs.tutorial_checkListOpened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.ChecklistExpl;
                        hintsN.SetSize(788f, 524.9f);
                        hintsN.LockTo("RobotUI", new Vector3(-1568.20f, 958.30f, 0.00f));
                        UItext.text = "Tijdens het oefenen van een protocol kun je in de checklist zien welke stappen je hebt doorlopen. Ook zie je welke stappen je nog moet uitvoeren om het protocol af te ronden.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.ChecklistExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.ChecklistBack;
                        hintsN.SetSize(452f, 162.1f);
                        hintsN.LockTo("RobotUI", new Vector3(-1650.00f, 958.20f, -34.70f));
                        UItext.text = "Klik op het pijltje om terug te gaan. ";
                        closeTab = true;
                    }
                    break;
                case TutorialStep.ChecklistBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.PressMessageCenter;
                        UItext.text = "Je hebt een bericht! Klik op ‘Berichten’ om het bericht te openen. ";
						hintsN.LockTo("RobotUI", new Vector3(849.00f, -244.00f, -34.70f));
                        hintsN.SetSize(452f, 164.8f);
                        hintsN.SetIconPosition(1);
                        GameObject.FindObjectOfType<RobotUIMessageTab>().NewMessage("Leren Spelen",
                            "De inhoud van de berichten verschijnt in dit venster. Klik op het pijltje linksboven in je scherm om terug te gaan. ",
                             RobotUIMessageTab.Icon.Info);
                        RobotUITabs.tutorial_messageCenterOpened = false;
                        tabToOpen = "MessageCenter";
                    }
                    break;
                case TutorialStep.PressMessageCenter:
                    if (RobotUITabs.tutorial_messageCenterOpened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        tabToOpen = "";
                        currentStep = TutorialStep.MessageCenterExpl;
                      
						hintsN.SetIconPosition(0);
                        hintsN.SetSize(512f, 332.5f);
                        hintsN.LockTo("RobotUI", new Vector3(-1305.70f, 937.90f, 0.00f));
                        UItext.text = "Tijdens het oefenen zullen wij jou berichten sturen om je te helpen en te informeren. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.MessageCenterExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.OpenMessage;
                        hintsN.SetSize(452f, 73.6f);
                        hintsN.LockTo("RobotUI", new Vector3(-1113.50f, 683.00f, 0.00f));
                        UItext.text = "Klik op een bericht.";
                        RobotUIMessageTab.tutorial_messageOpened = false;
                        openMailMessage = true;
                    }
                    break;
                case TutorialStep.OpenMessage:
                    if (RobotUIMessageTab.tutorial_messageOpened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.CloseMessageCenter;
                        hintsN.SetIconPosition(1);
                        hintsN.SetSize(452f, 199.5f);
                        hintsN.LockTo("RobotUI", new Vector3(-557.30f, 809.80f, 0.00f));
                        UItext.text = "De inhoud van de berichten verschijnt in dit venster. Klik op het pijltje linksboven in je scherm om terug te gaan. ";
                        RobotUITabs.tutorial_back = false;
                        closeTab = true;
                    }
                    break;
                case TutorialStep.CloseMessageCenter:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.CloseRobotUI;
                        hintsN.SetSize(452f, 252.3f);
                        hintsN.LockTo("RobotUI", new Vector3(1880.40f, 1299.00f, 0.00f));
						hintsN.SetIconPosition(1);
                        UItext.text = "Sluit de tablet af door op het kruisje te klikken. Je kunt de tablet altijd weer openen door op het tableticoontje te klikken. ";
                        expectedRobotUIstate = false;
                    }
                    break;
                case TutorialStep.CloseRobotUI:
                    if (player.tutorial_robotUI_closed)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.DevHintsExpl;
                        hintsN.SetSize(452f, 414.7f);
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("DevHint", new Vector3(375.39f, -84.72f, 0.00f));
                        UItext.text = "Hiernaast zien we de werkwijze die je moet doorlopen om het protocol succesvol af te ronden.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.DevHintsExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.OpenExtraHints;
                        hintsN.SetSize(452f, 414.7f);
                        hintsN.LockTo("ExtraButton", new Vector3(81.50f, -22.50f, 0.00f));
                        UItext.text = "Als je niet weet hoe je een stap moet uitvoeren, klik dan op het informatie icoontje. ";
                        hintsUI.tutorial_extraOpened = false;
                        expectedHintsState = true;
                    }
                    break;
                case TutorialStep.OpenExtraHints:
                    if (hintsUI.tutorial_extraOpened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.CloseHints;
                        hintsN.SetSize(323.8f, 674.1f);
                        hintsN.LockTo("Extra", new Vector3(523.70f, 426.20f, 0.00f));
                        UItext.text = "Hier komt tijdens het spelen van een protocol extra uitleg te staan over hoe je een stap kunt uitvoeren. Laten we het scherm weer sluiten door nogmaals op het informatie icoon te klikken.";
                        hintsUI.tutorial_extraClosed = false;
                        expectedHintsState = false;
                    }
                    break;
                case TutorialStep.CloseHints:
                    if (hintsUI.tutorial_extraClosed)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.Done;

                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        UItext.text = "Gefeliciteerd! Je hebt nu alles geleerd over de menu's en iconen binnen Care Up. Veel plezier met het oefenen van de verpleegtechnische handelingen!";
                    }
                    break;
                case TutorialStep.Done:
                    currentStep = TutorialStep.None;
                    TutorialEnd();
                    break;
            }
        }
    }

    public void OnTutorialButtonClick_Movement()
    {
        string sceneName = "Tutorial_Movement";
        string bundleName = "tutorial_move";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }
}
