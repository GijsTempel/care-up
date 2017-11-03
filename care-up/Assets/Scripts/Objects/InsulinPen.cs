using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                -0.0004f * controlBone.localPosition.y);

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
            if (name == "InsulinPenWithNeedle"
                && controls.SelectedObject.GetComponent<PersonObjectPart>() != null
                && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (info[0] == name && info[1] == "Patient")
                {
                    actionManager.OnUseOnAction(name, "Patient");
                    if (SceneManager.GetActiveScene().name == "Insulin Injection" )
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("InsulinInjection", target);
                    }

                    return true;
                }
            }
        }

        if (info[0] == "InsulinPenWithNeedle" && info[1] == "")
        {
            if (inventory.LeftHandEmpty())
            {
                PlayerAnimationManager.PlayAnimation("UseRight " + name);
                actionManager.OnUseOnAction(name, "");
                name = "VentedInsulinPenWithNeedle";
                return true;
            }
            else if (inventory.RightHandEmpty())
            {
                PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                actionManager.OnUseOnAction(name, "");
                name = "VentedInsulinPenWithNeedle";
                return true;
            }
            else return false;
        }
        else if (info[0] == "VentedInsulinPenWithNeedle" && info[1] == "")
        {
            if (inventory.LeftHandEmpty())
            {
                PlayerAnimationManager.PlayAnimation("UseRight " + name);
                actionManager.OnUseOnAction(name, "");
                name = "InsulinPenWithNeedle";
                return true;
            }
            else if (inventory.RightHandEmpty())
            {
                PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                actionManager.OnUseOnAction(name, "");
                name = "InsulinPenWithNeedle";
                return true;
            }
            else return false;
        }

        actionManager.OnUseOnAction(name, controls.SelectedObject != null && controls.CanInteract ? controls.SelectedObject.name : "");

        return (info[0] == name && controls.SelectedObject != null && info[1] == controls.SelectedObject.name);
    }
}
