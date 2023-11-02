using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToGroupVR : MonoBehaviour
{

    public string walkToGroupName = "";
    public string description;
    PlayerScript player;
    public Transform teleportationAnchor;
    public GameObject iconObject;
    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
    }

    public Transform GetTeleportationAnchor()
    {
        return teleportationAnchor;
    }
    
    public void PlayerOut()
    {
        iconObject.SetActive(true);
        GetComponent<Collider>().enabled = true;
    }

    public void PlayerWalkedIn()
    {
        iconObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        
        DebugScreenControl debugScreenControl = GameObject.FindObjectOfType<DebugScreenControl>();
        if (debugScreenControl != null)
        {
            debugScreenControl.UpdateScreenPositionToWTG(walkToGroupName);
        }
    } 
}
