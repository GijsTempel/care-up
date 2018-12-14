using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial_Combining : TutorialManager {

    public AudioClip Popup;
    public AudioClip Done;
    public AudioClip Robot1;
    public AudioClip Robot2;
    public AudioClip Robot3;
    public AudioClip RobotShort1;
    public AudioClip RobotShort2;

    AudioSource audioSource;

    public enum TutorialStep {
        First,
        Welcome,
        MoveTo,
        PickOne,
        PickTwo,
        OpenControls,
        ClickUseOn,
        Combine,
        Explanaiton,
        OpenControls2,
        Decombine,
        Done,
        None
    }

    TutorialStep currentStep = TutorialStep.First;



    public bool decombiningAllowed = false;

    protected override void Update () {
        base.Update ();

        if (!Paused ()) {
            audioSource = GetComponent<AudioSource> ();

            switch (currentStep) {

                case TutorialStep.First:
                audioSource.PlayOneShot (Popup, 0.1F);
                audioSource.PlayOneShot(Robot1, 0.1F);
                currentStep = TutorialStep.Welcome;
                hintsN.SetSize (788f, 524.9f);
                hintsN.LockTo ("UI(Clone)", new Vector3 (-393.80f, 215f, 0.00f));
                UItext.DOText("In deze les ga je leren hoe je objecten in je handen kunt combineren en uit elkaar kunt halen.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                SetUpTutorialNextButton ();


                break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.MoveTo;
						hintsN.ResetSize();
						hintsN.LockTo("WorkField", new Vector3(0.00f, 0.51f, -0.75f));
                        UItext.DOText("We beginnen door te bewegen naar het werkveld. Klik op het werkveld. ", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.PickOne;
						hintsN.SetIconPosition(3);
						hintsN.LockTo("Syringe", new Vector3(0.00f, 0.11f, -0.10f));
                        UItext.DOText("Je ziet een spuit en een naald liggen. Pak de spuit door erop te klikken. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        handsInventory.tutorial_pickedLeft = false;

                        itemToPick = "Syringe";

                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("Syringe").transform.position;
                    }
                    break;
                case TutorialStep.PickOne:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        handsInventory.tutorial_pickedLeft = false;
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PickTwo;
                        UItext.DOText("Heel goed! Pak nu ook de naald op door erop te klikken.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        hintsN.LockTo("AbsorptionNeedle", new Vector3(0.00f, 0.00f, -0.08f));

                        handsInventory.tutorial_pickedRight = false;

                        itemToPick = "AbsorptionNeedle";
                        
                        particleHint.transform.position = GameObject.Find("AbsorptionNeedle").transform.position;
                    }
                    break;
                case TutorialStep.PickTwo:
                    if (handsInventory.tutorial_pickedRight)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "";

                        particleHint.SetActive(false);

                        //hintsBox.anchoredPosition = new Vector2(-67f, 37f);
						hintsN.LockTo("Syringe", new Vector3(0.00f, -0.03f, -0.03f));
						hintsN.SetIconPosition(0);

                        currentStep = TutorialStep.OpenControls;
                        UItext.DOText("Klik op de spuit om te zien welke opties je hebt. Je kunt op elk object klikken om de opties weer te geven. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_itemControls = false;
                        player.itemControlsToInit = "Syringe";
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        hintsN.LockTo("/ItemControls/ItemControlsGroup/UseOnButton", new Vector3(20.10f, -116.00f, 0.00f));
                        UItext.DOText("Klik op het +icoon. Dit icoon zorgt ervoor dat je objecten kunt combineren.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        player.tutorial_UseOnControl = false;

                        currentStep = TutorialStep.Combine;
                        //hintsBox.anchoredPosition = new Vector2(746f, -149.45f);
                        hintsN.LockTo("/Player/CinematicControl/Arms/Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/toolHolder.R/AbsorptionNeedle", new Vector3(-0.12f, -0.02f, 0.00f));
                        hintsN.SetIconPosition(1);
                        UItext.text = "Klik nu op het object in je andere hand om de objecten met elkaar te combineren.";
                        UItext.DOText("Goed dat je er weer bent. Ik zal je uitleggen hoe je objecten kunt oppakken en terugleggen. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Combine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.OpenControls2;
						hintsN.LockTo("SyringeWithAbsorptionNeedleCap", new Vector3(0.00f, 0.00f, -0.06f));
                        UItext.DOText("Haal de dop van de naald door op de spuit met opzuignaald te klikken. Hierdoor zie je weer de opties. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        player.tutorial_itemControls = false;
                        player.itemControlsToInit = "SyringeWithAbsorptionNeedleCap";
                    }
                    break;
                case TutorialStep.OpenControls2:
                    if (player.tutorial_itemControls)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.Decombine;
                        hintsN.LockTo("/ItemControls/ItemControlsGroup/CombineButton", new Vector3(35.60f, -106.60f, 0.00f));
                        UItext.DOText("Klik op het –icoon. Met dit icoon haal je de objecten uit elkaar of open je een object. Bijvoorbeeld een verpakking. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        handsInventory.tutorial_combined = false;
                        decombiningAllowed = true;
                    }
                    break;
                case TutorialStep.Decombine:
                    if (handsInventory.tutorial_combined)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Done, 0.1F);
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.Done;       
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        UItext.text = "Goed gedaan! Dit was de leermodule over het combineren en scheiden van objecten.";

                        SetPauseTimer(5.0f);
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
