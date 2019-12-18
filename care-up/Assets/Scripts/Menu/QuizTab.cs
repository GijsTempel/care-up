using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class QuizTab : MonoBehaviour
{
    public struct Question
    {
        public string text;
        public int answerID;
        public List<Answer> answers;
        public int points;
    };

    public struct Answer
    {
        public string text;
        public string descr;
    }

    public bool continueBtn = false;

    [HideInInspector]
    public bool quiz = false;

    private List<List<Question>> questionList = new List<List<Question>>();
    private List<List<Question>> encounterList = new List<List<Question>>();

    private int currentStep = 0;
    private int currentEncounter = 0;
    private int currentQuestionID = 0;

    public List<Button> buttons = new List<Button>();
    private bool[] buttonsActive = new bool[4];

    public Text descriptionText;
    public Button continueButton;
    public Button backToOptionsButton;
    public Text answeredTitleText;

    public Text quastionTitle;

    public GameObject answerPanel;
    public GameObject questionsPanel;
    public GameObject w_descriptionPanel;

    private PlayerPrefsManager pref;
    private EndScoreManager endScoreManager;

    private ActionManager manager;

    public static float encounterDelay = -1f;

    public void Init(string name)
    {
        manager = GameObject.FindObjectOfType<ActionManager>();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Quiz/" + name);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        RandomQuiz.randomQuestionsList = new List<Question>();

        XmlNodeList steps = xmlFile.FirstChild.NextSibling.ChildNodes;
        foreach (XmlNode s in steps)
        {
            List<Question> step = new List<Question>();
            List<Question> encounter = new List<Question>();
            XmlNodeList questions = s.ChildNodes;

            foreach (XmlNode q in questions)
            {
                Question question = new Question();
                question.answers = new List<Answer>();

                question.text = q.Attributes["text"].Value;
                int.TryParse(q.Attributes["answer"].Value, out question.answerID);

                if (q.Attributes["points"] != null)
                {
                    int.TryParse(q.Attributes["points"].Value, out question.points);
                }
                else
                {
                    question.points = 1;
                }

                --question.answerID; // let ppl write 1-4, but we need 0-3 as indexes

                XmlNodeList answers = q.ChildNodes;
                foreach (XmlNode a in answers)
                {
                    Answer t = new Answer();
                    t.text = a.Attributes["text"].Value;
                    t.descr = (a.Attributes["descr"] != null) ? a.Attributes["descr"].Value : "";
                    question.answers.Add(t);
                }
                if (s.Name == "encounter")
                    encounter.Add(question);
                else if (s.Name == "random")
                    RandomQuiz.randomQuestionsList.Add(question);
                else
                    step.Add(question);
            }
            if (step.Count > 0)
                questionList.Add(step);
            if (encounter.Count > 0)
                encounterList.Add(encounter);
        }
        // descriptionText = transform.GetChild(1).Find("ScrollViewDescription/Viewport/Content/Description").GetComponent<Text>();
        // continueButton = transform.GetChild(1).Find("Continue").GetComponent<Button>();
        // backToOptionsButton = transform.GetChild(1).Find("Back").GetComponent<Button>();
        // answeredTitleText = transform.GetChild(1).Find("AnswerTitle").GetComponent<Text>();
        continueBtn = false;
        continueButton.gameObject.SetActive(false);
        backToOptionsButton.gameObject.SetActive(false);
        answerPanel.SetActive(false);
        descriptionText.text = "";
    }

    private void Start()
    {
        PlayerScript.quiz = this;
        gameObject.SetActive(false);

        if (buttons.Count == 0)
        {
            buttons.Add(transform.GetChild(0).Find("Answer1").GetComponent<Button>());
            buttons.Add(transform.GetChild(0).Find("Answer2").GetComponent<Button>());
            buttons.Add(transform.GetChild(0).Find("Answer3").GetComponent<Button>());
            buttons.Add(transform.GetChild(0).Find("Answer4").GetComponent<Button>());
        }
        if (continueButton == null)
        {
            continueButton = transform.GetChild(1).Find("Continue").GetComponent<Button>();
        }

        continueButton.onClick.AddListener(OnContinueButton);

        if (backToOptionsButton == null)
        {
            backToOptionsButton = transform.GetChild(1).Find("Continue").GetComponent<Button>();
        }

        backToOptionsButton.onClick.AddListener(OnBackToOptionsButton);

        endScoreManager = GameObject.FindObjectOfType<EndScoreManager>();
    }

    public void NextQuizQuestion(bool random = false, bool encounter = false)
    {
        quiz = true;
        if (random == false)
        {
            if (encounter == false)
            {
                if (currentStep >= questionList.Count)
                {
                    print(currentStep);
                    print(questionList.Count);

                    PlayerScript.actionsLocked = false;
                    return;
                }
            }
            else if (currentEncounter >= encounterList.Count)
            {
                PlayerScript.actionsLocked = false;
                return;
            }
        }

        // open UI
        //GameObject.FindObjectOfType<PlayerScript>().OpenRobotUI();
        // disable close button
        //GameObject.FindObjectOfType<RobotManager>().ToggleCloseBtn(false);
        // enable quiz icon

        //GameObject.FindObjectOfType<PatientInfoManager>().SetTabActive("QuizTab");
        gameObject.SetActive(true);

        int currentQuestionID;
        Question current;

        if (random)
        {
            currentQuestionID = Random.Range(0, RandomQuiz.randomQuestionsList.Count);
            current = RandomQuiz.randomQuestionsList[currentQuestionID];
        }
        else if (encounter)
        {
            encounterDelay = -1f;
            GameUI.encounterStarted = false;
            currentQuestionID = Random.Range(0, encounterList[currentEncounter].Count);
            current = encounterList[currentEncounter][currentQuestionID];
        }
        else
        {
            currentQuestionID = Random.Range(0, questionList[currentStep].Count);
            current = questionList[currentStep][currentQuestionID];
        }

        quastionTitle.text = current.text;

        for (int i = 0; i < current.answers.Count; i++)
        {
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = current.answers[i].text;
        }

        buttons[current.answerID].onClick.AddListener(delegate { CorrectAnswer(current.answers[current.answerID].descr, random, encounter); });
        for (int i = 0; i < current.answers.Count; i++)
        {
            if (i != current.answerID)
            {
                string descr = current.answers[i].descr;
                buttons[i].onClick.AddListener(delegate { WrongAnswer(descr, random, encounter); });
            }

            buttons[i].gameObject.SetActive(true);
            buttonsActive[i] = true;
        }

        for (int i = current.answers.Count; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
            buttonsActive[i] = false;
        }

        if (random)
            RandomQuiz.randomQuestionsList.RemoveAt(currentQuestionID);

        descriptionText.text = "";

        if ((random == false) && (encounter == false))
        {
            if (endScoreManager != null)
            {
                endScoreManager.quizQuestionsTexts.Add(current.text);
            }
            else
            {
                Debug.LogWarning("No EndScoreManager found. Start from 1st scene.");
            }
        }
    }

    public void CorrectAnswer(string description, bool random = false, bool encounter = false)
    {
        //transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(1).gameObject.SetActive(true);
        //transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().interactable = true;
        //transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        questionsPanel.SetActive(false);
        answerPanel.SetActive(true);

        if ((encounter == false) && (random == false))
        {
            GameObject.Find("GameLogic").GetComponent<ActionManager>().UpdatePointsDirectly(
                questionList[currentStep][currentQuestionID].points);
        }

        ActionManager.CorrectAction();

        answeredTitleText.text = "Heel goed!";
        w_descriptionPanel.SetActive(description != "");
        descriptionText.text = description;

        if (random == false)
        {
            ++currentStep;
        }
        if (encounter)
        {
            ++currentEncounter;
        }

        foreach (Button b in buttons)
        {
            b.onClick.RemoveAllListeners();
        }
        gameObject.SetActive(true);
        continueBtn = true;
        continueButton.gameObject.SetActive(true);
        backToOptionsButton.gameObject.SetActive(false);
    }

    private void ChangeButtonPosition()
    {
        if (transform.GetChild(1).Find("ScrollViewDescription/Scrollbar Vertical") != null)
        {
            Vector3 pos = transform.position;
            pos.y = -100;
            pos.x = +70;

            backToOptionsButton.transform.position = pos;
            continueButton.transform.position = pos;
        }
    }

    public void WrongAnswer(string description, bool random = false, bool encounter = false)
    {
//#if UNITY_EDITOR
//        PlayerPrefsManager prefsManager = FindObjectOfType<PlayerPrefsManager>();
//        if (prefsManager != null)
//            if (prefsManager.testingMode)
//            {
//                CorrectAnswer(description);
//                return;
//            }
//#endif

        //----------------------------------------
        //transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(1).gameObject.SetActive(true);
        //transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().interactable = true;
        //transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        gameObject.SetActive(true);
        questionsPanel.SetActive(false);
        answerPanel.SetActive(true);


        if ((random == false) && (encounter == false))
        {
            GameObject.Find("GameLogic").GetComponent<ActionManager>().ActivatePenalty();
        }
        ActionManager.WrongAction();

        answeredTitleText.text = "Helaas, dit antwoord is niet goed";
        descriptionText.text = description;
        w_descriptionPanel.SetActive(description != "");
        continueBtn = false;
        continueButton.gameObject.SetActive(false);
        backToOptionsButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
        if (endScoreManager != null)
        {
            if ((random == false) && (encounter == false))
            {
                endScoreManager.quizWrongIndexes.Add(currentStep);
            }
        }
        else
        {
            Debug.LogWarning("No EndScoreManager found. Start from 1st scene.");
        }
    }

    public void OnContinueButton()
    {
        GameObject.FindObjectOfType<PatientInfoManager>().SetInteractability(true);
        GameObject.FindObjectOfType<RobotManager>().ToggleCloseBtn(true);
        //--------------
        //transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().interactable = false;
        //transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

        Transform scrollbar = transform.Find("QuizTab/QuizAnsweredDynamicCanvas/ScrollViewDescription/Scrollbar Vertical");
        if (scrollbar != null)
            scrollbar.GetComponent<Scrollbar>().value = 1;

        Continue();

        questionsPanel.SetActive(true);
        answerPanel.SetActive(false);

        // disable quiz icon
        //icons.Find("QuizTab").gameObject.SetActive(false);
        quiz = false;
        // close tab
        // BackButton();
        // close UI 
        GameObject.FindObjectOfType<PlayerScript>().CloseRobotUI();
        // enable player if needed
        PlayerScript.actionsLocked = false;
        //if (QuizTab.encounterDelay > 0)
        //    QuizTab.encounterDelay = -1;
    }

    public void Continue()
    {
        continueBtn = false;
        continueButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnBackToOptionsButton()
    {
        questionsPanel.SetActive(true);
        answerPanel.SetActive(false);
        //transform.GetChild(0).gameObject.SetActive(true);
        //transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().interactable = true;
        //transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        //transform.GetChild(1).gameObject.SetActive(false);
    }
}

