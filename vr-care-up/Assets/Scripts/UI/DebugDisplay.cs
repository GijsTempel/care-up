using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    Dictionary<string, string> debugLogs = new Dictionary<string, string>();


    public GameObject displayObject;
    public string marker = "";
    Text displayTextUnit;
    TextMeshProUGUI displayTextMeshUnit;


    int errorCount = 0;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        
    }

    void Start()
    {
        if (displayObject.GetComponent<Text>() != null)
            displayTextUnit = displayObject.GetComponent<Text>();
        else if (displayObject.GetComponent<TextMeshProUGUI>())
            displayTextMeshUnit = displayObject.GetComponent<TextMeshProUGUI>();
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }


    void HandleLog (string logString, string stackTrace, LogType type)
    {
        if (marker != ""){
            if (!logString.Contains(marker))
                return;
        }
        if (type == LogType.Error)
        {
            debugLogs.Add("Error" + errorCount.ToString(), logString);
            errorCount++;
        }
        if (type == LogType.Log) 
        {
            string[] splitString = logString.Split(char.Parse(":"));
            string debugKey = splitString[0];
            string debugValue = splitString.Length > 1 ? splitString[1] : "";

            if (debugLogs.ContainsKey(debugKey))
                debugLogs[debugKey] = debugValue;
            else
                debugLogs.Add(debugKey, debugValue);
        }
       
        string displayText = "";
        foreach (KeyValuePair<string, string> log in debugLogs) {
            string colorBegin = "";
            string colorEnd = "";
            if (displayTextMeshUnit != null)
            {
                colorEnd = "</color>";
                if (log.Key.Contains("Error"))
                    colorBegin = "<color=\"red\">";
                else
                    colorBegin = "<color=\"white\">";
            }
            
            displayText += colorBegin + log.Key + ": " + log.Value + colorEnd + "\n";
            
        }
        if (displayTextUnit != null)
            displayTextUnit.text = displayText;
        else if (displayTextMeshUnit != null)
            displayTextMeshUnit.text = displayText;
    }
}
