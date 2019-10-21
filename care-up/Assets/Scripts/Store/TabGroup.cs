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
                       confirmPanel = default,
                       purchasedBtn = default,
                       renewBtn = default,
                       onSaleBtn = default;

    private GameObject pagesContainer = default,
                       buyBtnPutOnText = default,
                       buyBtnCoin = default,
                       tabBtnPrefab = default,
                       tabPagePrefab = default,
                       productItem = default,
                       tab = default,
                       page = default;

    private Transform tabParent = default,
                      itemParent = default;

    [SerializeField]
    private UIParticleSystem currencyParticles = default(UIParticleSystem);

    private Text buyBtnText = default;

    private TabButton selectedTab;
    int selectedTabIndex = 0;

    private List<ProductButton> DressedButtons = new List<ProductButton>();
    private List<ProductButton> SelectedButtons = new List<ProductButton>();

    private ProductButton selectedItemBtn = null;
    private PlayerAvatar mainAvatar;
    private List<TabButton> tabs;
    private List<GameObject> pages = new List<GameObject>();
    CharacterСarousel carousel;
    PlayerPrefsManager pref;

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
        selectedTabIndex = index;
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

    void Dress()
    {
        if (selectedItemBtn != null)
        {
            if (selectedItemBtn.item.purchased)
            {
                if (DressedButtons[selectedTabIndex] != null)
                    DressedButtons[selectedTabIndex].SetDressOn(false);
                DressedButtons[selectedTabIndex] = selectedItemBtn;
                DressedButtons[selectedTabIndex].SetDressOn(true);
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
            Dress();
            selectedItemBtn.Select(true);
            buyBtnCoin.SetActive(true);

            if (btn.item.category == "Hat")
            {
                mainAvatar.LoadNewHat(btn.item.name);
            }
            else if (btn.item.category == "Glasses")
            {
                mainAvatar.LoadNewGlasses(btn.item.index);
            }
            else if (btn.item.category == "Body")
            {
                if (btn.item.purchased)
                    CharacterInfo.bodyType = btn.item.index;
                mainAvatar.avatarData.bodyType = btn.item.index;
                mainAvatar.UpdateCharacter();
            }
            if (btn.item.purchased)
            {
                CharacterInfo.UpdateCharacter(btn.item);
                carousel.UpdateSelected(mainAvatar.avatarData);
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
                buyBtn.SetActive(false);
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
                if (selectedTabIndex == 2)
                {
                    CharacterInfo.bodyType = selectedItemBtn.item.index;
                    mainAvatar.avatarData.bodyType = selectedItemBtn.item.index;
                    mainAvatar.UpdateCharacter();
                }
            }
        }
        ShowConfirmPanel(false);
        UpdatePurchesBtn();
        Dress();
        carousel.UpdateSelected(mainAvatar.avatarData);
    }

    public void InitializeTabPanel()
    {
        foreach (StoreCategory category in PlayerPrefsManager.storeManager.StoreItems)
        {
            InitializePrefabs(category);
            ChangeAxis();
            DressedButtons.Add(null);
            // SelectedButtons.Add(new ProductButton());
        }

        for (int i = 1; i < pagesContainer.transform.childCount; i++)
        {
            pages.Add(pagesContainer.transform.GetChild(i).gameObject);
        }

        OnTabSelected(tabs[0]);
        DisplayItemsInStore();
        UpdatePurchesBtn();
    }

    //------------------------------------------------
    public void DisplayItemsInStore()
    {
        CharacterItem currentCharacter = null;
        if (pref != null)
        {
            if (PlayerPrefsManager.storeManager.CharacterItems.Count >= pref.CarouselPosition)
                currentCharacter = PlayerPrefsManager.storeManager.CharacterItems[pref.CarouselPosition];
        }
        for (int i = 0; i < pages.Count; i++)
        {
            itemParent = pages[i].transform.Find("StoreTabPage/content");

            foreach (Transform child in itemParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            StoreItem baseItem = null;
            if (currentCharacter != null)
            {
                //Hats------------------------
                if (i == 0)
                {
                    StoreItem xItem = new StoreItem(0, 0, "x", "Hat", true);
                    ProductButton xBtn = InstantiateProduct(xItem);
                    if (currentCharacter.playerAvatar.hat == "")
                    {
                        DressedButtons[0] = xBtn;
                        xBtn.SetDressOn(true);
                    }
                    if (currentCharacter.defaultAvatarData.hat != "")
                    {
                        baseItem = new StoreItem(0, 0, currentCharacter.defaultAvatarData.hat, "Hat", true);
                        ProductButton baseHatBtn = InstantiateProduct(baseItem);
                        if (currentCharacter.playerAvatar.hat == currentCharacter.defaultAvatarData.hat)
                        {
                            DressedButtons[0] = baseHatBtn;
                            baseHatBtn.SetDressOn(true);
                        }
                    }
                }
                //Glasses------------------------
                else if (i == 1)
                {
                    StoreItem xxItem = new StoreItem(-500, 0, "x", "Glasses", true);
                    ProductButton xxBtn = InstantiateProduct(xxItem);
                    if (mainAvatar.avatarData.glassesType == -1)
                    {
                        DressedButtons[1] = xxBtn;
                        xxBtn.SetDressOn(true);
                    }
                    if (currentCharacter.defaultAvatarData.glassesType != -1)
                    {
                        int gl = currentCharacter.defaultAvatarData.glassesType;
                        baseItem = new StoreItem(gl, 0, "gl_" + gl.ToString(), "Glasses", true);
                        ProductButton baseGlassesBtn = InstantiateProduct(baseItem);
                        if (currentCharacter.playerAvatar.glassesType == currentCharacter.defaultAvatarData.glassesType)
                        {
                            DressedButtons[1] = baseGlassesBtn;
                            baseGlassesBtn.SetDressOn(true);
                        }
                    }
                }
                //Bodies------------------------
                else if (i == 2)
                {
                    int _body = currentCharacter.defaultAvatarData.bodyType;
                    baseItem = new StoreItem(_body, 0, "body_" + _body.ToString(), "Body", true);

                    ProductButton baseBodyBtn = InstantiateProduct(baseItem);

                    if (currentCharacter.playerAvatar.bodyType == currentCharacter.defaultAvatarData.bodyType)
                    {
                        DressedButtons[2] = baseBodyBtn;
                        baseBodyBtn.SetDressOn(true);
                    }
                }
            }

            int avIndex = mainAvatar.avatarData.GetHatOffsetIndex();

            foreach (StoreItem item in PlayerPrefsManager.storeManager.StoreItems[i].items)
            {
                if (i == 0)
                {
                    HatsPositioningDB.HatInfo info = pref.hatsPositioning.GetHatInfo(avIndex, item.name);
                    if (info != null)
                        if (info.excluded)
                            continue;
                }
                if (baseItem != null)
                {
                    if (baseItem.name == item.name)
                        continue;
                }

                ProductButton btn = null;

                if (item.category == "Body")
                {
                    if (currentCharacter.defaultAvatarData.gender == CareUpAvatar.Gender.Female)
                    {
                        if (item.index >= 100000)
                        {
                            btn = InstantiateProduct(item);
                        }
                    }
                    else if (item.index < 100000)
                    {
                        btn = InstantiateProduct(item);
                    }
                }
                else
                {
                    btn = InstantiateProduct(item);
                }

                if (i == 0 && currentCharacter.playerAvatar.hat == item.name)
                {
                    DressedButtons[0] = btn;
                    btn.SetDressOn(true);
                }
                else if (i == 1 && currentCharacter.playerAvatar.glassesType == item.index)
                {
                    DressedButtons[1] = btn;
                    btn.SetDressOn(true);
                }
                else if (i == 2 && currentCharacter.playerAvatar.bodyType == item.index)
                {
                    DressedButtons[2] = btn;
                    btn.SetDressOn(true);
                }
            }

            ChangeAxis();
        }
    }

    private void Start()
    {
        carousel = GameObject.FindObjectOfType<CharacterСarousel>();
        pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        InitializeElements();
        buyBtnCoin.SetActive(false);

        InitializeTabPanel();

        // purchasedBtn.GetComponent<Button>().onClick.AddListener(() => FilterProducts(FilterParam.Purchased));
        // onSaleBtn.GetComponent<Button>().onClick.AddListener(() => FilterProducts(FilterParam.OnSale));
        // renewBtn.GetComponent<Button>().onClick.AddListener(RenewPanel);
    }

    private void InitializePrefabs(StoreCategory storeCategory)
    {
        // setting tab button
        tab = Instantiate(tabBtnPrefab, tabParent);
        tab.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{storeCategory.icon}");

        page = Instantiate(tabPagePrefab, pagesContainer.transform);
        itemParent = page.transform.Find("StoreTabPage/content");
    }

    private ProductButton InstantiateProduct(StoreItem item)
    {
        GameObject i = Instantiate(productItem, itemParent);
        ProductButton btn = i.GetComponent<ProductButton>();
        btn.Initialize(item, this);
        return btn;
    }

    private void ModifyTab(TabButton button, Sprite sprite, Vector3 vector3)
    {
        button.background.rectTransform.localScale = vector3;
        button.background.sprite = sprite;
    }

    private void InitializeElements()
    {
        mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();
        buyBtnText = buyBtn.transform.Find("Text").GetComponent<Text>();
        buyBtnPutOnText = buyBtn.transform.Find("PutOn").gameObject;
        buyBtnCoin = buyBtn.transform.Find("Coin").gameObject;
        buyBtnSprite = buyBtn.GetComponent<Image>().sprite;
        pagesContainer = GameObject.Find("PageContainer");

        tabBtnPrefab = Resources.Load<GameObject>("Prefabs/StoreTab");
        tabPagePrefab = Resources.Load<GameObject>("Prefabs/PageHolder");
        productItem = Resources.Load<GameObject>("Prefabs/ProductPanel");
        tabParent = GameObject.Find("StoreTabContainer").transform;
    }

    private void ChangeAxis()
    {
        bool axisChanged = false;

        page.transform.Find("Scrollbar").GetComponent<Scrollbar>().onValueChanged.AddListener((changeAxis) =>
        {
            GridLayoutGroup gridLayoutGroup = itemParent.GetComponent<GridLayoutGroup>();

            if (gridLayoutGroup != null && !axisChanged)
            {
                gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
                axisChanged = true;
            }
        });
    }
}