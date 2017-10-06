using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    ActionManager manager;

    // Use this for initialization
    void Start()
    {

        manager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (manager == null) Debug.LogError("No camera mode");
        else
        {
            transform.GetChild(0).Find("Retry").GetComponent<Button>().onClick.AddListener(() => manager.OnRetryButtonClick());
            transform.GetChild(0).Find("Menu").GetComponent<Button>().onClick.AddListener(() => manager.OnMainMenuButtonClick());
        }
    }

    public void SetDescription(string value)
    {
        transform.Find("Panel").Find("Description").GetComponent<Text>().text = value;
    }
}
