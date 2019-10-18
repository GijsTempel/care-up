using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buyButton = default,
                       adjustButton = default;

    [SerializeField]
    private Text currencyText = default(Text);

    private static StoreManager storeManager = PlayerPrefsManager.storeManager;
    private SimpleGestureController gestureController = new SimpleGestureController();
    private UMP_Manager uMP_Manager;
    private LoadCharacterScene loadCharacter;
    private CharacterСarousel сarrousel;
    public GameObject BuyBtnCoinIcon;
    public GameObject BuyBtnFreeText;
    public Text BuyBtnText;
    public GameObject ConfirmationPanel;
    public int CurrentPrice;

    [SerializeField]
    private UIParticleSystem currencyParticles = default;

    public void BuyButtonPressed()
    {
        if (CurrentPrice == 0)
        {
            BuyCharacter();
        }
        else
        {
            ShowConfirmationPanel(true);
        }
    }
    public void Adjust()
    {
        uMP_Manager.ChangeWindow(9);
        AdjustCharacter();
        loadCharacter.LoadCharacter();
        GameObject.FindObjectOfType<TabGroup>().DisplayItemsInStore();
    }

    public void ShowConfirmationPanel(bool value)
    {
        ConfirmationPanel.SetActive(value);
    }

    public void SetStoreInfo(int characterIndex)
    {
        CurrentPrice = storeManager.CharacterItems[characterIndex].price;
        bool purchased = storeManager.CharacterItems[characterIndex].purchased;
        string price = CurrentPrice.ToString();
        if (CurrentPrice == 0)
        {
            price = "";
            BuyBtnCoinIcon.SetActive(false);
            BuyBtnFreeText.SetActive(true);
        }
        else
        {
            BuyBtnCoinIcon.SetActive(true);
            BuyBtnFreeText.SetActive(false);
        }

        adjustButton.SetActive(purchased);
        BuyBtnText.GetComponent<Text>().text = price;
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
        currencyText.text = storeManager.Currency.ToString();
        //buyButton?.GetComponent<Button>().onClick.AddListener(BuyCharacter);
    }

    public void AdjustCharacter()
    {
        int characterIndex = CharacterСarousel.CurrentCharacter;
        storeManager.AdjustCharacter(storeManager.GetItemIndex(characterIndex));
    }

    public void BuyCharacter()
    {
        int characterIndex = CharacterСarousel.CurrentCharacter;
        if (storeManager.PurchaseCharacter(storeManager.GetItemIndex(characterIndex)))
        {
            adjustButton.SetActive(true);
            currencyText.text = storeManager.Currency.ToString();
            if (storeManager.CharacterItems[characterIndex].price > 0)
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
            PurchaseFail();
        }
    }

    private void PurchaseFail()
    {
        uMP_Manager.ShowDialog(8);
    }
}