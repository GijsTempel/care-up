using UnityEngine;
using UnityEngine.UI;

public class StorePopUpsManager : MonoBehaviour
{
    [SerializeField]
    private Text clarificationNote = default(Text);

    [SerializeField]
    private Currency currency = default(Currency);

    public enum Currency
    {
        Coins,
        Diamonds
    }

    public void SetClarificationText()
    {
        string coinsText = "CU munten kan je verdienen door protocollen te spelen. Hoe hoger je score hoe meer munten je krijgt.";
        string diamondsText = "Om diamanten te verdienen moet je 100% scoren op een protocol.";

        clarificationNote.text = currency == Currency.Coins ? coinsText : diamondsText;
    }

    public void PurchaseFail(Text info, StorePopUpsManager.Currency currencyType)
    {
        string coinsText = "Je hebt niet genoeg munten!";
        string diamondsText = "Je hebt niet genoeg diamanten!";

        info.text = currencyType == StorePopUpsManager.Currency.Coins ? coinsText : diamondsText;
        GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(8);
    }
}
