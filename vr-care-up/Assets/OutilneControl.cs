using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutilneControl : MonoBehaviour
{
    public List<Outline> outlines; 

    void Start()
    {
        ShowOutline(false);
    }

    public void ShowOutline(bool toShow)
    {
        foreach(Outline outline in outlines)
        {
            outline.enabled = toShow;
        }
    }
}
