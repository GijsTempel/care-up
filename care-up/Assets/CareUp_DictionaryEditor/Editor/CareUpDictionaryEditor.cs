using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System.Text;

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
        bool someKeysRepeat = false;
        Vector2 scrollPos = new Vector2();
        int selectedSet = -1;
        string searchText = "";
        List<WordToChangeData> wordsToChange = new List<WordToChangeData>();
        // List<WordToChangeData> keysToChange = new List<WordToChangeData>();
        int removeFromSet = -1;
        string keyToRemove = "";
        List<Dictionary<string, string>> setOfDictionaries = new List<Dictionary<string, string>>();
        string dictListFile = "dicts";
        string editValue = "";
        string editKey = "";
        string editControlKey = "";

        Dictionary<string, List<string>> keyUsageDict = new Dictionary<string, List<string>>();

        int editInSet = -1;
        string editOriginalKey = "";
        Vector2 TextEditorScroll = new Vector2();

        List<string> dictNames = new List<string>();
        [MenuItem("Tools/Dictionary Editor")]
        static void Init()
        {
            EditorWindow.GetWindow<DictionaryEditor>();
        }

        void SaveChanges(int _dictID = -1)
        {
            int startSaveFrom = 0;
            int endSaveOn = dictNames.Count;
            if (_dictID > -1)
            {
                startSaveFrom = _dictID;
                endSaveOn = _dictID + 1;
            }
            for (int i = startSaveFrom; i < endSaveOn; i++)
            {
                string dictName = dictNames[i];
                string actionFilePath = "Assets/Resources/Dictionaries/"+ dictName + ".json";

                var stringBuilder = new StringBuilder();
                stringBuilder.Append("{\n");
                foreach(string key in setOfDictionaries[i].Keys)
                {
                    stringBuilder.Append("  \"" + key + "\": \"" + setOfDictionaries[i][key] + "\",\n");
                }
                stringBuilder.Append("}");

                using (StreamWriter swriter = new StreamWriter(actionFilePath))
                    swriter.Write(stringBuilder.ToString());
            }
        }
