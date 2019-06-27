
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Handles EndScore scene.
/// </summary>
public class EndScoreManager : MonoBehaviour
{

    private int points;
    private int score;
    private float time;

    private PlayerPrefsManager manager;

    public float percent;

    public string completedSceneName;
    public string completedSceneBundle;

    private List<string> steps;
    private List<string> stepsDescr;
    private List<int> wrongStepIndexes;
    private List<int> correctStepIndexes;

    private MBS.WUADisplay achievements;

    public List<string> quizQuestionsTexts = new List<string>();
    public List<int> quizWrongIndexes = new List<int>();

    private ActionManager actionManager;    //points, steps
    private GameTimer gameTimer; // time

    private Sprite halfStar;
    private Sprite fullStar;

    private bool emailsSent = false;

    void Start()
    {
        SceneManager.sceneLoaded += OnLoaded;

        manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        achievements = GameObject.Find("AchievementsDisplayPrefab").GetComponent<MBS.WUADisplay>();

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
            uiFolder.Find("ScoreScreen").Find("Points").GetComponent<Text>().text = "Punten: " + points;
            uiFolder.Find("ScoreScreen").Find("Time").GetComponent<Text>().text = string.Format("Tijd: {0}:{1:00}", (int)time / 60, (int)time % 60);

            //uiFolder.GetChild(1).FindChild("Steps").GetComponent<Text>().text = wrongSteps;

            actualScene = true;

            //update practice score & stars, update UI accordingly
            manager.UpdatePracticeHighscore(points, score);

            Transform stepParent = GameObject.Find("Interactable Objects/Canvas/PracticeStepsScreen/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            for (int i = 0; i < steps.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ProtocolPracticeSteps"), stepParent);
                step.transform.Find("Text").GetComponent<Text>().text = steps[i];

