using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour
{
    public GameObject crown;

    public void SetScoreLine(string name, string score, int i)
    {
        transform.Find("name").GetComponent<Text>().text = name;
        //transform.Find("time/Text").GetComponent<Text>().text = "";
        transform.Find("score/Text").GetComponent<Text>().text = score;

        if (i < 3)
            crown.SetActive(name != "");
    }
}
