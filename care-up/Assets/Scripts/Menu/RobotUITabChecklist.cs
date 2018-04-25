﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUITabChecklist : RobotUITabs {

    private static Text[] items;
    //private static int current = 0;

    protected override void Start()
    {
        base.Start();
        items = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).transform.GetComponentsInChildren<Text>(true);

        foreach(Text i in items)
        {
            i.text = "";
        }

        int index = 0;
        ActionManager manager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        foreach (CareUp.Actions.Action a in manager.ActionList)
        {
            if (index < items.Length)
            {
                items[index++].text = a.shortDescr;
            }
        }

        //current = 0;
    }

    /* old one
    public static void StrikeStep()
    {
        if (current >= items.Length)
            return;

        items[current].text = StrikeThrough(items[current].text);
        ++current;
    } */

    public static void StrikeStep(int index)
    {
        if (index < 0 || index >= items.Length)
        {
            Debug.LogWarning("Index out of range.");
            return;
        }

        items[index].text = StrikeThrough(items[index].text);
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
