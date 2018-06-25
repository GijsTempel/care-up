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
                    hintsBox.anchoredPosition = new Vector2(-0.00011874f, 0.00024414f);
                    hintsBox.sizeDelta = new Vector2(788f, 524.9f);
                    UItext.text = "Welkom, in deze oefening zullen wij je uitleggen wat alle menu's en iconen in Care Up betekenen.";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.RobotIntro;
                        hintsBox.anchoredPosition = new Vector2(442f, 184f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        UItext.text = "Dit is Olivia. Olivia is jou helper robot. Olivia heeft op dit moment nog niet veel functies maar dit zal veranderen in de toekomst. Wel geeft ze aan wanneer een actie goed of fout is uitgevoerd. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.RobotIntro:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenRobotUI;
                        hintsBox.anchoredPosition = new Vector2(-473, -19);
                        UItext.text = "Het icon met de tablet kun je aanklikken om je tablet te openen. Op de tablet kun je veel informatie vinden. Probeer de tablet erbij te pakken door op het icoon te klikken.";
                        expectedRobotUIstate = true;
                    }
                    break;
                case TutorialStep.OpenRobotUI:
                    if (player.tutorial_robotUI_opened)
                    {
                        currentStep = TutorialStep.TimeExpl;
                        hintsBox.anchoredPosition = new Vector2(-170f, 253f);
                        UItext.text = "Het klokje met de tijd geeft aan hoelang je bezig bent met het protocol. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TimeExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PointsExpl;
                        hintsBox.anchoredPosition = new Vector2(79f, 253f);
                        UItext.text = "Hier worden het aantal punten weergegeven die je hebt behaald. ";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PointsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PercentExpl;
                        hintsBox.anchoredPosition = new Vector2(492f, 253f);
                        UItext.text = "Hier staat in % aangegeven hoe ver je binnen het protocol bent.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PercentExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.TabsExpl;
                        hintsBox.anchoredPosition = new Vector2(0f, -308.88f);
                        UItext.text = "Je kunt op de apps die te zien zijn klikken om de app te openen.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TabsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PressGeneral;
                        hintsBox.anchoredPosition = new Vector2(-27f, -98f);
                        UItext.text = "Laten we de 'Algemene' app openen door op het icoon te klikken. Probeer dit nu.";
                        RobotUITabs.tutorial_generalOpened = false;
                        tabToOpen = "GeneralTab";
                    }
                    break;
                case TutorialStep.PressGeneral:
                    if (RobotUITabs.tutorial_generalOpened)
                    {
                        currentStep = TutorialStep.GeneralExpl;
                        hintsBox.anchoredPosition = new Vector2(0f, 0f);
                        UItext.text = "In de app 'Algemeen' vind je algemene informatie zoals de naam van het protocol, geluidsknop & de terugkeren naar het hoofdmenu knop.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.GeneralExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.GeneralBack;
                        hintsBox.anchoredPosition = new Vector2(-287f, 129f);
                        UItext.text = "Laten we de app sluiten door op de terug knop te drukken. ";
                        closeTab = true;
                    }
                    break;
                case TutorialStep.GeneralBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.PressChecklist;
                        hintsBox.anchoredPosition = new Vector2(334f, -149.45f);
                        UItext.text = "Laten we nu gaan kijken naar de checklist app.";
                        RobotUITabs.tutorial_checkListOpened = false;
                        tabToOpen = "CheckListTab";
                    }
                    break;
                case TutorialStep.PressChecklist:
                    if (RobotUITabs.tutorial_checkListOpened)
                    {
                        currentStep = TutorialStep.ChecklistExpl;
                        hintsBox.anchoredPosition = new Vector2(0f, 0f);
                        UItext.text = "Tijdens het spelen van protocollen kan je in de checklist app zien welke stappen je hebt doorlopen en welke stappen je nog moet uitvoeren om het protocol af te ronden.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.ChecklistExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.ChecklistBack;
                        hintsBox.anchoredPosition = new Vector2(-284f, 122f);
                        UItext.text = "Laten we terug gaan.";
                        closeTab = true;
                    }
                    break;
                case TutorialStep.ChecklistBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.PressMessageCenter;
                        UItext.text = "Oh wow, je hebt een bericht! Laten we gaan kijken! Klik op het berichten icoon.";
                        hintsBox.anchoredPosition = new Vector2(360f, -288f);
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
                        hintsBox.anchoredPosition = new Vector2(131f, -91f);
                        UItext.text = "Dit is de berichten app. Tijdens het spelen zullen wij jou berichten sturen om je te helpen en te informeren.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.MessageCenterExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenMessage;
                        hintsBox.anchoredPosition = new Vector2(-19f, 41f);
                        UItext.text = "Laten we kijken wat er in het bericht staat door op de titel te klikken.";
                        RobotUIMessageTab.tutorial_messageOpened = false;
                        openMailMessage = true;
                    }
                    break;
                case TutorialStep.OpenMessage:
                    if (RobotUIMessageTab.tutorial_messageOpened)
                    {
                        currentStep = TutorialStep.CloseMessageCenter;
                        hintsBox.anchoredPosition = new Vector2(159f, -262f);
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
                        hintsBox.anchoredPosition = new Vector2(617f, 193f);
                        UItext.text = "Laten we de tablet afsluiten. Je kunt de tablet altijd weer openen door op het icoon te klikken.";
                        expectedRobotUIstate = false;
                    }
                    break;
                case TutorialStep.CloseRobotUI:
                    if (player.tutorial_robotUI_closed)
                    {
                        currentStep = TutorialStep.DevHintsExpl;
                        hintsBox.anchoredPosition = new Vector2(61f, 308.49f);
                        UItext.text = "Hier zien we de stappen die je moet doorlopen om het protocol succesvol af te ronden.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.DevHintsExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenExtraHints;
                        hintsBox.anchoredPosition = new Vector2(-536, 95f);
                        UItext.text = "Als je niet weet hoe je een stap moet uitvoeren dan kun je klikken op het informatie icoon. Laten we op het icoon klikken";
                        hintsUI.tutorial_extraOpened = false;
                        expectedHintsState = true;
                    }
                    break;
                case TutorialStep.OpenExtraHints:
                    if (hintsUI.tutorial_extraOpened)
                    {
                        currentStep = TutorialStep.CloseHints;
                        hintsBox.anchoredPosition = new Vector2(236.25f, 55f);
                        UItext.text = "Hier komt tijdens het spelen van een protocol extra uitleg te staan over hoe je een stap kunt uitvoeren. Laten we het scherm weer sluiten door nogmaals op het informatie icoon te klikken.";
                        hintsUI.tutorial_extraClosed = false;
                        expectedHintsState = false;
                    }
                    break;
                case TutorialStep.CloseHints:
                    if (hintsUI.tutorial_extraClosed)
                    {
                        currentStep = TutorialStep.Done;
                        hintsBox.anchoredPosition = new Vector2(493f, -286f);
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
