using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUIMessageTab : RobotUITabs
{
    private List<GameObject> list;
    private Transform itemsParent;

    protected override void Start()
    {
        base.Start();

        list = new List<GameObject>();
        itemsParent = transform.Find("GeneralDynamicCanvas");
    }

    public void NewMessage(string text)
    {
        GameObject newObject = new GameObject("Item");
        newObject.transform.parent = itemsParent;

        newObject.AddComponent<Text>();
        newObject.GetComponent<Text>().text = text;

        // add script that controls if message seen?
        // newObject.AddComponent<blabla>();
        // newObject.GetComponent<blabla>().seen = false;
    }
}
