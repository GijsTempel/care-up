using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsableObject : InteractableObject {
    
    static private ActionManager actionManager;

    protected override void Start () {
        base.Start();

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");
    }

    protected override void Update()
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

    public void Use()
    {
        if (!ViewModeActive())
        {
            switch(name)
            {
                case "Cheat Sheet":
                    {
                        Text hintText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
                        if ( hintText.text == actionManager.CurrentDescription )
                        {
                            return; // hint is not new, no action pefrormed
                        }
                        else
                        {
                            hintText.text = actionManager.CurrentDescription;
                        }
                    }
                    break;
                case "WorkField":
                    {
                        GameObject.Find("Player").GetComponent<AnimationManager>().PlayAnimation("Clean_Table");
                    }
                    break;
            }
            actionManager.OnUseAction(gameObject.name);
        }
    }
}
