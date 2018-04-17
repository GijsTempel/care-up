﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FraxiparineSyringe : PickableObjectWithInfo
{

    public bool updatePlunger = false;
    public bool updateTube = false;

    private Transform plunger;
    private Transform tube;

    public override void SaveInfo(ref Vector3 left, ref Vector3 right)
    {
        left = new Vector3(
            (updatePlunger ? 1.0f : 0.0f),
            Mathf.InverseLerp(-0.013f, 0.06f, plunger.localPosition.y),
            0.0f);

        right = new Vector3(
            (updateTube ? 1.0f : 0.0f),
            Mathf.InverseLerp(-0.013f, 0.06f, tube.localPosition.y),
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
                Mathf.Lerp(-0.013f, 0.06f, left.y),
                plunger.localPosition.z);

        updateTube = right.x == 1.0f ? true : false;

        if (tube == null)
        {
            tube = transform.Find("Frexi_tube");
        }

        tube.localPosition = new Vector3(
            tube.localPosition.x,
            Mathf.Lerp(-0.013f, 0.06f, right.y),
            tube.localPosition.z);
    }

    protected override void Start()
    {
        base.Start();

        plunger = transform.Find("Fraxi_plunger");
        if (plunger == null) Debug.LogError("No plunger found!");

        tube = transform.Find("Frexi_tube");
        if (tube == null) Debug.LogError("No protecting tube found!");
    }

    protected override void Update()
    {
        base.Update();

        if (updatePlunger)
        {
            plunger.localPosition = new Vector3(
                plunger.localPosition.x,
                Mathf.Lerp(-0.013f, 0.06f, leftControlBone.localPosition.y),
                plunger.localPosition.z);
        }

        if (updateTube)
        {
            if (tube != null)
            {
                tube.localPosition = new Vector3(
                    plunger.localPosition.x,
                    Mathf.Lerp(-0.013f, 0.06f, rightControlBone.localPosition.y),
                    plunger.localPosition.z);
            }
        }
    }

    public override bool Use(bool hand = false, bool noTarget = false)
    {
        tutorial_usedOn = true;
        string[] info = actionManager.CurrentUseOnInfo;

        if (controls.SelectedObject != null && controls.CanInteract)
        {
            if (name == "Frexi" && controls.SelectedObject.name == "NeedleCup" && info[1] == "NeedleCup")
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
            info = actionManager.CurrentUseOnInfo;
            if ((info[0] == "Frexi_with_needle_cap" || info[0] == "Frexi") && info[1] == "")
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

        return (info[0] == name && controls.SelectedObject != null && info[1] == controls.SelectedObject.name);
    }
}