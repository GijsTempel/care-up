using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RobotUIMessage : MonoBehaviour
{
    private bool messageNew = false;

    private string title;
    private string content;
    
    private static Text contentObject;
    private Text text;
    private Image icon;

    static Tutorial_UI tutorial_UI;

    private void Init()
    {
        if (contentObject == null)
        {
            contentObject = GameObject.FindObjectOfType<RobotUIMessageTab>().transform.Find("GeneralDynamicCanvas")
                .Find("ScrollViewMessege").Find("Viewport").Find("Content").Find("Text").GetComponent<Text>();
        }

        text = transform.GetComponentInChildren<Text>();
        icon = transform.Find("Image").GetComponent<Image>();

        tutorial_UI = GameObject.FindObjectOfType<Tutorial_UI>();
    }

    public void OnClick()
    {
        if (tutorial_UI != null && tutorial_UI.openMailMessage == false)
        {
            return;
        }

        if (messageNew)
        {
            messageNew = false;
            text.fontStyle = FontStyle.Normal;
            RobotManager.SetNotification(RobotManager.NotificationNumber - 1);
        }

        if (contentObject)
        {
            contentObject.text = content;
        }

        RobotUIMessageTab.tutorial_messageOpened = true;
    }

    public void NewMessage(string title, string message, Sprite i)
    {
        Init();

        messageNew = true;
        text.text = title;
        content = message;

        icon.sprite = i;

        text.fontStyle = FontStyle.Bold;

        transform.SetSiblingIndex(0);
    }
}
