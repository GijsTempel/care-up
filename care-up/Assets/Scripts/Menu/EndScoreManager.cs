﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Handles EndScore scene.
/// </summary>
public class EndScoreManager : MonoBehaviour
{
    public float percent;
    public string completedSceneName;
    public string completedSceneBundle;

    public List<string> quizQuestionsTexts = new List<string>();
    public List<int> quizWrongIndexes = new List<int>();

    private int points;
    private int score;
    private float time;

    private List<string> steps;
    private List<string> stepsDescr;
    private List<int> wrongStepIndexes;
    private List<int> correctStepIndexes;

    private PlayerPrefsManager manager;
    private MBS.WUADisplay achievements;

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
            uiFolder.Find("ScoreScreen/ScoreInfo/InfoHolder/Info/Points").GetComponent<Text>().text = "Punten: " + points;
            uiFolder.Find("ScoreScreen/ScoreInfo/InfoHolder/Info/Time").GetComponent<Text>().text = string.Format("Tijd: {0}:{1:00}", (int)time / 60, (int)time % 60);

            //uiFolder.GetChild(1).FindChild("Steps").GetComponent<Text>().text = wrongSteps;

            actualScene = true;

            //update practice score & stars, update UI accordingly
            manager.UpdatePracticeHighscore(points, score);

            Transform stepParent = uiFolder.Find("PracticeStepsScreen/Image/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            for (int i = 0; i < steps.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ProtocolPracticeSteps"), stepParent);
                step.transform.Find("Text").GetComponent<Text>().text = steps[i];

                Sprite correctSprite = Resources.Load<Sprite>("Sprites/item_select_check");
                if (correctStepIndexes.Contains(i))
                    step.transform.Find("ToggleNo").GetComponent<Image>().sprite = correctSprite;
            }

            Transform starsFolder = GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreInfo/TopBar/Stars/Stars").transform;

            if (score >= 1.0f)
            {
                starsFolder.Find("Star1").GetComponent<Image>().sprite = fullStar;
            }

            if (score >= 2.0f)
            {
                starsFolder.Find("Star2").GetComponent<Image>().sprite = fullStar;
            }

            if (score >= 3.0f)
            {
                starsFolder.Find("Star2").GetComponent<Image>().sprite = fullStar;
            }
        }
        else if (SceneManager.GetActiveScene().name == "EndScore_Test")
        {
            SetBasicText();

            Transform stepParent = GameObject.Find("Interactable Objects/Canvas/StepScreen/Image/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

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

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreInfo/ResultInfoHolder/Value")
                .GetComponent<Text>().text = Mathf.FloorToInt(percent * 100f).ToString() + "%";

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreInfo/ResultInfoHolder/Result")
                .GetComponent<Text>().text = (percent < 0.7f) ? "Onvoldoende" : "Voldoende";

            //GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreInfo/InfoHolder/InfoGroup/Info/Points")
            //    .GetComponent<Text>().text = "Punten: " + points;

            //GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreInfo/InfoHolder/InfoGroup/Info/Time")
            //    .GetComponent<Text>().text = string.Format("Tijd: {0}:{1:00}", (int)time / 60, (int)time % 60);

            actualScene = true;

            // show/hide buttons
            bool flag = (percent > 0.7f && manager.subscribed);

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

            emailsSent = flag;

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Buttons/BackToMainMenu")
                .GetComponent<Button>().onClick.AddListener(ConditionalHomeButton);
        }
        if (actualScene)
        {
          
            Transform quizParent = GameObject.Find("Interactable Objects/Canvas/Questionscreen/Image/QuizForm/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            for (int i = 0; i < quizQuestionsTexts.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ProtocolQuestion"), quizParent);
                step.transform.Find("Text").GetComponent<Text>().text = quizQuestionsTexts[i];

                bool wrong = quizWrongIndexes.Contains(i);
                step.transform.Find("Toggles/ToggleYes").GetComponent<Toggle>().isOn = !wrong;
                step.transform.Find("Toggles/ToggleNo").GetComponent<Toggle>().isOn = wrong;
            }

            if (time >= 900.0f)

                if (time >= 900.0f)
                {
                    achievements.UpdateKeys("MoreThan15", 1);
                }
                else if (time <= 300.0f)
                {
                    achievements.UpdateKeys("within5", 1);
                }

            string scompleted = DatabaseManager.FetchField("AchievementData", "ProtocolsFinished");
            int completed = scompleted != "" ? int.Parse(scompleted) + 1 : 1;

            DatabaseManager.UpdateField("AchievementData", "ProtocolsFinished", completed.ToString());

            if (completed >= 1)
            {
                achievements.UpdateKeys("FinishedProtocol", 1);
            }
            if (completed >= 3)
            {
                achievements.UpdateKeys("Finished3Protocols", 1);
            }
            if (completed >= 5)
            {
                achievements.UpdateKeys("Finished5Protocols", 1);
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
        Transform quizForm = GameObject.Find("Interactable Objects/Canvas/Questionscreen/Image/QuizForm").transform;
        if (quizQuestionsTexts.Count == 0)
        {         
            quizForm.GetChild(2).gameObject.SetActive(false);
            quizForm.GetChild(3).gameObject.SetActive(false);
            quizForm.GetChild(4).gameObject.SetActive(true);           
        }
        else
            quizForm.GetChild(4).gameObject.SetActive(false);
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
            "InfoHolder/FullNameInfoHolder/InputFieldHolder/InputField/Text").GetComponent<Text>().text;

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
            GameObject.Find("Interactable Objects/Canvas/CertificatePopOp/Certificate/InfoHolder/RegText").SetActive(true);
            if (manager.validatedScene == false)
            {   // changing pop up text if scene is not validated
                GameObject.Find("Interactable Objects/Canvas/CertificatePopOp/Certificate/InfoHolder/RegText").GetComponent<Text>().text
                    = "Neem snel een kijkje in je mailbox! Daar vind je je certificaat.";
            }
        }

        else
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
        }
    }
}

