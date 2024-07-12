using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using CareUp.Localize;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography.X509Certificates;
using SFB;
using System.IO;
using System;

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
        string dictFileName = LocalizationManager.GetCurrentDictPaht(true) + "DictChanges.json";
        string filePath = Path.Combine(Application.persistentDataPath, dictFileName);
        File.WriteAllText(filePath, "Hey look here is some text.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadExistingChanges()
    {
        List<string> localNames = LocalizationManager.GetLocalNames();
        
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
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", Application.persistentDataPath, "", false);
    }
}
