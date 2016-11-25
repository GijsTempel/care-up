using UnityEngine;
using System.Collections;

public class ExaminableObject : InteractableObject {

    public string state = "good";
    
    public void OnExamine()
    {
        actionManager.OnExamineAction(name, state);
    }
}

