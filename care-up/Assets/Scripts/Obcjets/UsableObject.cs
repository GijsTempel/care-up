using UnityEngine;
using System.Collections;

public class UsableObject : MonoBehaviour {

    static private ActionManager actionManager;

	void Start () {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");
	}

    public void Use()
    {
        actionManager.OnUseAction(gameObject.name);
    }
}
