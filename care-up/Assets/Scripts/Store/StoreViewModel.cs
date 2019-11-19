using UnityEngine;
using UnityEngine.UI;

public class StoreViewModel : MonoBehaviour
{
    private GameObject currencyText;
    private Text presentNumberText;

    public static int SavedCoins { get; set; }

    [SerializeField]
    private Button goToCharacterStoreButton;

    private void Start()
    {
        UpdateCurrancyPanel();
    }

    public void UpdateCurrancyPanel()
    {
        currencyText = GameObject.Find("TitlePanel/TitlePanel/Panel/CurrencyPanel/ValuePanel/Text");

        if (currencyText != null)
            currencyText.GetComponent<Text>().text = PlayerPrefsManager.storeManager.Currency.ToString();
    }

    public static bool ShowRewardDialogue(Text panelText, GameObject popUp = null)
    {
        if (SavedCoins <= 0)
            SavedCoins = ActionManager.Points;

        if (SavedCoins > 0)
        {
            if (panelText != null)
            {
                int rewardCoins = RoundToBigger(SavedCoins);
                panelText.text = "+" + rewardCoins.ToString();
                PlayerPrefsManager.storeManager.ModifyCurrencyBy(rewardCoins);

                UMP_Manager manager = GameObject.FindObjectOfType<UMP_Manager>();

                if (popUp != null)
                {
                    popUp.SetActive(true);
                }
                else if (manager != null)
                {
                    manager.ShowDialog(9);
                }
                SavedCoins = ActionManager.Points = 0;

                return true;
            }
        }
        return false;
    }

    private static int RoundToBigger(int value)
    {
        if (value % 10 != 0) value = (value / 10) * 10 + 10;
        return value;
    }
}
