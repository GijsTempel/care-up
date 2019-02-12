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

    [HideInInspector]
    public bool examined = false;
    [HideInInspector]
    public bool stopExamined = false;

    public enum TutorialStep {
        First,
        Welcome,
        MoveTo,
        Pickinsulin,
        ExplainOptions,
        OpenOptions,
        ExplainInspecting,
        Inspect,
        CloseInspect,
        PickNeedle,
        OpenOptions2,
        ExplainCombine,
        Combine,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    public bool decombiningAllowed = false;

    protected override void Update () {
        base.Update ();

        if (!Paused ()) {

            audioSource = GetComponent<AudioSource> ();

            switch (currentStep) {
                case TutorialStep.First:
                audioSource.PlayOneShot (Popup, 0.1F);
                audioSource.PlayOneShot (Robot1, 0.1F);
                currentStep = TutorialStep.Welcome;
                hintsN.SetSize (632.5f, 521.7f);
                hintsN.SetIconPosition (1);
                hintsN.LockTo ("robot", new Vector3 (-380.15f, -6.04f, 188.40f));
                UItext.DOText ("In deze training leer je hoe je gecombineerde objecten kunt gebruiken. Denk hierbij aan het weggooien van een naald in de naaldcontainer of het gebruiken van een injectiespuit op je cliënt.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                SetUpTutorialNextButton ();

                break;
                case TutorialStep.Welcome:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    hintsN.SetSize (483.8f, 210f);
                    hintsN.SetIconPosition (0);
                    hintsN.LockTo ("WorkField", new Vector3 (2.23f, 0.59f, -1.30f));
                    currentStep = TutorialStep.MoveTo;
                    UItext.DOText ("We beginnen door te bewegen naar het werkveld. Klik op het werkveld.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    player.tutorial_movedTo = false;
                }
                break;
                case TutorialStep.MoveTo:
                if (player.tutorial_movedTo) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    player.tutorial_movedTo = false;
                    hintsN.SetSize (506.8f, 175f);
                    currentStep = TutorialStep.Pickinsulin;
                    hintsN.LockTo ("InsulinPen", new Vector3 (0.00f, 0.06f, -0.06f));
                    hintsN.SetIconPosition (3);
                    UItext.DOText ("Pak de insuline pen op door erop te klikken. ", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    handsInventory.tutorial_pickedLeft = false;

                    itemToPick = "InsulinPen";

                    particleHint.SetActive (true);
                    particleHint.transform.position = GameObject.Find ("InsulinPen").transform.position;
                }
                break;
                case TutorialStep.Pickinsulin:
                if (handsInventory.tutorial_pickedLeft) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort2, 0.1F);
                    handsInventory.tutorial_pickedLeft = false;
                    itemToPick = "";

                    particleHint.SetActive (false);

                    currentStep = TutorialStep.ExplainOptions;
                    hintsN.SetSize (632.5f, 660.7f);
                    hintsN.LockTo ("InsulinPen", new Vector3 (0.00f, 0.00f, 0.00f));
                    hintsN.SetIconPosition (0);
                    UItext.DOText ("Je kan in Care Up een optie menu openen. In het optie menu kan je door op het + icoon te klikken objecten in je handen combineren en door op het - icoon te klikken kan je objecten uit elkaar houden. Met het vergrootglas kan je het object inspecteren. En speciale handelingen zoals ontluchten kan je ook via dit optie menu doen.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.ExplainOptions:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    player.tutorial_itemControls = false;

                    currentStep = TutorialStep.OpenOptions;
                    hintsN.SetSize (506.8f, 175f);
                    hintsN.LockTo ("UseOnButton", new Vector3 (-2.70f, -124.70f, 0.00f));
                    hintsN.SetIconPosition (0);
                    UItext.DOText ("Open het optie menu door op de insuline pen te klikken", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    player.tutorial_itemControls = false;
                    player.itemControlsToInit = "InsulinPen";
                }
                break;
                case TutorialStep.OpenOptions:
                if (player.tutorial_itemControls) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot2, 0.1F);
                    player.tutorial_UseOnControl = false;
                    hintsN.SetSize (506.8f, 300f);
                    currentStep = TutorialStep.ExplainInspecting;
                    hintsN.LockTo ("NeedleCup", new Vector3 (0.00f, 0.00f, -0.15f));
                    hintsN.SetIconPosition (0);
                    UItext.DOText ("Nu zie je het optie menu en kan je door op het vergrootglas de insuline pen inspecteren.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.ExplainInspecting:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (506.8f, 277.6f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.Inspect;
                    hintsN.LockTo ("NeedleCup", new Vector3 (0.00f, 0.00f, -0.15f));
                    UItext.DOText ("Inspecteer nu de insuline pen.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    examined = false;
                }
                break;
                case TutorialStep.Inspect:
                if (examined) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (506.8f, 277.6f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.CloseInspect;
                    hintsN.LockTo ("SceneLoader 1", new Vector3 (363.61f, -22.40f, 0.00f));
                    UItext.DOText ("Stop nu met inspecteren door ergens op het scherm te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    stopExamined = false;
                }
                break;
                case TutorialStep.CloseInspect:
                if (stopExamined) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (506.8f, 277.6f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.PickNeedle;
                    hintsN.LockTo ("InsulinOpenedNeedlePackage", new Vector3 (363.61f, -22.40f, 0.00f));
                    UItext.DOText ("Pak nu de naald van de tafel door erop te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    handsInventory.tutorial_pickedRight = false;

                    itemToPick = "InsulinOpenedNeedlePackage";

                    particleHint.SetActive (true);
                    particleHint.transform.position = GameObject.Find ("InsulinOpenedNeedlePackage").transform.position;
                }
                break;
                case TutorialStep.PickNeedle:
                if (handsInventory.tutorial_pickedRight) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (506.8f, 277.6f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.OpenOptions2;
                    hintsN.LockTo ("SceneLoader 1", new Vector3 (363.61f, -22.40f, 0.00f));
                    UItext.DOText ("Open nu de opties van de naald door erop te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    player.tutorial_itemControls = false;
                    player.itemControlsToInit = "InsulinOpenedNeedlePackage";
                }
                break;
                case TutorialStep.OpenOptions2:
                if (player.tutorial_itemControls) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (550.5f, 450.7f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.ExplainCombine;
                    hintsN.LockTo ("InsulinPen", new Vector3 (0.00f, 0.00f, 0.00f));
                    UItext.DOText ("Je kan twee objecten met elkaar te combineren door op het + icoon te klikken en dan op het andere object waar me je wilt combineren.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.ExplainCombine:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (506.8f, 277.6f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.Combine;
                    hintsN.LockTo ("SceneLoader 1", new Vector3 (363.61f, -22.40f, 0.00f));
                    UItext.DOText ("Combineer nu de naald met de insuline pen.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    handsInventory.tutorial_combined = false;
                    decombiningAllowed = true;
                }
                break;
                case TutorialStep.Combine:
                if (handsInventory.tutorial_combined) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    handsInventory.tutorial_combined = false;
                    currentStep = TutorialStep.Done;
                    hintsN.LockTo ("SceneLoader 1", new Vector3 (262.50f, -69.10f, 0.00f));
                    UItext.DOText ("Goed gedaan! Dit was de leermodule over het optie menu.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    SetPauseTimer (5.0f);
                }
                break;
                case TutorialStep.Done:
                if (!Paused ()) {
                    currentStep = TutorialStep.None;
                    TutorialEnd ();
                }
                break;
            }
        }
    }

    public void OnTutorialButtonClick_PersonDialogues () {
        string sceneName = "Tutorial_Talk";
        string bundleName = "tutorial_talking";
        bl_SceneLoaderUtils.GetLoader.LoadLevel (sceneName, bundleName);
    }
}
