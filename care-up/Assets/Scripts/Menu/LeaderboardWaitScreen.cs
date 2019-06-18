using UnityEngine;
using UnityEngine.UI;

public class LeaderboardWaitScreen : MonoBehaviour
{
    [SerializeField] Image spinner = default(Image);
    //[SerializeField] GameObject leaderBoard = null; never used
    [SerializeField] float speed = 300f;

    void Update() => spinner.transform.Rotate(0f, 0f, -speed * Time.deltaTime);
}


