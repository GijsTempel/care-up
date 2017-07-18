using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Syringe : PickableObject {

    public bool updatePlunger = false;

    private Transform plunger;

    private Vector3 plungerSavedPos;

    public Vector3 PlungerPosition
    {
        get { return plunger.localPosition; }
        set
        {
            if (plunger != null)
            {
                plunger.localPosition = value;
            }
            else
            {
                transform.Find("syringePlunger").localPosition = value;
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        plunger = transform.Find("syringePlunger");
        if (plunger == null) Debug.LogError("No plunger found!");
    }

    protected override void Update()
    {
        base.Update();

        if ( updatePlunger )
        {
            plunger.localPosition = new Vector3(
                plunger.localPosition.x,
                Mathf.Lerp(-0.013f, 0.06f, controlBone.localPosition.y),
                plunger.localPosition.z);
        }
    }

    public override bool Use(bool hand = false)
    {
        string[] info = actionManager.CurrentUseOnInfo;
        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if (name == "SyringeWithInjectionSNeedleCap"
                && controls.SelectedObject.GetComponent<PersonObjectPart>() != null
                && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (info[0] == "SyringeWithInjectionSNeedleCap" && info[1] == "Patient")
                {
                    actionManager.OnUseOnAction("SyringeWithInjectionSNeedleCap", "Patient");
                    if (SceneManager.GetActiveScene().name == "Injection Subcutaneous v2" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous v2_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous v2_desolve")
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("SubcutaneousInjection v2", target);
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection scene v2")
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("Injection v2", target);
                    }
                    return true;
                }
            }
            else if (name == "SyringeWithInjectionNeedleCap"
                && controls.SelectedObject.GetComponent<PersonObjectPart>() != null
                && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (info[0] == "SyringeWithInjectionNeedleCap" && info[1] == "Patient")
                {
                    actionManager.OnUseOnAction("SyringeWithInjectionNeedleCap", "Patient");
                    if (SceneManager.GetActiveScene().name == "Injection" ||
                        SceneManager.GetActiveScene().name == "Injection_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection_disolve")
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("Injection", target);
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection Subcutaneous" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous_desolve")
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("SubcutaneousInjection", target);
                    }
                    return true;
                }
            }
            else if (name == "Syringe" && controls.SelectedObject.name == "Person")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "Syringe" && info[1] == "Person")
                {
                    actionManager.OnUseOnAction("Syringe", "Person");
                    AnimationSequence animationSequence = new AnimationSequence("WingedNeedle");
                    animationSequence.NextStep();
                    return true;
                }
            }
            else if (name == "SyringeWithAbsorptionNeedle"
                    && controls.SelectedObject.GetComponent<PersonObjectPart>() != null
                    && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "SyringeWithAbsorptionNeedle" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("SyringeWithAbsorptionNeedle", "Hand");
                    return true; // for tutorial
                }
            }
        }
        else // cannot interact or target == ""
        {
            if (name == "SyringeWithAbsorptionNeedle")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "SyringeWithAbsorptionNeedle" && info[1] == "")
                {
                    if (inventory.LeftHandEmpty())
                    {
                        PlayerAnimationManager.PlayAnimation("UseRight " + name);
                        actionManager.OnUseOnAction("SyringeWithAbsorptionNeedle", "");
                        return true; // fix for venting syringe
                    }
                    else if (inventory.RightHandEmpty())
                    {
                        PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                        actionManager.OnUseOnAction("SyringeWithAbsorptionNeedle", "");
                        return true; // fix for venting syringe
                    }
                    else return false;
                }
            }
            else if (name == "SyringeWithAbsorptionSNeedle")
            {
                info = actionManager.CurrentUseOnInfo;
                if (info[0] == "SyringeWithAbsorptionSNeedle" && info[1] == "")
                {
                    if (inventory.LeftHandEmpty())
                    {
                        PlayerAnimationManager.PlayAnimation("UseRight " + name);
                        actionManager.OnUseOnAction("SyringeWithAbsorptionSNeedle", "");
                        return true; // fix for venting syringe
                    }
                    else if (inventory.RightHandEmpty())
                    {
                        PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                        actionManager.OnUseOnAction("SyringeWithAbsorptionSNeedle", "");
                        return true; // fix for venting syringe
                    }
                    else return false;
                }
            }
        }

        actionManager.OnUseOnAction(name, controls.SelectedObject != null && controls.CanInteract ? controls.SelectedObject.name : "");

        return (info[0] == name && controls.SelectedObject != null && info[1] == controls.SelectedObject.name);
    }

}
