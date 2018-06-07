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
                    UItext.text = "Welcome. This tutorial will teach you how to combine items in hands";
                    SetUpTutorialNextButton();

                    GameObject.Find("DevHint").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Come to table";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    {
                        player.tutorial_movedTo = false;
                        
                        currentStep = TutorialStep.PickBoth;
                        UItext.text = "Come close to items. Pick the syringe and needle.";

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
                        UItext.text = "Click on an item in hand";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.ClickUseOn;
                        UItext.text = "Click UseOn";

                        player.tutorial_UseOnControl = false;
                    }
                    break;
                case TutorialStep.ClickUseOn:
                    if (player.tutorial_UseOnControl)
                    {
                        player.tutorial_UseOnControl = false;

                        currentStep = TutorialStep.Combine;
                        UItext.text = "Select item that is in another hand";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Combine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.OpenControls2;
                        UItext.text = "Now Decombining. Click item in hand";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenControls2:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.Decombine;
                        UItext.text = "Select decombine";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Decombine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;

                        currentStep = TutorialStep.Done;
                        UItext.text = "Great work! This concludes picking up tutorial.";

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
