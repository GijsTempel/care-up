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

    /*protected override void Update()
    {
        if (actionManager.CurrentUseObject == name || 
            (actionManager.CurrentUseObject == "HandCleaner" && name == "WorkField"))
        {
            base.Update();

            if (controls.MouseClicked() && cameraMode.CurrentMode == CameraMode.Mode.Free)
            {
                if (controls.SelectedObject == gameObject && controls.CanInteract)
                {
                    if (handsInventory.Empty())
                    {
                        Use();
                    }
                }
            }
        }
    }*/

    public virtual void Use()
    {
        if (!ViewModeActive())
        {
            switch(name)
            {
                case "HandCleaner":
                    {
                        string message = "Clean your hands even better than this";
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
            tutorial_used = true;
            actionManager.OnUseAction(gameObject.name);
            PlayerAnimationManager.PlayAnimation("Use " + name, transform);
            Reset();
        }
    }
}
