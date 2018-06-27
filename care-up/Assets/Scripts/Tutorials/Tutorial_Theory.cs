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
                    UItext.text = "Welkom, in deze oefening zullen wij je uitleggen hoe je de theorie van een protocol kunt bekijken.";
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.OpenRobotUI;
                        hintsN.ResetSize();
                        hintsN.LockTo("/UI/RobotUITrigger", new Vector3(84.20f, -43.70f, 0.00f));
                        UItext.text = "Het icon met de tablet kun je aanklikken om je tablet te openen. Op de tablet kun je veel informatie vinden. Probeer de tablet erbij te pakken door op het icoon te klikken.";
                        expectedRobotUIstate = true;
                    }
                    break;
                case TutorialStep.OpenRobotUI:
                    if (player.tutorial_robotUI_opened)
                    {
                        currentStep = TutorialStep.PressTheory;
                        hintsN.LockTo("RobotUI", new Vector3(-897.20f, 597.00f, -99.80f));
                        hintsN.SetIconPosition(3);
                        UItext.text = "Laten we de 'Theorie' app openen door op het icoon te klikken.";
                        RobotUITabs.tutorial_infoTabOpened = false;
                        tabToOpen = "InfoTab";
                    }
                    break;
                case TutorialStep.PressTheory:
                    if (RobotUITabs.tutorial_infoTabOpened)
                    {
                        currentStep = TutorialStep.TheoryExpl;
                        hintsN.SetSize(788f, 524.9f);
                        hintsN.LockTo("UI(Clone)", new Vector3(-1314.83f, 719.20f, 0.00f));
                        hintsN.SetIconPosition(0);
                        UItext.text = "In de app 'Theorie' vindt je alle theorie die hoort bij het protocol die je aan het oefenen bent. Theorie kun je op ieder moment openen en bekijken.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.TheoryExpl:
                    if (nextButtonClicked)
                    {
                        hintsN.ResetSize();
                        hintsN.LockTo("RobotUI", new Vector3(-1373.80f, 958.20f, -34.70f));
                        currentStep = TutorialStep.PressScroll;
                        UItext.text = "Druk op de knop met de streepjes om alle theorie te tonen.";

                        infoTab.tutorial_listButton = false;
                    }
                    break;
                case TutorialStep.PressScroll:
                    if (infoTab.tutorial_listButton)
                    {
                        infoTab.tutorial_listButton = false;
                        hintsN.LockTo("RobotUI", new Vector3(-1132.30f, 408.00f, -34.70f));
                        currentStep = TutorialStep.ChangePDF;
                        UItext.text = "In deze lijst kun je theorie vinden over verschillende onderwerpen. Probeer nu een ander theorie onderwerp te openen door op een van de knoppen te drukken.";

                        infoTab.tutorial_changedPDF = false;
                    }
                    break;
                case TutorialStep.ChangePDF:
                    if (infoTab.tutorial_changedPDF)
                    {
                        infoTab.tutorial_changedPDF = false;
                        hintsN.SetIconPosition(1);
                        hintsN.LockTo("RobotUI", new Vector3(75.00f, 836.00f, -34.70f));
                        currentStep = TutorialStep.FullScrOn;
                        UItext.text = "Je kunt overschakelen naar een volledige scherm weergave om de theorie beter te kunnen lezen. Probeer dit nu door op de knop met de twee pijlen te drukken.";

                        infoTab.tutorial_fullscreen = false;
                    }
                    break;
                case TutorialStep.FullScrOn:
                    if (infoTab.tutorial_fullscreen)
                    {
                        infoTab.tutorial_fullscreen = false;
                        hintsN.LockTo("RobotUI", new Vector3(486.09f, 990.40f, -34.70f));
                        currentStep = TutorialStep.FullScrOff;
                        UItext.text = "Volledige scherm weergave kun je afsluiten door op de knop met het kruisje te drukken. Probeer dit nu.";

                        pdfViewer = FindObjectOfType<PDFFullScreenViewer>();
                        pdfViewer.tutorial_closedFullScrPDF = false;
                    }
                    break;
                case TutorialStep.FullScrOff:
                    if (pdfViewer.tutorial_closedFullScrPDF)
                    {
                        currentStep = TutorialStep.TheoryBack;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("RobotUI", new Vector3(-1826.90f, 936.80f, -99.80f));
                        UItext.text = "Dat was alles over de theorie app. Laten we de apps sluiten door op de knop te drukken met het pijltje naar links. ";

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
                        UItext.text = "Heel goed. Laten we tot slot de tablet weer sluiten door op de knop met het kruisje te drukken. ";
                        expectedRobotUIstate = false;
                    }
                    break;
                case TutorialStep.CloseIpad:
                    if (player.tutorial_robotUI_closed)
                    {
                        currentStep = TutorialStep.Done;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("/Player/CinematicControl/Arms/Armature/Hips/Spine/Spine1/Spine2/Neck/Head/Camera/Canvas/SceneLoader 1", new Vector3(123.80f, -69.10f, 0.00f));
                        UItext.text = "Gefeliciteerd! Je hebt nu alles geleerd over hoe je theorie kunt opzoeken binnen Care Up. Veel plezier met het oefenen van de verpleegtechnische handelingen!";
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
