using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RobotUITabs : MonoBehaviour {

    protected static List<RobotUITabs> tabs = new List<RobotUITabs>();
    protected static Transform icons;

    protected GameObject tabTrigger;

    protected RectTransform[] children;

    [HideInInspector]
    public static bool tutorial_back = false;
    [HideInInspector]
    public static bool tutorial_generalOpened = false;
    [HideInInspector]
    public static bool tutorial_checkListOpened = false;
    [HideInInspector]
    public static bool tutorial_messageCenterOpened = false;
    [HideInInspector]
    public static bool tutorial_infoTabOpened = false;

    Tutorial_UI tutorial_UI;
    Tutorial_Theory tutorial_theory;

    protected virtual void Start()
    {
        tabs.Add(this);
        tabs.RemoveAll(item => item == null);

        icons = transform.parent.Find("TabletIcons");
        tabTrigger = icons.Find(name).gameObject;
        children = transform.GetComponentsInChildren<RectTransform>();
        
        GameObject sceneTitle = GameObject.Find("SceneTitle");
        GameObject manager = GameObject.Find("Preferences");
        if (sceneTitle != null && (manager != null && manager.GetComponent<PlayerPrefsManager>() != null))
        {
            sceneTitle.GetComponent<Text>().text = manager.GetComponent<PlayerPrefsManager>().currentSceneVisualName;
        }

        tutorial_UI = GameObject.FindObjectOfType<Tutorial_UI>();
        tutorial_theory = GameObject.FindObjectOfType<Tutorial_Theory>();
        if (tutorial_UI != null)
        {
            GameObject exitBtn = GameObject.Find("Exit");
            if (exitBtn != null)
            {
                exitBtn.GetComponent<Button>().interactable = false;
            }
        }

        SetTabActive(false);

        if (GameObject.Find("Preferences") != null)
        {
            if (!GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().practiceMode)
            {
                if (name == "InfoTab" || name == "CheckListTab")
                {
                    SetTabActive(false);
                    gameObject.SetActive(false);
                    tabTrigger.SetActive(false);
                    tabs.Remove(this);
                }
            }
        }

        tabTrigger.GetComponent<Button>().onClick.AddListener(OnTabSwitch);

        GameObject backBtn = transform.Find("Button").gameObject;
        backBtn.GetComponent<Button>().onClick.AddListener(BackButton);
    }

    public void OnTabSwitch()
    {
        if ((tutorial_UI != null && tutorial_UI.tabToOpen != name) ||
            (tutorial_theory != null && tutorial_theory.tabToOpen != name))
        {
            return;
        }

        QuizTab tab = GameObject.FindObjectOfType<QuizTab>();
        if (tab != null)
        {
            if (tab.transform.Find("Continue") != null && tab.transform.Find("Continue").gameObject.activeSelf)
            {
                GameObject.FindObjectOfType<QuizTab>().Continue();
            }
        }

        foreach (RobotUITabs t in tabs)
        {
            t.SetTabActive(false);
        }

        icons.gameObject.SetActive(false);

        SetTabActive(true);
    }

    protected virtual void SetTabActive(bool value)
    {
        foreach (RectTransform child in children)
        {
            if (child.name != "Tab" && child.parent.name != "Tab" && child.GetComponent<RobotUITabs>() == null)
            {
                child.gameObject.SetActive(value);
            }
        }

        switch (name)
        {
            case "GeneralTab":
                tutorial_generalOpened = true;
                break;
            case "CheckListTab":
                tutorial_checkListOpened = true;
                break;
            case "MessageCenter":
                tutorial_messageCenterOpened = true;
                break;
            case "InfoTab":
                tutorial_infoTabOpened = true;
                break;
        }
    }

    protected void BackButton()
    {
        if ((tutorial_UI != null && tutorial_UI.closeTab == false) ||
            ( tutorial_theory != null && tutorial_theory.closeTab == false))
        {
            return;
        }

        tutorial_back = true;

        icons.gameObject.SetActive(true);

        foreach (RobotUITabs t in tabs)
        {
            t.SetTabActive(false);
        }
    }
}
