using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Picking : TutorialManager {

    public enum TutorialStep
    {
        First,
        Welcome,
        PickOne,
        PickTwo,
        DropOne,
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
                        currentStep = TutorialStep.PickOne;
                        UItext.text = "Come close to items. Picking is performed simply by clicking on desired item. Try to pick up piece of cloth.";
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
                        UItext.text = "First item picked will go in left hand, second - in right hand. If player can also examine item, picking is done in two steps:" +
                            "\n 1. Click on item to open controls menu. \n 2. Select \'pick up\' option. \n Try to pick up bottle of medicine.";
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
                        currentStep = TutorialStep.DropOne;
                        UItext.text = "Good job! Now let's proceed to putting items back on table. Putting items is done similarly. Click on desired item, then select \'Drop\' option from controls menu. " +
                            "\n Try to drop cloth.";
                        itemToDrop = "Cloth";
                        handsInventory.tutorial_droppedLeft = false;
                    }
                    break;
                case TutorialStep.DropOne:
                    if (handsInventory.tutorial_droppedLeft)
                    {
                        handsInventory.tutorial_droppedLeft = false;
                        currentStep = TutorialStep.DropTwo;
                        UItext.text = "You're doing great! Now drop another item too. ";
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
