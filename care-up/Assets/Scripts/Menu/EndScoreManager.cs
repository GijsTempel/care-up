using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles EndScore scene.
/// </summary>
public class EndScoreManager : MonoBehaviour {
    
    private int points;
    private int score;
    private float time;
    private string wrongSteps;
    private string completedSceneName;

    private ActionManager actionManager;    //points, steps
    private GameTimer gameTimer; // time

    void Start()
    {
        SceneManager.sceneLoaded += OnLoaded;
    }

    /// <summary>
    /// Sets object variables in scene after loading.
    /// </summary>
    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        if (SceneManager.GetActiveScene().name == "EndScore")
        {
            GameObject.Find("Score").GetComponent<Text>().text = "Score: " + score;
            GameObject.Find("Points").GetComponent<Text>().text = "Points: " + points;
            GameObject.Find("Time").GetComponent<Text>().text = string.Format("Time: {0}:{1:00}", (int)time / 60, (int)time % 60);
            GameObject.Find("Steps").GetComponent<Text>().text = wrongSteps;

            if (score >= 1)
            {
                GameObject.Find("Star1").GetComponent<Image>().color = Color.green;
            }

            if (score >= 3)
            {
                GameObject.Find("Star2").GetComponent<Image>().color = Color.green;
            }

            if (score >= 5)
            {
                GameObject.Find("Star3").GetComponent<Image>().color = Color.green;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().SetSceneCompletionData(
                completedSceneName, score, string.Format("Time: {0}m{1:00}s", (int)time / 60, (int)time % 60));
        }
    }

    /// <summary>
    /// Gets necesarry variables and loads EndScore scene.
    /// </summary>
    public void LoadEndScoreScene()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");

        gameTimer = GameObject.Find("GameLogic").GetComponent<GameTimer>();
        if (gameTimer == null) Debug.LogError("No timer found");

        points = actionManager.Points;
        score = Mathf.FloorToInt(5.0f * points / actionManager.TotalPoints);
        time = gameTimer.CurrentTime;
        wrongSteps = actionManager.WrongSteps;
        completedSceneName = SceneManager.GetActiveScene().name;

        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("EndScore");
        //SceneManager.LoadScene("EndScore");
    }
}
