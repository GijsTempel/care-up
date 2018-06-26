using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Theory : TutorialManager
{
    public enum TutorialStep
    {
        First,
        Welcome,
        OpenRobotUI,
        PressTheory,
        TheoryExpl,
        PressScroll,
        ChangePDF,
        FullScrOn,
        FullScrOff,
        TheoryBack,
        CloseIpad,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;
    
    public string tabToOpen = "";
    public bool closeTab = false;
    public bool expectedRobotUIstate = false;

    private RobotUITabInfo infoTab;
    private PDFFullScreenViewer pdfViewer;

    protected override void Update()
    {
        base.Update();

        if (!Paused())
        {
            switch (currentStep)
            {
                case TutorialStep.First:
                    infoTab = GameObject.FindObjectOfType<RobotUITabInfo>();
                    
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetSize(788f, 524.9f);
                    hintsN.LockTo("UI(Clone)", new Vector3(-393.80f, 214.70f, 0.00f));
                    UItext.text = "Welkom, in deze oefening zullen wij je uitleggen wat alle menu's en iconen in Care Up betekenen.";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenRobotUI;
                        hintsN.LockTo("/UI/RobotUITrigger", new Vector3(53.24f, -48.91f, 0.00f));
                        UItext.text = "Het icon met de tablet kun je aanklikken om je tablet te openen. Op de tablet kun je veel informatie vinden. Probeer de tablet erbij te pakken door op het icoon te klikken.";
                        expectedRobotUIstate = true;
                    }
                    break;
                case TutorialStep.OpenRobotUI:
                    if (player.tutorial_robotUI_opened)
                    {
                        currentStep = TutorialStep.PressTheory;
                        hintsN.LockTo("RobotUI", new Vector3(-990.90f, -49.30f, -99.80f));
                        UItext.text = "Laten we de 'Algemene' app openen door op het icoon te klikken. Probeer dit nu.";
                        RobotUITabs.tutorial_infoTabOpened = false;
                        tabToOpen = "InfoTab";
                    }
                    break;
                case TutorialStep.PressTheory:
                    if (RobotUITabs.tutorial_infoTabOpened)
                    {
                        currentStep = TutorialStep.TheoryExpl;
                        hintsN.LockTo("RobotUI", new Vector3(-694.10f, 338.70f, -99.80f));
                        UItext.text = "In de app 'Algemeen' vind je algemene informatie zoals de naam van het protocol, geluidsknop & de terugkeren naar het hoofdmenu knop.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TheoryExpl:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.PressScroll;
                        UItext.text = "Press open list button";

                        infoTab.tutorial_listButton = false;
                    }
                    break;
                case TutorialStep.PressScroll:
                    if (infoTab.tutorial_listButton)
                    {
                        infoTab.tutorial_listButton = false;

                        currentStep = TutorialStep.ChangePDF;
                        UItext.text = "Select another PDF";

                        infoTab.tutorial_changedPDF = false;
                    }
                    break;
                case TutorialStep.ChangePDF:
                    if (infoTab.tutorial_changedPDF)
                    {
                        infoTab.tutorial_changedPDF = false;

                        currentStep = TutorialStep.FullScrOn;
                        UItext.text = "Turn on fullscreen mode";

                        infoTab.tutorial_fullscreen = false;
                    }
                    break;
                case TutorialStep.FullScrOn:
                    if (infoTab.tutorial_fullscreen)
                    {
                        infoTab.tutorial_fullscreen = false;

                        currentStep = TutorialStep.FullScrOff;
                        UItext.text = "Turn off fullscreen mode";

                        pdfViewer = FindObjectOfType<PDFFullScreenViewer>();
                        pdfViewer.tutorial_closedFullScrPDF = false;
                    }
                    break;
                case TutorialStep.FullScrOff:
                    if (pdfViewer.tutorial_closedFullScrPDF)
                    {
                        currentStep = TutorialStep.TheoryBack;
                        hintsN.LockTo("RobotUI", new Vector3(-1826.90f, 936.80f, -99.80f));
                        UItext.text = "Laten we de app sluiten door op de terug knop te drukken. ";

                        closeTab = true;
                    }
                    break;
                case TutorialStep.TheoryBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.CloseIpad;
                        hintsN.LockTo("RobotUI", new Vector3(230.00f, 1299.00f, 0.00f));
                        hintsN.SetIconPosition(1);
                        UItext.text = "Laten we de tablet afsluiten. Je kunt de tablet altijd weer openen door op het icoon te klikken.";
                        expectedRobotUIstate = false;
                    }
                    break;
                case TutorialStep.CloseIpad:
                    if (player.tutorial_robotUI_closed)
                    {
                        currentStep = TutorialStep.Done;
                        hintsN.LockTo("/Player/CinematicControl/Arms/Armature/Hips/Spine/Spine1/Spine2/Neck/Head/Camera/Canvas/SceneLoader 1", new Vector3(216.10f, 24.94f, 0.00f));
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
