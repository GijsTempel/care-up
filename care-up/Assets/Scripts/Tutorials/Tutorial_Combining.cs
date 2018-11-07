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

    public bool decombiningAllowed = false;

    protected override void Update()
    {
        base.Update();

        if (!Paused())
        {
            switch (currentStep)
            {
                case TutorialStep.First:
                    currentStep = TutorialStep.Welcome;
                    //hintsBox.anchoredPosition = new Vector2(-14.8f, 9.9f);
                    //hintsBox.sizeDelta = new Vector2(788f, 524.9f);
					hintsN.SetSize(788f, 524.9f);
                    hintsN.LockTo("UI(Clone)", new Vector3(-393.80f, 215f, 0.00f));
                    UItext.text = "Welkom. In deze leermodule zal je leren hoe je objecten in je handen kunt combineren en uit elkaar kunt halen.";
                    SetUpTutorialNextButton();

    
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
						//hintsBox.anchoredPosition = new Vector2(165f, -265.64f);
						//hintsBox.sizeDelta = new Vector2(472.5f, 298.9f);
						hintsN.ResetSize();
						hintsN.LockTo("WorkField", new Vector3(0.00f, 0.51f, -0.75f));
                        UItext.text = "Laten we beginnen door te bewegen naar het werkveld. Doe dit door te klikken op het werkveld.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;
                        
                        currentStep = TutorialStep.PickOne;
						hintsN.SetIconPosition(3);
						hintsN.LockTo("Syringe", new Vector3(0.00f, 0.11f, -0.10f));
                        UItext.text = "We zien een spuit en een naald op het werkveld liggen. Pak de spuit op door erop te klikken.";

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
                        UItext.text = "Heel goed! Pak nu ook de naald op. Doe dit door op de naald te klikken.";

						hintsN.LockTo("AbsorptionNeedle", new Vector3(0.00f, 0.00f, -0.08f));

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

                        //hintsBox.anchoredPosition = new Vector2(-67f, 37f);
						hintsN.LockTo("Syringe", new Vector3(0.00f, -0.03f, -0.03f));
						hintsN.SetIconPosition(0);

                        currentStep = TutorialStep.OpenControls;
                        UItext.text = "Nu we beide objecten in onze handen hebben kunnen we erop klikken om de opties te tonen. Klik nu op de spuit.";

                        player.tutorial_itemControls = false;
                        player.itemControlsToInit = "Syringe";
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        hintsN.LockTo("/ItemControls/ItemControlsGroup/UseOnButton", new Vector3(20.10f, -116.00f, 0.00f));
                        UItext.text = "Klik op het + icoon. Het + icoon gebruik je in Care Up om objecten te combineren of met elkaar te gebruiken.'";

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        player.tutorial_UseOnControl = false;

                        currentStep = TutorialStep.Combine;
                        //hintsBox.anchoredPosition = new Vector2(746f, -149.45f);
                        hintsN.LockTo("/Player/CinematicControl/Arms/Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/toolHolder.R/AbsorptionNeedle", new Vector3(-0.12f, -0.02f, 0.00f));
                        hintsN.SetIconPosition(1);
                        UItext.text = "Klik nu op het object in je andere hand om de objecten met elkaar te combineren.";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Combine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.Explanaiton;
						hintsN.LockTo("UI(Clone)", new Vector3(-239.20f, 0.00f, 0.00f));
						hintsN.SetIconPosition(0);
                        UItext.text = "De opties tonen voor objecten kan zowel met het object in je linkerhand als in je rechterhand.";
                        SetUpTutorialNextButton();
                    }
                    break;
                case TutorialStep.Explanaiton:
                    if (nextButtonClicked)
                    { 
                        currentStep = TutorialStep.OpenControls2;
						hintsN.LockTo("SyringeWithAbsorptionNeedleCap", new Vector3(0.00f, 0.00f, -0.06f));
                        UItext.text = "Laten we nu de dop van de naald afhalen. Dit noemen we in Care Up 'Scheiden'. Klik op de spuit met opzuignaald die je in je hand vast hebt om het opties menu te openen.";

                        player.tutorial_itemControls = false;
                        player.itemControlsToInit = "SyringeWithAbsorptionNeedleCap";
                    }
                    break;
                case TutorialStep.OpenControls2:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.Decombine;
                        hintsN.LockTo("/ItemControls/ItemControlsGroup/CombineButton", new Vector3(35.60f, -106.60f, 0.00f));
                        UItext.text = "Kies vervolgens het - icoon. Het - icoon gebruik je in Care Up om objecten te scheiden, uit elkaar te halen of te openen.";

                        handsInventory.tutorial_combined = false;
                        decombiningAllowed = true;
                    }
                    break;
                case TutorialStep.Decombine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.Done;
                        
                        hintsN.LockTo("SceneLoader 1", new Vector3(262.50f, -69.10f, 0.00f));
                        UItext.text = "Goed gedaan! Dit was de leermodule over het combineren en scheiden van objecten.";

                        SetPauseTimer(5.0f);
                    }
                    break;
                case TutorialStep.Done:
                    if (!Paused())
                    {
                        currentStep = TutorialStep.None;
                        TutorialEnd();
                    }
                    break;
            }
        }
    }
}
