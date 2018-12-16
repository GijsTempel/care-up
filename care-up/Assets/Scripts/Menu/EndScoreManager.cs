
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

    public string completedSceneName;
    public string completedSceneBundle;

    private List<string> steps;
    private List<string> stepsDescr;
    private List<int> wrongStepIndexes;
    private List<int> correctStepIndexes;

    public List<string> quizQuestionsTexts = new List<string> ();
    public List<int> quizWrongIndexes = new List<int> ();

    private ActionManager actionManager;    //points, steps
    private GameTimer gameTimer; // time

    private Sprite halfStar;
    private Sprite fullStar;

    void Start () {
        SceneManager.sceneLoaded += OnLoaded;

        fullStar = Resources.Load<Sprite> ("Sprites/Stars/star");
    }

    /// <summary>
    /// Sets object variables in scene after loading.
    /// </summary>
    private void OnLoaded (Scene s, LoadSceneMode m) {
        bool actualScene = false; // for later lines
        if (SceneManager.GetActiveScene ().name == "EndScore") {
            Transform uiFolder = GameObject.Find ("Canvas").transform;
            Transform secondScreen = GameObject.Find ("StepScreen").transform;

            //uiFolder.Find("Left").Find("Score").GetComponent<Text>().text = "Score: " + score;
            uiFolder.Find ("ScoreScreen").Find ("Points").GetComponent<Text> ().text = "Punten: " + points;
            uiFolder.Find ("ScoreScreen").Find ("Time").GetComponent<Text> ().text = string.Format ("Tijd: {0}:{1:00}", (int)time / 60, (int)time % 60);

            //uiFolder.GetChild(1).FindChild("Steps").GetComponent<Text>().text = wrongSteps;

            Transform layoutGroup = secondScreen.Find ("Right").Find ("WrongstepScroll").Find ("WrongstepViewport").Find ("LayoutGroup");
            EndScoreWrongStepDescr[] stepObjects = layoutGroup.GetComponentsInChildren<EndScoreWrongStepDescr> ();

            for (int i = 0; i < steps.Count && i < stepObjects.Length; ++i) {
                stepObjects[i].GetComponent<Text> ().text = steps[i];
                stepObjects[i].text = stepsDescr[i];
                stepObjects[i].wrong = wrongStepIndexes.Contains (i);
            }
            actualScene = true;
        } else if (SceneManager.GetActiveScene ().name == "EndScore_Test") {
            Transform stepParent = GameObject.Find ("Interactable Objects/Canvas/StepScreen/ObservationForm/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            for (int i = 0; i < steps.Count; ++i) {
                GameObject step = GameObject.Instantiate (Resources.Load<GameObject> ("Prefabs/ProtocolEvaluationStep"), stepParent);
                step.transform.Find ("Text").GetComponent<Text> ().text = steps[i];

                bool correct = correctStepIndexes.Contains (i);
                step.transform.Find ("ToggleYes").GetComponent<Toggle> ().isOn = correct;
                step.transform.Find ("ToggleNo").GetComponent<Toggle> ().isOn = !correct;
            }

            Transform quizParent = GameObject.Find ("Interactable Objects/Canvas/StepScreen/QuizForm/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            for (int i = 0; i < quizQuestionsTexts.Count; ++i) {
                GameObject step = GameObject.Instantiate (Resources.Load<GameObject> ("Prefabs/ProtocolQuestion"), quizParent);
                step.transform.Find ("Text").GetComponent<Text> ().text = quizQuestionsTexts[i];

                bool wrong = quizWrongIndexes.Contains (i);
                step.transform.Find ("ToggleYes").GetComponent<Toggle> ().isOn = !wrong;
                step.transform.Find ("ToggleNo").GetComponent<Toggle> ().isOn = wrong;
            }

            float percent = 1.0f *
                (correctStepIndexes.Count + (quizQuestionsTexts.Count - quizWrongIndexes.Count))
                / (steps.Count + quizQuestionsTexts.Count);

            GameObject.Find ("Interactable Objects/Canvas/ScoreScreen/Score_percentage/ScoreText")
                .GetComponent<Text> ().text = Mathf.FloorToInt (percent * 100f).ToString () + "%";

            GameObject.Find ("Interactable Objects/Canvas/ScoreScreen/Score_result/ResultText")
                .GetComponent<Text> ().text = (percent < 0.7f) ? "Onvoldoende" : "Voldoende";

            GameObject.Find ("Interactable Objects/Canvas/ScoreScreen/Points")
                .GetComponent<Text> ().text = "Punten: " + points;
            GameObject.Find ("Interactable Objects/Canvas/ScoreScreen/Time")
                .GetComponent<Text> ().text = string.Format ("Tijd: {0}:{1:00}", (int)time / 60, (int)time % 60);

            actualScene = true;

            // add send mail function to the button ?
            Button btn = GameObject.Find("Interactable Objects/Canvas/Send_Score/Buttons/SendButton").GetComponent<Button>();
            btn.onClick.AddListener(EndScoreSendMailResults);

            // show/hide buttons
            bool flag = (percent > 0.7f && GameObject.FindObjectOfType<PlayerPrefsManager>().subscribed);
            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Buttons/NextButton").SetActive(flag);
            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Buttons/Back to main menu").SetActive(!flag);

            GameObject.Find("Interactable Objects/Canvas/Send_Score/Top/Scenetitle").GetComponent<Text>().text = GameObject.FindObjectOfType<PlayerPrefsManager>().currentSceneVisualName;
            
        } else {
            quizQuestionsTexts.Clear ();
            quizWrongIndexes.Clear ();
        }

        if (actualScene) {
            if (score >= 1.0f) {
                GameObject.Find ("Star1").GetComponent<Image> ().sprite = fullStar;
            }

            if (score >= 2.0f) {
                GameObject.Find ("Star2").GetComponent<Image> ().sprite = fullStar;
            }

            if (score >= 3.0f) {
                GameObject.Find ("Star3").GetComponent<Image> ().sprite = fullStar;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameObject.Find ("Preferences").GetComponent<PlayerPrefsManager> ().SetSceneCompletionData (
                completedSceneName, points, Mathf.RoundToInt (time));
        }
    }

    /// <summary>
    /// Gets necesarry variables and loads EndScore scene.
    /// </summary>
    public void LoadEndScoreScene () {
        actionManager = GameObject.Find ("GameLogic").GetComponent<ActionManager> ();
        if (actionManager == null) Debug.LogError ("No action manager found.");

        gameTimer = GameObject.Find ("GameLogic").GetComponent<GameTimer> ();
        if (gameTimer == null) Debug.LogError ("No timer found");

        time = gameTimer.CurrentTime;

        points = actionManager.Points;
        score = Mathf.FloorToInt (3.0f * points / actionManager.TotalPoints);

        Debug.Log ("Current points: " + points);
        Debug.Log ("Total available points: " + actionManager.TotalPoints);
        Debug.Log ("Calculated stars (3 * points/total): " + score);

        int multiplier = (time < 300.0f ? 3 : (time < 600.0f ? 2 : 1));
        points *= multiplier;
        Debug.Log ("Multiplied final score: " + points);

        completedSceneName = SceneManager.GetActiveScene ().name;

        steps = actionManager.StepsList;
        stepsDescr = actionManager.StepsDescriptionList;
        wrongStepIndexes = actionManager.WrongStepIndexes;
        correctStepIndexes = actionManager.CorrectStepIndexes;

        PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager> ();
        if (manager != null && manager.practiceMode) {
            bl_SceneLoaderUtils.GetLoader.LoadLevel ("EndScore");
        } else {
            bl_SceneLoaderUtils.GetLoader.LoadLevel ("EndScore_Test");
        }
    }
    public void EndScoreSendMailResults()
    {
        string topic = "Care Up accreditatie aanvraag";
        string content = "Completed scene: " + GameObject.FindObjectOfType<PlayerPrefsManager>().currentSceneVisualName + "\n";
        content += "Username: " + MBS.WULogin.username + "\n";

        Text text = GameObject.Find("Interactable Objects/Canvas/Send_Score/GameObject (1)/Username/Text").GetComponent<Text>();
        content += text.text;

        PlayerPrefsManager.__sendMail(topic, content);
        Debug.Log("E-mail verzonden");
    }
}

