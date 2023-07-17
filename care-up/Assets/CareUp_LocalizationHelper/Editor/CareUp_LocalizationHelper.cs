using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUp.Localize;
using UnityEditor;

public class CareUp_LocalizationHelper
{
    [MenuItem("Tools/CareUp Localization/Find Keys")]
    private static void SetTriggers()
    {
        foreach(UILocalization t in GameObject.FindObjectsOfType<UILocalization>())
        {
            Text text = t.GetComponent<Text>();
            
            if (text != null)
            {
                string path = GetGameObjectPath(t.gameObject);
                Debug.Log(text.text + " | " + path);
            }
        }
    }
    
    private static string GetGameObjectPath(GameObject go)
    {
        if (go == null)
            return "";

        var path = "/" + go.name;
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = string.Format("{0}{1}", "/" + go.name, path);
        }
        return path;
    }
}
