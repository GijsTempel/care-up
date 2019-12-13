using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace CareUp.Localize
{
    public static class LocalizationManager
    {
        static string[] dicts = new string[] {
            "TextData",
            "Text_Ostomy_Care",
            "TextData_AED"
        };

        //public static LocalizationManager instance;
        private static bool loadedDicts = false;
        private static GameObject gameLogic;
        private static Dictionary<string, string> localizedText;
        private static bool isReady = false;
        //private string missingTextString = "Text not found";

        // Use this for initialization

        public static void LoadLocalizedText(string fileName)
        {
            gameLogic = GameObject.Find("GameLogic");
            if (localizedText == null)
                localizedText = new Dictionary<string, string>();
            TextAsset _data = (TextAsset)Resources.Load("Dictionaries/" + fileName);
            string jsonString = _data.text;
            JSONNode data = JSON.Parse(jsonString);
            foreach (string key in data.Keys)
            {
                if (!localizedText.ContainsKey(key))
                    localizedText.Add(key, data[key]);
            }

            isReady = true;
        }

        public static void LoadAllDictionaries()
        {
            //Resources.Load("Dictionaries/");
            foreach (string fileName in dicts)
            {
                LoadLocalizedText(fileName);
            }
            loadedDicts = true;
        }

        public static string GetValueIfKey(string key)
        {
            if (!loadedDicts)
                LoadAllDictionaries();
            if (key == null)
                return "";
            if (key.Length == 0)
                return "";
            string result = key;
            bool debugMode = false;
            if (gameLogic != null)
            {
                debugMode = gameLogic.GetComponent<ActionManager>().TextDebug;
            }

            if (key[0] == '[')
            {
                string value = GetLocalizedValue(key.Substring(1, key.Length - 2));
                if (value != null)
                {
                    result = value;
                    if (Application.isEditor && debugMode)
                    {
                        result = key + value;
                    }
                }
            }
            return result;
        }

        public static string GetLocalizedValue(string key)
        {
            if (!loadedDicts)
                LoadAllDictionaries();
            if (localizedText != null)
            {
                if (localizedText.Keys.Count > 0)
                {
                    if (localizedText.ContainsKey(key))
                    {
                        return localizedText[key];
                    }
                }
            }
            return null;
        }

        public static bool GetIsReady()
        {
            return isReady;
        }
    }
}