using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial_Talk : TutorialManager
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
        MoveTo,
        OpenOptions,
        Talk,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    private InjectionPatient patient;

    protected override void Start()
    {
        base.Start();

        patient = GameObject.FindObjectOfType<InjectionPatient>();
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
                    audioSource.PlayOneShot(RobotShort1, 0.1F);
                    GameObject.FindObjectOfType<InjectionPatient>().allowToTalk = false;
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetIconPosition(1);
                    hintsN.SetSize(366.8f, 415f);
                    hintsN.LockTo("robot", new Vector3(-0.04f, -0.22f, 0.21f));
                    UItext.DOText("In deze training leer je hoe je een gesprek kunt starten. ", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        hintsN.SetSize(452f, 200f);
                        hintsN.LockTo("Patient", new Vector3(2.87f, 0.77f, 0.00f));
                        currentStep = TutorialStep.MoveTo;
                        UItext.DOText("Klik op de cliënt om naar hem toe te lopen.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.OpenOptions;
                        hintsN.LockTo("RightShoulder", new Vector3(0.00f, 0.00f, 0.29f));
                        UItext.DOText("Klik nogmaals op de cliënt om een gesprek te starten", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        hintsN.SetIconPosition(1);
                        patient.tutorial_used = false;
                        GameObject.FindObjectOfType<InjectionPatient>().allowToTalk = true;
                    }
                    break;
                case TutorialStep.OpenOptions:
                    if (patient.tutorial_used)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        patient.tutorial_used = false;
                        hintsN.SetSize(452f, 159f);
                        currentStep = TutorialStep.Talk;

                        hintsN.LockTo("SelectionDialogue(Clone)", new Vector3(35.80f, 59.50f, 0.00f));
                        hintsN.SetIconPosition(1);
                        UItext.DOText("Klik op “goedemorgen” om de cliënt te begroeten.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        patient.tutorial_talked = false;
                    }
                    break;
                case TutorialStep.Talk:
                    if (patient.tutorial_talked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Done, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        patient.tutorial_talked = false;
						hintsN.SetIconPosition(0);
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        currentStep = TutorialStep.Done;
                        UItext.DOText("Goed gedaan. Nu weet je hoe je een gesprek kunt starten.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                    }
                    break;
                case TutorialStep.Done:
                    if (patient.tutorial_greetingEnded)
                    {
                        currentStep = TutorialStep.None;
                        TutorialEnd();
                    }
                    break;
            }
        }
    }
    public void OnTutorialButtonClick_Sequences()
    {
        string sceneName = "Tutorial_Sequence";
        string bundleName = "tutorial_sequences";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

}
