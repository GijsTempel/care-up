using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NWalkToGroupButton : MonoBehaviour
{
    //WalkToGroup linkedWalkToGroup = null; value never used
    Color ButtonColor = Color.white;
    //bool mouse_over = false; value never used
    GameUI gameUI;
    List<GameObject> icons = new List<GameObject>();
    PlayerScript ps;

    void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        ps = GameObject.FindObjectOfType<PlayerScript>();
        foreach(Transform _icon in transform.Find("nColor/Circle"))
        {
            icons.Add(_icon.gameObject);
        }
    }

    public void ButtonClicked()
    {

    }

    void UpdateLook()
    {

    }
}
