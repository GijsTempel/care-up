using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buyButton = default(GameObject),
                       adjustButton = default(GameObject);

    [SerializeField]
    private Text currencyText = default(Text);

    private static StoreManager storeManager = PlayerPrefsManager.storeManager;
    private SimpleGestureController gestureController = new SimpleGestureController();
    private UMP_Manager uMP_Manager;
    private LoadCharacterScene loadCharacter;
    private CharacterСarrousel сarrousel;
    public GameObject BuyBtnCoinIcon;
    public GameObject BuyBtnFreeText;
    public Text BuyBtnText;
    public GameObject ConfirmationPanel;
    public int CurrentPrice;

    [SerializeField]
    private UIParticleSystem currencyParticles = default(UIParticleSystem);

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
        loadCharacter.LoadCharacter();
    }


    public void ShowConfirmationPanel(bool value)
    {
        ConfirmationPanel.SetActive(value);
    }

    public void SetStoreInfo(int characterIndex)
    {
        CurrentPrice = storeManager.CharacterItems[characterIndex].price;
        bool purchased = storeManager.CharacterItems[characterIndex].purchased;
        int p = storeManager.CharacterItems[characterIndex].price;
        string price = p.ToString();
        if (p == 0)
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

    // private void Update()
    // {
    //     if (сarrousel != null)
    //         gestureController.ManageSwipeGestures(() => сarrousel.Scroll(1), () => сarrousel.Scroll(-1));
    // }

    private void Initialize()
    {
        uMP_Manager = GameObject.FindObjectOfType<UMP_Manager>();
        loadCharacter = GameObject.FindObjectOfType<LoadCharacterScene>();

        сarrousel = GameObject.FindObjectOfType<CharacterСarrousel>();

        currencyText.text = storeManager.Currency.ToString();
        
        //buyButton?.GetComponent<Button>().onClick.AddListener(BuyCharacter);
    }

    public void BuyCharacter()
    {
        int characterIndex = CharacterСarrousel.CurrentCharacter;
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

    //private void SetCharacters(List<GameObject> items)
    //{
    //    foreach (GameObject item in items)
    //    {
    //        characterCreation.Initialize(item);

    //        (bool purchased, int price) = SetCurrentItem(ref index);

    //        item.transform.parent.Find("Checkmark").gameObject.SetActive(parameters[index].purchased);

    //        if (item.transform.parent.name == "CenterGuy")
    //        {
    //            storeitemIndex = index;
    //            // buyButton.transform.GetChild(0).GetComponent<Text>().text = price.ToString();

    //            if (parameters[index].purchased)
    //                adjustButton.SetActive(true);
    //            else
    //                adjustButton.SetActive(false);
    //        }

    //        index++;
    //    }
    //}

    //public void SetDefaultCharacters(List<GameObject> items)
    //{
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        characterCreation.Initialize(items[i]);

    //        gender = parameters[i].gender == "Female" ? PlayerAvatarData.Gender.Female : PlayerAvatarData.Gender.Male;
    //        characterCreation.SetCurrent(gender, parameters[i].headType, parameters[i].bodyType, parameters[i].glassesType);
    //    }
    //}

    //private (bool purchased, int price) SetCurrentItem(ref int index)
    //{
    //    if (index < 0)
    //        index = parameters.Count - 1;

    //    else if (index >= parameters.Count)
    //        index = 0;

    //    gender = parameters[index].gender == "Female" ? PlayerAvatarData.Gender.Female : PlayerAvatarData.Gender.Male;
    //    characterCreation.SetCurrent(gender, parameters[index].headType, parameters[index].bodyType, parameters[index].glassesType);

    //    return (parameters[index].purchased, parameters[index].price);
    //}

    //private IEnumerator SetAnimation()
    //{
    //    yield return new WaitForSeconds(0.6f);
    //    SetAnimationTrigger(0, "idle1");

    //    yield return new WaitForSeconds(1);
    //    SetAnimationTrigger(2, "idle1");
    //}

    //private IEnumerator ResetScale()
    //{
    //    yield return new WaitForSeconds(5.5f);
    //    mainCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
    //}

    //private void SetAnimationTrigger(int index, string name)
    //{
    //    characters[index].GetComponent<Animator>().SetTrigger(name);
    //}
}