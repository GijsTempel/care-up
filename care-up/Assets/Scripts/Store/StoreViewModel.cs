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
        currencyText = GameObject.Find("NumbersStackPanel/CurrencyPanel/Panel/Text").GetComponent<Text>();
        currencyText.text = PlayerPrefsManager.storeManager.Currency.ToString();
    }
}
  