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

        transparencyFix = true;
    }

    protected override void Update()
    {
        base.Update();

        if ( updatePlunger )
        {
            float plunger_pos = 0f;
            if (leftControlBone != null)
            {
                plunger_pos = leftControlBone.localPosition.y;
            }
            else
            {
                plunger_pos = GameObject.FindObjectOfType<PlayerAnimationManager>().leftModifier02;
            }
            plunger.localPosition = new Vector3(
                plunger.localPosition.x,
                
                Mathf.Lerp(-0.013f, 0.06f, plunger_pos),
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
        GameObject TempSelected = controls.SelectedObject;
        if (noTarget)
            TempSelected = null;

        if (TempSelected != null && controls.CanInteract)
        {
            if (name == "SyringeWithInjectionSNeedleCap"
                && TempSelected.GetComponent<PersonObjectPart>() != null
                && TempSelected.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (actionManager.CompareUseOnInfo("SyringeWithInjectionSNeedleCap", "Patient"))
                {
                    actionManager.OnUseOnAction("SyringeWithInjectionSNeedleCap", "Patient");
                    if (SceneManager.GetActiveScene().name == "Injection Subcutaneous v2" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous v2_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous v2_desolve")
                    {
                        Transform target = TempSelected.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("SubcutaneousInjection v2", target);
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection scene v2")
                    {
                        Transform target = TempSelected.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("Injection v2", target);
                    }
                    return true;
                }
            }
            else if (name == "SyringeWithInjectionNeedleCap"
                && TempSelected.GetComponent<PersonObjectPart>() != null
                && TempSelected.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (actionManager.CompareUseOnInfo("SyringeWithInjectionNeedleCap", "Patient"))
                {
					string triggerName = "";
					
                    actionManager.OnUseOnAction("SyringeWithInjectionNeedleCap", "Patient");
                    if (SceneManager.GetActiveScene().name == "Injection" ||
                        SceneManager.GetActiveScene().name == "Injection_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection_disolve" ||
                        SceneManager.GetActiveScene().name == "Tutorial_Sequence")
                    {
                        Transform target = TempSelected.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("Injection", target);
						
						triggerName = "ShowArm";
                    }
                    else if (SceneManager.GetActiveScene().name == "Injection Subcutaneous" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous_ampoule" ||
                        SceneManager.GetActiveScene().name == "Injection Subcutaneous_desolve")
                    {
                        Transform target = TempSelected.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("SubcutaneousInjection", target);
						
						triggerName = "ShowBellyForInsulin";
                    }

                    PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
                    if (manager != null && !manager.practiceMode)
                    {
                        InjectionPatient patient = GameObject.FindObjectOfType<InjectionPatient>();
                        if (patient.pulledUp == false)
                        {
                            patient.pulledUp = true;
                            patient.GetComponent<Animator>().SetTrigger(triggerName);
                        }
                    }

                    return true;
                }
            }
            else if (name == "Syringe" && TempSelected.name == "Person")
            {
                if (actionManager.CompareUseOnInfo("Syringe", "Person"))
                {
                    actionManager.OnUseOnAction("Syringe", "Person");
                    AnimationSequence animationSequence = new AnimationSequence("WingedNeedle");
                    animationSequence.NextStep();
                    return true;
                }
            }
            else if ((name == "SyringeWithAbsorptionNeedle" ||
                     name == "SyringeWithInjectionNeedle") &&
                     TempSelected.name == "NeedleCup")
            {
                if (actionManager.CompareUseOnInfo("SyringeWithAbsorptionNeedle", "NeedleCup") ||
                    actionManager.CompareUseOnInfo("SyringeWithInjectionNeedle", "NeedleCup"))
                {
                    if (inventory.LeftHandEmpty())
                    {
                        TempSelected.GetComponent<PickableObject>().Reset();
                        PlayerAnimationManager.PlayAnimation("UseRight " + name + " " + "NeedleCup", 
                            TempSelected.transform);
                        actionManager.OnUseOnAction(name, "NeedleCup");
                        return true;
                    }
                    else if (inventory.RightHandEmpty())
                    {
                        TempSelected.GetComponent<PickableObject>().Reset();
                        PlayerAnimationManager.PlayAnimation("UseLeft " + name + " " + "NeedleCup", 
                            TempSelected.transform);
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
        }
        else // cannot interact or target == ""
        {
            if (noTarget) {
                if (actionManager.CompareUseOnInfo(name, ""))
                {
                    if (inventory.rightHandObject == this)
                    {
                        PlayerAnimationManager.PlayAnimation("UseRight " + name);
                        actionManager.OnUseOnAction(name, "");
                        return true; 
                    }
                    if (inventory.leftHandObject == this)
                    {
                        PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                        actionManager.OnUseOnAction(name, "");
                        return true;
                    }
                }
            }
        }

        actionManager.OnUseOnAction(name, TempSelected != null ? TempSelected.name : "");

        return (TempSelected != null && actionManager.CompareUseOnInfo(name, TempSelected.name));
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
