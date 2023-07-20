using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CareUp.Localize;
using System.Linq;


public class CareUpLocalizationHelper : EditorWindow
{
    string localizationDebugInfo = "";
    bool toForceUpdateKeys = false;
    string uiOrganizeDictFileName = "";

    [MenuItem("Tools/CareUp Localization/Localization Helper")]
    static void Init()
    {
        EditorWindow.GetWindow<CareUpLocalizationHelper>();
    }

    void OnGUI()
    {
        var defaultColor = GUI.backgroundColor;
        if (GUILayout.Button("Update scene localization statistics"))
            localizationDebugInfo = GetSceneLocalizaionDebugInfo();
        EditorGUILayout.LabelField(localizationDebugInfo);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Force update all keys: ", GUILayout.Width(130));
        toForceUpdateKeys = EditorGUILayout.Toggle(toForceUpdateKeys, GUILayout.Width(60));
        EditorGUILayout.LabelField("Dictionary file name: ", GUILayout.Width(130));
        uiOrganizeDictFileName = EditorGUILayout.TextField(uiOrganizeDictFileName);

        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Collect, process and store text from UI"))
            localizationDebugInfo = ProcessUITextDataToDict(uiOrganizeDictFileName);
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Force clear UILocalization keys"))
            localizationDebugInfo = ClearKeys();
        GUI.backgroundColor = defaultColor;

    }

    string ClearKeys()
    {
        int keysCount = 0;
        string logText = "";
        foreach (UILocalization uil in GameObject.FindObjectsOfType<UILocalization>(true))
        {
            uil.key = "";
            keysCount++;
        }
        logText = "Keys cleared:" + keysCount.ToString();
        return logText;
    }

    string ProcessUITextDataToDict(string dictName)
    {
        if (dictName == "")
            return "No JSON file name added!";
        List<UILocalization> elements = new List<UILocalization>();
        List<string> textData = new List<string>();

        DictionaryEditor.ClearDicts();
        DictionaryEditor.SetLocalizationIndex(0);
        DictionaryEditor.AddDictName(dictName);
        DictionaryEditor.LoadDictionary(DictionaryEditor.GetDictName(0).Replace("\r", ""));
        
        Dictionary<string, string> currentDict = DictionaryEditor.GetDict(0);
        int elementsFound = 0;
        int keysAssigned = 0;
        int emptyTextObjects = 0;
        foreach(UILocalization uil in GameObject.FindObjectsOfType<UILocalization>(true))
        {
            elementsFound++;
            bool toProcessElement = false;
            if (!toForceUpdateKeys)
            {
                if (uil.key == "" || !(currentDict.ContainsKey(uil.key)))
                    toProcessElement = true;
            }
            else
                toProcessElement = true;
            
            if (toProcessElement)
            {

                string currentText = uil.GetText().Replace("\"", "“").Replace("\r", "");
                if (currentText == "") 
                {
                    emptyTextObjects++;
                    continue;
                }
                string keyFound = DictionaryEditor.GetKeyIfTextInDict(currentDict, currentText);
                string currentKey = keyFound;
                if (keyFound == "")
                {
                    currentKey = DictionaryEditor.GenerateUniqueKey("ui_", currentDict.Keys.ToList(), currentText);
                    currentDict.Add(currentKey, currentText);
                    keysAssigned++;
                }

                uil.key = currentKey;
            }
        }
        string logText = "UI Elements Found: [" + elementsFound.ToString() + "] Keys Assigned: [" + keysAssigned.ToString() + "]";
        logText += " Empty texts: [" + emptyTextObjects.ToString() + "]";
        DictionaryEditor.SetDictionary(currentDict, 0);
        DictionaryEditor.SaveChanges(0);
        DictionaryEditor.SetSelectedDictName(dictName);
        return logText;
    }


    string GetSceneLocalizaionDebugInfo()
    {
        string _info = "";
        int uiKeysCount = 0;
        int uiLocalizationComponentsCount = 0;
        int emptyTextObjects = 0;
        foreach(UILocalization uil in GameObject.FindObjectsOfType<UILocalization>(true))
        {
            uiLocalizationComponentsCount++;
            if (uil.key != "")
                uiKeysCount++;
            if (uil.GetText() == "")
                emptyTextObjects++;

        }
        _info += "Localization Components found: [" + uiLocalizationComponentsCount.ToString() + "]";
        _info += " Keys found: [" + uiKeysCount.ToString() + "]";
        _info += " Empty texts: [" + emptyTextObjects.ToString() + "]";

        return _info;
    }

}
