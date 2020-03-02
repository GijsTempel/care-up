using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour
{
    public GameObject crown;
    public Button infoButton;

    public void SetScoreLine(string name, string score, int i, string uid = "")
    {
        if (score == "0")
        {
            score = "";
            name = "";
        }

        transform.Find("name").GetComponent<Text>().text = name;
        transform.Find("score/Text").GetComponent<Text>().text = score;

        infoButton.onClick.RemoveAllListeners();

        if (uid != "")
        {
            infoButton.gameObject.SetActive(true);
            int iuid = int.Parse(uid);
            // set function to load info to the button
            infoButton.onClick.AddListener(() => GameObject.FindObjectOfType<LevelSelectionScene_UI>().RequestCharacterInfoByUID(iuid));
            infoButton.onClick.AddListener(() => GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(10));
            infoButton.onClick.AddListener(() => SetSelectedPlayerName(name));
        }
        else
            infoButton.gameObject.SetActive(false);

        if (i < 3)
            crown.SetActive(name != "");
    }

    public void SetSelectedPlayerName(string name)
    {
        PlayerPrefsManager.HighscorePlayerName = name;
    }
}
