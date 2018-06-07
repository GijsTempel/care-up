using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Combining : TutorialManager {

    public enum TutorialStep
    {
        First,
        Welcome,
        MoveTo,
        PickBoth,
        OpenControls,
        ClickUseOn,
        Combine,
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
                    UItext.text = "Welkom. In deze leermodule zal je leren hoe je objecten in je handen kunt combineren en uit elkaar kunt halen.";
                    SetUpTutorialNextButton();

                    GameObject.Find("DevHint").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Laten we beginnen door te bewegen naar het werkveld. Doe dit door te klikken op het werkveld.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;
                        
                        currentStep = TutorialStep.PickBoth;
                        UItext.text = "We zien nu een spuit en een naald liggen. Pak beide objecten op door erop te klikken.";

                        handsInventory.tutorial_pickedLeft = false;
                        handsInventory.tutorial_pickedRight = false;

                        itemToPick = "Syringe";
                        itemToPick2 = "AbsorptionNeedle";

                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("Syringe").transform.position;
                        particleHint_alt.SetActive(true);
                        particleHint_alt.transform.position = GameObject.Find("AbsorptionNeedle").transform.position;
                    }
                    break;
                case TutorialStep.PickBoth:
                    if (handsInventory.tutorial_pickedLeft && handsInventory.tutorial_pickedRight)
                    {
                        handsInventory.tutorial_pickedLeft = false;
                        handsInventory.tutorial_pickedRight = false;
                        itemToPick = itemToPick2 = "";

                        particleHint.SetActive(false);
                        particleHint_alt.SetActive(false);

                        currentStep = TutorialStep.OpenControls;
                        UItext.text = "Nu we beide objecten in onze handen hebben kunnen we erop klikken om de opties te tonen. Klik nu op de naald of de spuit.";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        UItext.text = "Kies nu voor de optie 'Gebruiken met...'";

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        player.tutorial_UseOnControl = false;

                        currentStep = TutorialStep.Combine;
                        UItext.text = "Klik nu op het object in je andere hand om de objecten met elkaar te combineren.";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Combine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.OpenControls2;
                        UItext.text = "Heel goed. Laten we nu de dop van de naald afhalen. Dit noemen we in Care Up 'Scheiden'. Klik op de spuit met opzuignaald die je in je hand vast hebt om het opties menu te openen.";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls2:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.Decombine;
                        UItext.text = "Kies vervolgens voor de optie 'Scheiden'. Afval (dopjes, gebruikte gaasjes etc.) wordt automatisch weggegooit na het combineren of scheiden. ";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Decombine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.Done;
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
