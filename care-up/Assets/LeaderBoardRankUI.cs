using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LeaderBoardRankUI : MonoBehaviour
{

    public List<GameObject> rankButtons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectRank(int r)
    {
        for (int i = 0; i < rankButtons.Count; i++)
        {
            rankButtons[i].transform.Find("Button").GetComponent<Button>().interactable = i != r;
            rankButtons[i].transform.Find("WCircle").GetComponent<Image>().enabled = i == r;
        }
    }
}
