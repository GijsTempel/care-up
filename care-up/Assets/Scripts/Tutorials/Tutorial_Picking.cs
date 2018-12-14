using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Picking : TutorialManager {

    public AudioClip Popup;
    AudioSource audioSource;

    public enum TutorialStep
    {
        First,
        Welcome,
        MoveTo,
        PickOne,
        PickTwo,
        OpenDropOne,
        DropOne,
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
                    currentStep = TutorialStep.Welcome;
                    hintsN.SetIconPosition(1);
                    hintsN.SetSize(456f, 393f);
                    hintsN.LockTo("robot", new Vector3(275.14f, 19.20f, -119.20f));
                    UItext.text = "Goed dat je er weer bent. Ik zal je uitleggen hoe je objecten kunt oppakken en terugleggen. ";
                    SetUpTutorialNextButton();

                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.MoveTo;
                        hintsN.SetIconPosition(3);
                        hintsN.SetSize(452f, 201.5f);
                        hintsN.LockTo("WorkField", new Vector3(1.10f, 1.06f, -0.01f));
                        UItext.text = "Laten we beginnen. Beweeg naar het werkveld door op het werkveld te klikken. ";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        currentStep = TutorialStep.PickOne;
						hintsN.LockTo("ClothPackage", new Vector3(0.00f, 0.14f, -0.13f));
						hintsN.SetIconPosition(3);
                        UItext.text = "Objecten oppakken kun je doen door erop te klikken. Probeer een gaasje uit de doos te pakken door op de doos te klikken. ";

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
                        handsInventory.tutorial_pickedLeft = false;
                        currentStep = TutorialStep.PickTwo;
						hintsN.LockTo("Medicine", new Vector3(0.00f, 0.16f, 0.00f));
                        UItext.text = "Het object welke je als eerste pakt verschijnt altijd in je linkerhand. Probeer nu het medicijn op te pakken door erop te klikken. ";
                        itemToPick = "Medicine";
                        handsInventory.tutorial_pickedRight = false;

                        particleHint.transform.position = GameObject.Find("Medicine").transform.position;
                    }
                    break;
                case TutorialStep.PickTwo:
                    if (handsInventory.tutorial_pickedRight)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "";

                        currentStep = TutorialStep.DropOne;
                        hintsN.LockTo("/Cloth", new Vector3(0.00f, 0.16f, 0.00f));
                        UItext.text = "Objecten kun je terug leggen door op het silhouette  te klikken. Klik nu op het silhouette van het gaasje om deze terug te leggen. ";

                        itemToDrop = "Cloth";
                        handsInventory.tutorial_droppedLeft = false;
                        
                        particleHint.transform.position = GameObject.Find("Cloth").transform.position;
                    }
                    break;
                case TutorialStep.DropOne:
                    if (handsInventory.tutorial_droppedLeft)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        handsInventory.tutorial_droppedLeft = false;
                        hintsN.LockTo("/Medicine", new Vector3(0.00f, 0.16f, 0.00f));
                        currentStep = TutorialStep.DropTwo;
                        UItext.text = "Klik nu op het silhouette van het medicijn om deze ook terug te leggen.  ";

                        itemToDrop = "Medicine";
                        handsInventory.tutorial_droppedRight = false;
                        particleHint.transform.position = GameObject.Find("Medicine").transform.position;
                    }
                    break;
                case TutorialStep.DropTwo:
                    if (handsInventory.tutorial_droppedRight)
                    {
                        audioSource.PlayOneShot (Popup, 0.1F);
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        hintsN.SetIconPosition(0);
                        handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.Done;
                        particleHint.SetActive(true);
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
}
