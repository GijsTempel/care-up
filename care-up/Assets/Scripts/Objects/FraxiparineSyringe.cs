using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FraxiparineSyringe : PickableObjectWithInfo
{
    public bool updatePlunger = false;
    public bool updateTube = false;

    private Transform plunger;
    private Transform tube;

    private float loPlunger = -0.0f;
    private float hiPlunger =  0.06f;
    private float loTube    = 0.00f;
    private float hiTube    =  -0.06f;

    public override void SaveInfo(ref Vector3 left, ref Vector3 right)
    {
        left = new Vector3(
            (updatePlunger ? 1.0f : 0.0f),
            Mathf.InverseLerp(loPlunger, hiPlunger, plunger.localPosition.y),
            0.0f);

        right = new Vector3(
            (updateTube ? 1.0f : 0.0f),
            Mathf.InverseLerp(loTube, hiTube, tube.localPosition.y),
            0.0f);
    }

    public override void LoadInfo(Vector3 left, Vector3 right)
    {
        updatePlunger = left.x == 1.0f ? true : false;

        if (plunger == null)
        {
            plunger = transform.Find("Fraxi_plunger");
        }

        plunger.localPosition = new Vector3(
                plunger.localPosition.x,
                Mathf.Lerp(loPlunger, hiPlunger, left.y),
                plunger.localPosition.z);

        updateTube = right.x == 1.0f ? true : false;

        if (tube == null)
        {
            tube = transform.Find("Frexi_tube");
        }

        tube.localPosition = new Vector3(
            tube.localPosition.x,
            Mathf.Lerp(loTube, hiTube, right.y),
            tube.localPosition.z);
    }

    protected override void Start()
    {
        base.Start();

        plunger = transform.Find("Fraxi_plunger");
        if (plunger == null) Debug.LogError("No plunger found!");

        tube = transform.Find("Frexi_tube");
        if (tube == null) Debug.LogError("No protecting tube found!");

        transparencyFix = true;
    }

    protected override void Update()
    {     
        if (updatePlunger)
        {
            plunger.localPosition = new Vector3(
                plunger.localPosition.x,
                Mathf.Lerp(loPlunger, hiPlunger, leftControlBone.localPosition.y),
                plunger.localPosition.z);
        }

        if (updateTube)
        {
            if (tube != null)
            {
                tube.localPosition = new Vector3(
                    plunger.localPosition.x,
                    Mathf.Lerp(loTube, hiTube, rightControlBone.localPosition.y),
                    plunger.localPosition.z);
            }
        }
    }

    public override bool Use(bool hand = false, bool noTarget = false)
    {
        tutorial_usedOn = true;

        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if (name == "Frexi_with_needle_cap"
                && controls.SelectedObject.GetComponent<PersonObjectPart>() != null
                && controls.SelectedObject.GetComponent<PersonObjectPart>().Person.name == "Patient")
            {
                if (actionManager.CompareUseOnInfo("Frexi_with_needle_cap", "Patient"))
                {
                    actionManager.OnUseOnAction("Frexi_with_needle_cap", "Patient");
                    if (SceneManager.GetActiveScene().name == "Fraxiparine_Injecteren")
                    {
                        Transform target = controls.SelectedObject.GetComponent<PersonObjectPart>().Person;
                        target.GetComponent<InteractableObject>().Reset();
                        controls.ResetObject();
                        PlayerAnimationManager.PlayAnimationSequence("FraxiparineInjection", target);
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
            else if (name == "Frexi" && controls.SelectedObject.name == "NeedleCup" 
                && actionManager.CompareUseOnInfo("Frexi","NeedleCup"))
            {
                string animation = (hand ? "UseLeft " : "UseRight ") + name + " NeedleCup";
                PlayerAnimationManager.PlayAnimation(animation, GameObject.Find("NeedleCup").transform);
                actionManager.OnUseOnAction(name, "NeedleCup");
                return true;
            }

        }

        // venting && tube
        if ((name == "Frexi_with_needle_cap" || name == "Frexi") && noTarget)
        {
            if (actionManager.CompareUseOnInfo("Frexi_with_needle_cap", "") || 
                 actionManager.CompareUseOnInfo("Frexi", ""))
            {
                if (inventory.LeftHandEmpty())
                {
                    PlayerAnimationManager.PlayAnimation("UseRight " + name);
                    actionManager.OnUseOnAction(name, "");
                    return true; // fix for venting syringe
                }
                else if (inventory.RightHandEmpty())
                {
                    PlayerAnimationManager.PlayAnimation("UseLeft " + name);
                    actionManager.OnUseOnAction(name, "");
                    return true; // fix for venting syringe
                }
                else
                {
                    EmptyHandsWarning();
                    return false;
                }
            }
        }

        actionManager.OnUseOnAction(name, controls.SelectedObject != null ? controls.SelectedObject.name : "");

        return (controls.SelectedObject != null && actionManager.CompareUseOnInfo(name, controls.SelectedObject.name));
    }
}