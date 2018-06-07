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
                    UItext.text = "Welcome. This tutorial will teach you how to use items on other items that are on the table.";
                    SetUpTutorialNextButton();

                    GameObject.Find("DevHint").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Move to table";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;

                        currentStep = TutorialStep.PickNeedle;
                        UItext.text = "Pick the needle.";

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
                        UItext.text = "Click the needle";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        UItext.text = "Select UseOn option";

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        player.tutorial_UseOnControl = false;
                        
                        currentStep = TutorialStep.UseOn;
                        UItext.text = "Click on the needle cup.";
                        
                        handsInventory.tutorial_itemUsedOn = false;
                    }
                    break;
                case TutorialStep.UseOn:
                    if (handsInventory.tutorial_itemUsedOn)
                    {
                        handsInventory.tutorial_itemUsedOn = false;
                        currentStep = TutorialStep.Done;
                        UItext.text = "Great work! This concludes picking up tutorial.";
                    }
                    break;
                case TutorialStep.Done:
                    currentStep = TutorialStep.None;
                    endPanel.SetActive(true);
                    player.enabled = false;
                    GameObject.FindObjectOfType<RobotManager>().enabled = false;
                    foreach (InteractableObject o in GameObject.FindObjectsOfType<InteractableObject>())
                    {
                        o.Reset();
                        o.enabled = false;
                    }
                    break;
            }
        }
    }
}
