using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Static object that can be used
/// </summary>
[RequireComponent(typeof(Renderer))]
public class UsableObject : InteractableObject {

    [HideInInspector]
    public bool tutorial_used = false;

    protected static HandsInventory handsInventory;

    protected override void Start()
    {
        base.Start();

        if (handsInventory == null)
        {
            handsInventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
            if (handsInventory == null) Debug.LogError("No inventory found");
        }
    }

    public virtual void Use()
    {
        if (!ViewModeActive())
        {
            tutorial_used = true;

            switch (name)
            {
                case "ClothPackage":
                    {
                        TutorialManager tutorial = GameObject.Find("GameLogic").GetComponent<TutorialManager>();
                        if (tutorial == null || (tutorial != null &&
                            ("Cloth" == tutorial.itemToPick || "Cloth" == tutorial.itemToPick2)))
                        {
                            GameObject gameObject = handsInventory.CreateObjectByName("Cloth", Vector3.zero);
                            handsInventory.PickItem(gameObject.GetComponent<PickableObject>());
                            Reset();
                        }
                        return;
                    }   // no break, return = end function
                case "HandCleaner":
                    {
                        InjectionPatient patient = GameObject.FindObjectOfType<InjectionPatient>();
                        if (patient != null)
                        {
                            patient.NextDialogue();
                        }
                        
                        string message = "Zorg voor een zorgvuldige handhygiëne. Handhygiëne is in dit protocol versneld om de gebruikerservaring te verbeteren";
                        RobotUIMessageTab messageCenter = GameObject.FindObjectOfType<RobotUIMessageTab>();
                        messageCenter.NewMessage("Zorgvuldige handhygiëne", message, RobotUIMessageTab.Icon.Info);
                    }
                    break;
                case "OldBandAid":
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
            actionManager.OnUseAction(gameObject.name);
            PlayerAnimationManager.PlayAnimation("Use " + name, transform);
            Reset();
        }
    }
}
