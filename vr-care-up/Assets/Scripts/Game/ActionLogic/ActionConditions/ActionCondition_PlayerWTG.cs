using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionCondition_PlayerWTG : MonoBehaviour
{
    public string walkToGroupName = "";
    public bool invert = false;
    PlayerScript player;
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
    }
    public bool Check()
    {
        if (player == null)
            return false;
        bool result = false;
        if (walkToGroupName == "")
            result =  true;
        else if (walkToGroupName == "-" && player.currentWTGName == "")
            result = true;
        else if (walkToGroupName == player.currentWTGName)
            result = true;

        if (invert)
            return !result;
        return result;
    }

}
