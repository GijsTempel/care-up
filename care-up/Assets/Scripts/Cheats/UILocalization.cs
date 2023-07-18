using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUp.Localize;
using UnityEngine.UI;
using TMPro;

public class UILocalization : MonoBehaviour
{
    public bool isDebugComponent = false;
    public string key = "";
    private Text text;
    private TextMeshProUGUI tPro;
    void Start()
    {
        if (isDebugComponent)
            Debug.Log("AAAA");
        text = GetComponent<Text>();
        tPro = GetComponent<TextMeshProUGUI>();
        SetText("$$$$$$$$$$$$$$$$");
        if (key != "")
        {
            string newText = LocalizationManager.GetLocalizedValue(key);
            if (newText != "")
            {
                SetText(newText);
            }
        }
    }

    void SetText(string value)
    {
        if (text != null)
            text.text = value;
        if (tPro != null)
            tPro.text = value;
    }
   
    string GetText()
    {
        text = GetComponent<Text>();
        tPro = GetComponent<TextMeshProUGUI>();
        string result = "";
        if (text != null)
            result = text.text;
        else if (tPro != null)
            result = tPro.text;
        return result;
    }
}
