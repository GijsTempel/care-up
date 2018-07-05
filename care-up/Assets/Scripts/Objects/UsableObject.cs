using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Static object that can be used
/// </summary>
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
            // unique object, not usable, but picking performed instead
            if (name == "ClothPackage")
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
            }

            if (actionManager.CompareUseObject(name))
            {
                switch (name)
                {
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

                PlayerAnimationManager.PlayAnimation("Use " + name, transform);
                tutorial_used = true;
            }

            actionManager.OnUseAction(gameObject.name);

            Reset();
        }
    }
}
