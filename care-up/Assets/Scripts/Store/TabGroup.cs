using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public enum FilterModes
    {
        diamond,
        purchased,
        newItems,
        favorite
    };
    public List<Button> TabButtons;
    public List<GameObject> TabContainers;

    [SerializeField]
    private GameObject buyBtn = default,
                       confirmPanel = default,
                       //purchasedBtn = default,
                       //renewBtn = default,
                       //onSaleBtn = default,
                       buyBtnCoin = default,
                       buyBtnDiamond = default,
                       productItem = default,
                       favoriteBtn = default;

    [SerializeField]
    private Text purchaseText = default;

    Dictionary<FilterModes, StoreItemFilterButton> FilterButtons = new Dictionary<FilterModes, StoreItemFilterButton>();

    bool initialized = false;
    int pages = 3;

    [SerializeField]
    private UIParticleSystem currencyParticles = default(UIParticleSystem), 
        extracurrencyParticles = default(UIParticleSystem);

    private Text buyBtnText = default;

    private GameObject FavButtonIconW;
    private GameObject FavButtonIconY;


    private TabButton selectedTab;
    int selectedTabIndex = 0;

    private List<ProductButton> dressedButtons = new List<ProductButton>();
    private List<ProductButton> selectedButtons = new List<ProductButton>();

    private ProductButton selectedItemBtn = null;
    private PlayerAvatar mainAvatar;
    CharacterСarousel carousel;
    PlayerPrefsManager pref;


    public void SwitchTab(int value)
    {
        foreach (Button b in TabButtons)
        {
            b.interactable = true;
        }
        TabButtons[value].interactable = false;
        foreach (GameObject g in TabContainers)
        {
            g.SetActive(false);
        }
        TabContainers[value].SetActive(true);
    }

    public void FilterItems(FilterModes filterMode)
    {
        bool state = FilterButtons[filterMode].GetState();
        foreach (FilterModes b_key in FilterButtons.Keys)
        {
            FilterButtons[b_key].Select(false);
        }
        FilterButtons[filterMode].Select(!state);
        SelectItem(null);
        DisplayItemsInStore();
        UpdateFavoriteButton();

    }

    public void ShowConfirmPanel(bool toShow)
    {
        confirmPanel.SetActive(toShow);
    }

    void Dress()
    {
        if (selectedItemBtn != null)
        {
            if (selectedItemBtn.item.purchased)
            {
                if (dressedButtons[selectedTabIndex] != null)
                    dressedButtons[selectedTabIndex].SetDressOn(false);
                dressedButtons[selectedTabIndex] = selectedItemBtn;
                dressedButtons[selectedTabIndex].SetDressOn(true);
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

            if (btn.item.extraPrice > 0)
            {
                buyBtnDiamond.SetActive(true);
                buyBtnCoin.SetActive(false);
            }
            else
            {
                buyBtnDiamond.SetActive(false);
                buyBtnCoin.SetActive(true);
            }

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
        else
            selectedItemBtn = null;

        UpdatePurchesBtn();
    }

    public void UpdatePurchesBtn()
    {
        if (selectedItemBtn != null)
        {
            if (!selectedItemBtn.item.purchased)
            {
                if (selectedItemBtn.item.extraPrice > 0)
                {
                    buyBtnText.text = selectedItemBtn.item.extraPrice.ToString();
                    buyBtnDiamond.SetActive(true);
                    buyBtnCoin.SetActive(false);
                    buyBtn.SetActive(true);
                    return;
                }

                buyBtnText.text = selectedItemBtn.item.price.ToString();
                buyBtnCoin.SetActive(true);
                buyBtnDiamond.SetActive(false);
                buyBtn.SetActive(true);
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
        UpdateFavoriteButton();
    }


    void UpdateFavoriteButton()
    {
        if (selectedItemBtn == null)
        {
            favoriteBtn.SetActive(false);
            return;
        }
        if (selectedItemBtn.item.name == "x")
        {
            favoriteBtn.SetActive(false);
            return;
        }
        StoreItem item = selectedItemBtn.item;
        if (item.purchased)
        {
            favoriteBtn.SetActive(true);
            FavButtonIconW.SetActive(!item.isFavourite);
            FavButtonIconY.SetActive(item.isFavourite);
        }
        else
            favoriteBtn.SetActive(false);

    }

    public void ResetBuyBtn()
    {
        buyBtn.SetActive(false);
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
                mainAvatar.SetAnimationAction(CareUpAvatar.Actions.Dance, true);

                selectedItemBtn.SetPurchased(true);
                GetComponent<StoreViewModel>().UpdateCurrancyPanel();
                GetComponent<StoreViewModel>().UpdateExtraCurrancyPanel();
                if (item.extraPrice > 0)
                {
                    GameObject.Find("cashRegisterEffect").GetComponent<AudioSource>().Play();
                    extracurrencyParticles.Play();
                }
                else if (item.price > 0)
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
                carousel.UpdateSelected(mainAvatar.avatarData);
            }
            else
            {
                StorePopUpsManager.Currency currencyType;
                currencyType = item.extraPrice > 0 ? StorePopUpsManager.Currency.Diamonds : StorePopUpsManager.Currency.Coins;

                GameObject.FindObjectOfType<StorePopUpsManager>().PurchaseFail(purchaseText, currencyType);
            }
        }
        ShowConfirmPanel(false);
        UpdatePurchesBtn();
        Dress();
    }

    public void InitializeTabPanel()
    {
        if (initialized)
            return;

        SwitchTab(0);
        carousel = GameObject.FindObjectOfType<CharacterСarousel>();
        pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        InitializeElements();
        buyBtnCoin.SetActive(false);
        buyBtnDiamond.SetActive(false);

        foreach (StoreCategory category in PlayerPrefsManager.storeManager.StoreItems)
        {
            dressedButtons.Add(null);
        }

        UpdatePurchesBtn();
        initialized = true;
    }

    public void DisplayItemsInStore()
    {
        InitializeTabPanel();
        CharacterItem currentCharacter = null;
        if (pref != null)
        {
            if (PlayerPrefsManager.storeManager.CharacterItems.Count >= pref.CarouselPosition)
                currentCharacter = PlayerPrefsManager.storeManager.CharacterItems[pref.CarouselPosition];
        }
        for (int i = 0; i < pages; i++)
        {
            //"Protocols/content"
            Transform itemParent = TabContainers[i].transform.Find("Protocols/content");

            foreach (Transform child in itemParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            StoreItem baseItem = null;
            if (currentCharacter != null)
            {
                //Hats
                if (i == 0)
                {
                    StoreItem xItem = new StoreItem(0, 0, "x", "Hat", true, false);
                    ProductButton xBtn = InstantiateProduct(xItem, i);
                    if (currentCharacter.playerAvatar.hat == "")
                    {
                        dressedButtons[0] = xBtn;
                        xBtn.SetDressOn(true);
                    }
                    if (currentCharacter.defaultAvatarData.hat != "")
                    {
                        baseItem = new StoreItem(0, 0, currentCharacter.defaultAvatarData.hat, "Hat", true, false);
                        ProductButton baseHatBtn = InstantiateProduct(baseItem, i);
                        if (currentCharacter.playerAvatar.hat == currentCharacter.defaultAvatarData.hat)
                        {
                            dressedButtons[0] = baseHatBtn;
                            baseHatBtn.SetDressOn(true);
                        }
                    }
                }
                //Glasses
                else if (i == 1)
                {
                    StoreItem xxItem = new StoreItem(-500, 0, "x", "Glasses", true, false);
                    ProductButton xxBtn = InstantiateProduct(xxItem, i);
                    if (mainAvatar.avatarData.glassesType == -1)
                    {
                        dressedButtons[1] = xxBtn;
                        xxBtn.SetDressOn(true);
                    }
                    if (currentCharacter.defaultAvatarData.glassesType != -1)
                    {
                        int gl = currentCharacter.defaultAvatarData.glassesType;
                        baseItem = new StoreItem(gl, 0, "gl_" + gl.ToString(), "Glasses", true, false);
                        ProductButton baseGlassesBtn = InstantiateProduct(baseItem, i);
                        if (currentCharacter.playerAvatar.glassesType == currentCharacter.defaultAvatarData.glassesType)
                        {
                            dressedButtons[1] = baseGlassesBtn;
                            baseGlassesBtn.SetDressOn(true);
                        }
                    }
                }
                //Bodies
                else if (i == 2)
                {
                    int _body = currentCharacter.defaultAvatarData.bodyType;
                    baseItem = new StoreItem(_body, 0, "body_" + _body.ToString(), "Body", true, false);

                    ProductButton baseBodyBtn = InstantiateProduct(baseItem, i);

                    if (currentCharacter.playerAvatar.bodyType == currentCharacter.defaultAvatarData.bodyType)
                    {
                        dressedButtons[2] = baseBodyBtn;
                        baseBodyBtn.SetDressOn(true);
                    }
                }
            }

            if (mainAvatar == null)
                mainAvatar = GameObject.Find("MainPlayerAvatar").GetComponent<PlayerAvatar>();
            if (pref == null)
                pref = GameObject.FindObjectOfType<PlayerPrefsManager>();

            int avIndex = mainAvatar.avatarData.GetHatOffsetIndex();

            foreach (StoreItem item in PlayerPrefsManager.storeManager.StoreItems[i].items)
            {
                //Filters----------------------------------
                if (FilterButtons.ContainsKey(FilterModes.purchased))
                {
                    if(FilterButtons[FilterModes.purchased].GetState() && !item.purchased)
                        continue;
                }
                if (FilterButtons.ContainsKey(FilterModes.diamond))
                {
                    if(FilterButtons[FilterModes.diamond].GetState() && (item.extraPrice <= 0))
                        continue;
                }
                if (FilterButtons.ContainsKey(FilterModes.newItems))
                {
                    if(FilterButtons[FilterModes.newItems].GetState() && !item.isNew)
                        continue;
                }
                if (FilterButtons.ContainsKey(FilterModes.favorite))
                {
                    if (FilterButtons[FilterModes.favorite].GetState() && (!item.isFavourite || !item.purchased))
                        continue;
                }
                //-----------------------------------------------

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
                    if (currentCharacter.playerAvatar.gender == CareUpAvatar.Gender.Female)
                    {
                        if (item.index >= 100000)
                        {
                            btn = InstantiateProduct(item, i);
                        }
                    }
                    else if (item.index < 100000)
                    {
                        btn = InstantiateProduct(item, i);
                    }
                }
                else
                {
                    btn = InstantiateProduct(item, i);
                }

                if (i == 0 && currentCharacter.playerAvatar.hat == item.name)
                {
                    dressedButtons[0] = btn;
                    btn.SetDressOn(true);
                }
                else if (i == 1 && currentCharacter.playerAvatar.glassesType == item.index)
                {
                    dressedButtons[1] = btn;
                    btn.SetDressOn(true);
                }
                else if (i == 2 && currentCharacter.playerAvatar.bodyType == item.index)
                {
                    dressedButtons[2] = btn;
                    btn.SetDressOn(true);
                }
            }
        }
    }

    public void AddToFavorite()
    {
        if (selectedItemBtn == null)
            return;
        if (selectedItemBtn.item.name == "x")
            return;
        StoreItem item = selectedItemBtn.item;
        PlayerPrefsManager.storeManager.ManageFavouriteItems(item.index);
        if (FilterButtons.ContainsKey(FilterModes.favorite))
        {
            if (FilterButtons[FilterModes.favorite].GetState())
                DisplayItemsInStore();
        }
        UpdateFavoriteButton();
        selectedItemBtn.UpdateFavIcon();
    }

    private void Start()
    {
        FavButtonIconW = favoriteBtn.transform.Find("Icon").gameObject;
        FavButtonIconY = favoriteBtn.transform.Find("IconY").gameObject;

        InitializeTabPanel();
        foreach(StoreItemFilterButton b in GameObject.FindObjectsOfType<StoreItemFilterButton>())
        {
            if (!FilterButtons.ContainsKey(b.FilterMode))
                FilterButtons.Add(b.FilterMode, b);
            else
                FilterButtons[b.FilterMode] = b;
        }
    }

    private ProductButton InstantiateProduct(StoreItem item, int TabNum)
    {
        Transform tab = TabContainers[TabNum].transform.Find("Protocols/content");
        GameObject i = Instantiate(productItem, tab);
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
        buyBtnCoin = buyBtn.transform.Find("Coin").gameObject;
        buyBtnDiamond = buyBtn.transform.Find("Diamond").gameObject;
        productItem = Resources.Load<GameObject>("NecessaryPrefabs/UI/ProductPanelBase");
    }
}
