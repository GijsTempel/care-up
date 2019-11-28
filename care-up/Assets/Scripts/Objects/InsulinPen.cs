using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsulinPen : PickableObjectWithInfo {

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
                -0.0004f * leftControlBone.localPosition.y);

            button.localRotation = Quaternion.Euler(
                button.localRotation.eulerAngles.x,
                button.localRotation.eulerAngles.y,
                -14.5f * leftControlBone.localPosition.y);
        }
    }

    public override bool Use(bool hand, bool noTarget = false)
    {
        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if (name == "InsulinPenWithNeedle"
                && controls.SelectedObject.GetComponent<PersonObjectPart>() != null
                && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (actionManager.CompareUseOnInfo(name, "Patient"))
                {
                    actionManager.OnUseOnAction(name, "Patient");
                    if (SceneManager.GetActiveScene().name == "Insulin Injection" )
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("InsulinInjection", target);
                    }

                    PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
                    if (manager != null && !manager.practiceMode)
                    {
                        InjectionPatient patient = GameObject.FindObjectOfType<InjectionPatient>();
                        if (patient.pulledUp == false)
                        {
                            patient.pulledUp = true;
                            patient.GetComponent<Animator>().SetTrigger("ShowBellyForInsulin");
                        }
                    }

                    return true;
                }
            }
        }

        if (actionManager.CompareUseOnInfo("InsulinPenWithNeedle", "") && noTarget
            && name == "InsulinPenWithNeedle")
        {
            if (!inventory.RightHandEmpty())
            {
                if (inventory.rightHandObject.name == name)
                {
                    PlayerAnimationManager.PlayAnimation("UseRight " + name);
                    actionManager.OnUseOnAction(name, "");
                    //name = "VentedInsulinPenWithNeedle";
                    return true;
                }
            }
            //else if (!inventory.LeftHandEmpty())
            if (!inventory.LeftHandEmpty())
            {
            if (inventory.leftHandObject.name == name)
                {
                    PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                    actionManager.OnUseOnAction(name, "");
                    //name = "VentedInsulinPenWithNeedle";
                    return true;
                }
            }
            else
            {
                EmptyHandsWarning();
                return false;
            }
        }
        else if (actionManager.CompareUseOnInfo("VentedInsulinPenWithNeedle", "") && noTarget
            && name == "VentedInsulinPenWithNeedle")
        {
            if (inventory.LeftHandEmpty())
            {
                PlayerAnimationManager.PlayAnimation("UseRight " + name);
                actionManager.OnUseOnAction(name, "");
                //name = "InsulinPenWithNeedle";
                return true;
            }
            else if (inventory.RightHandEmpty())
            {
                PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                actionManager.OnUseOnAction(name, "");
                //name = "InsulinPenWithNeedle";
                return true;
            }
            else
            {
                EmptyHandsWarning();
                return false;
            }
        }
        else if (actionManager.CompareUseOnInfo("InsulinPen", "") && noTarget
            && name == "InsulinPen")
        {
            if (!inventory.RightHandEmpty())
            {
                if (inventory.rightHandObject.name == name)
                {
                    PlayerAnimationManager.PlayAnimation("UseRight " + name);
                    actionManager.OnUseOnAction(name, "");
                    return true;
                }
            }

            if (!inventory.LeftHandEmpty())
            {
                if (inventory.leftHandObject.name == name)
                {
                    PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                    actionManager.OnUseOnAction(name, "");
                    return true;
                }
            }

            EmptyHandsWarning();
            return false;
        }

        actionManager.OnUseOnAction(name, controls.SelectedObject != null ? controls.SelectedObject.name : "");

        return (controls.SelectedObject != null && actionManager.CompareUseOnInfo(name, controls.SelectedObject.name));
    }

    public override void SaveInfo(ref Vector3 left, ref Vector3 right)
    {
        left = new Vector3(
            (animateButton ? 1.0f : 0.0f),
            button.localRotation.eulerAngles.z / (-14.5f),
            0.0f);
    }

    public override void LoadInfo(Vector3 left, Vector3 right)
    {
        animateButton = left.x == 1.0f ? true : false;

        if (button == null)
        {
            button = transform.Find("insulinPenButton");
        }

        button.localPosition = new Vector3(
            button.localPosition.x,
            button.localPosition.y,
            -0.0004f * left.y);

        button.localRotation = Quaternion.Euler(
            button.localRotation.eulerAngles.x,
            button.localRotation.eulerAngles.y,
            -14.5f * left.y);
    }
}
