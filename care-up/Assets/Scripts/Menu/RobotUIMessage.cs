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
    private Image icon;

    private void Init()
    {
        if (contentObject == null)
        {
            contentObject = GameObject.FindObjectOfType<RobotUIMessageTab>().transform.Find("GeneralDynamicCanvas")
                .Find("ScrollViewMessege").Find("Viewport").Find("Content").Find("Text").GetComponent<Text>();
        }

        text = transform.GetComponentInChildren<Text>();
        icon = transform.Find("Image").GetComponent<Image>();
    }

    public void OnClick()
    {
        if (messageNew)
        {
            messageNew = false;
            text.color = Color.black;
            RobotManager.SetNotification(RobotManager.NotificationNumber - 1);
        }

        if (contentObject)
        {
            contentObject.text = content;
        }
    }

    public void NewMessage(string title, string message, Sprite i)
    {
        Init();

        messageNew = true;
        text.text = title;
        content = message;

        icon.sprite = i;

        text.color = Color.red;

        transform.SetSiblingIndex(0);
    }
}
