using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System;
using MBS;

/// <summary>
/// Handles EndScore scene.
/// </summary>
public class EndScoreManager : MonoBehaviour
{
    private int points;
    private int score;
    private float time;

    private PlayerPrefsManager manager;

    public int percent;

    public string completedSceneName;
    public string completedSceneBundle;
    public Text reward;

    private List<string> steps;
    private List<int> wrongStepIndexes;
    private List<int> correctStepIndexes;
    public int randomEventQuestionsCount = 0;
    private MBS.WUADisplay achievements;

    public List<string> quizQuestionsTexts = new List<string>();
    public List<string> randQuestionsTexts = new List<string>();

    public List<int> quizWrongIndexes = new List<int>();
    public List<int> randomWrongIndexes = new List<int>();


    private ActionManager actionManager;    //points, steps
    private GameTimer gameTimer; // time

    private Sprite halfStar;
    private Sprite fullStar;

    private bool emailsSent = false;
    public static bool showReward = false;
    ActionsPanel actionsPanel;
    void Start()
    {
        SceneManager.sceneLoaded += OnLoaded;

        manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        achievements = GameObject.Find("AchievementsDisplayPrefab").GetComponent<MBS.WUADisplay>();
        fullStar = Resources.Load<Sprite>("Sprites/Stars/star");
    }

    public int GetRandWrongIndexe(string eventText)
    {
        int result = -1;
        for (int i = 0; i < randQuestionsTexts.Count; i++)
        {

            if (randQuestionsTexts[i] == eventText)
            {
                result = i;
            }
        }
        return result;
    }

    public int GetQuizWrongIndexe(string quizText)
    {
        int result = -1;
        for (int i = 0; i < quizQuestionsTexts.Count; i++)
        {

            if (quizQuestionsTexts[i] == quizText)
            {
                result = i;
            }
        }
        return result;
    }
    private void InitializeObjects()
    {
        if (actionManager == null)
            actionManager = GameObject.FindObjectOfType<ActionManager>();
        if (actionsPanel == null)
            actionsPanel = GameObject.FindObjectOfType<ActionsPanel>();
    }
    /// <summary>
    /// Sets object variables in scene after loading.
    /// </summary>
    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        bool actualScene = false;

        if (SceneManager.GetActiveScene().name == "EndScore")
        {
            Transform uiFolder = GameObject.Find("Canvas").transform;
            SetBasicText();
            uiFolder.Find("ScoreScreen/ScoreInfo/InfoHolder/Info/Points").GetComponent<Text>().text = "Punten: " + points;
            uiFolder.Find("ScoreScreen/ScoreInfo/InfoHolder/Info/Time").GetComponent<Text>().text = string.Format("Tijd: {0}:{1:00}", (int)time / 60, (int)time % 60);

            actualScene = true;

            //update practice score & stars, update UI accordingly
            manager.UpdatePracticeHighscore(points, score);

            Transform stepParent = uiFolder.Find("PracticeStepsScreen/Image/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            for (int i = 0; i < steps.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/ProtocolPracticeSteps"), stepParent);
                step.transform.Find("Text").GetComponent<Text>().text = steps[i];

