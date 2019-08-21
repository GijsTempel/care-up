﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private Sprite tabIdle = default(Sprite);

    [SerializeField]
    private Sprite tabHover = default(Sprite);

    [SerializeField]
    private Sprite tabActive = default(Sprite);

    private TabButton selectedTab;
    private List<GameObject> pages = new List<GameObject>();
    private List<TabButton> tabs;

    private bool gridModified = false;
    private GameObject pagesContainer;

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

    public void ChangeGridAxisValue()
    {
        if (!gridModified)
        {
            GridLayoutGroup gridLayoutGroup = pagesContainer.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<GridLayoutGroup>();
            gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
            gridModified = true;
        }
    }

    private void Start()
    {
        pagesContainer = GameObject.Find("PageContainer");

        Transform pagesHolder = GameObject.Find("PageContainer/PageHolder").transform;

        GameObject tabBtnPrefab = Resources.Load<GameObject>("Prefabs/StoreTab");
        GameObject tabPagePrefab = Resources.Load<GameObject>("Prefabs/StoreTabPage");
        GameObject productItem = Resources.Load<GameObject>("Prefabs/ProductPanel");
        Transform tabParent = GameObject.Find("StoreTabContainer").transform;
        
        foreach(StoreCategory cat in PlayerPrefsManager.storeManager.StoreItems)
        {
            // setting tab button
            GameObject tab = Instantiate(tabBtnPrefab, tabParent);
            // set visual name ? something = cat.name
            // set icon ? something = cat.icon

            // setting tab page
            GameObject page = Instantiate(tabPagePrefab, pagesHolder);
            Transform itemParent = page.transform.Find("content");
            foreach(StoreItem item in cat.items)
            {
                GameObject i = Instantiate(productItem, itemParent);
                // set name ?
                i.transform.Find("Price/Cost").GetComponent<Text>().text = item.price.ToString();
                i.transform.Find("Checkmark").gameObject.SetActive(item.purchased);
            }
        }
        
        for (int i = 1; i < pagesHolder.childCount; i++)
        {
            pages.Add(pagesHolder.GetChild(i).gameObject);
        }

        OnTabSelected(tabs[0]);
    }

    private void GetColumnAndRow(GridLayoutGroup glg, out int column, out int row)
    {
        column = 0;
        row = 0;

        if (glg.transform.childCount == 0)
            return;

        column = 1;
        row = 1;

        RectTransform firstChildObj = glg.transform.
            GetChild(0).GetComponent<RectTransform>();

        Vector2 firstChildPos = firstChildObj.anchoredPosition;
        bool stopCountingRow = false;

        for (int i = 1; i < glg.transform.childCount; i++)
        {
            RectTransform currentChildObj = glg.transform.GetChild(i).GetComponent<RectTransform>();

            Vector2 currentChildPos = currentChildObj.anchoredPosition;

            if (firstChildPos.y == currentChildPos.y)
            {
                column++;
                stopCountingRow = true;
            }
            else
            {
                if (!stopCountingRow)
                    row++;
            }
        }
    }
}