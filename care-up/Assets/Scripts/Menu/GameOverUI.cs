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
            transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => manager.OnRetryButtonClick());
            transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => manager.OnMainMenuButtonClick());
        }
    }
}
