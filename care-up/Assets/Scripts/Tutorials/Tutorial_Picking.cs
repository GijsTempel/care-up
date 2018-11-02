using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Picking : TutorialManager {

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
            switch (currentStep)
            {
                case TutorialStep.First:
                    currentStep = TutorialStep.Welcome;
					hintsN.SetSize(788f, 524.9f);
                    hintsN.LockTo("UI(Clone)", new Vector3(-393.80f, 214.70f, 0.00f));
                    UItext.text = "Welkom. In deze leermodule zul je leren hoe je objecten kunt oppakken en terugleggen";
                    SetUpTutorialNextButton();

                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
						hintsN.ResetSize();
						hintsN.LockTo("WorkField", new Vector3(0.00f, 0.49f, -0.73f));
                        UItext.text = "Laten we beginnen. Beweeg naar het werkveld door erop te klikken.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    { 
                        currentStep = TutorialStep.PickOne;
						hintsN.LockTo("ClothPackage", new Vector3(0.00f, 0.14f, -0.13f));
						hintsN.SetIconPosition(3);
                        UItext.text = "Objecten oppakken kun je doen door erop te klikken. Probeer nu een gaasje te pakken uit de doos door op de doos te klikken.";

                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "Cloth";

                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("ClothPackage").transform.position;
                    }
                    break;
                case TutorialStep.PickOne:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        handsInventory.tutorial_pickedLeft = false;
                        currentStep = TutorialStep.PickTwo;
						hintsN.LockTo("Medicine", new Vector3(0.00f, 0.16f, 0.00f));
                        UItext.text = "Het eerst object die je pakt verschijnt altijd in je linkerhand. Probeer nu het medicijn op te pakken door erop te klikken.";
                        itemToPick = "Medicine";
                        handsInventory.tutorial_pickedRight = false;

                        particleHint.transform.position = GameObject.Find("Medicine").transform.position;
                    }
                    break;
                case TutorialStep.PickTwo:
                    if (handsInventory.tutorial_pickedRight)
                    {
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "";

                        currentStep = TutorialStep.DropOne;

                        UItext.text = "Drop cloth";

                        itemToDrop = "Cloth";
                        handsInventory.tutorial_droppedLeft = false;
                    }
                    break;
                case TutorialStep.DropOne:
                    if (handsInventory.tutorial_droppedLeft)
                    {
                        handsInventory.tutorial_droppedLeft = false;

                        currentStep = TutorialStep.DropTwo;
                        UItext.text = "Drop medicine";

                        itemToDrop = "Medicine";
                        handsInventory.tutorial_droppedRight = false;
                    }
                    break;
                case TutorialStep.DropTwo:
                    if (handsInventory.tutorial_droppedRight)
                    {
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        hintsN.SetIconPosition(0);
                        handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.Done;
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
