using UnityEngine;
using UnityEngine.UI;

public class StoreViewModel : MonoBehaviour
{
    private Text currencyText;
    private Text presentNumberText;

    [SerializeField]
    private Button goToCharacterStoreButton;

    private void Start()
    {
        currencyText = GameObject.Find("NumbersStackPanel/CurrencyPanel/Panel/Text").GetComponent<Text>();
        currencyText.text = PlayerPrefsManager.storeManager.Currency.ToString();
    }  

    public static void ShowRewardDialogue(Text panelText)
    {
        if (ActionManager.Points > 0)
        {
            int rewardCoins = RoundToBigger(ActionManager.Points);

            PlayerPrefsManager.storeManager.ModifyCurrencyBy(rewardCoins);

            if (panelText != null)
            {
                panelText.text = "+" + rewardCoins.ToString();
                GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(9);
            }

            ActionManager.Points = 0;
        }
    }

    private static int RoundToBigger(int value)
    {
        if (value % 10 != 0) value = (value / 10) * 10 + 10;
        return value;
    }
}
  