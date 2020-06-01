﻿using System.Collections.Generic;
using UnityEngine;

public class ExtraObjectOptions : MonoBehaviour
{
    public string TrashBin = "";

    public List<GameObject> hidenObjects;
    public List<GameObject> neededObjects;

    public Dictionary<string, string> neededObjectsData = new Dictionary<string, string>();
    public Dictionary<string, string> neededObjectsArticle = new Dictionary<string, string>();


    void Start()
    {
        foreach (GameObject obj in neededObjects)
        {
            if(obj != null)
            {
                if (obj.GetComponent<InteractableObject>() != null)
                {
                    if (!neededObjectsData.ContainsKey(obj.name))
                    {
                        neededObjectsData.Add(obj.name, obj.GetComponent<InteractableObject>().description);
                        neededObjectsArticle.Add(obj.name, obj.GetComponent<InteractableObject>().nameArticle);
                    }

                }
            }
        }       
    }

    public string HasNeeded(string str)
    {
        if (neededObjectsData.ContainsKey(str))
            return neededObjectsData[str];
        return "";
    }

    public string HasNeededArticle(string str)
    {
        if (neededObjectsArticle.ContainsKey(str))
            return neededObjectsArticle[str];
        return "";
    }

    public void _show(string _name, bool value)
    {
        foreach (GameObject o in hidenObjects)
        {
            if (o != null)
            {
                if (o.name == _name)
                {
                    o.SetActive(value);
                }
            }
        }
    }

    public void _toggle(string _name)
    {
        foreach (GameObject o in hidenObjects)
        {
            if (o.name == _name)
            {
                o.SetActive(!o.activeSelf);
            }
        }
    }
}
