using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial_Picking : TutorialManager {

    public AudioClip Popup;
    public AudioClip Done;
    public AudioClip Robot1;
    public AudioClip Robot2;
    public AudioClip Robot3;
    public AudioClip RobotShort1;
    public AudioClip RobotShort2;
    AudioSource audioSource;

    [SerializeField]
    private ExaminableObject medicine;

    public enum TutorialStep
    {
        First,
        Welcome,
        MoveTo,
        PickOne,
        PickTwo,
        OpenDropOne,
        DropOne,
        InspectMedicine,
        CloseInspect,
        OpenDropTwo,
        DropTwo,
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
                    hintsN.SetIconPosition(1);
                    hintsN.SetSize(456f, 393f);
                    hintsN.LockTo("robot", new Vector3(275.14f, 19.20f, -119.20f));
                    UItext.DOText("Goed dat je er weer bent. Ik zal je uitleggen hoe je objecten kunt oppakken en terugleggen. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                    SetUpTutorialNextButton();

                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort1, 0.1F);
                        currentStep = TutorialStep.MoveTo;
                        hintsN.SetIconPosition(3);
                        hintsN.SetSize(452f, 250f);
                        hintsN.LockTo("WorkField", new Vector3(1.10f, 1.06f, -0.01f));
                        UItext.DOText("Laten we beginnen. Beweeg naar het werkveld door op het werkveld te klikken.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot2, 0.1F);
                        hintsN.SetSize(452f, 300f);
                        currentStep = TutorialStep.PickOne;
						hintsN.LockTo("ClothPackage", new Vector3(0.00f, 0.14f, -0.13f));
						hintsN.SetIconPosition(3);
                        UItext.DOText("Objecten oppakken kun je doen door erop te klikken. Probeer een gaasje uit de doos te pakken door op de doos te klikken. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);

                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "Cloth";

                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("ClothPackage").transform.position;
                    }
                    break;
                case TutorialStep.PickOne:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot3, 0.1F);
                        hintsN.SetSize(452f, 350f);
                        handsInventory.tutorial_pickedLeft = false;
                        currentStep = TutorialStep.PickTwo;
						hintsN.LockTo("Medicine", new Vector3(0.00f, 0.16f, 0.00f));
                        UItext.DOText("Het object welke je als eerste pakt verschijnt altijd in je linkerhand. Probeer nu het medicijn op te pakken door erop te klikken.", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        itemToPick = "Medicine";
                        handsInventory.tutorial_pickedRight = false;

                        particleHint.transform.position = GameObject.Find("Medicine").transform.position;
                    }
                    break;
                case TutorialStep.PickTwo:
                    if (handsInventory.tutorial_pickedRight)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        audioSource.PlayOneShot(Robot1, 0.1F);
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "";
                        hintsN.SetSize(452f, 325f);
                        currentStep = TutorialStep.DropOne;
                        hintsN.LockTo("Cloth", new Vector3(0.26f, -0.12f, 0.00f));
                        UItext.DOText("Objecten kun je terug leggen door dubbel te klikken op het object. Dubbelklik nu op het gaasje om deze terug te leggen. ", 1, true, ScrambleMode.All).SetEase(Ease.Linear);
                        UItext.text = " ";

                        itemToDrop = "Cloth";
                        handsInventory.tutorial_droppedLeft = false;
                        
                        particleHint.transform.position = GameObject.Find("Cloth").transform.position;
                    }
                    break;
                case TutorialStep.DropOne:
                if (handsInventory.tutorial_droppedLeft) {
                        audioSource.PlayOneShot(Popup, 0.1F);
                        audioSource.PlayOneShot(RobotShort2, 0.1F);
                        hintsN.SetSize(550f, 300f);
                        handsInventory.tutorial_droppedLeft = false;
                        hintsN.SetIconPosition(2);
                        hintsN.LockTo("robot", new Vector3(0.00f, -1.76f, 0.21f));
                        currentStep = TutorialStep.DropTwo;
                        UItext.DOText("Objecten kun je ook terug leggen door op het blauwe silhouette te klikken van het object. Klik op het silhouette van het medicijn om deze terug te leggen.", 0.5f, true, ScrambleMode.All).SetEase(Ease.Linear);
                        itemToDrop = "Medicine";
                        handsInventory.tutorial_droppedRight = false;
                        particleHint.transform.position = GameObject.Find("Medicine").transform.position;
                }
                break;
                case TutorialStep.DropTwo:
                    if (handsInventory.tutorial_droppedRight)
                    {
                        audioSource.PlayOneShot (Done, 0.1F);
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        hintsN.SetIconPosition(0);
                        handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.Done;
                        particleHint.SetActive(false);
                        UItext.text = "Goed gedaan. Nu weet je hoe je objecten kunt oppakken en kunt terugleggen.";
                        itemToDrop = "";
                    }
                    break;
                case TutorialStep.Done:
                    currentStep = TutorialStep.None;
                    TutorialEnd();
                    break;
            }
        }
    }
    public void OnTutorialButtonClick_Combining()
    {
        string sceneName = "Tutorial_Combining";
        string bundleName = "tutorial_combine";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }
}
