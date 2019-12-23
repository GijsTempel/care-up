using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CongratulationTab : MonoBehaviour
{
    public GameObject diamantEffect;
    public GameObject congratPanel;
    public Text coinText;
    public Text diamantText;
    //GameObject diamantSection;

    public void HideDialogue()
    {
        gameObject.SetActive(false);
        PlayerPrefsManager pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        pref.muteMusicForEffect = false;
        pref.ToPlayMusic(true);
    }

    public void ShowDialogue(int coins, int diamants = 0)
    {
        gameObject.SetActive(true);
        string coinTextValue = "+" + coins.ToString();
        if (coins < 10)
            coinTextValue += "  ";
        coinText.text = coinTextValue;
        diamantText.text = "+" + diamants.ToString();
        if (diamants == 0)
        {
            diamantText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            ShowDiamantEffect(true);
        }
    }
    public void ShowDiamantEffect(bool value)
    {
        diamantEffect.SetActive(value);
        congratPanel.SetActive(!value);
    }
}
