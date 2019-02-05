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
                hintsN.SetSize (445.2f, 400f);
                    hintsN.LockTo("robot", new Vector3(722.70f, -28.60f, -340.70f));
                    hintsN.SetIconPosition(1);
                    UItext.DOText("In deze training ga ik je leren hoe je objecten in je handen kunt combineren en uit elkaar kunt halen.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                SetUpTutorialNextButton ();


                break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.MoveTo;
                        hintsN.SetSize(445.2f, 225.1f);
                        hintsN.LockTo("WorkField", new Vector3(0.00f, 0.51f, -0.75f));
                        UItext.DOText("We beginnen door te bewegen naar het werkveld. Klik op het werkveld. ", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        hintsN.SetIconPosition(0);
                        player.tutorial_movedTo = false;
                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("WorkField").transform.position;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot1, 0.1F);
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
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        currentStep = TutorialStep.PickTwo;
                        UItext.DOText("Heel goed! Pak nu ook de naald op door erop te klikken.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
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
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "";

                        hintsN.LockTo("robot", new Vector3(-1.22f, 0.00f, 1.13f)); hintsN.LockTo("robot", new Vector3(-1.22f, 0.00f, 1.13f));
                        UItext.DOText("Swipe nu om de naald en spuit te combineren.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        particleHint.SetActive(false);

                        currentStep = TutorialStep.Combine;
                        handsInventory.tutorial_combined = false;
                        decombiningAllowed = true;
                    }
                    break;
                case TutorialStep.Combine:
                    if (handsInventory.tutorial_combined)
                    {
                        decombiningAllowed = false;
                        handsInventory.tutorial_combined = false;
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);

                        hintsN.LockTo("robot", new Vector3(-1.22f, 0.00f, 1.13f)); hintsN.LockTo("robot", new Vector3(-1.22f, 0.00f, 1.13f));
                        UItext.DOText("Swipe vertically", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        particleHint.SetActive(false);
                        hintsN.SetIconPosition(0);
                        hintsN.SetSize(495.7f, 266.6f);

                        currentStep = TutorialStep.Decombine;
                        handsInventory.tutorial_combined = false;
                        decombiningAllowed = true;
                    }
                    break;
                case TutorialStep.Decombine:
                    if (handsInventory.tutorial_combined)
                    {
                        decombiningAllowed = false;
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Done, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.Done;       
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        UItext.DOText("Goed gedaan! Dit was de leermodule over het combineren en scheiden van objecten.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

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

    public void OnTutorialButtonClick_UsingOn()
    {
        string sceneName = "Tutorial_UseOn";
        string bundleName = "tutorial_useon";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }
}
