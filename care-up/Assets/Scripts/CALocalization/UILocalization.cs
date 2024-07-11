using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUp.Localize;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class UILocalization : MonoBehaviour
{
    public bool isDebugComponent = false;
    public string key = "";
    private Text text;
    private TextMeshProUGUI tPro;
    private UILocalEditButton localEditButton;

    void Start()
    {
        if (isDebugComponent)
            Debug.Log("AAAA");
        text = GetComponent<Text>();
        tPro = GetComponent<TextMeshProUGUI>();
        SetText("$$$$$$$$$$$$$$$$");
        UpdateText();
        GameObject localHoverImageGO = Instantiate(
            Resources.Load<GameObject>("NecessaryPrefabs/UI/UILocalHoverImage"), transform) as GameObject;

        GameObject localEditButtonGO = Instantiate(
            Resources.Load<GameObject>("NecessaryPrefabs/UI/UILocalEditButton"), transform) as GameObject;
        localEditButton = localEditButtonGO.GetComponent<UILocalEditButton>();
        localEditButton.SetHoverImage(localHoverImageGO);
        localHoverImageGO.SetActive(false);
        localEditButton.SetUILocalization(this);
    }
    void OnEnable()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        if (key != "")
        {
            string newText = LocalizationManager.GetLocalizedValue(key);
            if (newText != "")
            {
                SetText("><><" + newText);
            }
        }
    }

    public void InitiateLocalEdit()
    {
        InGameLocalEditTool localTool = GameObject.FindObjectOfType<InGameLocalEditTool>();
        if (localEditButton != null)
        {
            localTool.InitiateLocalEdit(key);
        }
    }

    public void SetText(string value)
    {
        if (text != null)
            text.text = value;
        if (tPro != null)
            tPro.text = value;
    }
   
    public string GetText()
    {
        text = GetComponent<Text>();
        tPro = GetComponent<TextMeshProUGUI>();
        string result = "";
        if (text != null)
            result = text.text;
        else if (tPro != null)
            result = tPro.text;
        return result;
    }

    void Update()
    {
        localEditButton.SetActive(Input.GetKey(KeyCode.LeftControl) == true);
    }
}
