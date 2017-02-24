using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsableObject : InteractableObject {

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
                        animationManager.PlayAnimation("UseHandCleaner", gameObject, gameObject);
                    }
                    break;
                case "WorkField":
                    {
                        GameObject.Find("Player").GetComponent<AnimationManager>().PlayAnimation("Clean_Table");
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
