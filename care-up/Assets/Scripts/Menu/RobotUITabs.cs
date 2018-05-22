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
            else
            {
                if (name == "InfoTab" )
                {
                    SetTabActive(false);
                    gameObject.SetActive(false);
                    tabTrigger.SetActive(false);
                    tabs.Remove(this);
                }
            }
        }
        else
        {
            if (name == "InfoTab")
            {
                SetTabActive(false);
                gameObject.SetActive(false);
                tabTrigger.SetActive(false);
                tabs.Remove(this);
            }
        }

        EventTrigger.Entry clickEvent = new EventTrigger.Entry();
        clickEvent.eventID = EventTriggerType.PointerClick;
        clickEvent.callback.AddListener((eventData) => { OnTabSwitch(); });

        tabTrigger.AddComponent<EventTrigger>();
        tabTrigger.GetComponent<EventTrigger>().triggers.Add(clickEvent);
        
        EventTrigger.Entry backBtnClickEvent = new EventTrigger.Entry();
        backBtnClickEvent.eventID = EventTriggerType.PointerClick;
        backBtnClickEvent.callback.AddListener((eventData) => { BackButton(); });

        GameObject backBtn = transform.Find("Button").gameObject;
        backBtn.AddComponent<EventTrigger>();
        backBtn.GetComponent<EventTrigger>().triggers.Add(backBtnClickEvent);
    }

    public void OnTabSwitch()
    {
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

        tabTrigger.GetComponent<Image>().color = new Color(0.0f, 0.831f, 1.0f, value ? 1.0f : 0.3f);
    }

    protected void BackButton()
    {
        icons.gameObject.SetActive(true);

        foreach (RobotUITabs t in tabs)
        {
            t.SetTabActive(false);
        }
    }
}
