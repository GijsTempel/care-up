using UnityEngine;
using UnityEngine.UI;

public class StoreViewModel : MonoBehaviour
{
    private Text currencyText;
    private Text presentNumberText;

    [SerializeField]
    private Button goToCharacterStoreButton;

    void Start()
    {
        GameObject.FindObjectOfType<LoadCharacterScene>().LoadCharacter();

        currencyText = GameObject.Find("NumbersStackPanel/CurrencyPanel/Panel/Text").GetComponent<Text>();
        presentNumberText = GameObject.Find("NumbersStackPanel/PresentPanel/Panel/Text").GetComponent<Text>();

        currencyText.text = PlayerPrefsManager.storeManager.Currency.ToString();
        presentNumberText.text = PlayerPrefsManager.storeManager.Presents.ToString();

        goToCharacterStoreButton.onClick.AddListener(() => GameObject.Find("w_char").SetActive(false));
    }
}
