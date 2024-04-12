using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutilneControl : MonoBehaviour
{
    public List<Outline> outlines;
    PlayerScript player;

    void Start()
    {
        ShowOutline(false);
    }

    public void ShowOutline(bool toShow)
    {
        if (player == null)
            player = GameObject.FindObjectOfType<PlayerScript>();

        
        PickableObject pickableObject = GetComponentInParent<PickableObject>();
        if (pickableObject != null && player != null)
        {
            if (player.GetHandWithThisObject(pickableObject.gameObject) != null)
                toShow = false;
        }
        foreach(Outline outline in outlines)
        {
            outline.enabled = toShow;
        }
    }
}
