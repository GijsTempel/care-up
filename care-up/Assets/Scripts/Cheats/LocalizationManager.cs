using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Text not found";

    // Use this for initialization

    public void LoadLocalizedText (string fileName) {
        if (localizedText == null)
            localizedText = new Dictionary<string, string> ();
        string filePath = Path.Combine (Application.streamingAssetsPath, fileName);

        if (File.Exists (filePath)) {
            string jsonString = File.ReadAllText (filePath);
            JSONNode data = JSON.Parse (jsonString);
            foreach (string key in data.Keys) {
                if (!localizedText.ContainsKey (key))
                    localizedText.Add (key, data[key]);
            }

        } else {
            Debug.LogError ("Cannot find file!");
        }

        isReady = true;
    }

    public string GetValueIfKey (string key) {
        if (key[0] == '[') {
            string value = GetLocalizedValue (key.Substring (1, key.Length - 2));
            if (value != null)
                return value;
        }
        return key;
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