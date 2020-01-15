using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CongratulationTab : MonoBehaviour
{
    public GameObject congratPanel;
    public Text coinText;
    public Text diamantText;
    public GameObject Buttons;
    bool countEffectOn = false;
    float countTime = 0f;
    //GameObject diamantSection;
    int coins = 0;
    int countCoins = 0;
    int diamonds = 0;
    float countStep = 0.1f;
    float coinCountTime = 0.5f;
    bool diamantShown = false;
    bool buttonsShown = false;

    public void HideDialogue()
    {
        gameObject.SetActive(false);
        PlayerPrefsManager pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        pref.muteMusicForEffect = false;
        pref.ToPlayMusic(true);
    }

    public void ShowDialogue(int _coins, int _diamands = 0)
    {
        gameObject.SetActive(true);
        coins = _coins;
        diamonds = _diamands;
        countEffectOn = true;
        countTime = 0f;
        diamantText.text = "+" + _diamands.ToString();
        diamantText.transform.parent.gameObject.SetActive(false);
        Buttons.SetActive(false);
        ShowCoins(0);
        countStep = coinCountTime / coins;
    }

    void ShowCoins(int _coins)
    {
        string coinTextValue = "+" + _coins.ToString();
        //if (_coins < 10)
        //    coinTextValue += "  ";
        coinText.text = coinTextValue;
    }

    private void Update()
    {
        if (countEffectOn)
        {
            countTime += Time.deltaTime;
            int timeStep = (int)Mathf.Floor(countTime / countStep);
            if (countCoins < coins)
            {
                if (timeStep > countCoins)
                {
                    countCoins += 1;
                    ShowCoins(countCoins);
                }
            }
            else if (diamonds > 0 && !diamantShown)
            {
                if (countTime > (coinCountTime + 1f))
                {
                    diamantText.transform.parent.gameObject.SetActive(true);
                    diamantShown = true;
                }
            }
            else if ((countTime > (coinCountTime + 2f) && !buttonsShown))
            {
                Buttons.SetActive(true);
                if (transform.Find("buttonSound").gameObject.activeSelf)
                    transform.Find("buttonSound").GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            enabled = false;
        }
        
    }

    public void ShowDiamantEffect(bool value)
    {
        congratPanel.SetActive(!value);
    }
}
