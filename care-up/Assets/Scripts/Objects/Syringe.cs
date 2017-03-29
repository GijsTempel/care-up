using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Syringe : PickableObject {

    public bool updatePlunger = false;

    private Transform plunger;

    protected override void Start()
    {
        base.Start();

        plunger = transform.FindChild("syringePlunger");
        if (plunger == null) Debug.LogError("No plunger found!");
    }

    protected override void Update()
    {
        base.Update();

        if ( updatePlunger )
        {
            plunger.localPosition = new Vector3(
                plunger.localPosition.x,
                controlBone.localPosition.y,
                plunger.localPosition.z);
        }
    }

    public override bool Use()
    {
        string[] info = actionManager.CurrentUseOnInfo;
        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if (name == "SyringeWithInjectionNeedle"
                && controls.SelectedObject.GetComponent<PersonObjectPart>() != null 
                && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (info[0] == "SyringeWithInjectionNeedle" && info[1] == "Hand")
                {
                    actionManager.OnUseOnAction("SyringeWithInjectionNeedle", "Hand");
                    if (SceneManager.GetActiveScene().name == "Injection" ||
                        SceneManager.GetActiveScene().name == "Injection_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection_disolve")
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        PlayerAnimationManager.PlayAnimationSequence("Injection", inventory.LeftHandObject == gameObject, target);
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection Subcutaneous" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous_desolve")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("SubcutaneousInjection");
                        animationSequence.NextStep();
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection Subcutaneous v2" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous v2_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous v2_desolve")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("SubcutaneousInjection v2");
                        animationSequence.NextStep();
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection scene v2")
                    {
                        AnimationSequence animationSequence = new AnimationSequence("Injection v2");
                        animationSequence.NextStep();
                    }
                    return true;
                }
            }
            else if (name == "Syringe"
                    && controls.SelectedObject.name == "Person")
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
                    && controls.SelectedObject.name == "Hand")
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
                        PlayerAnimationManager.PlayAnimation("UseRight SyringeWithNeedle");
                        actionManager.OnUseOnAction("SyringeWithAbsorptionNeedle", "");
                        return true; // fix for venting syringe
                    }
                    else if (inventory.RightHandEmpty())
                    {
                        PlayerAnimationManager.PlayAnimation("UseLeft SyringeWithNeedle");
                        actionManager.OnUseOnAction("SyringeWithAbsorptionNeedle", "");
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
