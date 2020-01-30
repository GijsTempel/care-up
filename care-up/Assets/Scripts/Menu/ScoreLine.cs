using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour
{
    public GameObject crown;

    public void SetScoreLine(string name, string score, int i, string uid = "")
    {
        transform.Find("name").GetComponent<Text>().text = name;
        //transform.Find("time/Text").GetComponent<Text>().text = "";
        transform.Find("score/Text").GetComponent<Text>().text = score;

        if (uid != "")
        {
            int iuid = int.Parse(uid);
            // set function to load info to the button
            //Button btn = transform.Find("charactersomething?").GetComponent<Button>();
            //btn.onClick.AddListener(() => GameObject.FindObjectOfType<LevelSelectionScene_UI>().RequestCharacterInfoByUID(iuid));
        }

        if (i < 3)
            crown.SetActive(name != "");
    }
}
