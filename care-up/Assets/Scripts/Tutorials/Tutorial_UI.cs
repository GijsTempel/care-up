using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_UI : TutorialManager
{
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
            switch (currentStep)
            {
                case TutorialStep.First:
                    currentStep = TutorialStep.Welcome;
					hintsN.SetSize(788f, 524.9f);
                    hintsN.LockTo("UI(Clone)", new Vector3(-393.80f, 214.70f, 0.00f));
                    UItext.text = "Welkom, in deze oefening zullen wij je uitleggen wat alle menu's en iconen in Care Up betekenen.";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.RobotIntro;
						hintsN.LockTo("robot", new Vector3(0.00f, -0.25f, 0.09f));
						hintsN.SetIconPosition(1);
						hintsN.ResetSize();
                        UItext.text = "Dit is Olivia. Olivia is jou helper robot. Olivia heeft op dit moment nog niet veel functies maar dit zal veranderen in de toekomst. Wel geeft ze aan wanneer een actie goed of fout is uitgevoerd. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.RobotIntro:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenRobotUI;
                        //hintsBox.anchoredPosition = new Vector2(-560, -68);
						hintsN.SetIconPosition(0);
						hintsN.LockTo("/UI/RobotUITrigger", new Vector3(53.24f, -48.91f, 0.00f));
                        UItext.text = "Het icon met de tablet kun je aanklikken om je tablet te openen. Op de tablet kun je veel informatie vinden. Probeer de tablet erbij te pakken door op het icoon te klikken.";
                        expectedRobotUIstate = true;
                    }
                    break;
                case TutorialStep.OpenRobotUI:
                    if (player.tutorial_robotUI_opened)
                    {
                        currentStep = TutorialStep.TimeExpl;
						hintsN.LockTo("Stopwatch", new Vector3(107.20f, -143.10f, 0.00f));
						UItext.text = "Het klokje met de tijd geeft aan hoelang je bezig bent met het protocol. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TimeExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PointsExpl;
                      
						hintsN.LockTo("Stopwatch", new Vector3(1297.10f, -149.20f, 0.00f));
                        UItext.text = "Hier worden het aantal punten weergegeven die je hebt behaald. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PointsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PercentExpl;
                     
						hintsN.LockTo("Stopwatch", new Vector3(2678.00f, -149.20f, 0.00f));
                        UItext.text = "Hier staat in % aangegeven hoe ver je binnen het protocol bent.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PercentExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.TabsExpl;
                     
						hintsN.LockTo("RobotUI", new Vector3(-428.00f, -396.14f, 0.00f));
                        UItext.text = "Je kunt op de apps die te zien zijn klikken om de app te openen.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TabsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PressGeneral;
               
						hintsN.LockTo("RobotUI", new Vector3(-990.90f, -49.30f, -99.80f));
                        UItext.text = "Laten we de 'Algemene' app openen door op het icoon te klikken. Probeer dit nu.";
                        RobotUITabs.tutorial_generalOpened = false;
                        tabToOpen = "GeneralTab";
                    }
                    break;
                case TutorialStep.PressGeneral:
                    if (RobotUITabs.tutorial_generalOpened)
                    {
                        currentStep = TutorialStep.GeneralExpl;
                      
						hintsN.LockTo("RobotUI", new Vector3(-694.10f, 338.70f, -99.80f));
                        UItext.text = "In de app 'Algemeen' vind je algemene informatie zoals de naam van het protocol en de geluidsknop.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.GeneralExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.GeneralBack;

						hintsN.LockTo("RobotUI", new Vector3(-1826.90f, 936.80f, -99.80f));
                        UItext.text = "Laten we de app sluiten door op de terug knop te drukken. ";
                        closeTab = true;
                    }
                    break;
                case TutorialStep.GeneralBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.PressChecklist;
                      
						hintsN.LockTo("RobotUI", new Vector3(153.20f, 22.10f, 0.00f));
                        UItext.text = "Laten we nu gaan kijken naar de checklist app.";
                        RobotUITabs.tutorial_checkListOpened = false;
                        tabToOpen = "CheckListTab";
                    }
                    break;
                case TutorialStep.PressChecklist:
                    if (RobotUITabs.tutorial_checkListOpened)
                    {
                        currentStep = TutorialStep.ChecklistExpl;
                    
						hintsN.LockTo("RobotUI", new Vector3(-511.90f, 379.10f, -34.70f));
                        UItext.text = "Tijdens het spelen van protocollen kan je in de checklist app zien welke stappen je hebt doorlopen en welke stappen je nog moet uitvoeren om het protocol af te ronden.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.ChecklistExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.ChecklistBack;
                     
						hintsN.LockTo("RobotUI", new Vector3(-1856.10f, 958.20f, -34.70f));
                        UItext.text = "Laten we terug gaan.";
                        closeTab = true;
                    }
                    break;
                case TutorialStep.ChecklistBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.PressMessageCenter;
                        UItext.text = "Oh wow, je hebt een bericht! Laten we gaan kijken!";
               
						hintsN.LockTo("RobotUI", new Vector3(849.00f, -244.00f, -34.70f));
						hintsN.SetIconPosition(1);
                        GameObject.FindObjectOfType<RobotUIMessageTab>().NewMessage("Leren Spelen",
                            "De inhoud van de berichten verschijnen in dit venster. Gebruik de terug knop linksboven in het scherm van de tablet om de app af te sluiten en verder te gaan!",
                             RobotUIMessageTab.Icon.Info);
                        RobotUITabs.tutorial_messageCenterOpened = false;
                        tabToOpen = "MessageCenter";
                    }
                    break;
                case TutorialStep.PressMessageCenter:
                    if (RobotUITabs.tutorial_messageCenterOpened)
                    {
                        tabToOpen = "";
                        currentStep = TutorialStep.MessageCenterExpl;
                      
						hintsN.SetIconPosition(0);
						hintsN.LockTo("RobotUI", new Vector3(-687.60f, -244.00f, -34.70f));
                        UItext.text = "Dit is de berichten app. Tijdens het spelen zullen wij jou berichten sturen om je te helpen en te informeren.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.MessageCenterExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenMessage;
                    
						hintsN.LockTo("RobotUI", new Vector3(-1113.50f, 683.00f, 0.00f));
                        UItext.text = "Laten we kijken wat er in het bericht staat door op de titel te klikken.";
                        RobotUIMessageTab.tutorial_messageOpened = false;
                        openMailMessage = true;
                    }
                    break;
                case TutorialStep.OpenMessage:
                    if (RobotUIMessageTab.tutorial_messageOpened)
                    {
                        currentStep = TutorialStep.CloseMessageCenter;
                    
						hintsN.LockTo("RobotUI", new Vector3(-1861.90f, 957.80f, 0.00f));
                        UItext.text = "Berichten kun je lezen wanneer je wilt. Er kan erg handige informatie in staan. Laten we de berichten app afsluiten door op de terug knop te klikken.";
                        RobotUITabs.tutorial_back = false;
                        closeTab = true;
                    }
                    break;
                case TutorialStep.CloseMessageCenter:
                    if (RobotUITabs.tutorial_back)
                    {
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.CloseRobotUI;
                    
						hintsN.LockTo("RobotUI", new Vector3(1880.40f, 1299.00f, 0.00f));
						hintsN.SetIconPosition(1);
                        UItext.text = "Laten we de tablet afsluiten. Je kunt de tablet altijd weer openen door op het icoon te klikken.";
                        expectedRobotUIstate = false;
                    }
                    break;
                case TutorialStep.CloseRobotUI:
                    if (player.tutorial_robotUI_closed)
                    {
                        currentStep = TutorialStep.DevHintsExpl;
             
						hintsN.SetIconPosition(0);
                        hintsN.LockTo("DevHint", new Vector3(375.39f, -84.72f, 0.00f));
                        UItext.text = "Hier zien we de stappen die je moet doorlopen om het protocol succesvol af te ronden.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.DevHintsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenExtraHints;

                        hintsN.LockTo("ExtraButton", new Vector3(81.50f, -22.50f, 0.00f));
                        UItext.text = "Als je niet weet hoe je een stap moet uitvoeren dan kun je klikken op het informatie icoon. Laten we op het icoon klikken";
                        hintsUI.tutorial_extraOpened = false;
                        expectedHintsState = true;
                    }
                    break;
                case TutorialStep.OpenExtraHints:
                    if (hintsUI.tutorial_extraOpened)
                    {
                        currentStep = TutorialStep.CloseHints;

                        hintsN.LockTo("ExtraButton", new Vector3(830.40f, -158.20f, 0.00f));
                        UItext.text = "Hier komt tijdens het spelen van een protocol extra uitleg te staan over hoe je een stap kunt uitvoeren. Laten we het scherm weer sluiten door nogmaals op het informatie icoon te klikken.";
                        hintsUI.tutorial_extraClosed = false;
                        expectedHintsState = false;
                    }
                    break;
                case TutorialStep.CloseHints:
                    if (hintsUI.tutorial_extraClosed)
                    {
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
}
