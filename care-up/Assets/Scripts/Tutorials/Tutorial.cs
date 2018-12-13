using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : TutorialManager {

    public AudioClip Popup;
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
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    Cheat_CurrentAction hintsUI;

    public string tabToOpen = "";
    public bool closeTab = false;
    public bool expectedRobotUIstate = false;
    public bool expectedHintsState = false;
    public bool biggerHintOpenend = false;

    protected override void Start () {
        base.Start ();

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
                    UItext.text = "Klik op de vergroot knop.";
                    biggerHintOpenend = true;
                }
                break;
                case TutorialStep.DevHintsMax:
                if (biggerHintOpenend == true) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    currentStep = TutorialStep.DevHintsExplMax;

                    hintsN.SetIconPosition (0);
                    hintsN.LockTo ("BiggerDevHint/Backbutton (1)", new Vector3 (375.39f, -84.72f, 0.00f));
                    UItext.text = "Hier kan je de hints vergroot zien, sluit de hint nu weer af door op het kruisje te klikken";
                }
                break;
                case TutorialStep.CloseHints:
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
                break;
            }
        }
    }
}