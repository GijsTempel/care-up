using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToGroupButton : MonoBehaviour {

    WalkToGroup linkedWalkToGroup = null;

    public void setWalkToGroup(WalkToGroup wtg)
    {
        linkedWalkToGroup = wtg;
    }
    

    public void MoveToGroup()
    {
        if (linkedWalkToGroup != null)
        {
            GameObject.FindObjectOfType<PlayerScript>().WalkToGroup(linkedWalkToGroup);
        }
    }

}
