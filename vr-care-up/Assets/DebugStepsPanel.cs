using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUp.Actions;
using Unity.VisualScripting;

public class DebugStepsPanel : MonoBehaviour
{

    List<ActionManagerStepTest> ActionStepButtons = new List<ActionManagerStepTest>();
    List<ActionManagerStepTest> complitedActionButtons = new List<ActionManagerStepTest>();
    public Transform contentTransform;
    ActionManager actionManager;
    float buildPanelTimer = 0;
    bool panelBuilt = false;
    int lastStepId = -1;
    float startTime;


    void Start()
    {
        startTime = Time.time;
        actionManager = GameObject.FindObjectOfType<ActionManager>();
    }

    public void buildActionsList()
    {
        if (actionManager != null)
        {
            foreach (Action a in actionManager.actionList)
            {
                GameObject ActionStep = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/ActionStepButton"), contentTransform);
                ActionStep.GetComponent<ActionManagerStepTest>().SetAction(a);
                ActionStepButtons.Add(ActionStep.GetComponent<ActionManagerStepTest>());
            }
        }
    }

    void Update()
    {
        if (panelBuilt)
        {
            if (lastStepId != actionManager.CurrentActionIndex)
            {
                int numberOfCompletedActions = actionManager.CompletedActions.Count;

                foreach (ActionManagerStepTest ab in ActionStepButtons)
                {
                    Action buttonAction = ab.GetAction();
                    if (numberOfCompletedActions > 0)
                    {
                        foreach(Action ca in actionManager.CompletedActions)
                        {
                            if (ca.compareActions(buttonAction) && ab.GetComplitTime() < 0f)
                            {
                                ab.SetCheckmark();
                                ab.SetCompliteTime(Time.time - startTime);
                            }
                        }
                    }
                    ab.UpdateLook(actionManager.CurrentActionIndex);
                    lastStepId = actionManager.CurrentActionIndex;
                }
            }
        }
        if (!panelBuilt)
        {
            buildPanelTimer += Time.deltaTime;

            if (buildPanelTimer > 0.1f)
            {
                buildActionsList();
                panelBuilt = true;
            }
        }

    }
}
