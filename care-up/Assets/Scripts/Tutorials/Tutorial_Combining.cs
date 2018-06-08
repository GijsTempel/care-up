using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Combining : TutorialManager {

    public enum TutorialStep
    {
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
                    UItext.text = "Welkom. In deze leermodule zal je leren hoe je objecten in je handen kunt combineren en uit elkaar kunt halen.";
                    SetUpTutorialNextButton();

                    GameObject.Find("DevHint").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
                        hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        UItext.text = "Laten we beginnen door te bewegen naar het werkveld. Doe dit door te klikken op het werkveld.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;
                        
                        currentStep = TutorialStep.PickOne;
                        hintsBox.anchoredPosition = new Vector2(236.25f, 149.45f); 
                        UItext.text = "We zien een spuit en een naald op het werkveld liggen. Pak beide objecten op door erop te klikken.";

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

                        currentStep = TutorialStep.PickTwo;
                        UItext.text = "Pick needle";

                        handsInventory.tutorial_pickedRight = false;

                        itemToPick = "AbsorptionNeedle";
                        
                        particleHint.transform.position = GameObject.Find("AbsorptionNeedle").transform.position;
                    }
                    break;
                case TutorialStep.PickTwo:
                    if (handsInventory.tutorial_pickedRight)
                    {
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = "";

                        particleHint.SetActive(false);

                        hintsBox.anchoredPosition = new Vector2(-67f, 37f);
                        currentStep = TutorialStep.OpenControls;
                        UItext.text = "Nu we beide objecten in onze handen hebben kunnen we erop klikken om de opties te tonen. Klik nu op de spuit.";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        hintsBox.anchoredPosition = new Vector2(352f, 16f);
                        UItext.text = "Kies nu voor de optie 'Gebruiken met...'";

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        player.tutorial_UseOnControl = false;

                        currentStep = TutorialStep.Combine;
                        hintsBox.anchoredPosition = new Vector2(746f, -149.45f);
                        UItext.text = "Klik nu op het object in je andere hand om de objecten met elkaar te combineren.";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Combine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.Explanaiton;
                        UItext.text = "Explanation about ability to also use right hand for options (with ok button)";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.Explanaiton:
                    if (nextButtonClicked)
                    { 
                        currentStep = TutorialStep.OpenControls2;
                        hintsBox.anchoredPosition = new Vector2(0f, -239f);
                        UItext.text = "Heel goed. Laten we nu de dop van de naald afhalen. Dit noemen we in Care Up 'Scheiden'. Klik op de spuit met opzuignaald die je in je hand vast hebt om het opties menu te openen.";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls2:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.Decombine;
                        hintsBox.anchoredPosition = new Vector2(391f, -108f);
                        UItext.text = "Kies vervolgens voor de optie 'Scheiden'. Afval (dopjes, gebruikte gaasjes etc.) wordt automatisch weggegooit na het combineren of scheiden. ";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Decombine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.Done;
                        hintsBox.anchoredPosition = new Vector2(512f, -267.19f);
                        UItext.text = "Goed gedaan! Dit was de leermodule over het combineren en scheiden van objecten.";

                        SetPauseTimer(5.0f);
                    }
                    break;
                case TutorialStep.Done:
                    if (!Paused())
                    {
                        currentStep = TutorialStep.None;
                        endPanel.SetActive(true);
                        player.enabled = false;
                        GameObject.FindObjectOfType<RobotManager>().enabled = false;
                        foreach (InteractableObject o in GameObject.FindObjectsOfType<InteractableObject>())
                        {
                            o.Reset();
                            o.enabled = false;
                        }
                    }
                    break;
            }
        }
    }
}
