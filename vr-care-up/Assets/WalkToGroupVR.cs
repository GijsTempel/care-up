using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToGroupVR : MonoBehaviour
{

    public string walkToGroupName = "";
    public string description;
    PlayerScript player;
    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();

    }
    
    public void PlayerWalkedIn()
    {
        // Debug.Log("@WTG:" + walkToGroupName);
        if (player != null)
            player.UpdateWalkToGroup(walkToGroupName);
        DebugScreenControl debugScreenControl = GameObject.FindObjectOfType<DebugScreenControl>();
        if (debugScreenControl != null)
        {
            debugScreenControl.UpdateScreenPositionToWTG(walkToGroupName);
        }
    } 
}
