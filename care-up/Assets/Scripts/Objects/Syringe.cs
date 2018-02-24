using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Syringe : PickableObjectWithInfo {
    
    public bool updatePlunger = false;
    public bool updateProtector = false;

    private Transform plunger;
    private Transform protector;

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

        protector = transform.GetChild(1).Find("Prot");
    }

    protected override void Update()
    {
        base.Update();

        if ( updatePlunger )
        {
            plunger.localPosition = new Vector3(
                plunger.localPosition.x,
                Mathf.Lerp(-0.013f, 0.06f, leftControlBone.localPosition.y),
                plunger.localPosition.z);
        }

        if (updateProtector)
        {
            if (protector != null)
            {
                protector.localRotation = Quaternion.Euler(0, 0, 
                    -2.0f * Mathf.Rad2Deg * rightControlBone.localPosition.y);
            }
        }
    }

    public override bool Use(bool hand = false, bool noTarget = false)
    {
        tutorial_usedOn = true;
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
                    Debug.Log(SceneManager.GetActiveScene().name);
                    actionManager.OnUseOnAction("SyringeWithInjectionNeedleCap", "Patient");
                    if (SceneManager.GetActiveScene().name == "Injection" ||
                        SceneManager.GetActiveScene().name == "Injection_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection_disolve" ||
                        SceneManager.GetActiveScene().name == "Tutorial")
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
            else if ((name == "SyringeWithAbsorptionNeedle" ||
                     name == "SyringeWithInjectionNeedle") &&
                     controls.SelectedObject.name == "NeedleCup")
            {
                info = actionManager.CurrentUseOnInfo;
                if ((info[0] == "SyringeWithAbsorptionNeedle" ||
                    info[0] == "SyringeWithInjectionNeedle")
                    && info[1] == "NeedleCup")
                {
                    if (inventory.LeftHandEmpty())
                    {
                        controls.SelectedObject.GetComponent<PickableObject>().Reset();
                        PlayerAnimationManager.PlayAnimation("UseRight " + name + " " + "NeedleCup", 
                            controls.SelectedObject.transform);
                        actionManager.OnUseOnAction(name, "NeedleCup");
                        return true;
                    }
                    else if (inventory.RightHandEmpty())
                    {
                        controls.SelectedObject.GetComponent<PickableObject>().Reset();
                        PlayerAnimationManager.PlayAnimation("UseLeft " + name + " " + "NeedleCup", 
                            controls.SelectedObject.transform);
                        actionManager.OnUseOnAction(name, "NeedleCup");
                        return true;
                    }
                    else
                    {
                        EmptyHandsWarning();
                        return false;
                    }
                }
            }

            // venting
            if (name == "SyringeWithAbsorptionNeedle" && noTarget)
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
                    else
                    {
                        EmptyHandsWarning();
                        return false;
                    }
                }
            }
        }
        else // cannot interact or target == ""
        {
            if (name == "SyringeWithAbsorptionNeedle" && noTarget)
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
                    else
                    {
                        EmptyHandsWarning();
                        return false;
                    }
                }
            }
            else if (name == "SyringeWithAbsorptionSNeedle" && noTarget)
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
                    else
                    {
                        EmptyHandsWarning();
                        return false;
                    }
                }
            }
        }

        actionManager.OnUseOnAction(name, controls.SelectedObject != null ? controls.SelectedObject.name : "");

        return (info[0] == name && controls.SelectedObject != null && info[1] == controls.SelectedObject.name);
    }

    protected override void SetShaderTo(Shader shader)
    {
        base.SetShaderTo(shader);

        foreach (Material m in rend.materials)
        {
            m.renderQueue = 3000;
        }

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.name != "ParticleHint")
            {
                foreach (Material m in r.materials)
                {
                    m.renderQueue = 3000;
                }
            }
        }
    }

    public override void SaveInfo(ref Vector3 left, ref Vector3 right)
    {
        left = new Vector3(
            (updatePlunger ? 1.0f : 0.0f), 
            Mathf.InverseLerp(-0.013f, 0.06f, plunger.localPosition.y), 
            0.0f);
    }

    public override void LoadInfo(Vector3 left, Vector3 right)
    {
        updatePlunger = left.x == 1.0f ? true : false;
        
        if (plunger == null)
        {
            plunger = transform.Find("syringePlunger");
        }
    
        plunger.localPosition = new Vector3(
                plunger.localPosition.x,
                Mathf.Lerp(-0.013f, 0.06f, left.y),
                plunger.localPosition.z);
    }
}
