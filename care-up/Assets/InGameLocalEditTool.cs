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
    public GameObject dictEditPanel;
    public Text keyTextLine;
    public TMP_InputField valueText;
    static InGameLocalEditTool _instance;

    Dictionary<string, Dictionary<string, string>> changesToLocalization = 
        new Dictionary<string, Dictionary<string,string>>();
    List<string> activeDicts = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        string dictFileName = LocalizationManager.GetCurrentDictPath(true) + "DictChanges.json";
        string filePath = Path.Combine(Application.persistentDataPath, dictFileName);
        Dictionary<string, string> testData = new Dictionary<string, string>();

        foreach(string lName in LocalizationManager.GetLocalizationNames())
        {
            changesToLocalization.Add(lName, new Dictionary<string, string>());
            changesToLocalization[lName].Add("test", "test123");
            changesToLocalization[lName].Add("test23", "test123");

        }
        File.WriteAllText(filePath, "Hey look here is some text.");
        SaveDictChanges();
    }

    void SaveDictChanges()
    {
        foreach (string k in changesToLocalization.Keys)
        {
            string dictFileName = k + "DictChanges.json";
            string filePath = Path.Combine(Application.persistentDataPath, dictFileName);
            JSONObject dataObj = new JSONObject();
            foreach (string dataKey in changesToLocalization[k].Keys)
            {
                dataObj.Add(dataKey, changesToLocalization[k][dataKey]);
            }
            File.WriteAllText(filePath, dataObj.ToString());
            Debug.Log(dataObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadExistingChanges()
    {
        List<string> localNames = LocalizationManager.GetLocalizationNames();
        
    }

    public void InitiateLocalEdit(string key)
    {
        string value = LocalizationManager.GetLocalizedValue(key);
        dictEditPanel.SetActive(true);
        keyTextLine.text = key;
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
