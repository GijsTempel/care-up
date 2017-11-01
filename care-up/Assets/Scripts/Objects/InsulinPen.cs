using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsulinPen : PickableObject {

    public bool animateButton = false;

    private Transform button;

    protected override void Start()
    {
        base.Start();

        animateButton = false;
        button = transform.Find("insulinPenButton");
    }

    protected override void Update()
    {
        base.Update();

        if (animateButton)
        {
            button.localPosition = new Vector3(
                button.localPosition.x,
                button.localPosition.y,
                -0.002f * controlBone.localPosition.y);

            button.localRotation = Quaternion.Euler(
                button.localRotation.eulerAngles.x,
                button.localRotation.eulerAngles.y,
                -14.5f * controlBone.localPosition.y);
        }
    }

    public override bool Use(bool hand)
    {
        string[] info = actionManager.CurrentUseOnInfo;
        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if (info[0] == "InsulinPen" && info[1] == "")
            {
                if (inventory.LeftHandEmpty())
                {
                    PlayerAnimationManager.PlayAnimation("UseRight " + name);
                    actionManager.OnUseOnAction("InsulinPen", "");
                    return true; // fix for venting 
                }
                else if (inventory.RightHandEmpty())
                {
                    PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                    actionManager.OnUseOnAction("InsulinPen", "");
                    return true; // fix for venting 
                }
                else return false;
            }
        }
        else
        {
            if (info[0] == "InsulinPen" && info[1] == "")
            {
                if (inventory.LeftHandEmpty())
                {
                    PlayerAnimationManager.PlayAnimation("UseRight " + name);
                    actionManager.OnUseOnAction("InsulinPen", "");
                    return true; // fix for venting 
                }
                else if (inventory.RightHandEmpty())
                {
                    PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                    actionManager.OnUseOnAction("InsulinPen", "");
                    return true; // fix for venting 
                }
                else return false;
            }
        }

        actionManager.OnUseOnAction(name, controls.SelectedObject != null && controls.CanInteract ? controls.SelectedObject.name : "");

        return (info[0] == name && controls.SelectedObject != null && info[1] == controls.SelectedObject.name);
    }
}
