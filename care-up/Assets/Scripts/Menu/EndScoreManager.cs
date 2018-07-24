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

    private List<string> steps;
    private List<string> stepsDescr;
    private List<int> wrongStepIndexes;
    private List<int> correctStepIndexes;

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
        bool actualScene = false; // for later lines
        if (SceneManager.GetActiveScene().name == "EndScore")
        {
            Transform uiFolder = GameObject.Find("Canvas").transform;

            //uiFolder.Find("Left").Find("Score").GetComponent<Text>().text = "Score: " + score;
            uiFolder.Find("Left").Find("Points").GetComponent<Text>().text = "Points: " + points;
            uiFolder.Find("Left").Find("Time").GetComponent<Text>().text = string.Format("Time: {0}:{1:00}", (int)time / 60, (int)time % 60);

            //uiFolder.GetChild(1).FindChild("Steps").GetComponent<Text>().text = wrongSteps;

            Transform layoutGroup = uiFolder.Find("Right").Find("WrongstepScroll").Find("WrongstepViewport").Find("LayoutGroup");
            EndScoreWrongStepDescr[] stepObjects = layoutGroup.GetComponentsInChildren<EndScoreWrongStepDescr>();

            for (int i = 0; i < steps.Count && i < stepObjects.Length; ++i)
            {
                stepObjects[i].GetComponent<Text>().text = steps[i];
                stepObjects[i].text = stepsDescr[i];
                stepObjects[i].wrong = wrongStepIndexes.Contains(i);
            }
            actualScene = true;
        }
        else if(SceneManager.GetActiveScene().name == "EndScore_Test")
        {
            Transform stepParent = GameObject.Find("Interactable Objects/Canvas/ObservationForm/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;
            
            for (int i = 0; i < steps.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ProtocolEvaluationStep"), stepParent);
                step.transform.Find("Text").GetComponent<Text>().text = steps[i];

                bool correct = correctStepIndexes.Contains(i);
                step.transform.Find("ToggleYes").GetComponent<Toggle>().isOn = correct;
                step.transform.Find("ToggleNo").GetComponent<Toggle>().isOn = !correct;
                Debug.Log("Step#" + i + " is " + correct);
            }

            float percent = 1.0f * correctStepIndexes.Count / steps.Count;

            GameObject.Find("Interactable Objects/Canvas/Score_percentage/ScoreText")
                .GetComponent<Text>().text = Mathf.FloorToInt(percent * 100f).ToString() + "%";

            GameObject.Find("Interactable Objects/Canvas/Score_result/ResultText")
                .GetComponent<Text>().text = (percent < 0.7f) ? "Onvoldoende" : "Voldoende";

            actualScene = true;
        }

        if (actualScene)
        {
            if (score >= 0.5f)
            {
                GameObject.Find("Star1").GetComponent<Image>().sprite = halfStar;
            }

            if (score >= 1.0f)
            {
                GameObject.Find("Star1").GetComponent<Image>().sprite = fullStar;
            }

            if (score >= 1.5f)
            {
                GameObject.Find("Star2").GetComponent<Image>().sprite = halfStar;
            }

            if (score >= 2.0f)
            {
                GameObject.Find("Star2").GetComponent<Image>().sprite = fullStar;
            }

            if (score >= 2.5f)
            {
                GameObject.Find("Star3").GetComponent<Image>().sprite = halfStar;
            }

            if (score >= 3.0f)
            {
                GameObject.Find("Star3").GetComponent<Image>().sprite = fullStar;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().SetSceneCompletionData(
                completedSceneName, points, Mathf.RoundToInt(time));
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

        int multiplier = (time < 300.0f ? 3 : (time < 600.0f ? 2 : 1));
        points = actionManager.Points;
        score = Mathf.FloorToInt(3.0f * points / actionManager.TotalPoints);
        points *= multiplier;

        completedSceneName = SceneManager.GetActiveScene().name;
        
        steps = actionManager.StepsList;
        stepsDescr = actionManager.StepsDescriptionList;
        wrongStepIndexes = actionManager.WrongStepIndexes;
        correctStepIndexes = actionManager.CorrectStepIndexes;

        PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (manager != null && manager.practiceMode)
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("EndScore");
        }
        else
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("EndScore_Test");
        }
    }
}
