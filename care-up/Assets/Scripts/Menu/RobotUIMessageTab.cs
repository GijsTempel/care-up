using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUIMessageTab : RobotUITabs
{
    public GameObject buttonPrefab;
    public Image errorIcon;
    public Image warningIcon;
    public Image infoIcon;

    private RobotUIMessage[] messages;
    private int iterator = 0;

    protected override void Start()
    {
        messages = transform.Find("GeneralDynamicCanvas").Find("ScrollViewTileMessege")
            .Find("Viewport").Find("Content").GetComponentsInChildren<RobotUIMessage>();

        foreach (RobotUIMessage m in messages)
        {
            m.gameObject.SetActive(false);
        }

        transform.Find("GeneralDynamicCanvas").Find("ScrollViewMessege").Find("Viewport").
            Find("Content").Find("Text").GetComponent<Text>().text = "";

        base.Start(); // at the end.. deactivates children
    }

    public void NewMessage(string title, string content)
    {
        if (iterator < messages.Length)
        {
            if (title == "")
            {
                title = content.Substring(0, 10) + "...";
            }

            messages[iterator].gameObject.SetActive(true);
            messages[iterator].NewMessage(title, content);

            ++iterator;
        }
    }
}