                Sprite correctSprite = Resources.Load<Sprite>("Sprites/item_select_check");
                if (correctStepIndexes.Contains(i))
                    step.transform.Find("ToggleNo").GetComponent<Image>().sprite = correctSprite;
            }

        }
        else if (SceneManager.GetActiveScene().name == "EndScore_Test")
        {
            if (manager.currentPracticePlays < manager.currentDifficultyLevel)
            {
                if (manager.currentDifficultyLevel < 4)
                {
                    // PlayerPrefsManager.AddOneToPracticePlays(manager.currentSceneVisualName);
                    PlayerPrefsManager.SetValueToSceneInCategory(manager.currentSceneVisualName, "PracticePlays", manager.currentDifficultyLevel);
                }
                else
                {
                    PlayerPrefsManager.AddOneToPracticePlays(manager.currentSceneVisualName);
                }
            }
            Transform stepParent = GameObject.Find("Interactable Objects/Canvas/StepScreen/Image/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            SetBasicText();

            for (int i = 0; i < steps.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/ProtocolEvaluationStep"), stepParent);
                step.transform.Find("Text").GetComponent<Text>().text = steps[i];

                bool correct = correctStepIndexes.Contains(i);
                step.transform.Find("ToggleYes").GetComponent<Toggle>().isOn = correct;
                step.transform.Find("ToggleNo").GetComponent<Toggle>().isOn = !correct;
            }

            percent = CalculatePercentage();
            ActionManager.percentage = percent;

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreInfo/ResultInfoHolder/Value")
                .GetComponent<Text>().text = percent.ToString() + "%";

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreInfo/ResultInfoHolder/Result")
                .GetComponent<Text>().text = (percent < 70) ? "Onvoldoende" : "Voldoende";

            actualScene = true;

            // show/hide buttons
            bool subscribed = manager.subscribed;
            if (manager.IsScenePurchasedByName(completedSceneName))
                subscribed = true;
            bool flag = (percent > 70 && (subscribed || HasFreeCert()));

            // update test highscore + save certificate date
            if (manager.currentDifficultyLevel >= 4)
            {
                manager.UpdateTestHighscore(percent);

                if (flag)
                {
                    //PlayerPrefsManager.__sendCertificateToUserMail(manager.currentSceneVisualName);
                    //PlayerPrefsManager.__openCertificate(manager.currentSceneVisualName);

                    achievements.UpdateKeys("FirstPassedExam", 1);

                    if (manager.validatedScene)
                    {
                        EndScoreSendMailResults();
                        AutomaticPEcourseValidation();
                    }
                }
                emailsSent = flag;
                // track amount of results per scene
                if (percent < 70)
                {
                    PlayerPrefsManager.AddOneToTestFails(manager.currentSceneVisualName);
                }
                else
                {
                    PlayerPrefsManager.AddOneToTestPassed(manager.currentSceneVisualName);
                }

            }
            else
            {
                manager.UpdatePracticeHighscore(points, score);
            }

            GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreScreenButtons/Panel (2)/BackToMainMenu")
                .GetComponent<Button>().onClick.AddListener(ConditionalHomeButton);
        }
        if (actualScene)
        {
            showReward = true;

            Transform quizParent = GameObject.Find("Interactable Objects/Canvas/Questionscreen/Image/QuizForm/WrongstepScroll/WrongstepViewport/LayoutGroup").transform;

            for (int i = 0; i < quizQuestionsTexts.Count; ++i)
            {
                GameObject step = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/ProtocolQuestion"), quizParent);
                step.transform.Find("Text").GetComponent<Text>().text = quizQuestionsTexts[i];

                bool wrong = quizWrongIndexes.Contains(i);
                step.transform.Find("Toggles/ToggleYes").GetComponent<Toggle>().isOn = !wrong;
                step.transform.Find("Toggles/ToggleNo").GetComponent<Toggle>().isOn = wrong;
            }

            if (SceneManager.GetActiveScene().name == "EndScore")
            {
                Transform starsFolder = GameObject.Find("Interactable Objects/Canvas/ScoreScreen/ScoreInfo/TopBar/Stars/Stars").transform;
                if (score >= 1.0f)
                    starsFolder.transform.Find("Star1").GetComponent<Image>().sprite = fullStar;
                if (score >= 2.0f)
                    starsFolder.transform.Find("Star2").GetComponent<Image>().sprite = fullStar;
                if (score >= 3.0f)
                    starsFolder.transform.Find("Star3").GetComponent<Image>().sprite = fullStar;
            }
            else if (SceneManager.GetActiveScene().name == "EndScore_test")
            {
                if (score >= 1.0f)
                    GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Stars/Star1").GetComponent<Image>().sprite = fullStar;
                if (score >= 2.0f)
                    GameObject.Find("Interactable Objects/Canvas/ScoreScreen/Stars/Star2").GetComponent<Image>().sprite = fullStar;
                if (score >= 3.0f)
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

            int counter = 0;
            // add in-game currency once 3 finishes?
            int.TryParse(DatabaseManager.FetchField("Store", "FinishedCounter"), out counter);
            if (++counter >= 3)
            {
                counter = 0;
                PlayerPrefsManager.storeManager.ModifyCurrencyBy(5);
            }
            DatabaseManager.UpdateField("Store", "FinishedCounter", counter.ToString());

            // add in-game store presents once 15 successful finishes?
            int.TryParse(DatabaseManager.FetchField("Store", "SuccessCounter"), out counter);
            if (percent >= 70) ++counter;
            if (counter >= 15)
            {
                counter = 0;
                PlayerPrefsManager.storeManager.ModifyPresentsBy(1);
            }
            DatabaseManager.UpdateField("Store", "SuccessCounter", counter.ToString());

            GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().SetSceneCompletionData(
                completedSceneName, points, Mathf.RoundToInt(time));

            // calculate xPoints added
            int xp = 1;
            int.TryParse(manager.currentSceneXPoints, out xp);

            // difficulty multipliers : 0/1/1.5/2/2
            // difficulties :  0/1/2/3/4
            int total_xp = xp;
            switch (manager.currentDifficultyLevel)
            {
                case 2:
                    xp = xp + xp/2;
                    break;
                case 3:
                case 4:
                    xp *= 2;
                    break;
                default:
                    break;
            }

            // if totalxp = 0, then it's the 0 difficulty aka "watch the video" so we award 5points
            if (total_xp == 0) 
            {
                total_xp = 5;
            }
            else // if not it's juts normal scene, we multiply by percentage to reduce xp for barely completed scenes
            {   
                total_xp = Mathf.FloorToInt(total_xp * (percent / 100f));
            }

            if (DatabaseManager.leaderboardDB.isInTheBoard)
            {
                DatabaseManager.leaderboardDB.AddPointsToCurrent(WULogin.UID, total_xp);
            }
            else {
                DatabaseManager.leaderboardDB.PushToLeaderboard(WULogin.UID, WULogin.display_name, total_xp);
            }

            DatabaseManager.UpdateCompletedSceneScore(manager.currentSceneVisualName, manager.currentDifficultyLevel, percent);
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
            quizForm.Find("YesNo").gameObject.SetActive(false);
            quizForm.Find("WrongstepScroll").gameObject.SetActive(false);
            quizForm.Find("BasicText").gameObject.SetActive(true);
        }
        else
            quizForm.Find("BasicText").gameObject.SetActive(false);
    }

    public int CalculatePercentage()
    {
        //If you need to test endscore scene with hith score, uncomment this line:
        //return 99;

        InitializeObjects();

        int correctSteps;
        int totalSteps;

        if (actionManager == null)
        {
            correctSteps = correctStepIndexes.Count;
            totalSteps = steps.Count;
        }
        else
        {
            correctSteps = actionManager.CorrectStepIndexes.Count;
            totalSteps = actionManager.StepsList.Count;
        }

        float result = (float)(correctSteps + (quizQuestionsTexts.Count - quizWrongIndexes.Count) 
            + (randQuestionsTexts.Count - randomWrongIndexes.Count))
            / (totalSteps + QuizTab.totalQuizesCount + randomEventQuestionsCount);
        if (actionsPanel != null)
        {
            if (actionsPanel.mode == ActionsPanel.Mode.Score)
            {
                actionsPanel.SetScore((int)(result * 100f));
                string ss = "totalSteps: " + totalSteps.ToString() + "\n";
                ss += "correctSteps: " + correctSteps.ToString() + "\n";
                ss += "quizQuestionsTexts: " + quizQuestionsTexts.Count.ToString() + "\n";
                ss += "quizWrongIndexes: " + quizWrongIndexes.Count.ToString() + "\n";
                ss += "totalQuizesCount: " + QuizTab.totalQuizesCount.ToString() + "\n";

                ss += "randomEventQuestionsCount: " + randomEventQuestionsCount.ToString() + "\n";
                ss += "randQuestionsTexts.Count: " + randQuestionsTexts.Count.ToString() + "\n";
                ss += "randomWrongIndexes.Count: " + randomWrongIndexes.Count.ToString() + "\n";
                ss += "randomEventBookmaks: " + ActionManager.randomEventBookmaks.Count.ToString() + "\n";
                ss += "(" + correctSteps.ToString() + " + (" + quizQuestionsTexts.Count.ToString()
                    + "-" + quizWrongIndexes.Count.ToString() + ")+("
                    + randQuestionsTexts.Count.ToString() + "-" + randomWrongIndexes.Count.ToString()
                    + "))/(" + totalSteps.ToString() + "+" + QuizTab.totalQuizesCount.ToString()
                    + "+" + randomEventQuestionsCount.ToString() + ")\n= " + result.ToString();
                actionsPanel.SetScoreDataText(ss);
            }
        }
        if (result < 0f) result = 0f;

        return Mathf.FloorToInt(result * 100f);
    }

    /// <summary>
    /// Gets necesarry variables and loads EndScore scene.
    /// </summary>
    public void LoadEndScoreScene()
    {
        StoreViewModel.SavedCoins = ActionManager.Points;
        InitializeObjects();
        if (actionManager == null) Debug.LogError("No action manager found.");

        gameTimer = GameObject.Find("GameLogic").GetComponent<GameTimer>();
        if (gameTimer == null) Debug.LogError("No timer found");

        time = gameTimer.CurrentTime;

        points = ActionManager.Points;
        score = Mathf.FloorToInt(3.0f * points / actionManager.TotalPoints);

        Debug.Log("Current points: " + points);
        Debug.Log("Total available points: " + actionManager.TotalPoints);
        Debug.Log("Calculated stars (3 * points/total): " + score);

        int multiplier = (time < 300.0f ? 3 : (time < 600.0f ? 2 : 1));
        points *= multiplier;
        Debug.Log("Multiplied final score: " + points);

        completedSceneName = SceneManager.GetActiveScene().name;

        steps = actionManager.StepsList;
        wrongStepIndexes = actionManager.WrongStepIndexes;
        correctStepIndexes = actionManager.CorrectStepIndexes;

        PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        //if (manager.practiceMode || manager.currentDifficultyLevel == 2)
        //{
        //bl_SceneLoaderUtils.GetLoader.LoadLevel("EndScore");
        //}
        //else
        //{
        bl_SceneLoaderUtils.GetLoader.LoadLevel("EndScore_Test");
        //}
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

    public void EndScoreSendMailResults()
    {
        achievements.UpdateKeys("StudyPoints", 1);

        int percent = GameObject.FindObjectOfType<EndScoreManager>().percent;

        string link = "https://leren.careup.online/MailSceneComplStats.php";
        link += "?name=" + MBS.WULogin.username;
        link += "&scene=" + manager.currentSceneVisualName;
        link += "&userMail=" + MBS.WULogin.email;
        link += "&bigNum=" + manager.bigNumber;
        link += "&percent=" + percent.ToString();

        link += "&courseID=" + manager.currentPEcourseID;
        link += "&moduleID=" + PlayerPrefsManager.GetCourseIDbyModuleID(manager.currentPEcourseID);
        link += "&datetime=" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")
                    .Replace("+", "%2B"); // "+" sign is operator in url, need to replace

        UnityWebRequest unityWebRequest = new UnityWebRequest(link.Replace(" ", "%20"));
        unityWebRequest.SendWebRequest();
        Debug.Log("E-mail verzonden");
    }
    
    bool HasFreeCert()
    {
        return manager.HasFreeCert(manager.currentSceneVisualName);
    }
    /// <summary>
    /// if email was sent - opens panel
    /// if email was not sent - loads main menu
    /// </summary>
    public void ConditionalHomeButton()
    {
        // special panel for demo
        bool subscribed = manager.subscribed;
        if (manager.IsScenePurchasedByName(completedSceneName))
            subscribed = true;
        if (!subscribed && !HasFreeCert())
        {
            GameObject.Find("Interactable Objects/Canvas/CertificateDemoPopOp").SetActive(true);
        }
        else if (emailsSent) // if not demo and email sent, special panel
        {
            GameObject.Find("Interactable Objects/Canvas/CertificatePopOp").SetActive(true);
            if (manager.validatedScene == false)
            {   // changing pop up text if scene is not validated
                GameObject.Find("/Interactable Objects/Canvas/CertificatePopOp/Certificate/RegText").GetComponent<Text>().text
                    = "Je kan je certificaat nu downloaden via de knop hieronder. Dit kan ook later vanuit de portal.";
            }
        }
        else // if not demo and emails not sent just load menu
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
        }
    }

    // testing mode for now (test.pe-online)
    // "GET" request is pretty bad practice, but this is 8th attempt and first thing that gave an actual server response
    public void AutomaticPEcourseValidation()
    {
        Debug.Log("AutomaticPEcourseValidation");

        if (manager.bigNumber == "")
        {
            Debug.LogWarning("BigNumber was empty, aborting PEcourseValidation.");
            return;
        }

        // Create a request for the URL.
        string _url = "https://www.pe-online.org/pe-services/pe-attendanceelearning/WriteAttendance.asmx/ProcessXML?sXML=";
       
        // someone's BIG for testing only // insuline for testing only
        _url += PlayerPrefsManager.GenerateAttendanceSXML(manager.bigNumber, manager.currentPEcourseID);

        // filter scenes that are not "connected"
        if (_url.Contains("<externalmoduleID>X</externalmoduleID>"))
        {
            Debug.Log("PE course not connected. Aborting AutomaticPEcourseValidation");
            return;
        }

        Uri uri = new Uri(_url, UriKind.Absolute);
        _url = uri.ToString();

        Debug.Log("Generated url: " + _url);

        // get request woo
        StartCoroutine(GetRequest(_url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}
