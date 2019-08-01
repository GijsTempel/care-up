using UnityEngine;
using UnityEngine.UI;

public class RobotUIMessageTab : RobotUITabs
{
    public GameObject buttonPrefab;
    private Sprite errorIcon;
    private Sprite warningIcon;
    private Sprite infoIcon;

    private Transform _parent;
    GameUI gameUI;

    private static Transform notification;

    [HideInInspector]
    public static bool tutorial_messageOpened = false;

    public enum Icon
    {
        Info,
        Warning,
        Error,
        Block,
        MWarning,
    }

    protected override void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        errorIcon = Resources.Load<Sprite>("Sprites/txt_field_ic_error");

        warningIcon = Resources.Load<Sprite>("Sprites/exclamation_b");
        infoIcon = Resources.Load<Sprite>("Sprites/question_t");

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
        if (!(GameObject.FindObjectOfType<TutorialManager>() == null
            || GameObject.FindObjectOfType<Tutorial_UI>() != null 
            || GameObject.FindObjectOfType<Tutorial_Theory>() != null))
        {
            // no messages for scenes with ipad disabled
            // ik, condition is dumb, but whatever
            return;
        }
        //--------------------------------------------------------
        if (icon == Icon.Block || icon == Icon.Warning )
        {
            gameUI.ShowBlockMessage(title, content);
        }

        //--------------------------------------------------------

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
            case Icon.MWarning:
                i = warningIcon;
                break;
            case Icon.Error:
            case Icon.Block:
                GameObject.FindObjectOfType<GameUI>().ButtonBlink(true);
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
