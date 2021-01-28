using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HighscoreCharacterPanel : MonoBehaviour
{
    public GameObject content;
    public GameObject highscoreLoadIcon;
    public GameObject playerView;
    public GameObject cutout;
    public Text SceneName;
    public Text PlayerName;

    private void OnEnable()
    {
        HideContent();
    }

    public void HideContent(bool value = true, bool showPlayer = true)
    {
        content.SetActive(!value);
        highscoreLoadIcon.SetActive(value);
        if (!value)
        {
            SceneName.text = PlayerPrefsManager.HighscoreSceneName;
            PlayerName.text = PlayerPrefsManager.HighscorePlayerName;
        }
        cutout.SetActive(!showPlayer);
        playerView.SetActive(showPlayer);
    }

}
