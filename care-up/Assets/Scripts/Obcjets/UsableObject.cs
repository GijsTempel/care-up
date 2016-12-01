using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsableObject : InteractableObject {
    
    protected override void Update()
    {
        if (actionManager.CurrentUseObject == name)
        {
            base.Update();

            if (Input.GetMouseButtonDown(0) && cameraMode.CurrentMode == CameraMode.Mode.Free)
            {
                if (controls.SelectedObject == gameObject && controls.CanInteract)
                {
                    Use();
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
            }
            actionManager.OnUseAction(gameObject.name);
            ResetShader();
        }
    }
}
