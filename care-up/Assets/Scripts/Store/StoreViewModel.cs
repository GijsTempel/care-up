using UnityEngine;
using UnityEngine.UI;

public class StoreViewModel : MonoBehaviour
{
    private GameObject currencyText = default;
    private GameObject extraCurrencyText = default;
    private Text presentNumberText = default;
    [SerializeField]
    private Button goToCharacterStoreButton = default;

    public static int SavedCoins { get; set; }

    private void Start()
    {
        UpdateCurrancyPanel();
        UpdateExtraCurrancyPanel();
    }

    public void UpdateCurrancyPanel()
    {
        currencyText = GameObject.Find("TitlePanel/TitlePanel/Panel/CurrencyPanel/ValuePanel/Text");

        if (currencyText != null)
            currencyText.GetComponent<Text>().text = PlayerPrefsManager.storeManager.Currency.ToString();
    }

    public void UpdateExtraCurrancyPanel()
    {
        extraCurrencyText = GameObject.Find("TitlePanel/TitlePanel/Panel/CUDiamondsPanel/ValuePanel/Text");

        if (extraCurrencyText != null)
            extraCurrencyText.GetComponent<Text>().text = PlayerPrefsManager.storeManager.ExtraCurrency.ToString();
    }

    public static bool ShowRewardDialogue(Text panelText, GameObject popUp = null)
    {
        bool value = false;

        if (SavedCoins <= 0)
            SavedCoins = ActionManager.Points;

        if (SavedCoins > 0 && (ActionManager.percentage > 30))
        {
            if (panelText != null)
            {
                int rewardCoins = RoundToBigger(SavedCoins);
                panelText.text = "+" + rewardCoins.ToString();
                PlayerPrefsManager.storeManager.ModifyCurrencyBy(rewardCoins);

                UMP_Manager manager = GameObject.FindObjectOfType<UMP_Manager>();


                SavedCoins = ActionManager.Points = 0;

                int diamants = 0;
                print("ActionManager.percentage  = " + ActionManager.percentage.ToString());
                if (ActionManager.percentage == 100)
                {
                    diamants = 1;
                    Debug.Log("Extra reward for 100% score");
                    PlayerPrefsManager.storeManager.ModifyExtraCurrencyBy(diamants);
                }

                if (popUp != null)
                    popUp.SetActive(true);
                else if (manager != null)
                    manager.ShowCongratulation(rewardCoins, diamants);
                value = true;
            }
        }
        if (SavedCoins > 0)
            SavedCoins = ActionManager.Points = 0;
        return value;
    }

    private static int RoundToBigger(int value)
    {
        if (value % 10 != 0) value = (value / 10) * 10 + 10;
        return value;
    }   
}
