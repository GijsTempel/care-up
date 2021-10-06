using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial_UI : TutorialManager
{
    public AudioClip Popup;
    public AudioClip Done;
    public AudioClip Robot1;
    public AudioClip Robot2;
    public AudioClip Robot3;
    public AudioClip RobotShort1;
    public AudioClip RobotShort2;
    AudioSource audioSource;

    public enum TutorialStep
    {
        First,
        Welcome,
        RobotIntro,
        OpenRobotUI,
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
        //PressChecklist,
        //ChecklistExpl,
        //ChecklistBack, 
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

    public GameObject Tweenbutton;

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
            audioSource = GetComponent<AudioSource>();

            switch (currentStep)
            {
                case TutorialStep.First:

                    audioSource.PlayOneShot(Popup, 0.1F);
                    audioSource.PlayOneShot(Robot1, 0.1F);
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetSize(570f, 418f);
                    hintsN.LockTo("robot", new Vector3(273.20f, 19.20f, -139.25f));
                    hintsN.SetIconPosition(1);

                    //.text = "Hallo! Voordat je aan de slag gaat met Care Up, willen we je graag nog een aantal dingen uitleggen.";
                    UItext.DOText("Hallo! Voordat je aan de slag gaat met Care Up, willen we je graag nog een aantal dingen uitleggen.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        Tweenbutton.transform.DOScale(1f, 0.4f).SetLoops(-1, LoopType.Yoyo);
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        currentStep = TutorialStep.RobotIntro;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.25f, 0.09f));
                        hintsN.SetIconPosition(1);
                        UItext.DOText("Ik ben Olivia en zal je op weg helpen om de besturing van Care Up te leren. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.RobotIntro:
                    if (nextButtonClicked)
                    {
                        hintsN.SetSize(570f, 226.6f);
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);
                        currentStep = TutorialStep.OpenRobotUI;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("RobotUITrigger", new Vector3(53.24f, -101.46f, 0.00f));
                        UItext.DOText("Klik op het tableticoontje om de tablet te openen. Op de tablet kun je veel informatie vinden. Neem maar eens een kijkje!", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        expectedRobotUIstate = true;
                        GameObject.Find("RobotUITrigger").GetComponent<ButtonBlinking>().StartBlinking();

                    }
                    break;
                case TutorialStep.OpenRobotUI:
                    if (player.tutorial_robotUI_opened)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.TabsExpl;

                        hintsN.LockTo("RobotUI", new Vector3(-428.00f, -213.19f, 0.00f));
                        UItext.DOText("Klik op de apps om ze te openen.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TabsExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        currentStep = TutorialStep.PressGeneral;
                        hintsN.SetSize(519f, 70.4f);
                        hintsN.LockTo("RobotUI", new Vector3(-990.90f, -49.30f, -99.80f));
                        UItext.DOText("Klik op het icoontje ‘Gegevens’", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);                                
                        RobotUITabs.tutorial_generalOpened = false;

                        foreach (ButtonBlinking s in GameObject.FindObjectsOfType<ButtonBlinking>())
                        {
                            if (s.name == "GeneralTab")
                                s.GetComponent<ButtonBlinking>().StartBlinking();
                        }

                        tabToOpen = "GeneralTab";
                    }
                    break;
                case TutorialStep.PressGeneral:
                    if (RobotUITabs.tutorial_generalOpened)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot1, 0.1F);
                        currentStep = TutorialStep.GeneralExpl;
                        hintsN.SetSize(788f, 318.3f);
                        hintsN.LockTo("RobotUI", new Vector3(-1301.16f, -377.96f, 0.00f));
                        UItext.DOText("Hier vind je cliëntgegevens en de toedienlijst. Gebruik dit om gegevens te controleren, bekijken en de toedienlijst af te tekenen. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.GeneralExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.PatientRecordsOpen;
                        UItext.text = "Bekijk de cliëntgegevens door op het icoontje te klikken. ";
                        UItext.DOText("Bekijk de cliëntgegevens door op het icoontje te klikken.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        hintsN.SetSize(452f, 164.8f);
                        hintsN.LockTo("RobotUI", new Vector3(-1006.70f, 38.00f, -99.80f));
                        RobotUITabs.tutorial_recordsOpened = false;
                        GameObject.FindObjectOfType<ButtonBlinkingOptions>().recordsTabBlink.SetTrigger("BlinkStart");                      
                        tabToOpen = "RecordsTab";
                    }
                    break;
                case TutorialStep.PatientRecordsOpen:
                    if (RobotUITabs.tutorial_recordsOpened)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        RobotUITabs.tutorial_recordsOpened = false;
                        hintsN.SetSize(788f, 524.9f);
                        hintsN.LockTo("RobotUI", new Vector3(-1301.16f, 958.30f, 0.00f));
                        currentStep = TutorialStep.PatientRecordsExpl;
                        UItext.DOText("Bekijk en controleer hier de cliëntgegevens. De controle vindt automatisch plaats wanneer je de cliëntgegevens opent.  ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PatientRecordsExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        currentStep = TutorialStep.PatientRecordsBack;
                        hintsN.SetSize(452f, 162.1f);
                        hintsN.LockTo("RobotUI", new Vector3(-1726.40f, 1017.60f, 0.00f));
                        UItext.DOText("Sluit de cliëntgegevens door op het pijltje te klikken. ", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        GameObject.FindObjectOfType<ButtonBlinkingOptions>().recordsTabBackButtonBlink.SetTrigger("BlinkStart");
                        closeTab = true;
                        tabToOpen = "GeneralTab";
                        RobotUITabs.tutorial_back = false;
                    }
                    break;
                case TutorialStep.PatientRecordsBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.PatientPrescriptionOpen;
                        hintsN.LockTo("RobotUI", new Vector3(268.00f, 111.00f, 0.00f));
                        UItext.DOText("Open de toedienlijst door op het icoon te klikken. ", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        GameObject.FindObjectOfType<ButtonBlinkingOptions>().prescriptionTabBlink.SetTrigger("BlinkStart");
                        RobotUITabs.tutorial_prescriptionOpened = false;
                        tabToOpen = "PrescriptionTab";
                    }
                    break;
                case TutorialStep.PatientPrescriptionOpen:
                    if (RobotUITabs.tutorial_prescriptionOpened)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);
                        RobotUITabs.tutorial_prescriptionOpened = false;
                        hintsN.SetSize(788f, 524.9f);
                        hintsN.LockTo("RobotUI", new Vector3(-1301.16f, 958.30f, 0.00f));
                        currentStep = TutorialStep.WritingDownExpl;
                        UItext.DOText("Bekijk en controleer hier de toedienlijst. De controle vindt automatisch plaats wanneer je de toedienlijst opent.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.WritingDownExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        currentStep = TutorialStep.PatientPrescriptionExpl;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("RobotUI", new Vector3(-1650.00f, 1017.60f, 0.00f));
                        hintsN.SetSize(452f, 162.1f);
                        UItext.DOText("Sluit de toedienlijst door op het pijltje te klikken.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        GameObject.FindObjectOfType<ButtonBlinkingOptions>().prescriptionTabBackButtonBlink.SetTrigger("BlinkStart");
                        closeTab = true;
                        tabToOpen = "GeneralTab";
                        RobotUITabs.tutorial_back = false;
                    }
                    break;
                case TutorialStep.PatientPrescriptionExpl:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot1, 0.1F);
                        currentStep = TutorialStep.PrescriptionBack;
                        hintsN.LockTo("RobotUI", new Vector3(845.00f, -105.00f, 0.00f));
                        hintsN.SetIconPosition(1);
                        hintsN.SetSize(652f, 408f);
                        UItext.DOText("Door op het afteken-/afsluiticoon te klikken, kun je een handeling aftekenen of afsluiten. Let op: hiermee rond je de handeling ook af!", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PrescriptionBack:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.GeneralBack;
                        hintsN.SetSize(452f, 162.1f);
                        hintsN.LockTo("RobotUI", new Vector3(-1650.00f, 936.80f, -99.80f));
                        hintsN.SetIconPosition(0);
                        UItext.DOText("Sluit de ‘Gegevens’ app door op het pijltje te klikken.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        GameObject.FindObjectOfType<ButtonBlinkingOptions>().generalTabBackButtonBlink.SetTrigger("BlinkStart");
                        RobotUITabs.tutorial_back = false;
                    }
                    break;
                case TutorialStep.GeneralBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.PressMessageCenter;
                        UItext.DOText("Je hebt een bericht! Klik op ‘Berichten’ om het bericht te openen. ", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        hintsN.LockTo("RobotUI", new Vector3(849.00f, -244.00f, -34.70f));
                        hintsN.SetSize(452f, 164.8f);
                        hintsN.SetIconPosition(1);
                        GameObject.FindObjectOfType<ButtonBlinkingOptions>().MessageTabBlink.SetTrigger("BlinkStart");                    
                        RobotUITabs.tutorial_messageCenterOpened = false;
                        tabToOpen = "MessageCenter";
                    }
                    break;
                case TutorialStep.PressMessageCenter:
                    if (RobotUITabs.tutorial_messageCenterOpened)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        tabToOpen = "";
                        currentStep = TutorialStep.MessageCenterExpl;
                        hintsN.SetIconPosition(0);
                        hintsN.SetSize(512f, 332.5f);
                        audioSource.PlayOneShot(Robot1, 0.1F);
                        hintsN.LockTo("RobotUI", new Vector3(-1305.70f, 937.90f, 0.00f));
                        UItext.DOText("Tijdens het oefenen zullen wij jou berichten sturen om je te helpen en te informeren.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.MessageCenterExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        currentStep = TutorialStep.OpenMessage;
                        hintsN.SetSize(452f, 73.6f);
                        hintsN.LockTo("RobotUI", new Vector3(-1113.50f, 683.00f, 0.00f));
                        UItext.DOText("Klik op een bericht", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        RobotUIMessageTab.tutorial_messageOpened = false;
                        openMailMessage = true;
                    }
                    break;
                case TutorialStep.OpenMessage:
                    if (RobotUIMessageTab.tutorial_messageOpened)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        currentStep = TutorialStep.CloseMessageCenter;
                        hintsN.SetIconPosition(1);
                        hintsN.SetSize(452f, 199.5f);
                        hintsN.LockTo("RobotUI", new Vector3(-557.30f, 809.80f, 0.00f));
                        GameObject.FindObjectOfType<ButtonBlinkingOptions>().messageTabBackButtonBlink.SetTrigger("BlinkStart");
                        UItext.DOText("De inhoud van de berichten verschijnt in dit venster. Klik op het pijltje linksboven in je scherm om terug te gaan. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        RobotUITabs.tutorial_back = false;
                        closeTab = true;
                    }
                    break;
                case TutorialStep.CloseMessageCenter:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.CloseRobotUI;
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        hintsN.SetSize(452f, 300f);
                        hintsN.LockTo("RobotUI", new Vector3(1880.40f, 1109.00f, 0.00f));
                        hintsN.SetIconPosition(1);
                        GameObject.FindObjectOfType<ButtonBlinkingOptions>().closeButtonBlink.SetTrigger("BlinkStart");
                        UItext.DOText("Sluit de tablet af door op het kruisje te klikken. Je kunt de tablet altijd weer openen door op het tableticoontje te klikken. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        expectedRobotUIstate = false;
                    }
                    break;
                case TutorialStep.CloseRobotUI:
                    if (player.tutorial_robotUI_closed)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot1, 0.1F);
                        currentStep = TutorialStep.DevHintsExpl;
                        hintsN.SetSize(452f, 414.7f);
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("DevHint", new Vector3(375.39f, -284.23f, 0.00f));
                        UItext.DOText("Boven in het scherm zien we de werkwijze die je moet volgen om het handeling succesvol af te ronden.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.DevHintsExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        currentStep = TutorialStep.OpenExtraHints;
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        hintsN.SetSize(452f, 414.7f);
                        hintsN.LockTo("ExtraButton", new Vector3(81.50f, -22.50f, 0.00f));
                        GameObject.FindObjectOfType<GameUI>().Blink.SetTrigger("BlinkStart");
                        UItext.DOText("Als je niet weet hoe je een stap moet uitvoeren, klik dan op het informatie icoontje. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        hintsUI.tutorial_extraOpened = false;
                        expectedHintsState = true;
                    }
                    break;
                case TutorialStep.OpenExtraHints:
                    if (hintsUI.tutorial_extraOpened)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot1, 0.1F);
                        currentStep = TutorialStep.CloseHints;
                        hintsN.SetSize(640f, 412f);
                        hintsN.LockTo("Extra", new Vector3(19.91f, 235.20f, 0.00f));
                        GameObject.FindObjectOfType<GameUI>().Blink.SetTrigger("BlinkStop");
                        UItext.DOText("Hier komt tijdens het spelen van de handeling extra uitleg te staan over hoe je een stap kunt uitvoeren. Laten we het scherm weer sluiten door nogmaals op het informatie icoon te klikken of door op de knop met het vinkje te klikken..", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        hintsUI.tutorial_extraClosed = false;
                        expectedHintsState = false;
                    }
                    break;
                case TutorialStep.CloseHints:
                    if (hintsUI.tutorial_extraClosed)
                    {
                        audioSource.PlayOneShot(Done, 0.1F);
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
