using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RobotUITabs : MonoBehaviour {

    protected static List<RobotUITabs> tabs = new List<RobotUITabs>();

    protected GameObject tabTrigger;

    protected RectTransform[] children;
    
    protected virtual void Start()
    {
        tabs.Add(this);
        tabs.RemoveAll(item => item == null);

        tabTrigger = transform.Find("Tab").gameObject;
        children = transform.GetComponentsInChildren<RectTransform>();

        SetTabActive(false);

        if (GameObject.Find("Preferences") != null)
        {
            if (!GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().practiceMode)
            {
                if (name == "InfoTab" || name == "CheckListTab")
                {
                    SetTabActive(false);
                    gameObject.SetActive(false);
                    tabs.Remove(this);
                }
                else
                {
                    SetTabActive(true);
                }
            }
            else
            {
                if (name == "GeneralTab")
                {
                    SetTabActive(true);
                }
            }
        }
        else
        {
            if (name == "GeneralTab")
            {
                SetTabActive(true);
            }
        }

        EventTrigger.Entry clickEvent = new EventTrigger.Entry();
        clickEvent.eventID = EventTriggerType.PointerClick;
        clickEvent.callback.AddListener((eventData) => { OnTabSwitch(); });

        tabTrigger.AddComponent<EventTrigger>();
        tabTrigger.GetComponent<EventTrigger>().triggers.Add(clickEvent);
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

        tabTrigger.GetComponent<Image>().color = new Color(0, 0, 1.0f, value ? 0.6f : 0.3f);
    }
}
