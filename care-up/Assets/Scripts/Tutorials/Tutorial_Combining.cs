using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Combining : TutorialManager {

    public enum TutorialStep
    {
        First,
        Welcome,
        PickBoth,
        Combine,
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

                        currentStep = TutorialStep.Combine;
                        UItext.text = "Combine";

                        handsInventory.tutorial_combined = false;
                    }
                    break;
                case TutorialStep.Combine:
                    if (handsInventory.tutorial_combined)
                    {
                        handsInventory.tutorial_combined = false;
                        currentStep = TutorialStep.Decombine;
                        UItext.text = "Decombine";
                    }
                    break;
                case TutorialStep.Decombine:
                    if (handsInventory.tutorial_droppedRight)
                    {
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
