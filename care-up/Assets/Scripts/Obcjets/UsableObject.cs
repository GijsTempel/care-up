using UnityEngine;
using System.Collections;

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
            actionManager.OnUseAction(gameObject.name);
        }
    }
}
