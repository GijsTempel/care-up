using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial_UseOn : TutorialManager {

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
        PickNeedle,
        OpenControls,
        ClickUseOn,
        UseOn,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

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
                    hintsN.SetSize(632.5f, 521.7f);
                    hintsN.SetIconPosition(1);
                    hintsN.LockTo("robot", new Vector3(-380.15f, -6.04f, 188.40f));
                    UItext.DOText("In deze training leer je hoe je gecombineerde objecten kunt gebruiken. Denk hierbij aan het weggooien van een naald in de naaldcontainer of het gebruiken van een injectiespuit op je cliënt.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                    SetUpTutorialNextButton();

                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        hintsN.SetSize(483.8f, 210f);
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("WorkField", new Vector3(2.23f, 0.59f, -1.30f));
                        currentStep = TutorialStep.MoveTo;
                        UItext.DOText("We beginnen door te bewegen naar het werkveld. Klik op het werkveld.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        player.tutorial_movedTo = false;
                        hintsN.SetSize(506.8f, 175f);
                        currentStep = TutorialStep.PickNeedle;
						hintsN.LockTo("AbsorptionNeedleNoCap", new Vector3(0.00f, 0.06f, -0.06f));
						hintsN.SetIconPosition(3);
                        UItext.DOText("Pak de gebruikte opzuignaald door erop te klikken. ", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        handsInventory.tutorial_pickedLeft = false;
                        
                        itemToPick = "AbsorptionNeedleNoCap";

                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("AbsorptionNeedleNoCap").transform.position;
                    }
                    break;
                case TutorialStep.PickNeedle:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "";

                        particleHint.SetActive(false);

                        currentStep = TutorialStep.OpenControls;
						hintsN.LockTo("AbsorptionNeedleNoCap", new Vector3(0.00f, 0.00f, 0.00f));
						hintsN.SetIconPosition(0);
                        UItext.DOText("Klik op de opzuignaald om de opties te tonen. ", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_itemControls = false;
                        player.itemControlsToInit = "AbsorptionNeedleNoCap";
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        hintsN.LockTo("UseOnButton", new Vector3(-2.70f, -124.70f, 0.00f));
                        hintsN.SetIconPosition(0);
                        UItext.DOText("Klik op het +icoon om de objecten met elkaar te combineren.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        player.tutorial_UseOnControl = false;
                        hintsN.SetSize(506.8f, 250f);
                        currentStep = TutorialStep.UseOn;
						hintsN.LockTo("NeedleCup", new Vector3(0.00f, 0.00f, -0.15f));
						hintsN.SetIconPosition(0);
                        UItext.DOText("Klik nu op het object waarmee je de naald wilt gebruiken. In dit geval klik je op de naaldcontainer. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        
                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOn:
                    if (handsInventory.tutorial_itemUsedOn)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);
                        audioSource.PlayOneShot(Done, 0.1F);
                        hintsN.SetSize(506.8f, 277.6f);
                        handsInventory.tutorial_itemUsedOn = false;
                        currentStep = TutorialStep.Done;
						hintsN.LockTo("SceneLoader 1", new Vector3(363.61f, -22.40f, 0.00f));
                        UItext.DOText("Super! Dit was de leermodule over het gebruiken van een object in je hand met een ander object zoals de naaldcontainer.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        SetPauseTimer(4.0f);
                    }
                    break;
                case TutorialStep.Done:
                    if (!Paused())
                    {
                        currentStep = TutorialStep.None;
                        TutorialEnd();
                    }
                    break;
            }
        }
    }

    public void OnTutorialButtonClick_PersonDialogues()
    {
        string sceneName = "Tutorial_Talk";
        string bundleName = "tutorial_talking";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }
}
