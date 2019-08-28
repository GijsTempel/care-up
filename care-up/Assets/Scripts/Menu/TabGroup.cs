﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private Sprite tabIdle, tabActive, topTabIdle, topTabActive = default(Sprite);

    private TabButton selectedTab;
    private GameObject pagesContainer;
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
            button.background.rectTransform.localScale = new Vector3(1.01f, 1.01f, 1);
        }
    }

    public void OnTabExit(TabButton button) { }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;

        ResetTabs();

        if (button == tabs[0])
        {
            ModifyTab(button, topTabActive, new Vector3(1.15f, 1.15f, 1f));
        }
        else
        {
            ModifyTab(button, tabActive, new Vector3(1.15f, 1.15f, 1f));
        }

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

                if (button == tabs[0])
                {
                    ModifyTab(button, topTabIdle, new Vector3(1, 1, 1));
                }
                else
                {
                    ModifyTab(button, tabIdle, new Vector3(1, 1, 1));
                }
            }
        }
    }

    private void ModifyTab(TabButton button, Sprite sprite, Vector3 vector3)
    {
        button.background.rectTransform.localScale = vector3;
        button.background.sprite = sprite;
    }

    private void Start()
    {
        pagesContainer = GameObject.Find("PageContainer");

        GameObject tabBtnPrefab = Resources.Load<GameObject>("Prefabs/StoreTab");
        GameObject tabPagePrefab = Resources.Load<GameObject>("Prefabs/PageHolder");
        GameObject productItem = Resources.Load<GameObject>("Prefabs/ProductPanel");
        Transform tabParent = GameObject.Find("StoreTabContainer").transform;

        foreach (StoreCategory cat in PlayerPrefsManager.storeManager.StoreItems)
        {
            // setting tab button
            GameObject tab = Instantiate(tabBtnPrefab, tabParent);

            tab.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{cat.icon}");
            // set visual name ? something = cat.name

            GameObject page = Instantiate(tabPagePrefab, pagesContainer.transform);

            Transform itemParent = page.transform.Find("StoreTabPage/content");

            page.transform.Find("Scrollbar").GetComponent<Scrollbar>().onValueChanged.AddListener((changeAxis) =>
            {
                GridLayoutGroup gridLayoutGroup = itemParent.GetComponent<GridLayoutGroup>();

                if (gridLayoutGroup != null)
                {
                    gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
                }
            });


            foreach (StoreItem item in cat.items)
            {
                GameObject i = Instantiate(productItem, itemParent);
                // set name ?
                i.transform.Find("Price/Cost").GetComponent<Text>().text = item.price.ToString();
                i.transform.Find("Checkmark").gameObject.SetActive(item.purchased);
            }
        }

        for (int i = 1; i < pagesContainer.transform.childCount; i++)
        {
            pages.Add(pagesContainer.transform.GetChild(i).gameObject);
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