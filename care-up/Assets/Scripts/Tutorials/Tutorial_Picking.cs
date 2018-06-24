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
                    hintsBox.anchoredPosition = new Vector2(-14.8f, 9.9f);
                    hintsBox.sizeDelta = new Vector2(788f, 524.9f);
                    UItext.text = "Welkom. In deze leermodule zul je leren hoe je objecten kunt oppakken en terugleggen";
                    SetUpTutorialNextButton();

                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
                        hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        UItext.text = "Laten we beginnen. Beweeg naar het werkveld door erop te klikken.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    { 
                        currentStep = TutorialStep.PickOne;
                        hintsBox.anchoredPosition = new Vector2(53f, 247f);
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
                        hintsBox.anchoredPosition = new Vector2(0f, 79f);
                        UItext.text = "Het eerst object die je pakt verschijnt altijd in je linkerhand. Probeer nu het medicijn op te pakken door erop te klikken.";
                        
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "Medicine";

                        particleHint.transform.position = GameObject.Find("Medicine").transform.position;
                    }
                    break;
                case TutorialStep.PickTwo:
                    if (handsInventory.tutorial_pickedRight)
                    {
                        handsInventory.tutorial_pickedRight = false;
                        hintsBox.anchoredPosition = new Vector2(49f, -74f);
                        itemToPick = "";

                        currentStep = TutorialStep.OpenDropOne;
                        UItext.text = "Goed gedaan. Laten we nu beide objecten terugleggen. Klik op het gaasje om de opties te tonen.";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenDropOne:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.DropOne;
                        hintsBox.anchoredPosition = new Vector2(507f, -199f);
                        UItext.text = "Kies vervolgens voor de optie 'Terugleggen'.";

                        itemToDrop = "Cloth";
                        handsInventory.tutorial_droppedLeft = false;
                    }
                    break;
                case TutorialStep.DropOne:
                    if (handsInventory.tutorial_droppedLeft)
                    {
                        handsInventory.tutorial_droppedLeft = false;
                        hintsBox.anchoredPosition = new Vector2(49f, -74f);
                        currentStep = TutorialStep.OpenDropTwo;
                        UItext.text = "Makkelijk toch? Klik nu op het medicijn om de opties te tonen.";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenDropTwo:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;
                        hintsBox.anchoredPosition = new Vector2(-121f, 0);
                        currentStep = TutorialStep.DropTwo;
                        UItext.text = "Kies ook hier weer voor de optie 'Terugleggen'.";

                        itemToDrop = "Medicine";
                        handsInventory.tutorial_droppedRight = false;
                    }
                    break;
                case TutorialStep.DropTwo:
                    if (handsInventory.tutorial_droppedRight)
                    {
                        hintsBox.anchoredPosition = new Vector2(548f, -274f);
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
