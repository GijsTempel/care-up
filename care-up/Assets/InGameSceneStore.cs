using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameSceneStore : MonoBehaviour
{
    public List<GameObject> tabs;
    public List<GameObject> tabsButtons;
    Sprite openTabImage = null;
    Sprite closeTabImage = null;

    int currentTab = 0;
    public void SwitchTab(int newTab)
    {
        if (newTab < tabs.Count && newTab != currentTab)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].SetActive(i == newTab);
                if (i != newTab)
                    tabsButtons[i].GetComponent<Image>().sprite = closeTabImage;
                else
                    tabsButtons[i].GetComponent<Image>().sprite = openTabImage;

            }
        }
        currentTab = newTab;
    }

    void Start()
    {
        openTabImage = tabsButtons[0].GetComponent<Image>().sprite;
        closeTabImage = tabsButtons[1].GetComponent<Image>().sprite;

    }

    void Update()
    {
        
    }
}
