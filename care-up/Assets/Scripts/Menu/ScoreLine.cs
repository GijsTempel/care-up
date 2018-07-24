using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour {
    public GameObject crown;

    public void SetScoreLine(string _name, string _time, string _score, int i)
    {
        transform.Find("name").GetComponent<Text>().text = _name;
        transform.Find("time/Text").GetComponent<Text>().text = _time;
        transform.Find("score/Text").GetComponent<Text>().text = _score;
        if (i < 3)
            crown.SetActive(_name != "");

    }

}
