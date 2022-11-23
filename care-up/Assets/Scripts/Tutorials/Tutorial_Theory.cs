using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Theory : TutorialManager
{

    public AudioClip Popup;
    AudioSource audioSource;

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
    //private PDFFullScreenViewer pdfViewer;

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
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.OpenRobotUI;
                        hintsN.ResetSize();
						hintsN.LockTo("RobotUITrigger", new Vector3(53.42f, -43.70f, 0.00f));
                        UItext.text = "Het icon met de tablet kun je aanklikken om je tablet te openen. Op de tablet kun je veel informatie vinden. Probeer de tablet erbij te pakken door op het icoon te klikken.";
                        expectedRobotUIstate = true;
                    }
                    break;
                case TutorialStep.OpenRobotUI:
                    if (player.tutorial_robotUI_opened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PressTheory;
						hintsN.LockTo("RobotUI", new Vector3(-997.40f, -430.00f, -99.80f));
                        hintsN.SetIconPosition(3);
                        UItext.text = "Laten we de 'Theorie' app openen door op het icoon te klikken.";
                        RobotUITabs.tutorial_infoTabOpened = false;
                        tabToOpen = "InfoTab";
                    }
                    break;
                case TutorialStep.PressTheory:
                    if (RobotUITabs.tutorial_infoTabOpened)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.ChangePDF;
                        hintsN.SetSize(788f, 524.9f);

						hintsN.LockTo("Stopwatch", new Vector3(3396.70f, -818.80f, 0.00f));

                        hintsN.SetIconPosition(1);
                        UItext.text = "In de app 'Theorie' vindt je de theorie die hoort bij het protocol die je aan het oefenen bent. Theorie kun je op ieder moment openen en bekijken.";
                        SetUpTutorialNextButton();
                    }
                    break;
                /*  case TutorialStep.TheoryExpl:
                      if (nextButtonClicked)
                      {
                          hintsN.ResetSize();
                          hintsN.SetIconPosition(0);
                          hintsN.LockTo("Stopwatch", new Vector3(438.90f, -531.30f, 0.00f));
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
                      break;*/
                case TutorialStep.ChangePDF:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);

                        /*infoTab.tutorial_changedPDF = false;
                        hintsN.SetIconPosition(1);
                        hintsN.LockTo("FullScreenPDFButton", new Vector3(-295.90f, -356.40f, 0.00f));
                        currentStep = TutorialStep.FullScrOn;
                        UItext.text = "Je kunt overschakelen naar een volledige scherm weergave om de theorie beter te kunnen lezen. Probeer dit nu door op de knop met de twee pijlen te drukken.";

                        infoTab.tutorial_fullscreen = false;*/
                        currentStep = TutorialStep.TheoryBack;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("RobotUI", new Vector3(-1631.30f, 936.80f, -99.80f));
                        UItext.text = "Dat was alles over de theorie app. Laten we de apps sluiten door op de knop te drukken met het pijltje naar links. ";

                        closeTab = true;
                    }
                    break;
                /*case TutorialStep.FullScrOn:
                    if (infoTab.tutorial_fullscreen)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        infoTab.tutorial_fullscreen = false;
                        hintsN.LockTo("ExitFromFullScreen", new Vector3(-83.40f, -61.70f, 0.00f));
                        currentStep = TutorialStep.FullScrOff;
                        UItext.text = "Volledige scherm weergave kun je afsluiten door op de knop met het kruisje te drukken. Probeer dit nu.";

                        pdfViewer = FindObjectOfType<PDFFullScreenViewer>();
                        pdfViewer.tutorial_closedFullScrPDF = false;
                    }
                    break;
                case TutorialStep.FullScrOff:
                    if (pdfViewer.tutorial_closedFullScrPDF)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.TheoryBack;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("RobotUI", new Vector3(-1631.30f, 936.80f, -99.80f));
                        UItext.text = "Dat was alles over de theorie app. Laten we de apps sluiten door op de knop te drukken met het pijltje naar links. ";

                        closeTab = true;
                    }
                    break; */
                case TutorialStep.TheoryBack:
                    if (RobotUITabs.tutorial_back)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        closeTab = RobotUITabs.tutorial_back = false;
                        currentStep = TutorialStep.CloseIpad;
                        hintsN.LockTo("RobotUI", new Vector3(1921.00f, 1116.00f, 0.00f));
                        hintsN.SetIconPosition(1);
                        UItext.text = "Heel goed. Laten we tot slot de tablet weer sluiten door op de knop met het kruisje te drukken. ";
                        expectedRobotUIstate = false;
                    }
                    break;
                case TutorialStep.CloseIpad:
                    if (player.tutorial_robotUI_closed)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        hintsN.ResetSize();
                        currentStep = TutorialStep.Done;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
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
