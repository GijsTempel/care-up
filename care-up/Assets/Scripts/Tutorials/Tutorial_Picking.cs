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
                    UItext.text = "Welcome. This tutorial will teach you how to pick and put items you see within a scene.";
                    SetUpTutorialNextButton();

                    GameObject.Find("DevHint").SetActive(false);
                    break;
                case TutorialStep.Welcome:
                    if (nextButtonClicked)
                    {
                        currentStep = TutorialStep.MoveTo;
                        UItext.text = "Come close to items.";

                        player.tutorial_movedTo = false;
                    }
                    break;
                case TutorialStep.MoveTo:
                    if (player.tutorial_movedTo)
                    { 
                        currentStep = TutorialStep.PickOne;
                        UItext.text = "Picking is performed simply by clicking on desired item. Try to pick up piece of cloth.";

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
                        UItext.text = "First item picked will go in left hand, second - in right hand. Pick up medicine now.";
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

                        currentStep = TutorialStep.OpenDropOne;
                        UItext.text = "Good job! Now let's proceed to putting items back on table. Putting items is done in two steps. First - click on cloth.";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenDropOne:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.DropOne;
                        UItext.text = "Now select Drop option";

                        itemToDrop = "Cloth";
                        handsInventory.tutorial_droppedLeft = false;
                    }
                    break;
                case TutorialStep.DropOne:
                    if (handsInventory.tutorial_droppedLeft)
                    {
                        handsInventory.tutorial_droppedLeft = false;

                        currentStep = TutorialStep.OpenDropTwo;
                        UItext.text = "You're doing great! Now click bottle of medicine";

                        player.tutorial_itemControls = false;
                    }
                    break;
                case TutorialStep.OpenDropTwo:
                    if (player.tutorial_itemControls)
                    {
                        player.tutorial_itemControls = false;

                        currentStep = TutorialStep.DropTwo;
                        UItext.text = "And select to drop this too";

                        itemToDrop = "Medicine";
                        handsInventory.tutorial_droppedRight = false;
                    }
                    break;
                case TutorialStep.DropTwo:
                    if (handsInventory.tutorial_droppedRight)
                    {
                        handsInventory.tutorial_droppedRight = false;
                        currentStep = TutorialStep.Done;
                        UItext.text = "Great work! This concludes picking up tutorial.";
                        itemToDrop = "";
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
