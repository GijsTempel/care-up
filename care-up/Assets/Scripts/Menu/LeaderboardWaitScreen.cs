using UnityEngine;
using UnityEngine.UI;

public class LeaderboardWaitScreen : MonoBehaviour
{
    [SerializeField] Image spinner;
    [SerializeField] GameObject leaderBoard;
    [SerializeField] float speed = 300f;

    void Update() => spinner.transform.Rotate(0f, 0f, -speed * Time.deltaTime);
}


