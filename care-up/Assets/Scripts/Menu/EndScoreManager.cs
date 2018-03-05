using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Handles EndScore scene.
/// </summary>
public class EndScoreManager : MonoBehaviour {
    
    private int points;
    private int score;
    private float time;
    private string completedSceneName;
    
    public string SceneName
    {
        get { return completedSceneName; }
    }

    private List<string> wrongSteps;
    private List<string> wrongStepsDescr;

    private ActionManager actionManager;    //points, steps
    private GameTimer gameTimer; // time

    private Sprite halfStar;
    private Sprite fullStar;

    void Start()
    {
        SceneManager.sceneLoaded += OnLoaded;

        halfStar = Resources.Load<Sprite>("Sprites/Stars/half star");
        fullStar = Resources.Load<Sprite>("Sprites/Stars/star");
    }

    /// <summary>
    /// Sets object variables in scene after loading.
    /// </summary>
    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        if (SceneManager.GetActiveScene().name == "EndScore")
        {
            Transform uiFolder = GameObject.Find("Canvas").transform;

            //uiFolder.Find("Left").Find("Score").GetComponent<Text>().text = "Score: " + score;
            uiFolder.Find("Left").Find("Points").GetComponent<Text>().text = "Points: " + points;
            uiFolder.Find("Left").Find("Time").GetComponent<Text>().text = string.Format("Time: {0}:{1:00}", (int)time / 60, (int)time % 60);

            //uiFolder.GetChild(1).FindChild("Steps").GetComponent<Text>().text = wrongSteps;

            Transform layoutGroup = uiFolder.Find("Right").Find("WrongstepScroll").Find("WrongstepViewport").Find("WrongstepContent").Find("LayoutGroup");
            EndScoreWrongStepDescr[] stepObjects = layoutGroup.GetComponentsInChildren<EndScoreWrongStepDescr>();

            for (int i = 0; i < wrongSteps.Count && i < stepObjects.Length; ++i)
            {
                stepObjects[i].GetComponent<Text>().text = wrongSteps[i];
                stepObjects[i].text = wrongStepsDescr[i];
            }

            if (score >= 1)
            {
                GameObject.Find("Star1").GetComponent<Image>().sprite = fullStar;
            }

            if (score >= 2)
            {
                GameObject.Find("Star2").GetComponent<Image>().sprite = halfStar;
            }

            if (score >= 3)
            {
                GameObject.Find("Star2").GetComponent<Image>().sprite = fullStar;
            }

            if (score >= 4)
            {
                GameObject.Find("Star3").GetComponent<Image>().sprite = halfStar;
            }

            if (score >= 5)
            {
                GameObject.Find("Star3").GetComponent<Image>().sprite = fullStar;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().SetSceneCompletionData(
                completedSceneName, score, string.Format("Tijd: {0}m{1:00}s", (int)time / 60, (int)time % 60));
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

        time = gameTimer.CurrentTime;
        points = Mathf.RoundToInt(actionManager.Points * (time < 300.0f ? 3.0f : (time < 600.0f ? 2.0f : 1.0f)));
        score = Mathf.FloorToInt(5.0f * points / actionManager.TotalPoints);
        completedSceneName = SceneManager.GetActiveScene().name;
        
        wrongSteps = actionManager.WrongSteps;
        wrongStepsDescr = actionManager.WrongStepsDescription;

        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("EndScore");
        //SceneManager.LoadScene("EndScore");
    }
}
