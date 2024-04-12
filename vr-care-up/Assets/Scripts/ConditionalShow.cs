using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalShow : MonoBehaviour
{
    // Show objects if expected step in ActionExpectant is correct
    ActionModule_ActionExpectant actionExpectant;
    PlayerScript player;
    bool savedIsCurrentAction = false;
    public bool hideOnPlayerAction = false;
    private bool forceUpdate = false;
    public List<GameObject> objectsToShow = new List<GameObject>();
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        actionExpectant = GetComponent<ActionModule_ActionExpectant>();
        ShowObjects(actionExpectant.isCurrentAction);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsInAction())
        {
            ShowObjects(false);
        }
        else
        {
            if (savedIsCurrentAction != actionExpectant.isCurrentAction || forceUpdate)
            {
                forceUpdate = false;
                ShowObjects(actionExpectant.isCurrentAction);
            }
            savedIsCurrentAction = actionExpectant.isCurrentAction;
        }
    }

    void ShowObjects(bool toShow)
    {
        foreach(GameObject g in objectsToShow)
        {
            if (!toShow && g.GetComponent<PickableObject>() != null && 
                player.GetHandWithThisObject(g) != null)
            {
                forceUpdate = true;
                continue;
            }
            g.SetActive(toShow);
        }
    }
}
