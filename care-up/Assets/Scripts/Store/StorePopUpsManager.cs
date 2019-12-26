using UnityEngine;
using UnityEngine.UI;

public class StorePopUpsManager : MonoBehaviour
{
    [SerializeField]
    private Text clarificationNote = default;

    [SerializeField]
    private Currency currency;

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
}
