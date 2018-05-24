using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUIMessageTab : RobotUITabs
{
    public GameObject buttonPrefab;
    private Sprite errorIcon;
    private Sprite warningIcon;
    private Sprite infoIcon;

    private Transform _parent;

    private static Transform notification;

    public enum Icon
    {
        Info,
        Warning,
        Error
    }

    protected override void Start()
    {
        errorIcon = Resources.Load<Sprite>("Sprites/ErrorIcon");
        warningIcon = Resources.Load<Sprite>("Sprites/Sign-Alert");
        infoIcon = Resources.Load<Sprite>("Sprites/Sign-Alert");

        _parent = transform.Find("GeneralDynamicCanvas").Find("ScrollViewTileMessege")
            .Find("Viewport").Find("Content");
        
        transform.Find("GeneralDynamicCanvas").Find("ScrollViewMessege").Find("Viewport").
            Find("Content").Find("Text").GetComponent<Text>().text = "";
        
        base.Start(); // at the end.. deactivates children

        notification = tabTrigger.transform.Find("Notification");
        notification.gameObject.SetActive(false);
    }

    public void NewMessage(string title, string content, Icon icon)
    {
        GameObject button = Instantiate(buttonPrefab, _parent);

        if (title == "")
        {
            title = content.Substring(0, 10) + "...";
        }

        button.gameObject.SetActive(true);

        Sprite i = infoIcon;
        switch (icon)
        {
            case Icon.Info:
                i = infoIcon;
                break;
            case Icon.Warning:
                i = warningIcon;
                break;
            case Icon.Error:
                i = errorIcon;
                break;
        }

        button.GetComponent<RobotUIMessage>().NewMessage(title, content, i);

        RobotManager.SetNotification(RobotManager.NotificationNumber + 1);

        Narrator.PlaySound("Notification");
    }

    public static void SetNotification(int n)
    {
        if (n > 0)
        {
            notification.gameObject.SetActive(true);
            notification.Find("Text").GetComponent<Text>().text = n.ToString();
        }
        else
        {
            notification.gameObject.SetActive(false);
        }
    }
}
