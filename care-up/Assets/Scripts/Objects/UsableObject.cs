using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsableObject : InteractableObject {

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

            if (Input.GetMouseButtonDown(0) && cameraMode.CurrentMode == CameraMode.Mode.Free)
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

    public void Use()
    {
        if (!ViewModeActive())
        {
            switch(name)
            {
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
            actionManager.OnUseAction(gameObject.name);
            Reset();
        }
    }
}
