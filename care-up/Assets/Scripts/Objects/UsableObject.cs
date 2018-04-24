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
            tutorial_used = true;

            switch (name)
            {
                case "ClothPackage":
                    {
                        GameObject gameObject = handsInventory.CreateObjectByName("Cloth", Vector3.zero);
                        handsInventory.PickItem(gameObject.GetComponent<PickableObject>());
                        Reset();
                        return;
                    }   // no break, return = end function
                case "HandCleaner":
                    {
                        string message = "Zorg volgens een zorgvuldige handhygiëne. Handhygiëne is in dit protocol versneld om de gebruikerservaring te verbeteren";
                        Camera.main.transform.Find("UI").Find("EmptyHandsWarning").
                                GetComponent<TimedPopUp>().Set(message);
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
