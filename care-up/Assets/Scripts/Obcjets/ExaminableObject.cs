using UnityEngine;
using System.Collections;

public class ExaminableObject : InteractableObject {

    public string state = "good";

    private static ActionManager actionManager;

    protected override void Start()
    {
        base.Start();

        if (actionManager == null)
        {
            actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
            if (actionManager == null) Debug.LogError("No action manager found");
        }
    }

    public void OnExamine()
    {
        actionManager.OnExamineAction(name, state);
    }
}