                Sprite correctSprite = Resources.Load<Sprite>("Sprites/item_select_check");
                if (correctStepIndexes.Contains(i))
                    step.transform.Find("ToggleNo").GetComponent<Image>().sprite = correctSprite;
            }

        }
        else if (SceneManager.GetActiveScene().name == "EndScore_Test")
        {
            Transform stepParent = GameObject.Find("Interactable Objects/Canvas/StepScreen/ObservationForm/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            for (int i = 0; i < steps.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ProtocolEvaluationStep"), stepParent);
                step.transform.Find("Text").GetComponent<Text>().text = steps[i];

                bool correct = correctStepIndexes.Contains(i);
                step.transform.Find("ToggleYes").GetComponent<Toggle>().isOn = correct;
                step.transform.Find("ToggleNo").GetComponent<Toggle>().isOn = !correct;
            }

            percent = 1.0f *
                (correctStepIndexes.Count + (quizQuestionsTexts.Count - quizWrongIndexes.Count))
                / (steps.Count + quizQuestionsTexts.Count);

            if (percent < 0f)
                percent = 0f;

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Score_percentage/ScoreText")
                .GetComponent<Text>().text = Mathf.FloorToInt(percent * 100f).ToString() + "%";

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Score_result/ResultText")
                .GetComponent<Text>().text = (percent < 0.7f) ? "Onvoldoende" : "Voldoende";

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Points")
                .GetComponent<Text>().text = "Punten: " + points;
            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Time")
                .GetComponent<Text>().text = string.Format("Tijd: {0}:{1:00}", (int)time / 60, (int)time % 60);

            actualScene = true;

            // show/hide buttons
            bool flag = (percent > 0.7f && manager.subscribed);

            GameObject.Find("Interactable Objects/Canvas/Send_Score/Top/Scenetitle").GetComponent<Text>().text = manager.currentSceneVisualName;

            // update test highscore + save certificate date
            manager.UpdateTestHighscore(percent);

            if (flag)
            {
                PlayerPrefsManager.__sendCertificateToUserMail(manager.currentSceneVisualName);

                achievements.UpdateKeys("FirstPassedExam", 1);

                if (manager.validatedScene)
                {
                    EndScoreSendMailResults();
                }
            }

            emailsSent = flag && manager.validatedScene;

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Buttons/Back to main menu")
                .GetComponent<Button>().onClick.AddListener(ConditionalHomeButton);

            // certificate set scene name
            GameObject.Find("Interactable Objects/Canvas/CertificatePanel/Top/Scenetitle")
                .GetComponent<Text>().text = manager.currentSceneVisualName;

            // certificate set btn function
            Button openCertificateBtn = GameObject.Find("Interactable Objects/Canvas/CertificatePanel/" +
                "ContentHolder/CertificateBTN").GetComponent<Button>();
            openCertificateBtn.onClick.AddListener(OpenCertificateBtn);

            // fullname pop up function set up
            GameObject.Find("Interactable Objects/Canvas/NamePopUp/BackToRegisterButton")
                .GetComponent<Button>().onClick.AddListener(SaveFullPlayerNameBtn);

            if (!flag || !manager.validatedScene)
            {
                GameObject.Find("Interactable Objects/Canvas/CertificatePanel/" +
                    "ContentHolder/Description (1)").SetActive(false);
                GameObject.Find("Interactable Objects/Canvas/CertificatePanel/" +
                    "ContentHolder/ScoreSendBTN").SetActive(false);
            }

        }
        if (actualScene)
        {
            SetBasicText();

            Transform quizParent = GameObject.Find("Interactable Objects/Canvas/Questionscreen/QuizForm/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;


            for (int i = 0; i < quizQuestionsTexts.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ProtocolQuestion"), quizParent);
                step.transform.Find("Text").GetComponent<Text>().text = quizQuestionsTexts[i];

                bool wrong = quizWrongIndexes.Contains(i);
                step.transform.Find("ToggleYes").GetComponent<Toggle>().isOn = !wrong;
                step.transform.Find("ToggleNo").GetComponent<Toggle>().isOn = wrong;
            }

            if (score >= 1.0f)
            {
                GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Stars/Star1").GetComponent<Image>().sprite = fullStar;
            }

            if (score >= 2.0f)
            {
                GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Stars/Star2").GetComponent<Image>().sprite = fullStar;
            }

            if (score >= 3.0f)
            {
                GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Stars/Star3").GetComponent<Image>().sprite = fullStar;
            }

            if (time >= 900.0f)
            {
                achievements.UpdateKeys("MoreThan15", 1);
            }
            else if (time <= 300.0f)
            {
                achievements.UpdateKeys("within5", 1);
            }

            if (PlayerPrefsManager.plays == 1)
            {
                achievements.UpdateKeys("FinishedProtocol", 1);
            }
            else if (PlayerPrefsManager.plays == 3)
            {
                achievements.UpdateKeys("FinishedProtocol", 2);
            }
            else if (PlayerPrefsManager.plays == 5)
            {
                achievements.UpdateKeys("FinishedProtocol", 2);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().SetSceneCompletionData(
                completedSceneName, points, Mathf.RoundToInt(time));
        }
        else
        {
            quizQuestionsTexts.Clear();
            quizWrongIndexes.Clear();
        }
    }

    public void SetBasicText()
    {
        if (quizQuestionsTexts.Count == 0)
        {
            Transform quizForm = GameObject.Find("Interactable Objects/Canvas/Questionscreen/QuizForm").transform;

            for (int i = 1; i < 5; i++)
            {
                if (i == 4)
                {
                    quizForm.GetChild(i).gameObject.SetActive(true);
                    break;
                }
                quizForm.GetChild(i).gameObject.SetActive(false);
            }
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

        points = actionManager.Points;
        score = Mathf.FloorToInt(3.0f * points / actionManager.TotalPoints);

        Debug.Log("Current points: " + points);
        Debug.Log("Total available points: " + actionManager.TotalPoints);
        Debug.Log("Calculated stars (3 * points/total): " + score);

        int multiplier = (time < 300.0f ? 3 : (time < 600.0f ? 2 : 1));
        points *= multiplier;
        Debug.Log("Multiplied final score: " + points);

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

    public void OpenCertificateBtn()
    {
        if (manager.fullPlayerName != "")
        {
            PlayerPrefsManager.__openCertificate(manager.currentSceneVisualName);
        }
        else
        {
            // open name pop up instead
            GameObject.Find("Interactable Objects/Canvas/NamePopUp").SetActive(true);
        }
    }

    public void SaveFullPlayerNameBtn()
    {
        manager.fullPlayerName = GameObject.Find("Interactable Objects/Canvas/NamePopUp/" +
            "FullnameHolder/FullName/Text").GetComponent<Text>().text;

        PlayerPrefsManager.SetFullName(manager.fullPlayerName);

        GameObject.Find("Interactable Objects/Canvas/NamePopUp").SetActive(false);
    }

    // copied from EndScoreSendMailResults.cs
    public void EndScoreSendMailResults()
    {
        string topic = "Care Up accreditatie aanvraag";
        string content = "Completed scene: " + manager.currentSceneVisualName + "\n";
        content += "Username: " + MBS.WULogin.username + "\n";
        content += "E-mail: " + MBS.WULogin.email + "\n";

        content += "Big- of registratienummer:" + manager.bigNumber + "\n";
        float percent = GameObject.FindObjectOfType<EndScoreManager>().percent;
        content += "Percentage: " + Mathf.FloorToInt(percent * 100f).ToString() + "%\n";

        achievements.UpdateKeys("StudyPoints", 1);

        PlayerPrefsManager.__sendMail(topic, content);
        Debug.Log("E-mail verzonden");
    }

    /// <summary>
    /// if email was sent - opens panel
    /// if email was not sent - loads main menu
    /// </summary>
    public void ConditionalHomeButton()
    {
        if (emailsSent)
        {
            GameObject.Find("Interactable Objects/Canvas/CertificatePopOp").SetActive(true);
        }
        else
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
        }
    }
}

