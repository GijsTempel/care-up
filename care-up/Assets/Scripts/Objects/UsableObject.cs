﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Static object that can be used
/// </summary>
[RequireComponent(typeof(Renderer))]
public class UsableObject : InteractableObject {

    [HideInInspector]
    public bool tutorial_used = false;
    public string PrefabToAppear = "";
    protected static HandsInventory handsInventory;

    protected override void Start()
    {
        base.Start();

        if (handsInventory == null)
        {
            handsInventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
            if (handsInventory == null) Debug.LogError("No inventory found");
        }
    }

    public virtual void Use()
    {
        if (!ViewModeActive())
        {
            // unique object, not usable, but picking performed instead
            if (PrefabToAppear != "")
            {
                TutorialManager tutorial = GameObject.Find("GameLogic").GetComponent<TutorialManager>();
                if (tutorial == null || (tutorial != null &&
                    (PrefabToAppear == tutorial.itemToPick || PrefabToAppear == tutorial.itemToPick2)))
                {
                    GameObject gameObject = handsInventory.CreateObjectByName(PrefabToAppear, Vector3.zero);
                    handsInventory.PickItem(gameObject.GetComponent<PickableObject>());
                    gameObject.GetComponent<PickableObject>().CreateGhostObject(true);
                    Reset();
                }
                return;
            }

            if (actionManager.CompareUseObject(name))
            {
                switch (name)
                {
                    case "HandCleaner":
                        {
                            InjectionPatient patient = GameObject.FindObjectOfType<InjectionPatient>();
                            if (patient != null)
                            {
                                patient.SkipGreetingDialogue();
                            }

                            CatherisationPatient catPatient = GameObject.FindObjectOfType<CatherisationPatient>();
                            if (catPatient != null)
                            {
                                catPatient.SkipGreetingDialogue();
                            }

                            string message = "Zorg voor een zorgvuldige handhygiëne. Handhygiëne is in dit protocol versneld om de gebruikerservaring te verbeteren";
                            RobotUIMessageTab messageCenter = GameObject.FindObjectOfType<RobotUIMessageTab>();
                            messageCenter.NewMessage("Zorgvuldige handhygiëne", message, RobotUIMessageTab.Icon.Info);
                        }
                        break;
                    case "OldBandAid":
                        {
                            Destroy(gameObject);
                        }
                        break;
                }

                PlayerAnimationManager.PlayAnimation("Use " + name, transform);
                tutorial_used = true;
            }

            actionManager.OnUseAction(gameObject.name);

            Reset();
        }
    }
}
