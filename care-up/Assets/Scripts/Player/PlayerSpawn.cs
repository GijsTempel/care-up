﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviour {

    public string quizName;
    public GameObject playerPrefab;

    [System.Serializable]
    public struct InfoPair
    {
        public string buttonName;
        public string prefabName;
    };

    public List<InfoPair> infoList = new List<InfoPair>();

    void Awake()
    {
        GameObject player = Instantiate(playerPrefab,
            transform.position, transform.rotation);
        player.name = "Player";
        
        GameObject itemControls = Instantiate(Resources.Load("Prefabs/ItemControls") as GameObject,
            transform.position, transform.rotation);
        itemControls.name = "ItemControls";

        GameObject itemDescription = Instantiate(Resources.Load("Prefabs/ItemDescription") as GameObject,
            transform.position, transform.rotation);
        itemDescription.name = "ItemDescription";

        GameObject iPad = Instantiate(Resources.Load("Prefabs/ipad") as GameObject,
            transform.position, transform.rotation);
        iPad.name = "ipad";

        RobotUITabInfo infotab = GameObject.FindObjectOfType<RobotUITabInfo>();
        RobotUIInfoButton[] buttons = infotab.transform.GetChild(1).Find("ItemList").GetComponentsInChildren<RobotUIInfoButton>();

        foreach (RobotUIInfoButton b in buttons)
        {
            b.gameObject.SetActive(false);
        }

        for (int i = 0; i < infoList.Count && i < buttons.Length; ++i)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = infoList[i].buttonName;
            buttons[i].Set(infoList[i].prefabName);
        }

        if (quizName != "")
        {
            GameObject.FindObjectOfType<QuizTab>().Init(quizName);
        }
        else
        {
            Debug.LogWarning("Quiz file name is blank.");
        }

        iPad.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(
            player.GetComponent<PlayerScript>().CloseRobotUI);

        GameObject.FindObjectOfType<GameTimer>().SetTextObject(
            GameObject.Find("RobotUI").transform.Find("GeneralTab").Find("GeneralDynamicCanvas")
            .Find("Timer").Find("Panel").Find("Time").GetComponent<Text>());

        GameTimer.FindObjectOfType<ActionManager>().SetUIObjects(
            GameObject.Find("GeneralTab").transform.Find("GeneralDynamicCanvas").Find("Points").Find("Panel").Find("PointsText").GetComponent<Text>(),
            GameObject.Find("GeneralTab").transform.Find("GeneralDynamicCanvas").Find("Percentage").Find("Panel").Find("PointsText").GetComponent<Text>());

        Destroy(gameObject);
    }
}
