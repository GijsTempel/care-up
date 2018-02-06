using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviour {

    public GameObject playerPrefab;

    [System.Serializable]
    public struct InfoPair
    {
        public string buttonName;
        public string prefabName;
    };

    public List<InfoPair> infoList = new List<InfoPair>();
    
	void Awake () {
        GameObject player = Instantiate(playerPrefab,
            transform.position, transform.rotation);
        player.name = "Player";

        RobotUITabInfo infotab = GameObject.FindObjectOfType<RobotUITabInfo>();
        RobotUIInfoButton[] buttons = infotab.transform.Find("ItemList").GetComponentsInChildren<RobotUIInfoButton>();

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

        Destroy(gameObject);
	}
}
