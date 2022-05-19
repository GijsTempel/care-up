using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class LeaderBoardItem : MonoBehaviour
{
    public Text userNameText;
    public Text userPositionText;
    public Text userXPText;
    public GameObject outline;
    public List<GameObject> medals;
    string userName;
    int userRank;
    int userPoints;
    int userPos;

    private void Start()
    {
        Debug.Log(MBS.WULogin.UID);
    }
    public void SetValues(string _userName, int rank, int points, int userID, int _pos)
    {
        userName = _userName;
        userRank = rank;
        userPoints = points;
        userPos = _pos;
        userNameText.text = userName;
        userXPText.text = userPoints.ToString() + " xp";
        userPositionText.text = userPos.ToString();
        for (int i = 0; i < medals.Count; i++)
        {
            medals[i].SetActive(i == _pos-1);
        }
        userPositionText.gameObject.SetActive(_pos > 3);
        outline.SetActive(MBS.WULogin.UID == userID);
    }
   
}
