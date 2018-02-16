using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUITabChecklist : RobotUITabs {

    private static Text[] items;
    private static int current = 0;

    protected override void Start()
    {
        base.Start();
        items = transform.GetChild(1).transform.GetComponentsInChildren<Text>(true);

        foreach(Text i in items)
        {
            i.text = "";
        }

        int index = 0;
        ActionManager manager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        foreach(CareUp.Actions.Action a in manager.ActionList)
        {
            items[index++].text = a.shortDescr;
        }
    }

    public static void StrikeStep()
    {
        if (current >= items.Length)
            return;

        items[current].text = StrikeThrough(items[current].text);
        ++current;
    }

    private static string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }
}
