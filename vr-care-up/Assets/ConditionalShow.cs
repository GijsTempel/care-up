using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalShow : MonoBehaviour
{
    // Show objects if expected step in ActionExpectant is correct
    ActionExpectant actionExpectant;
    bool savedIsCurrentAction = false;
    public List<GameObject> objectsToShow = new List<GameObject>();
    void Start()
    {
        actionExpectant = GetComponent<ActionExpectant>();
        ShowObjects(actionExpectant.isCurrentAction);
    }

    // Update is called once per frame
    void Update()
    {
        if (savedIsCurrentAction != actionExpectant.isCurrentAction)
            ShowObjects(actionExpectant.isCurrentAction);
        savedIsCurrentAction = actionExpectant.isCurrentAction;
    }

    void ShowObjects(bool toShow)
    {
        foreach(GameObject g in objectsToShow)
            g.SetActive(toShow);
    }
}
