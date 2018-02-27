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

    private List<Question> questionList = new List<Question>();
    private int currentQuestionID = 0;

    private List<Button> buttons = new List<Button>();
    private bool[] buttonsActive = new bool[4];

    private Text descriptionText;
    private Button continueButton;
    private Button switchToInfoButton;

    public void Init(string name)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Quiz/" + name);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);

        XmlNodeList questions = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach(XmlNode q in questions)
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
            foreach(XmlNode a in answers)
            {
                Answer t = new Answer();
                t.text = a.Attributes["text"].Value;
                t.descr = a.Attributes["descr"].Value;
                question.answers.Add(t);
            }

            questionList.Add(question);
        }

        descriptionText = transform.Find("Description").GetComponent<Text>();
        continueButton = transform.Find("Continue").GetComponent<Button>();
        switchToInfoButton = transform.Find("SwitchToInfo").GetComponent<Button>();

        continueButton.gameObject.SetActive(false);
        descriptionText.text = "";
    }

    protected override void Start()
    {
        base.Start();

        GameObject.FindObjectOfType<PlayerScript>().quiz = this;
        gameObject.SetActive(false);

        buttons.Add(transform.Find("Answer1").GetComponent<Button>());
        buttons.Add(transform.Find("Answer2").GetComponent<Button>());
        buttons.Add(transform.Find("Answer3").GetComponent<Button>());
        buttons.Add(transform.Find("Answer4").GetComponent<Button>());

        continueButton.onClick.AddListener(OnContinueButton);
        switchToInfoButton.onClick.AddListener(OnSwitchToInfoTabButton);
    }

    public void NextQuizQuestion()
    {
        if (currentQuestionID >= questionList.Count)
            return;

        RobotManager manager = GameObject.FindObjectOfType<RobotManager>();
        manager.TriggerUI(true); // open UI
        manager.ToggleCloseBtn(false); // disable close button

        gameObject.SetActive(true);
        OnTabSwitch();

        Question current = questionList[currentQuestionID];
        transform.Find("QuestionText").GetComponent<Text>().text = current.text;
        
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
    }

    public void CorrectAnswer(string description)
    {
        GameObject.Find("GameLogic").GetComponent<ActionManager>().UpdatePointsDirectly(questionList[currentQuestionID].points);
        ActionManager.CorrectAction();

        descriptionText.text = description;

        ++currentQuestionID;

        foreach (Button b in buttons)
        {
            b.onClick.RemoveAllListeners();
        }

        continueButton.gameObject.SetActive(true);
    }

    public void WrongAnswer(string description)
    {
        descriptionText.text = description;
        GameObject.Find("GameLogic").GetComponent<ActionManager>().ActivatePenalty();
        ActionManager.WrongAction();
    }

    public void OnContinueButton()
    {
        Continue();

        GameObject.FindObjectOfType<RobotUITabs>().OnTabSwitch(); // switch tab to something else
        GameObject.FindObjectOfType<RobotManager>().TriggerUI(false); // close UI ?
    }

    public void Continue()
    {
        continueButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
        GameObject.FindObjectOfType<RobotManager>().ToggleCloseBtn(true); // enable close btn
    }

    public void OnSwitchToInfoTabButton()
    {
        RobotUITabs infoTab = tabs.Find(x => x.name == "InfoTab");
        if (infoTab != null)
            infoTab.OnTabSwitch();
    }

    protected override void SetTabActive(bool value)
    {
        base.SetTabActive(value);
        
        if (!value)
            continueButton.gameObject.SetActive(false);

        if (value)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(buttonsActive[i]);
            }
        }
    }
}
