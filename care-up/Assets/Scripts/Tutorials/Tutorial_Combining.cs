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
        ExplainHSwipe,
        ExplainHSwipe2,
        ExplainVSwipe,
        ExplainVSwipe2,
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
                        UItext.DOText("We beginnen door te bewegen naar het werkveld. Klik op het werkveld.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
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
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);

                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "";

                        hintsN.SetSize(620f, 430f);
                        hintsN.LockTo("robot", new Vector3(-0.79f, -0.19f, 1.13f));
                        hintsN.SetIconPosition(0);

                        currentStep = TutorialStep.ExplainHSwipe;

                        UItext.DOText("Je kunt objecten met elkaar combineren door naar rechts of links te swipen op je tablet of telefoon.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.ExplainHSwipe:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "";

                        hintsN.SetSize(900f, 480f);
                        hintsN.LockTo("robot", new Vector3(-0.79f, -0.19f, 1.40f));

                        currentStep = TutorialStep.ExplainHSwipe2;
                        UItext.DOText("Op de computer kan dit door op de linkermuisknop te klikken en ingedrukt te houden. Beweeg vervolgens de muis naar links of rechts terwijl je de linkermuisknop ingedrukt houdt.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.ExplainHSwipe2:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);

                        hintsN.SetSize(585f, 315f);
                        hintsN.LockTo("robot", new Vector3(-1.70f, -0.81f, 0.85f));

                        UItext.DOText("Combineer de spuit met de naald door naar links/recht te swipen of door de linkermuisknop ingedrukt te houden en de muis naar links/rechts te bewegen.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
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

                        hintsN.SetSize(759f, 450f);
                        hintsN.LockTo("robot", new Vector3(-0.79f, -0.29f, 1.20f));

                        UItext.DOText("Objecten uit elkaar halen of scheiden, zoals een spuit uit de verpakking halen of de dop van een naald verwijderen, doe je door omhoog te swipen op je tablet of telefoon.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        currentStep = TutorialStep.ExplainVSwipe;
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.ExplainVSwipe:
                    if (nextButtonClicked)
                    {
                        
                        decombiningAllowed = false;
                        handsInventory.tutorial_combined = false;
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);

                        hintsN.SetSize(900f, 480f);
                        hintsN.LockTo("robot", new Vector3(-0.79f, -0.19f, 1.40f));
                        UItext.DOText("Op de computer kan dit door op de linkermuisknop te klikken en ingedrukt te houden. Beweeg vervolgens de muis omhoog terwijl je de linkermuisknop ingedrukt houdt.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                         currentStep = TutorialStep.ExplainVSwipe2;
                         SetUpTutorialNextButton();
                        }
                        break;
                case TutorialStep.ExplainVSwipe2:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);

                        hintsN.SetSize(585f, 345f);
                        hintsN.LockTo("robot", new Vector3(-1.70f, -0.75f, 0.85f));
                        UItext.DOText("Haal de dop van de naald door omhoog/omlaag te swipen of door de linkermuisknop ingedrukt te houden en de muis omhoog te bewegen.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        particleHint.SetActive(false);

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
