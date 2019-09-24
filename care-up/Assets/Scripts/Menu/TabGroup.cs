using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private Sprite tabIdle = default(Sprite),
                   tabActive = default(Sprite),
                   topTabIdle = default(Sprite), 
                   topTabActive = default(Sprite);

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
            button.GetComponent<CanvasGroup>().alpha = 0.8f;
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
                button.GetComponent<CanvasGroup>().alpha = 1f;

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

            bool axisChanged = false;

            page.transform.Find("Scrollbar").GetComponent<Scrollbar>().onValueChanged.AddListener((changeAis) =>
            {              
                GridLayoutGroup gridLayoutGroup = itemParent.GetComponent<GridLayoutGroup>();

                if (gridLayoutGroup != null && !axisChanged)
                {
                    gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
                    axisChanged = true;
                }
            });


            foreach (StoreItem item in cat.items)
            {
                GameObject i = Instantiate(productItem, itemParent);
                // set name ?
                i.transform.Find("Price/Cost").GetComponent<Text>().text = item.price.ToString();
                i.transform.Find("Price").gameObject.SetActive(!item.purchased); // hide the price if item is purchased
                i.GetComponent<Button>().onClick.AddListener(() => PurchaseItemBtn(i, item));

                i.transform.Find("Checkmark").gameObject.SetActive(false); // checkmark for selected icon

                // set up icon?
            }
        }

        for (int i = 1; i < pagesContainer.transform.childCount; i++)
        {
            pages.Add(pagesContainer.transform.GetChild(i).gameObject);
        }

        OnTabSelected(tabs[0]);
    }

    public void PurchaseItemBtn(GameObject obj, StoreItem item)
    {
        if (item.purchased)
        {
            // deactivate other nearby checkmarks
            foreach (Transform child in obj.transform.parent)
                child.Find("Checkmark").gameObject.SetActive(false);

            // enable mark for selected item
            obj.transform.Find("Checkmark").gameObject.SetActive(true);

            // do smth after selecting?
            // like wear the piece of clothing?
        }
        else
        {
            // pop up instead of straight up purchase?
            PlayerPrefsManager.storeManager.Purchase(item.index);
            // update ui
            obj.transform.Find("Price").gameObject.SetActive(false);
            GameObject.Find("AdjustCharacter/NumbersStackPanel/CurrencyPanel/Panel/Text").GetComponent<Text>().text
                = PlayerPrefsManager.storeManager.Currency.ToString();
        }
    }
}