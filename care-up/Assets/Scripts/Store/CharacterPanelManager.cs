using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buyButton = default,
                       adjustButton = default;

    [SerializeField]
    private Text currencyText = default(Text);

    [SerializeField]
    private Text extraCurrencyText = default(Text);

    private static StoreManager storeManager = PlayerPrefsManager.storeManager;
    private SimpleGestureController gestureController = new SimpleGestureController();
    private UMP_Manager uMP_Manager;
    private LoadCharacterScene loadCharacter;
    private CharacterСarousel сarrousel;
    public GameObject buyBtnCoinIcon;
    public GameObject buyBtnDiamondIcon;
    public GameObject buyBtnFreeText;
    public Text buyBtnText;
    public GameObject confirmationPanel;
    public int currentPrice;

    [SerializeField]
    private UIParticleSystem currencyParticles, extraCurrencyParticles = default;

    [SerializeField]
    private Text purchaseText = default;

    private void OnEnable()
    {
        UpdateCurrencyPanel();
        UpdateExtraCurrencyPanel();
    }

    public void Adjust()
    {
        if (uMP_Manager == null)
            uMP_Manager = GameObject.FindObjectOfType<UMP_Manager>();
        uMP_Manager.ChangeWindow(9);
        AdjustCharacter();
        loadCharacter.LoadCharacter();
        GameObject.FindObjectOfType<StoreViewModel>()?.UpdateCurrancyPanel();
        GameObject.FindObjectOfType<StoreViewModel>()?.UpdateExtraCurrancyPanel();

        TabGroup tabGroup = GameObject.FindObjectOfType<TabGroup>();

        if (tabGroup != null)
        {
            tabGroup.DisplayItemsInStore();
            tabGroup.ResetBuyBtn();
        }
    }

    public void ShowConfirmationPanel(bool value)
    {
        confirmationPanel.SetActive(value);
    }

    public void SetStoreInfo(int characterIndex)
    {
        CharacterItem characterItem = storeManager.CharacterItems[characterIndex];
        currentPrice = characterItem.extraPrice > 0 ? characterItem.extraPrice : characterItem.price;
        bool purchased = characterItem.purchased;
        string price = currentPrice.ToString();

        if (characterItem.price == 0)
        {
            price = "";
            buyBtnCoinIcon.SetActive(false);
            buyBtnDiamondIcon.SetActive(false);
            buyBtnFreeText.SetActive(true);
        }
        else if (characterItem.extraPrice > 0)
        {
            buyBtnDiamondIcon.SetActive(true);
            buyBtnCoinIcon.SetActive(false);
            buyBtnFreeText.SetActive(false);
        }
        else
        {
            buyBtnCoinIcon.SetActive(true);
            buyBtnFreeText.SetActive(false);
            buyBtnDiamondIcon.SetActive(false);
        }

        adjustButton.SetActive(purchased);
        buyBtnText.GetComponent<Text>().text = price;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        uMP_Manager = GameObject.FindObjectOfType<UMP_Manager>();
        loadCharacter = GameObject.FindObjectOfType<LoadCharacterScene>();
        сarrousel = GameObject.FindObjectOfType<CharacterСarousel>();
        UpdateCurrencyPanel();
        UpdateExtraCurrencyPanel();
    }

    public void UpdateCurrencyPanel()
    {
        if (currencyText != null)
            currencyText.text = storeManager.Currency.ToString();
    }

    public void UpdateExtraCurrencyPanel()
    {
        if (extraCurrencyText != null)
            extraCurrencyText.text = storeManager.ExtraCurrency.ToString();
    }

    public void AdjustCharacter()
    {
        if (сarrousel == null)
            сarrousel = GameObject.FindObjectOfType<CharacterСarousel>(); ;
        int characterIndex = сarrousel.CurrentCharacter;
        storeManager.AdjustCharacter(storeManager.GetItemIndex(characterIndex));
    }

    public void BuyCharacter()
    {
        if (сarrousel == null)
            сarrousel = GameObject.FindObjectOfType<CharacterСarousel>();
        int characterIndex = сarrousel.CurrentCharacter;
        CharacterItem character = storeManager.CharacterItems[characterIndex];

        if (storeManager.PurchaseCharacter(storeManager.GetItemIndex(characterIndex)))
        {
            сarrousel.SetAnimation();
            adjustButton.SetActive(true);
            currencyText.text = storeManager.Currency.ToString();
            extraCurrencyText.text = storeManager.ExtraCurrency.ToString();


            if (character.extraPrice > 0)
            {
                extraCurrencyParticles.Play();
                GameObject.Find("cashRegisterEffect").GetComponent<AudioSource>().Play();
            }
            else if (character.price > 0)
            {
                currencyParticles.Play();
                GameObject.Find("cashRegisterEffect").GetComponent<AudioSource>().Play();
            }
            else
            {
                GameObject.Find("swoopEffect").GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            StorePopUpsManager manager = GameObject.FindObjectOfType<StorePopUpsManager>();
            StorePopUpsManager.Currency currencyType;
            currencyType = character.extraPrice > 0 ? StorePopUpsManager.Currency.Diamonds : StorePopUpsManager.Currency.Coins;

            manager.PurchaseFail(purchaseText,currencyType);
        }
    }
}