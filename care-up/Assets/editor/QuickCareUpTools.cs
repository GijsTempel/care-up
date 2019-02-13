using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuickCareUpTools
{
    [MenuItem("Tools/Start Action %#x")]
    private static void SetTriggers()
    {
        ActionStarter actionStarter = GameObject.FindObjectOfType<ActionStarter>();
        if (actionStarter != null)
        {
            actionStarter.StartAction();
        } 
    }
}