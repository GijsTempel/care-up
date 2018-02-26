using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class QuizTab : RobotUITabs {

    public string quizFilename = "test";

    public struct Question
    {
        public string text;
        public int answerID;
        public List<string> answers;
    };

    private List<Question> questionList = new List<Question>();
    private int currentQuestionID = 0;

    private List<Button> buttons = new List<Button>();

    private void Awake()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Quiz/" + quizFilename);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);

        XmlNodeList questions = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach(XmlNode q in questions)
        {
            Question question = new Question();
            question.answers = new List<string>();

            question.text = q.Attributes["text"].Value;
            int.TryParse(q.Attributes["answer"].Value, out question.answerID);
            --question.answerID; // let ppl write 1-4, but we need 0-3 as indexes

            XmlNodeList answers = q.ChildNodes;
            foreach(XmlNode a in answers)
            {
                question.answers.Add(a.Attributes["text"].Value);
            }

            questionList.Add(question);
        }
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

        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = current.answers[i];
        }

        buttons[current.answerID].onClick.AddListener(CorrectAnswer);
        for(int i = 0; i < 4; i++)
        {
            if (i != current.answerID)
            {
                buttons[i].onClick.AddListener(WrongAnswer);
            }
        }
    }

    public void CorrectAnswer()
    {
        Debug.Log("Quiz::CorrectAnswer");

        ++currentQuestionID;

        foreach (Button b in buttons)
        {
            b.onClick.RemoveAllListeners();
        }
        
        gameObject.SetActive(false);
        GameObject.FindObjectOfType<RobotUITabs>().OnTabSwitch(); // switch tab to something else

        RobotManager manager = GameObject.FindObjectOfType<RobotManager>();
        manager.ToggleCloseBtn(true); // enable close btn
        //manager.TriggerUI(false); // close UI ?
    }

    public void WrongAnswer()
    {
        Debug.Log("Quiz::WrongAnswer");
    }
}
