using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial_Movement : TutorialManager
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
        PointsExpl,
        MoveToTable,
        MoveBack,
        MoveToDoctor,
        FreeLookExpl,
        MoveWithFreeLook,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    private GameObject wfPos;
    private GameObject docPos;


    protected override void Start()
    {
        base.Start();

        wfPos = GameObject.Find("WorkFieldPos");
        docPos = GameObject.Find("DoctorPos");

        wfPos.SetActive(false);
        docPos.SetActive(false);
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
                    audioSource.PlayOneShot(Robot1, 0.1F);
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetIconPosition(1);
                    hintsN.SetSize(456f, 393f);
                    hintsN.LockTo("robot", new Vector3(275.14f, 19.20f, -119.20f));
                    UItext.DOText("Welkom terug, ik ga je leren hoe je door de omgeving van Care Up kunt bewegen. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                    SetUpTutorialNextButton();
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        currentStep = TutorialStep.PointsExpl;
                        hintsN.SetIconPosition(1);
                        hintsN.SetSize(685f, 486f);
                        hintsN.LockTo("robot", new Vector3(273.20f, 19.20f, -118.50f));
                        UItext.DOText("Binnen Care Up kun je bewegen door te klikken op interessante objecten. Je ontdekt de objecten door er met de muis overheen te bewegen. Speel je op de tablet of telefoon? Dan zijn de namen van de objecten ook weergegeven. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.PointsExpl:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        //hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                        //hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        hintsN.LockTo("WorkField", new Vector3(0.80f, 0.78f, 0.40f));
                        hintsN.ResetSize();
                        currentStep = TutorialStep.MoveToTable;
                        UItext.DOText("Probeer nu naar het werkveld te bewegen. Dit kun je doen door op het werkveld te klikken.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        hintsN.SetIconPosition(2);
                        player.tutorial_movedTo = false;
                        wfPos.SetActive(true);
                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("WorkField").transform.position;
                    }
                    break;
                case TutorialStep.MoveToTable:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);
                        wfPos.SetActive(false);
                        player.tutorial_movedTo = false;
                        hintsN.SetIconPosition(1);
                        //hintsBox.anchoredPosition = new Vector2(681f, 175f);
                        //hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        hintsN.LockTo("MoveBackButton", new Vector3(-59.90f, -160.20f, 0.00f));
                        GameObject.Find("MoveBackButton").GetComponent<Animator>().SetTrigger("BlinkStart");
                        hintsN.SetSize(560f, 425f);
                        hintsN.SetIconPosition(1);
                        currentStep = TutorialStep.MoveBack;
                        UItext.DOText("We zijn naar het werkveld verplaatst. Op ieder moment kun je terugkeren naar de beginpositie. Dit wordt het overzicht genoemd. Wil je terug? Klik dan rechtsboven in op de knop ‘Terug naar overzicht’. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        particleHint.SetActive(false);
                        player.tutorial_movedBack = false;
                    }
                    break;
                case TutorialStep.MoveBack:
                    if (player.tutorial_movedBack)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        player.tutorial_movedBack = false;
                        hintsN.SetSize(560f, 177f);
                        hintsN.SetIconPosition(3);
                        hintsN.LockTo("doc", new Vector3(-34.60f, 0.67f, -20.00f));
                        currentStep = TutorialStep.MoveToDoctor;
                        GameObject.Find("JoystickKnob").GetComponent<Animator>().SetTrigger("BlinkStart");
                        GameObject.Find("JoystickBackground").GetComponent<Animator>().SetTrigger("BlinkStart");
                        UItext.DOText("Heel goed. Probeer nu richting je collega te bewegen door op haar te klikken.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        player.tutorial_movedTo = false;
                        docPos.SetActive(true);
                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("doc").transform.position;
                    }
                    break;
                case TutorialStep.MoveToDoctor:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot1, 0.1F);
                        docPos.SetActive(false);
                        player.tutorial_movedTo = false;
                        hintsN.LockTo("doc", new Vector3(126.70f, 17.26f, 274.22f));
                        hintsN.SetSize(820f, 591f);
                        currentStep = TutorialStep.FreeLookExpl;
                        hintsN.SetIconPosition(0);
                        UItext.DOText("Wil je om je heen kijken? Dit kan door middel van de joystick. Beweeg de joystick in de richting waar je heen wilt kijken en laat los om te stoppen met rondkijken.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.FreeLookExpl:
                    if (nextButtonClicked)
                    {
                        GameObject.Find("JoystickKnob").GetComponent<Animator>().SetTrigger("BlinkStart");
                        GameObject.Find("JoystickBackground").GetComponent<Animator>().SetTrigger("BlinkStart");
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        currentStep = TutorialStep.MoveWithFreeLook;
                        UItext.DOText("Beweeg vanaf je collega direct naar het werkveld zonder terug te keren naar het overzicht.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
						hintsN.ResetSize();
						hintsN.SetIconPosition(1);
                        hintsN.LockTo("doc", new Vector3(0.48f, 2.43f, 1.22f));
                        GameObject.FindObjectOfType<GameUI>().allowObjectControlUI = false;
                        player.tutorial_movedTo = false;
                        wfPos.SetActive(true);
                        particleHint.transform.position = GameObject.Find("WorkField").transform.position;
                    }
                    break;
                case TutorialStep.MoveWithFreeLook:
                    if (player.tutorial_movedTo)
                    {
                        particleHint.SetActive(false);
                        audioSource.PlayOneShot (Done, 0.1F);
                        wfPos.SetActive(false);
                        //hintsBox.anchoredPosition = new Vector2(502f, -346f);
                        //hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
						hintsN.LockTo("UI(Clone)", new Vector3(376.67f, -44.90f, 0.00f));
						hintsN.SetIconPosition(0);
                        currentStep = TutorialStep.Done;
                        UItext.text = "Gefeliciteerd! Je weet nu hoe je kunt rond bewegen binnen Care Up.";
                    }
                    break;
                case TutorialStep.Done:
                    currentStep = TutorialStep.None;
                    TutorialEnd();
                    break;
            }
        }
	}
    public void OnTutorialButtonClick_Picking()
    {
        string sceneName = "Tutorial_Picking";
        string bundleName = "tutorial_pick";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }
}
