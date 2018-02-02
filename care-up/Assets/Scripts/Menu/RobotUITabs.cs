using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RobotUITabs : MonoBehaviour {

    private static List<RobotUITabs> tabs = new List<RobotUITabs>();

    private GameObject tabTrigger;

    private RectTransform[] children;
    
    protected virtual void Start()
    {
        tabs.Add(this);

        tabTrigger = transform.Find("Tab").gameObject;
        children = transform.GetComponentsInChildren<RectTransform>();

        SetTabActive(false);

        if (name == "InfoTab")
        {
            SetTabActive(true);
        }

        EventTrigger.Entry clickEvent = new EventTrigger.Entry();
        clickEvent.eventID = EventTriggerType.PointerClick;
        clickEvent.callback.AddListener((eventData) => { OnTabSwitch(); });

        tabTrigger.AddComponent<EventTrigger>();
        tabTrigger.GetComponent<EventTrigger>().triggers.Add(clickEvent);

    }

    private void OnTabSwitch()
    {
        foreach (RobotUITabs t in tabs)
        {
            t.SetTabActive(false);
        }

        SetTabActive(true);
    }

    private void SetTabActive(bool value)
    {
        foreach (RectTransform child in children)
        {
            child.gameObject.SetActive(value);
        }

        tabTrigger.GetComponent<Image>().color = new Color(0, 0, 1.0f, value ? 0.6f : 0.3f);
        tabTrigger.SetActive(true);
        gameObject.SetActive(true);
    }
}
