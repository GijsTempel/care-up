using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Text))]
public class RobotUIMessage : MonoBehaviour
{
    private bool messageNew = false;

    private string title;
    private string content;
    
    private static Text contentObject;
    private Text text;

    private void Init()
    {
        if (contentObject == null)
        {
            contentObject = GameObject.FindObjectOfType<RobotUIMessageTab>().transform.Find("GeneralDynamicCanvas")
                .Find("ScrollViewMessege").Find("Viewport").Find("Content").Find("Text").GetComponent<Text>();
        }

        text = transform.GetComponentInChildren<Text>();
    }

    public void OnClick()
    {
        Debug.Log("click");
        messageNew = false;

        if (contentObject)
        {
            contentObject.text = content;
        }

        text.color = Color.black;
    }

    public void NewMessage(string title, string message)
    {
        Init();

        messageNew = true;
        text.text = title;
        content = message;

        text.color = Color.red;
    }
}
