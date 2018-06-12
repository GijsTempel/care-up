using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_UseOn : TutorialManager {

    public enum TutorialStep
    {
        First,
        Welcome,
        MoveTo,
        PickNeedle,
        OpenControls,
        ClickUseOn,
        UseOn,
        Done,
        None
    }

    private TutorialStep currentStep = TutorialStep.First;

    protected override void Update()
    {
        base.Update();

        if (!Paused())
        {
            switch(currentStep)
            {
                case TutorialStep.First:
                    currentStep = TutorialStep.Welcome;
                    hintsBox.anchoredPosition = new Vector2(-14.8f, 9.9f);
                    hintsBox.sizeDelta = new Vector2(788f, 524.9f);

                    UItext.text = "Welkom. In deze leermodule zal je leren hoe je objecten met andere objecten kunt gebruiken. Denk hierbij aan het weggooien van een naald in de naaldcontainer of het gebruiken van een injectiespuit op de cliënt.";
                    SetUpTutorialNextButton();

                    GameObject.Find("DevHint").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
                        hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Laten we beginnen door te bewegen naar het werkveld. Doe dit door te klikken op het werkveld.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;

                        currentStep = TutorialStep.PickNeedle;
                        hintsBox.anchoredPosition = new Vector2(309f, -322f);
                        UItext.text = "Heel goed. Laten we nu de gebruikte opzuignaald oppakken door erop te klikken.";

                        handsInventory.tutorial_pickedLeft = false;
                        
                        itemToPick = "AbsorptionNeedleNoCap";

                        particleHint.SetActive(true);
                        particleHint.transform.position = GameObject.Find("AbsorptionNeedleNoCap").transform.position;
                    }
                    break;
                case TutorialStep.PickNeedle:
                    if (handsInventory.tutorial_pickedLeft)
                    {
                        handsInventory.tutorial_pickedLeft = false;
                        itemToPick = "";

                        particleHint.SetActive(false);

                        currentStep = TutorialStep.OpenControls;
                        hintsBox.anchoredPosition = new Vector2(48f, -221f);
                        UItext.text = "Nu je de naald vast hebt, klik nogmaals op de naald om de opties te tonen. ";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        hintsBox.anchoredPosition = new Vector2(527f, 68f);
                        UItext.text = "Kies voor de optie 'Gebruiken met...' ";

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        player.tutorial_UseOnControl = false;
                        
                        currentStep = TutorialStep.UseOn;
                        hintsBox.anchoredPosition = new Vector2(75f, 0.0f);
                        UItext.text = "Klik nu op het object waarmee je de naald wilt gebruiken. In dit geval de naaldcontainer.";
                        
                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOn:
                    if (handsInventory.tutorial_itemUsedOn)
                    {
                        handsInventory.tutorial_itemUsedOn = false;
                        currentStep = TutorialStep.Done;
                        hintsBox.anchoredPosition = new Vector2(513f, -304f);
                        UItext.text = "Super! Dit was de leermodule over het gebruiken van een object in je hand met een ander object zoals de naaldcontainer.";
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
