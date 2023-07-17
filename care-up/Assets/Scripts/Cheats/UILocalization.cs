using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUp.Localize;
using UnityEngine.UI;


public class UILocalization : MonoBehaviour
{
    public string key = "";
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
        if (text != null)
        {
            text.text = "&&&&&&&&&&";
        }
    }
}
