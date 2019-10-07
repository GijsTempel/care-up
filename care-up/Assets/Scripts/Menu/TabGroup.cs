using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private Sprite tabIdle = default,
                   tabActive = default,
                   topTabIdle = default,
                   topTabActive = default,
                   buyBtnSprite = default,
                   putOnBtnSprite = default;

    [SerializeField]
    private GameObject buyBtn = default,
                       confirmPanel = default;

    private GameObject pagesContainer = default,
                       buyBtnPutOnText = default,
                       buyBtnCoin = default;

    [SerializeField]
    private UIParticleSystem currencyParticles = default(UIParticleSystem);

    private Text buyBtnText = default;

    private TabButton selectedTab;
    private ProductButton selectedItemBtn = null;
    private PlayerAvatar mainAvatar;
    private List<TabButton> tabs;
    private List<GameObject> pages = new List<GameObject>();

    public void ShowConfirmPanel(bool toShow)
    {
        confirmPanel.SetActive(toShow);
    }

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

    public void SelectItem(ProductButton btn)
    {
        if (selectedItemBtn != null)
        {
            selectedItemBtn.Select(false);
        }
        if (btn != null)
        {
            selectedItemBtn = btn;
            selectedItemBtn.Select(true);
            buyBtnCoin.SetActive(true);

            if (btn.item.category == "Heat")
            {
                GameObject.FindObjectOfType<LoadCharacterScene>().LoadCharacter();
                mainAvatar.LoadNewHeat(btn.item.name);
                CharacterInfo.UpdateCharacter(btn.item);
                GameObject.FindObjectOfType<CharacterСarrousel>().UpdateSelected();
                //PlayerPrefsManager.storeManager.SetHeat(btn.item.name);
            }
            else
            {
                mainAvatar.LoadNewHeat("");
            }
        }

        UpdatePurchesBtn();
    }

    public void UpdatePurchesBtn()
    {
        if (selectedItemBtn != null)
        {
            if (!selectedItemBtn.item.purchased)
            {
                buyBtnText.text = selectedItemBtn.item.price.ToString();
                buyBtnCoin.SetActive(true);
                buyBtn.SetActive(true);
                buyBtn.GetComponent<Image>().sprite = buyBtnSprite;
                buyBtnPutOnText.SetActive(false);
            }
            else
            {
                buyBtnText.text = "";
                buyBtnPutOnText.SetActive(true);
                buyBtnCoin.SetActive(false);
                buyBtn.SetActive(true);
                buyBtn.GetComponent<Image>().sprite = putOnBtnSprite;
            }
        }
        else
        {
            buyBtn.SetActive(false);
        }
    }

    public void PurchesBtnClicked()
    {
        if (selectedItemBtn != null)
        {
            if (!selectedItemBtn.item.purchased)
                ShowConfirmPanel(true);
        }
        UpdatePurchesBtn();
    }

    public void PurchaseSelectedItem()
    {
        StoreItem item = selectedItemBtn.item;
        CharacterInfo.UpdateCharacter(item);
        GameObject.FindObjectOfType<LoadCharacterScene>().LoadCharacter();

        if (!item.purchased)
        {
            if (PlayerPrefsManager.storeManager.Purchase(item.index))
            {
                selectedItemBtn.SetPurchased(true);
                GameObject.Find("AdjustCharacter/NumbersStackPanel/CurrencyPanel/Panel/Text").GetComponent<Text>().text
                    = PlayerPrefsManager.storeManager.Currency.ToString();
                if (item.price > 0)
                {
                    GameObject.Find("cashRegisterEffect").GetComponent<AudioSource>().Play();
                    currencyParticles.Play();
                }
                else
                {
                    GameObject.Find("swoopEffect").GetComponent<AudioSource>().Play();
                }
            }
        }
        ShowConfirmPanel(false);
        UpdatePurchesBtn();
    } 

    private void ModifyTab(TabButton button, Sprite sprite, Vector3 vector3)
    {
        button.background.rectTransform.localScale = vector3;
        button.background.sprite = sprite;
    }

    private void Start()
    {
        mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();
        buyBtnText = buyBtn.transform.Find("Text").GetComponent<Text>();
        buyBtnPutOnText = buyBtn.transform.Find("PutOn").gameObject;
        buyBtnCoin = buyBtn.transform.Find("Coin").gameObject;
        buyBtnSprite = buyBtn.GetComponent<Image>().sprite;

        buyBtnCoin.SetActive(false);
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
                ProductButton btn = i.GetComponent<ProductButton>();
                btn.Initialize(item, this);
            }
        }

        for (int i = 1; i < pagesContainer.transform.childCount; i++)
        {
            pages.Add(pagesContainer.transform.GetChild(i).gameObject);
        }

        OnTabSelected(tabs[0]);
        UpdatePurchesBtn();
    }
}