using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

public class LocalizationManager {
    string[] dicts = new string[] {
        "TextData"
    };

    public static LocalizationManager instance;
    GameObject gameLogic;
    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    //private string missingTextString = "Text not found";

    // Use this for initialization

    public void LoadLocalizedText (string fileName) {
        gameLogic = GameObject.Find("GameLogic");
        if (localizedText == null)
            localizedText = new Dictionary<string, string> ();
        TextAsset _data = (TextAsset)Resources.Load("Dictionaries/" + fileName);
        string jsonString = _data.text;
        JSONNode data = JSON.Parse (jsonString);
        foreach (string key in data.Keys) {
            if (!localizedText.ContainsKey (key))
                localizedText.Add (key, data[key]);
        }

        isReady = true;
    }

    public void LoadAllDictionaries()
    {
        //Resources.Load("Dictionaries/");
        foreach (string fileName in dicts) {
            LoadLocalizedText(fileName);
        }
    }

    public string GetValueIfKey (string key) {
        if (key == null)
            return "";
        if (key.Length == 0)
            return "";
        string result = key;
        bool debugMode = false;
        if (gameLogic != null){
            debugMode = gameLogic.GetComponent<ActionManager>().TextDebug;
        }

        if (key[0] == '[') {
            string value = GetLocalizedValue (key.Substring (1, key.Length - 2));
            if (value != null){
                result = value;
                if (Application.isEditor && debugMode){
                    result = key + value;
                }
            }
        }
        return result;
    }

    public string GetLocalizedValue (string key) {
        if (localizedText != null) {
            if (localizedText.Keys.Count > 0) {
                if (localizedText.ContainsKey (key)) {
                    return localizedText[key];
                }
            }
        }
        return null;
    }

    public bool GetIsReady () {
        return isReady;
    }

}