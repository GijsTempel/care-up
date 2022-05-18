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

    string userName;
    int userRank;
    int userPoints;
    int userPos;

    private void Start()
    {
    }
    public void SetValues(string _userName, int rank, int points, int _pos)
    {
        userName = _userName;
        userRank = rank;
        userPoints = points;
        userPos = _pos;
        userNameText.text = userName + " " + userRank.ToString() + " " + userPoints.ToString();
        userPositionText.text = userPos.ToString();
    }
   
}