/// <summary>
/// Closing a text editor with canceling or saving cahnges to the attribute
/// </summary>
/// <param name="toSave">To save or not to save</param>
        void ExitTextEdit(bool toSave = false)
        {
            if (toSave && editKey != "")
            {
                if (editKey == editControlKey)
                {
                    setOfDictionaries[editInSet][editKey] = editValue;
                }
                else
                {
                    setOfDictionaries[editInSet].Remove(editControlKey);
                    setOfDictionaries[editInSet].Add(editKey, editValue);
                }
            }
            editInSet = -1;
            GUI.FocusControl(null);
        }

        bool CheckForKeyCopies(string key, int _dictID)
        {
            for(int i = 0; i < setOfDictionaries.Count; i++)
            {
                if (i == _dictID)
                    continue;
                if (setOfDictionaries[i].ContainsKey(key))
                    return true;
            }
            return false;
        }

        void CheckInActionFiles()
        {
            keyUsageDict.Clear();
            var info = new DirectoryInfo("Assets/Resources/Xml/Actions");
            var fileInfo = info.GetFiles();
            List<string> actionFiles = new List<string>();
            List<TextAsset> textAssets = new List<TextAsset>();
            foreach (FileInfo file in fileInfo)
            {
                if (file.Extension == ".xml")
                {
                    actionFiles.Add(file.Name.Split('.')[0]);
                    TextAsset textAsset = Resources.Load("Xml/Actions/" + file.Name.Split('.')[0], typeof(TextAsset))  as TextAsset;
                    textAssets.Add(textAsset);
                }
            }
            // TextAsset textAsset = Resources.Load("Xml/Actions/Actions_AED", typeof(TextAsset))  as TextAsset;
            for(int i = 0; i < setOfDictionaries.Count; i++)
            {
                foreach(string key in setOfDictionaries[i].Keys)
                {
                    if (!keyUsageDict.ContainsKey(key))
                    {
                        List<string> usedInFiles = new List<string>();

                        foreach(TextAsset asset in textAssets)
                        {
                            if (asset.text.Contains(key))
                            {
                                usedInFiles.Add(asset.name);
                            }
                        }
                        if (usedInFiles.Count > 0)
                        {
                            keyUsageDict.Add(key, usedInFiles);
                        }
                    }
                }
            }
        }

        void OnGUI()
        {
            someKeysRepeat = false;
            GUIStyle warningStyle  = new GUIStyle();
            warningStyle.normal.textColor = Color.red;

            GUIStyle greenStyle  = new GUIStyle();
            greenStyle.normal.textColor = new Color(0f, 0.5f, 0f);

            GUIStyle horizontalLine;
            horizontalLine = new GUIStyle();
            horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            horizontalLine.margin = new RectOffset(0, 0, 4, 4);
            horizontalLine.fixedHeight = 1;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reload All Dictionary Data"))
                ReloadDictionaries();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("[ " + dictNames.Count.ToString() + " ] dictionaries found");
            if (editInSet != -1)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Cancel Edit"))
                    ExitTextEdit();
                if (GUILayout.Button("Save Edit"))
                    ExitTextEdit(true);

                EditorGUILayout.EndHorizontal();
                if (editInSet >= 0)
                    EditorGUILayout.LabelField("Editing key and value in dictionary [ " + dictNames[editInSet] + " ]", greenStyle);
                editKey = EditorGUILayout.TextField("Key: ", editKey);
                TextEditorScroll = EditorGUILayout.BeginScrollView(TextEditorScroll);
                editValue = EditorGUILayout.TextArea(editValue, GUILayout.Height(position.height - 30));
                EditorGUILayout.EndScrollView();
            }
            else
            {
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
                    string saveButtonText = "Save all changes";
                    if (selectedSet != -1)
                    {
                        saveButtonText = "Save [ " + dictNames[selectedSet] + " ]"; 
                    }
                    if (GUILayout.Button(saveButtonText))
                        SaveChanges();
                    if (GUILayout.Button("Check keys usage in action files"))
                        CheckInActionFiles();

                    List<string> dictsRollDownList = new List<string>();
                    dictsRollDownList.Add("__Show All__");
                    foreach(string d in dictNames)
                        dictsRollDownList.Add(d);
                    selectedSet = EditorGUILayout.Popup(selectedSet + 1, dictsRollDownList.ToArray()) - 1;
                    GUILayout.Box(GUIContent.none, horizontalLine);

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
                        EditorGUILayout.LabelField("Dictionary: [ " + dictNames[i] + " ]", greenStyle);
                        GUILayout.Box(GUIContent.none, horizontalLine);
                        if (GUILayout.Button("__Add Component__"))
                        {
                            editValue = "";
                            editKey = "";
                            editInSet = i;
                            editControlKey = editKey;
                            GUI.FocusControl(null);
                        }
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
                            int usageNum = 0;
                            if (keyUsageDict.ContainsKey(key))
                            {
                                usageNum = keyUsageDict[key].Count;
                                string tootTip = "";
                                foreach(string s in keyUsageDict[key])
                                    tootTip += s + "\n";
                                GUIContent usageText = new GUIContent("[ " + usageNum.ToString() + " ]", tootTip);

                                EditorGUILayout.LabelField(usageText, greenStyle, GUILayout.Width(30));
                            }
                            else
                                EditorGUILayout.LabelField("[ " + usageNum.ToString() + " ]", GUILayout.Width(30));

                            string _line = "[ " + key + " ] ";
                            if (CheckForKeyCopies(key, i))
                            {
                                someKeysRepeat = true;
                                EditorGUILayout.LabelField(_line, warningStyle);
                            }
                            else
                                EditorGUILayout.LabelField(_line);
                            UIWords.Add("");

                            EditorGUI.BeginChangeCheck();
                            UIWords[UIWords.Count - 1] = EditorGUILayout.TextField(setOfDictionaries[i][key]);
                            if (EditorGUI.EndChangeCheck())
                            {
                                wordsToChange.Add(new WordToChangeData(i,key,UIWords[UIWords.Count - 1]));
                            }
                            
                            if (GUILayout.Button("E", GUILayout.Width(22)))
                            {
                                editValue = setOfDictionaries[i][key];
                                editKey = key;
                                editInSet = i;
                                editControlKey = editKey;
                                GUI.FocusControl(null);
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
                    GUILayout.Box(GUIContent.none, horizontalLine);
                    if (someKeysRepeat)
                        EditorGUILayout.LabelField("Warning: Some Keys repeat in multiple dictionaries!", warningStyle);
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
        }

        void ReloadDictionaries()
        {
            dictNames.Clear();
            selectedSet = -1;
            editInSet = -1;
            setOfDictionaries.Clear();
            GUI.FocusControl(null);
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
