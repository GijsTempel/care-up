using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScoreManager : MonoBehaviour {
    
    private int points;
    private int score;
    private float time;

    private ActionManager actionManager;    //points, steps
    private GameTimer gameTimer; // time

    void Start()
    {
        SceneManager.sceneLoaded += OnLoaded;
    }

    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        if (SceneManager.GetActiveScene().name == "EndScore")
        {
            GameObject.Find("Score").GetComponent<Text>().text = "Score: " + score;
            GameObject.Find("Points").GetComponent<Text>().text = "Points: " + points;
            GameObject.Find("Time").GetComponent<Text>().text = string.Format("Time: {0}:{1:00}", (int)time / 60, (int)time % 60);
        }
    }

    public void LoadEndScoreScene()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");

        gameTimer = GameObject.Find("GameLogic").GetComponent<GameTimer>();
        if (gameTimer == null) Debug.LogError("No timer found");

        points = actionManager.Points;
        score = Mathf.FloorToInt(5.0f * points / actionManager.ActionList.Count);
        time = gameTimer.CurrentTime;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("EndScore");
    }
	
}
