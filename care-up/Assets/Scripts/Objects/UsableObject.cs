using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Static object that can be used
/// </summary>
public class UsableObject : InteractableObject {

    [HideInInspector]
    public bool tutorial_used = false;

    private static HandsInventory handsInventory;

    protected override void Start()
    {
        base.Start();

        if (handsInventory == null)
        {
            handsInventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
            if (handsInventory == null) Debug.LogError("No inventory found");
        }
    }

    protected override void Update()
    {
        if (actionManager.CurrentUseObject == name)
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
    }

    public virtual void Use()
    {
        if (!ViewModeActive())
        {
            switch(name)
            {
                case "HandCleaner":
                    {
                        PlayerAnimationManager.PlayAnimation("UseHandCleaner", gameObject.transform);
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
            Reset();
        }
    }
}
