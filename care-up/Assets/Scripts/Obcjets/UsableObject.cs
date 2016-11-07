using UnityEngine;
using System.Collections;

public class UsableObject : InteractableObject {
    
    static private ActionManager actionManager;

    protected override void Start () {
        base.Start();

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");
    }
    
    public void Use()
    {
        actionManager.OnUseAction(gameObject.name);
    }
}
