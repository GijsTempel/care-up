using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour
{
    public GameObject crown;

    public void SetScoreLine(string name, string score, int i, string uid = "")
    {
        // print(name);
        transform.Find("name").GetComponent<Text>().text = name;
        //transform.Find("time/Text").GetComponent<Text>().text = "";
        transform.Find("score/Text").GetComponent<Text>().text = score;

        if (uid != "")
        {
            int iuid = int.Parse(uid);
            // set function to load info to the button
            Button btn = transform.Find("infoButton/Button").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => GameObject.FindObjectOfType<LevelSelectionScene_UI>().RequestCharacterInfoByUID(iuid));
            btn.onClick.AddListener(() => GameObject.FindObjectOfType<UMP_Manager>().ShowDialog(10));
        }

        if (i < 3)
            crown.SetActive(name != "");
    }
}
