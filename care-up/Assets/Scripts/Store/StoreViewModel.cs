using UnityEngine;
using UnityEngine.UI;

public class StoreViewModel : MonoBehaviour
{
    private Text currencyText;
    private Text presentNumberText;

    public static int SavedCoins { get; set; }

    [SerializeField]
    private Button goToCharacterStoreButton;

    private void Start()
    {
        currencyText = GameObject.Find("NumbersStackPanel/CurrencyPanel/Panel/Text").GetComponent<Text>();
        currencyText.text = PlayerPrefsManager.storeManager.Currency.ToString();
    }

    public static void ShowRewardDialogue(Text panelText)
    {
        if (SavedCoins <= 0)
            SavedCoins = ActionManager.Points;

        if (SavedCoins > 0)
        {
            int rewardCoins = RoundToBigger(SavedCoins);

            PlayerPrefsManager.storeManager.ModifyCurrencyBy(rewardCoins);

            if (panelText != null)
            {
                panelText.text = "+" + rewardCoins.ToString();
                GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(9);
                SavedCoins = ActionManager.Points = 0;
            }
        }
    }

    private static int RoundToBigger(int value)
    {
        if (value % 10 != 0) value = (value / 10) * 10 + 10;
        return value;
    }
}
