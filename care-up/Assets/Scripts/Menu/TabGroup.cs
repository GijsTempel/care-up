using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private Sprite tabIdle;

    [SerializeField]
    private Sprite tabHover;

    [SerializeField]
    private Sprite tabActive;

    private TabButton selectedTab;
    private List<GameObject> pages = new List<GameObject>();
    private List<TabButton> tabs;

    public void Subscribe(TabButton button)
    {
        if (tabs == null)
            tabs = new List<TabButton>();

        tabs.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();

        if (button != selectedTab)
        {
            button.background.sprite = tabHover;
            button.background.rectTransform.localScale = new Vector3(1.01f, 1.01f, 1);
        }
    }

    public void OnTabExit(TabButton button) { }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;

        ResetTabs();

        button.background.sprite = tabActive;
        button.background.rectTransform.localScale = new Vector3(1.15f, 1.15f, 1f);

        int index = button.transform.GetSiblingIndex();

        for (int i = 0; i < pages.Count; i++)
        {
            if (i == index)
                pages[i].SetActive(true);
            else
                pages[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabs)
        {
            if (button != null)
            {
                if (button == selectedTab)
                    continue;

                button.background.rectTransform.localScale = new Vector3(1, 1, 1);
                button.background.sprite = tabIdle;
            }
        }
    }

    private void Start()
    {
        GameObject pagesHolder = GameObject.Find("PagesContainer/PageHolder");

        for (int i = 0; i < pagesHolder.transform.childCount - 1; i++)
        {
            pages.Add(pagesHolder.transform.GetChild(i).gameObject);
        }

        OnTabSelected(tabs[tabs.Count - 1]);
    }
}