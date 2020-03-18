using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SimpleJSON;

namespace CareUp.Localize
{
    public struct WordToChangeData
    {
        public WordToChangeData(int _dictID, string _key, string _newWord)
        {
            dictID = _dictID;
            key = _key;
            newWord = _newWord;
        }
        public int  dictID { get; set;}
        public string  key { get; set;}
        public string  newWord { get; set;}
    }

    class DictionaryEditor : EditorWindow
    {
        Vector2 scrollPos = new Vector2();
        int selectedSet = -1;
        string searchText = "";
        List<WordToChangeData> wordsToChange = new List<WordToChangeData>();
        // List<WordToChangeData> keysToChange = new List<WordToChangeData>();
        int removeFromSet = -1;
        string keyToRemove = "";
        List<Dictionary<string, string>> setOfDictionaries = new List<Dictionary<string, string>>();
        string dictListFile = "dicts";
        List<string> dictNames = new List<string>();
        [MenuItem("Tools/Dictionary Editor")]
        static void Init()
        {
            EditorWindow.GetWindow<DictionaryEditor>();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reload All Dictionary Data"))
                ReloadDictionaries();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("[ " + dictNames.Count.ToString() + " ] dictionaries found");
            List<string> UIWords = new List<string>();
            if (setOfDictionaries.Count > 0 && dictNames.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                searchText = EditorGUILayout.TextField(searchText);
                if (GUILayout.Button("C", GUILayout.Width(22)))
                {
                    GUI.FocusControl(null);
                    searchText = "";
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Show All"))
                    selectedSet = -1;

                selectedSet = EditorGUILayout.Popup(selectedSet, dictNames.ToArray());
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                int startFrom = 0;
                int endOn = dictNames.Count;
                if (selectedSet >= 0 && selectedSet < setOfDictionaries.Count)
                {
                    startFrom = selectedSet;
                    endOn = selectedSet + 1;
                }
                for(int i = startFrom; i < endOn; i++)
                {
                    EditorGUILayout.LabelField("___" + dictNames[i]);
                    foreach(string key in setOfDictionaries[i].Keys)
                    {
                        if (searchText != "")
                        {
                            if (searchText[0] == '@')
                            {
                                if (!key.ToLower().Contains(searchText.Substring(1)))
                                    continue;
                            }
                            else
                            {
                                if(!setOfDictionaries[i][key].ToLower().Contains(searchText))
                                    continue;
                            }
                        }
                        EditorGUILayout.BeginHorizontal();

                        string _line = "[ " + key + " ] ";
                        EditorGUILayout.LabelField(_line);
                        UIWords.Add("");

                        EditorGUI.BeginChangeCheck();
                        UIWords[UIWords.Count - 1] = EditorGUILayout.TextField(setOfDictionaries[i][key]);
                        if (EditorGUI.EndChangeCheck())
                        {
                            wordsToChange.Add(new WordToChangeData(i,key,UIWords[UIWords.Count - 1]));
                        }
                        if (GUILayout.Button("-", GUILayout.Width(22)))
                        {
                            removeFromSet = i;
                            keyToRemove = key;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView();
                if (wordsToChange.Count > 0)
                {
                    foreach(WordToChangeData _data in wordsToChange)
                    {
                        setOfDictionaries[_data.dictID][_data.key] = _data.newWord;
                    }
                    wordsToChange.Clear();
                }
                if (removeFromSet != -1 && keyToRemove != "")
                {
                    setOfDictionaries[removeFromSet].Remove(keyToRemove);
                    removeFromSet = -1;
                    keyToRemove = "";
                }
            }
        }

        void ReloadDictionaries()
        {
            dictNames.Clear();
            setOfDictionaries.Clear();
            TextAsset dictListData = (TextAsset)Resources.Load("Dictionaries/" + dictListFile);
            foreach(string dictName in dictListData.text.Split('\n'))
            {   
                if (!string.IsNullOrEmpty(dictName))
                {
                    dictNames.Add(dictName);
                }
            }
            foreach (string fileName in dictNames)
            {
                LoadDictionary(fileName);
            }
        }

        void LoadDictionary(string fileName)
        {
            Dictionary<string, string> currentDict = new Dictionary<string, string>();
            TextAsset _data = (TextAsset)Resources.Load("Dictionaries/" + fileName);
            if (_data != null)
            {
                string jsonString = _data.text;
                JSONNode data = JSON.Parse(jsonString);
                foreach (string key in data.Keys)
                {
                    if (!currentDict.ContainsKey(key))
                        currentDict.Add(key, data[key]);
                }
            }
            setOfDictionaries.Add(currentDict);
        }
    }
}
