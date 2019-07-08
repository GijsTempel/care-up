using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class QuizTab : RobotUITabs {

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

    private List<List<Question>> questionList = new List<List<Question>>();
    private int currentStep = 0;
    private int currentQuestionID = 0;

    private List<Button> buttons = new List<Button>();
    private bool[] buttonsActive = new bool[4];

    private Text descriptionText;
    private Button continueButton;
  
    public bool continueBtn = false;

    [HideInInspector]
    public bool quiz = false;

    PlayerPrefsManager pref;

    EndScoreManager endScoreManager;

    public void Init(string name)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Quiz/" + name);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);

        XmlNodeList steps = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode s in steps)
        {
            List<Question> step = new List<Question>();

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
                    t.descr = a.Attributes["descr"].Value;
                    question.answers.Add(t);
                }

                step.Add(question);
            }

            questionList.Add(step);
        }

        descriptionText = transform.GetChild(0).Find("Description").GetComponent<Text>();
        continueButton = transform.GetChild(0).Find("Continue").GetComponent<Button>();

        continueButton.gameObject.SetActive(continueBtn);
        descriptionText.text = "";
    }

    protected override void Start()
    {
        base.Start();

        PlayerScript.quiz = this;
        gameObject.SetActive(false);

        buttons.Add(transform.GetChild(0).Find("Answer1").GetComponent<Button>());
        buttons.Add(transform.GetChild(0).Find("Answer2").GetComponent<Button>());
        buttons.Add(transform.GetChild(0).Find("Answer3").GetComponent<Button>());
        buttons.Add(transform.GetChild(0).Find("Answer4").GetComponent<Button>());

        if (continueButton == null)
        {
            continueButton = transform.GetChild(1).Find("Continue").GetComponent<Button>();
        }
        continueButton.onClick.AddListener(OnContinueButton);

        tabTrigger.SetActive(false);

        endScoreManager = GameObject.FindObjectOfType<EndScoreManager>();
    }

    public void NextQuizQuestion()
    {
        quiz = true;
        if (currentStep >= questionList.Count)
            return;

        // open UI
        GameObject.FindObjectOfType<PlayerScript>().OpenRobotUI();
        // disable close button
        GameObject.FindObjectOfType<RobotManager>().ToggleCloseBtn(false);
        // enable quiz icon
        icons.Find("QuizTab").gameObject.SetActive(true);

        gameObject.SetActive(true);
        OnTabSwitch();

        int currentQuestionID = Random.Range(0, questionList[currentStep].Count);

        Question current = questionList[currentStep][currentQuestionID];
        transform.GetChild(0).Find("QuestionText").GetComponent<Text>().text = current.text;

        for (int i = 0; i < current.answers.Count; i++)
        {
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = current.answers[i].text;
        }

        buttons[current.answerID].onClick.AddListener(delegate { CorrectAnswer(current.answers[current.answerID].descr); });
        for (int i = 0; i < current.answers.Count; i++)
        {
            if (i != current.answerID)
            {
                string descr = current.answers[i].descr;
                buttons[i].onClick.AddListener(delegate { WrongAnswer(descr); });
            }

            buttons[i].gameObject.SetActive(true);
            buttonsActive[i] = true;
        }

        for (int i = current.answers.Count; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
            buttonsActive[i] = false;
        }

        descriptionText.text = "";

        if (endScoreManager != null)
        {
            endScoreManager.quizQuestionsTexts.Add(current.text);
        }
        else
        {
            Debug.LogWarning("No EndScoreManager found. Start from 1st scene.");
        }
    }

    public void CorrectAnswer(string description)
    {
        GameObject.Find("GameLogic").GetComponent<ActionManager>().UpdatePointsDirectly(questionList[currentStep][currentQuestionID].points);
        ActionManager.CorrectAction();

        descriptionText.text = description;

        ++currentStep;

        foreach (Button b in buttons)
        {
            b.onClick.RemoveAllListeners();
        }

        continueBtn = true;
        continueButton.gameObject.SetActive(continueBtn);

        GameObject.FindObjectOfType<RobotManager>().ToggleCloseBtn(true); // enable close btn
    }

    public void WrongAnswer(string description)
    {
#if UNITY_EDITOR
        if (GameObject.FindObjectOfType<PlayerPrefsManager>() != null)
            if (GameObject.FindObjectOfType<PlayerPrefsManager>().testingMode)
            {
                CorrectAnswer(description);
                return;
            }
#endif
        descriptionText.text = description;
        GameObject.Find("GameLogic").GetComponent<ActionManager>().ActivatePenalty();
        ActionManager.WrongAction();

        if (endScoreManager != null)
        {
            endScoreManager.quizWrongIndexes.Add(currentStep);
        }
        else
        {
            Debug.LogWarning("No EndScoreManager found. Start from 1st scene.");
        }
    }

public void OnContinueButton()
    {
        Continue();

        // disable quiz icon
        icons.Find("QuizTab").gameObject.SetActive(false);
        quiz = false;
        // close tab
        BackButton();
        // close UI 
        GameObject.FindObjectOfType<PlayerScript>().CloseRobotUI();
        // enable player if needed
        PlayerScript.actionsLocked = false;
    }

    public void Continue()
    {
        continueBtn = false;
        continueButton.gameObject.SetActive(continueBtn);
        gameObject.SetActive(false);
    }

    protected override void SetTabActive(bool value)
    {
        base.SetTabActive(value);

        if (!value)
        {
            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(continueBtn);
            }
            else
            {
                transform.GetChild(0).Find("Continue").gameObject.SetActive(continueBtn);
            }
        }
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(buttonsActive[i]);
            }
        }
    }
}
