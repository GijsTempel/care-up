using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using CareUp.Localize;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography.X509Certificates;
using SFB;

public class InGameLocalEditTool : MonoBehaviour
{
    public GameObject dictEditPanel;
    public Text keyTextLine;
    public TMP_InputField valueText;
    static InGameLocalEditTool _instance;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
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
