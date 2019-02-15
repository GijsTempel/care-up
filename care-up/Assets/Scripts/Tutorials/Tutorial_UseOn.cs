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
        ExplainDeCombine,
        DeCombine,
        OpenOptions3,
        ExplainActions,
        VentInsulin,
        RemoveNeedle,
        OpenActions4,
        ExplainUseOn,
        ClickPlus,
        ClickContainer,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;


    public bool decombiningAllowed = false;
    public bool ventAllowed = false;

    protected override void Update () {
        base.Update ();

        if (!Paused ()) {

            audioSource = GetComponent<AudioSource> ();

            switch (currentStep) {
                case TutorialStep.First:
                audioSource.PlayOneShot (Popup, 0.1F);
                decombiningAllowed = false;
                audioSource.PlayOneShot (Robot1, 0.1F);
                currentStep = TutorialStep.Welcome;
                hintsN.SetSize (496f, 422f);
                hintsN.SetIconPosition (1);
                hintsN.LockTo ("robot", new Vector3 (-380.15f, -6.04f, 188.40f));
                UItext.DOText ("In deze training leer je hoe je het optie menu kunt gebruiken. ", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                SetUpTutorialNextButton ();

                break;
                case TutorialStep.Welcome:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    hintsN.SetSize (483.8f, 230f);
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
                    hintsN.LockTo("InsulinPen", new Vector3(0.00f, 0.13f, -0.06f));
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
                    hintsN.SetSize (632.5f, 360f);
                    hintsN.LockTo("robot", new Vector3(0.63f, -0.63f, -0.33f));
                    hintsN.SetIconPosition (1);
                    UItext.DOText ("Ieder object binnen Care Up heeft een optie menu. Met het optie menu kun bepaalde acties uitvoeren.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                    
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
                        hintsN.LockTo("InsulinPen", new Vector3(0.00f, 0.00f, -0.06f));
                        hintsN.SetIconPosition (0);
                    UItext.DOText ("Open het optie menu door op de insuline pen te klikken", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);
                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("InsulinPen").transform.position;
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
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.31f, -0.02f));
                        hintsN.SetIconPosition (1);
                    UItext.DOText ("Klikken op een object in je hand opent het optie menu.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

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
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.31f, -0.02f));
                        hintsN.SetIconPosition(1);
                        UItext.DOText ("Probeer de insuline pen te controleren door op het vergrootglas icoon te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    examined = false;
                }
                break;
                case TutorialStep.Inspect:
                if (examined) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (600f, 400f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.CloseInspect;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("SceneLoader 1", new Vector3(-295.60f, 155.50f, 0.00f));
                        UItext.DOText ("Heel goed! Objecten die gecontroleerd moeten worden hebben altijd een vergrootglas icoon in het optie menu. Sluit de controle door ergens op het scherm te klikken of door op de 'Sluiten' knop te drukken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

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
                        hintsN.SetIconPosition(2);
                        hintsN.LockTo("robot", new Vector3(0.00f, 0.34f, 0.33f));
                        UItext.DOText ("Pak de naald van het werkveld door erop te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

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

                    hintsN.SetIconPosition(1);
                        hintsN.LockTo("InsulinOpenedNeedlePackage", new Vector3(0.00f, -0.12f, -0.09f));
                        UItext.DOText ("Open nu het optie menu van de naald door op de naald te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    particleHint.SetActive (false);

                    player.tutorial_itemControls = false;
                    player.itemControlsToInit = "InsulinOpenedNeedlePackage";
                }
                break;
                case TutorialStep.OpenOptions2:
                if (player.tutorial_itemControls) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (600f, 450.7f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.ExplainCombine;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.17f, 0.22f));
                        decombiningAllowed = false;
                    UItext.DOText ("Naast swipen, kun je objecten ook combineren door op het + icoon te klikken. Vervolgens klik je op het andere object in je hand.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
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
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.30f, 0.14f));
                        hintsN.SetIconPosition(1);
                     UItext.DOText ("Combineer nu de naald met de insuline pen door op het + icoon te selecteren en vervolgens op de insulinepen te klikken", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    currentStep = TutorialStep.Combine;
                    handsInventory.tutorial_combined = false;
                    decombiningAllowed = true;
                }
                break;
                case TutorialStep.Combine:
                if (handsInventory.tutorial_combined) {
                    decombiningAllowed = false;
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (515.8f, 290.6f);
                    currentStep = TutorialStep.ExplainDeCombine;
                    handsInventory.tutorial_itemUsedOn = false;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.35f, 0.04f));
                        UItext.DOText ("Keurig! Open het optie menu van de insuline pen door op de insuline pen te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    player.tutorial_itemControls = false;
                    player.itemControlsToInit = "InsulinPenWithNeedlePackage";
                }
                break;
                case TutorialStep.ExplainDeCombine:
                if (player.tutorial_itemControls) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (506.8f, 277.6f);
                    handsInventory.tutorial_itemUsedOn = false;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.35f, 0.04f));
                        hintsN.SetIconPosition(1);
                    UItext.DOText ("Naast omhoog/omlaag swipen, kun je objecten ook scheiden door op het - icoon te klikken, Probeer dit nu.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    currentStep = TutorialStep.DeCombine;
                    handsInventory.tutorial_combined = false;
                    decombiningAllowed = true;
                }
                break;
                case TutorialStep.DeCombine:
                if (handsInventory.tutorial_combined) {
                    decombiningAllowed = false;
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (506.8f, 277.6f);
                    currentStep = TutorialStep.OpenOptions3;
                    handsInventory.tutorial_itemUsedOn = false;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.35f, 0.04f));
                        UItext.DOText ("Open nogmaals het optie menu door op de insuline pen te klikken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    player.tutorial_itemControls = false;
                    player.itemControlsToInit = "InsulinPenWithNeedle";
                }
                break;
                case TutorialStep.OpenOptions3:
                if (player.tutorial_itemControls) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (550.5f, 450.7f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.ExplainActions;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.33f, 0.05f));
                        hintsN.SetIconPosition(1);
                        UItext.DOText ("Sommige handelingen bevatten speciale acties. Deze speciale acties worden in het optie menu getoond wanneer je de actie dient uit te voeren.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.ExplainActions:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (550.5f, 450.7f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.VentInsulin;
                        hintsN.SetIconPosition(1);
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.33f, 0.05f));
                        UItext.DOText ("Ontlucht nu de insuline pen door 'Ontluchten' te selecteren in het optie menu.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    handsInventory.LeftHandObject.GetComponent<PickableObject> ().tutorial_usedOn = false;
                    ventAllowed = true;
                }
                break;
                case TutorialStep.VentInsulin:
                if (handsInventory.LeftHandObject.GetComponent<PickableObject> ().tutorial_usedOn) {
                    handsInventory.LeftHandObject.GetComponent<PickableObject> ().tutorial_usedOn = false;
                    ventAllowed = false;
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (550.5f, 450.7f);
                    handsInventory.tutorial_itemUsedOn = false;
                    hintsN.LockTo("robot", new Vector3(0.00f, -0.33f, 0.13f));
                    UItext.DOText ("Probeer nu de naald van de insuline spuit te halen door te swipen of door het optie menu te gebruiken.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    handsInventory.ReplaceHandObject (true, "InsulinPenWithNeedle");

                    currentStep = TutorialStep.RemoveNeedle;
                    handsInventory.tutorial_combined = false;
                    decombiningAllowed = true;
                }
                break;
                case TutorialStep.RemoveNeedle:
                if (handsInventory.tutorial_combined) {
                    decombiningAllowed = false;
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                        hintsN.SetSize(550.5f, 150f);
                        handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.OpenActions4;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.33f, 0.13f));
                        UItext.DOText ("Open het optie menu van de naald.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    player.tutorial_itemControls = false;
                    player.itemControlsToInit = "InsulinNeedle";
                }
                break;
                case TutorialStep.OpenActions4:
                if (player.tutorial_itemControls) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (700f, 600f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.ExplainUseOn;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.33f, 0.13f));
                        UItext.DOText ("Naast het combineren kan het + icoon kan ook worden gebruikt om objecten met elkaar te gebruiken. Denk hierbij aan het weggooien van de naald in de naalden container of het gebruiken van de spuit op een cliënt om het injecteren te starten.", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    handsInventory.RightHandObject.GetComponent<PickableObject> ().depoistNeedle = true;

                    SetUpTutorialNextButton ();
                }
                break;
                case TutorialStep.ExplainUseOn:
                if (nextButtonClicked) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    hintsN.SetSize (550.5f, 150f);
                    handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.ClickPlus;
                        hintsN.LockTo("robot", new Vector3(0.00f, -0.37f, 0.13f));
                        UItext.DOText ("Klik nu op het + icoon in het optie menu", 1, true, ScrambleMode.All).SetEase (Ease.Linear);

                    player.tutorial_UseOnControl = false;
                }
                break;
                case TutorialStep.ClickPlus:
                if (player.tutorial_UseOnControl) {
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Robot3, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                        hintsN.SetSize(550.5f, 150f);
                        handsInventory.tutorial_itemUsedOn = false;
                    currentStep = TutorialStep.ClickContainer;
                        hintsN.SetIconPosition(0);
                        hintsN.LockTo("NeedleCup", new Vector3(0.00f, 0.28f, -0.17f));
                        UItext.DOText ("Klik vervolgens op de naalden container", 1, true, ScrambleMode.All).SetEase (Ease.Linear);
                    handsInventory.RightHandObject.GetComponent<PickableObject> ().depoistNeedle = false;

                    handsInventory.tutorial_itemUsedOn = false;
                }
                break;
                case TutorialStep.ClickContainer:
                if (handsInventory.tutorial_itemUsedOn) {
                    handsInventory.RightHandObject.GetComponent<PickableObject> ().depoistNeedle = true;
                    handsInventory.tutorial_combined = false;
                    audioSource.PlayOneShot (Popup, 0.1F);
                    audioSource.PlayOneShot (Done, 0.1F);
                    audioSource.PlayOneShot (RobotShort1, 0.1F);
                    handsInventory.tutorial_combined = false;
                    currentStep = TutorialStep.Done;
                    hintsN.LockTo ("SceneLoader 1", new Vector3 (262.50f, -69.10f, 0.00f));
                    UItext.DOText ("Goed gedaan! Dit was de leermodule over het optie menu.", 0.5f, true, ScrambleMode.All).SetEase (Ease.Linear);

                    SetPauseTimer (2.0f);
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
