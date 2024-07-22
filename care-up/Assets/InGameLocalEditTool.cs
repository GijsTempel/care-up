using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using CareUp.Localize;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System;
using System.Linq;
using SimpleJSON;


public class InGameLocalEditTool : MonoBehaviour
{
    public bool dataLoaded = false;
    public GameObject dictEditPanel;
    public Text keyTextLine;
    string currentKey = "";
    public TMP_InputField valueText;
    static InGameLocalEditTool _instance;
    bool ctrlKeyDown = false;
    bool isDictEditPopupOpen = false;
    const float popupWaitTime = 0.2f;
    float popupTimeOut = 0f;

    Dictionary<string, Dictionary<string, string>> changesToLocalization = 
        new Dictionary<string, Dictionary<string,string>>();
    List<string> activeDicts = new List<string>();


    void Start()
    {
        LoadExistingChanges();
    }

    public void ApplyChange()
    {
        string value = valueText.text;
        AddOrChangeValue(currentKey, value, true);
        RefrashTextElements();
        dictEditPanel.SetActive(false);
    }

    public void DeleteSelectedEntry()
    {
        DeleteDictElement(currentKey);
        RefrashTextElements();
        dictEditPanel.SetActive(false);
    }


    public void AddUILocalizationComponentToGO(GameObject go, string key)
    {
        if (LocalizationManager.CountKeysInText(key) == 0)
            return;
        key = LocalizationManager.StripBracketsFromKey(key);

        if (go.GetComponent<UILocalization>() == null)  
        {
            UILocalization newGoUILocalization = go.AddComponent<UILocalization>();
            newGoUILocalization.Initialization();
        }
        UILocalization goUILocalization = go.GetComponent<UILocalization>();    
        if (goUILocalization != null)
        {
            goUILocalization.key = key;
            goUILocalization.UpdateText();
        }
    }

    public string GetLocalizedValue(string key)
    {
        if (!dataLoaded)
            return "";
        string localName = LocalizationManager.GetCurrentDictPath(true);
        if (changesToLocalization.Keys.Contains(localName))
        {
            if (changesToLocalization[localName].Keys.Contains(key))
            {
                return changesToLocalization[localName][key];
            }
        }
        return "";
    }

    void SaveDictChanges(string dictName = "")
    {

        List<string> dictsToSave = new List<string>();
        if (dictName != "")
        {
            dictsToSave.Add(dictName);
        }
        else
        {
            foreach(string n in LocalizationManager.GetLocalizationNames())
                dictsToSave.Add(n);
        }
        foreach (string k in dictsToSave)
        {
            if (!changesToLocalization.Keys.Contains(k))
                continue;

            string filePath = GetFilePathFromDictName(k);
            JSONObject dataObj = new JSONObject();
            foreach (string dataKey in changesToLocalization[k].Keys)
            {
                dataObj.Add(dataKey, changesToLocalization[k][dataKey].Replace("\n", "<br>"));
            }
            File.WriteAllText(filePath, dataObj.ToString(4));
        }
    }
    string GetFilePathFromDictName(string dictName)
    {
        string dictFileName = dictName + "DictChanges.json";
        string filePath = Path.Combine(Application.persistentDataPath, dictFileName);
        return filePath;
    }

    void AddOrChangeValue(string key, string value, bool toSave = false, string dictKey = "")  
    {
        if (dictKey == "")
            dictKey = LocalizationManager.GetCurrentDictPath(true);
        if (!changesToLocalization.Keys.Contains(dictKey))
        {
            changesToLocalization.Add(dictKey, new Dictionary<string, string>());
        }
        changesToLocalization[dictKey][key] = value;
        if (toSave)
            SaveDictChanges(dictKey);
    }


    public void DeleteDictElement(string key)
    {
        string dictKey = LocalizationManager.GetCurrentDictPath(true);
        changesToLocalization[dictKey].Remove(key);
        SaveDictChanges(dictKey);
    }


    void RefrashTextElements()
    {
        foreach (UILocalization u in GameObject.FindObjectsOfType<UILocalization>(true))
        {
            u.UpdateText();
        }
    }

    void Update()
    {
        if (!PlayerPrefsManager.GetDevMode())
            return;
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ctrlKeyDown = true;
            popupTimeOut = popupWaitTime;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ctrlKeyDown = false;
            if (isDictEditPopupOpen)
            {
                transform.Find("Panel/DictEditorExtraPanel").GetComponent<Animation>().Play("dictEditClose");
                isDictEditPopupOpen = false;
            }
        }

        if (ctrlKeyDown && popupTimeOut > 0)
        {
            popupTimeOut -= Time.deltaTime;
            if (popupTimeOut < 0)
            {
                transform.Find("Panel/DictEditorExtraPanel").GetComponent<Animation>().Play("dictEditOpen");
                isDictEditPopupOpen = true;
            }
        }
    
    }

    void LoadExistingChanges()
    {
        if (dataLoaded)
            return;
        foreach(string localName in LocalizationManager.GetLocalizationNames())
        {
            string filePath = GetFilePathFromDictName(localName);
            if (!System.IO.File.Exists(filePath))
                continue;
            string jsonString = File.ReadAllText(filePath);
            JSONNode data = JSON.Parse(jsonString);
            foreach (string key in data.Keys)
            {
                AddOrChangeValue(key, 
                    data[key].ToString().Replace("<br>", "\n").Replace("\"",""), false, localName);
            }
        }
        dataLoaded = true;
        RefrashTextElements();
    }


    public void InitiateLocalEdit(string key)
    {
        string value = LocalizationManager.GetLocalizedValue(key);
        dictEditPanel.SetActive(true);
        string dictName = LocalizationManager.GetCurrentDictPath(true);
        keyTextLine.text = dictName + " : " + key;
        currentKey = key;
        valueText.text = value;
    }

    void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void HidePanel()
    {
        dictEditPanel.SetActive(false);
    }
    
    public void OpenRuntimeDictFolder()
    {
        Application.OpenURL("file://" + Application.persistentDataPath);
    }
}
