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
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetSize(481.4f, 455.7f);
                    hintsN.LockTo("robot", new Vector3(0.00f, -0.33f, 1.84f));
                    UItext.DOText("In deze training leer je hoe je gecombineerde objecten kunt gebruiken. Denk hierbij aan het weggooien van een naald in de naaldcontainer of het gebruiken van een injectiespuit op je cliënt.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                    SetUpTutorialNextButton();

                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);

                        hintsN.ResetSize();
						hintsN.LockTo("WorkField", new Vector3(0.00f, 0.51f, -0.73f));
                        currentStep = TutorialStep.MoveTo;
                        UItext.DOText("We beginnen door te bewegen naar het werkveld. Klik op het werkveld.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        player.tutorial_movedTo = false;

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
                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "";

                        particleHint.SetActive(false);

                        currentStep = TutorialStep.OpenControls;
						hintsN.LockTo("AbsorptionNeedleNoCap", new Vector3(0.00f, 0.00f, 0.00f));
						hintsN.SetIconPosition(0);
                        UItext.DOText("Klik op de opzuignaald om de opties te tonen. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_itemControls = false;
                        player.itemControlsToInit = "AbsorptionNeedleNoCap";
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        hintsN.LockTo("UseOnButton", new Vector3(45.10f, -274.50f, 0.00f));
                        hintsN.SetIconPosition(3);
                        UItext.DOText("Klik op het +icoon om de objecten met elkaar te combineren.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        player.tutorial_UseOnControl = false;
                        
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
}
