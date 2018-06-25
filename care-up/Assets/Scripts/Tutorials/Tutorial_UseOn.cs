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
                    //hintsBox.anchoredPosition = new Vector2(-14.8f, 9.9f);
                    //hintsBox.sizeDelta = new Vector2(788f, 524.9f);
					hintsN.SetSize(788f, 382f);
                    hintsN.LockTo("UI(Clone)", new Vector3(-393.80f, 214.70f, 0.00f));
                    UItext.text = "Welkom. In deze leermodule zal je leren hoe je objecten met andere objecten kunt gebruiken. Denk hierbij aan het weggooien van een naald in de naaldcontainer of het gebruiken van een injectiespuit op de cliënt.";
                    SetUpTutorialNextButton();

     //               GameObject.Find("DevHint").SetActive(false);
					//GameObject.Find("Extra").SetActive(false);
                    //GameObject.Find("ExtraButton").SetActive(false);

                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
						//hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
						//hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
						hintsN.ResetSize();
						hintsN.LockTo("WorkField", new Vector3(0.00f, 0.51f, -0.73f));
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
                        //hintsBox.anchoredPosition = new Vector2(372.73f, -237f);
						hintsN.LockTo("AbsorptionNeedleNoCap", new Vector3(0.00f, 0.42f, -0.06f));
						hintsN.SetIconPosition(3);
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
                        //hintsBox.anchoredPosition = new Vector2(207f, -138f);
						hintsN.LockTo("AbsorptionNeedleNoCap", new Vector3(0.00f, 0.00f, 0.00f));
						hintsN.SetIconPosition(0);
                        UItext.text = "Nu je de naald vast hebt, klik nogmaals op de naald om de opties te tonen. ";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        //hintsBox.anchoredPosition = new Vector2(527f, 68f);
						hintsN.LockTo("UseOnButton", new Vector3(172.97f, 318.30f, 0.00f));
						hintsN.SetIconPosition(3);
                        UItext.text = "Kies voor de optie 'Gebruiken met...' ";

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        player.tutorial_UseOnControl = false;
                        
                        currentStep = TutorialStep.UseOn;
                        //hintsBox.anchoredPosition = new Vector2(566f, -69f);
						hintsN.LockTo("NeedleCup", new Vector3(0.00f, 0.00f, -0.07f));
						hintsN.SetIconPosition(0);
                        UItext.text = "Klik nu op het object waarmee je de naald wilt gebruiken. In dit geval de naaldcontainer.";
                        
                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOn:
                    if (handsInventory.tutorial_itemUsedOn)
                    {
                        handsInventory.tutorial_itemUsedOn = false;
                        currentStep = TutorialStep.Done;
						hintsN.LockTo("SceneLoader 1", new Vector3(363.61f, -22.40f, 0.00f));
                        //hintsBox.anchoredPosition = new Vector2(513f, -304f);
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
