using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using MBS;

public class LeaderboardDB : MonoBehaviour
{
    public const int LeagueLimit = 30;
    public readonly float[] PromotionZone = { .5f, .5f, .33f, .16f, 0 };
    public readonly float[] DemotionZone = { 1f, .75f, .75f, .5f, .5f };

    [System.Serializable]
    public class LeaderboardLine {
        public int UserID;
        public string Name;
        public int Rank;
        public int LeagueID;
        public int Points;
    }

    public bool isInTheBoard;
    public static int currentRank;
    public static List<LeaderboardLine> board; // <===== this is your main list of current league leaderboard 

    public void Init()
    {
        //if (DatabaseManager.IsEligibleForLeaderboard == false)
        //    return;

        board = new List<LeaderboardLine>();
        isInTheBoard = false;
        currentRank = -1;

        //StartCoroutine(FetchDB(31)); // testing with custom ID
        StartCoroutine(FetchDB(WULogin.UID));
    }

    IEnumerator FetchDB(int UserID)
    {
        string url = "https://leren.careup.online/Leaderboard/fetch_leaderboard.php?user_id=" + UserID.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else if (webRequest.downloadHandler.text != "")
            {
                // most potato json parsing ever, there's no proper parser in old unity apparently?

                string result = webRequest.downloadHandler.text.Remove(0,1);
                result = result.Remove(result.Length-2,2);
                string[] lines = result.Split('}');
                foreach (string l in lines) {
                    string L = l + '}';
                    if (L.StartsWith(",")) L = L.Remove(0,1);
                    
                    LeaderboardLine nl = new LeaderboardLine();
                    nl = JsonUtility.FromJson<LeaderboardLine>(L);
                    board.Add(nl);

                    if (currentRank < 0)
                    {
                        currentRank = nl.Rank;
                    }
                }

                isInTheBoard = true;
            }
        }
    }

    // if we were not in the board yet, we need to push ourselves
    public void PushToLeaderboard(int uID, int pts)
    {
        if (DatabaseManager.IsEligibleForLeaderboard == false)
            return;

        if (!isInTheBoard) {
            StartCoroutine(PushToLeaderboardWeb(uID, DatabaseManager.LeaderboardName, pts));
        } 
        else 
        {
            Debug.LogWarning("UserID is already in current league, add pts instead?");
        }
    }

    IEnumerator PushToLeaderboardWeb(int UserID, string name, int Pts)
    {
        string url = "https://leren.careup.online/Leaderboard/leaderboard_push.php";
        url += "?pts=" + Pts.ToString();
        url += "&user_id=" + UserID.ToString();
        url += "&limit=" + LeagueLimit.ToString();
        url += "&name=" + name;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else if (webRequest.downloadHandler.text != "")
            {
                Debug.Log(webRequest.downloadHandler.text);
                isInTheBoard = true;
            }
        }
    }

    // if we are in current board, we'll just add points
    public void AddPointsToCurrent(int uID, int pts)
    {
        if (DatabaseManager.IsEligibleForLeaderboard == false)
            return;

        if (isInTheBoard) {
            StartCoroutine(AddPtsWeb(uID, pts));
        }
        else
        {
            Debug.LogWarning("User is not in current leagues, push to league instead?");
        }
    }

    IEnumerator AddPtsWeb(int UserID, int Pts)
    {
        string url = "https://leren.careup.online/Leaderboard/leaderboard_addpts.php";
        url += "?pts=" + Pts.ToString();
        url += "&user_id=" + UserID.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else if (webRequest.downloadHandler.text != "")
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
