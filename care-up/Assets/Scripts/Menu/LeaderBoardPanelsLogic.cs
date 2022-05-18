using UnityEngine;

public class LeaderBoardPanelsLogic : MonoBehaviour
{
    public void HideInfoBar()
    {
        GameObject.Find("Leaderboard").GetComponent<Animator>().SetTrigger("start");
        GameObject.FindObjectOfType<LeaderBoard>().top.SetActive(true);
        GameObject.FindObjectOfType<LeaderBoard>().topDescription.SetActive(true);
        GameObject.FindObjectOfType<LeaderBoard>().leaderboard.SetActive(false);
        GameObject.FindObjectOfType<LeaderBoard>().infoBar.SetActive(false);
        GameObject.FindObjectOfType<LeaderBoard>().leftBar.SetActive(true);
        //GameObject.FindObjectOfType<UMP_Manager>().LeaderBoardSearchBar.gameObject.SetActive(true);
        //GameObject.FindObjectOfType<UMP_Manager>().LeaderBoardSearchBar.text = "";
    }
}
