using System.Xml;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour {

    public enum ActionType
    {
        ObjectCombine,
        ObjectUse,
        PersonTalk,
        ObjectUseOn,
        ObjectExamine
    };

    public string actionListName;

    private List<Action> actionList = new List<Action>();
    private List<string> wrongStepsList = new List<string>();

    private int points = 0;
    private int currentAction = 0;

    public List<Action> ActionList
    {
        get { return actionList; }
    }

    public string WrongSteps
    {
        get
        {
            string wrongSteps = "";
            foreach (string step in wrongStepsList)
                wrongSteps += step + "\n";
            return wrongSteps;
        }
    }

    public int Points
    {
        get { return points; }
        set { points = value; }
    }

    public int CurrentAction
    {
        get { return currentAction; }
        set { currentAction = value; }
    }

    public string CurrentDescription
    {
        get {
            return actionList.Where(action =>
          action.SubIndex == currentAction &&
          action.matched == false).First().description;
        }
    }

    void Start () {

        XmlDocument xmlFile = new XmlDocument();
        xmlFile.Load("Assets/Resources/Xml/Actions/" + actionListName + ".xml");
        XmlNodeList actions = xmlFile.FirstChild.NextSibling.ChildNodes; // xml is not a node, bug

        foreach ( XmlNode action in actions )
        {
            int index;
            int.TryParse(action.Attributes["index"].Value, out index);
            string type = action.Attributes["type"].Value;
            string descr = action.Attributes["description"].Value;
           
            switch (type)
            {
                case "combine":
                    string left = action.Attributes["left"].Value;
                    string right = action.Attributes["right"].Value;
                    actionList.Add(new CombineAction(left, right, index, descr));
                    break;
                case "use":
                    string use = action.Attributes["value"].Value;
                    actionList.Add(new UseAction(use, index, descr));
                    break;
                case "talk":
                    string topic = action.Attributes["topic"].Value;
                    actionList.Add(new TalkAction(topic, index, descr));
                    break;
                case "useOn":
                    string useItem = action.Attributes["item"].Value;
                    string target = action.Attributes["target"].Value;
                    actionList.Add(new UseOnAction(useItem, target, index, descr));
                    break;
                case "examine":
                    string exItem = action.Attributes["item"].Value;
                    string expected = action.Attributes["expected"].Value;
                    actionList.Add(new ExamineAction(exItem, expected, index, descr));
                    break;
                default:
                    Debug.LogError("No action type found: " + type);
                    break;
            }
        }
    }

    public void OnCombineAction(string leftHand, string rightHand)
    {
        string[] info = { leftHand, rightHand };
        bool occured = Check(info, ActionType.ObjectCombine);
        points += occured ? 1 : -1;

        Debug.Log("Combine " + leftHand + " and " + rightHand + " with result " + occured);

        CheckScenarioCompleted();
    }

    public void OnUseAction(string useObject)
    {
        string[] info = { useObject };
        bool occured = Check(info, ActionType.ObjectUse);
        points += occured ? 1 : -1;

        Debug.Log("Use " + useObject + " with result " + occured);

        CheckScenarioCompleted();
    }

    public void OnTalkAction(string topic)
    {
        string[] info = { topic };
        bool occured = Check(info, ActionType.PersonTalk);
        points += occured ? 1 : -1;

        Debug.Log("Say " + topic + " with result " + occured);

        CheckScenarioCompleted();
    }
    
    public void OnUseOnAction(string item, string target)
    {
        string[] info = { item, target };
        bool occured = Check(info, ActionType.ObjectUseOn);
        points += occured ? 1 : -1;

        Debug.Log("Use " + item + " on " + target + " with result " + occured);

        CheckScenarioCompleted();
    }

    public void OnExamineAction(string item, string expected)
    {
        string[] info = { item, expected };
        bool occured = Check(info, ActionType.ObjectExamine);
        points += occured ? 1 : 0; // no penalty

        Debug.Log("Examine " + item + " with state " + expected + " with result " + occured);

        CheckScenarioCompleted();
    }

    public bool Check(string[] info, ActionType type)
    {
        bool matched = false;

        List<Action> sublist = actionList.Where(action =>
            action.SubIndex == currentAction &&
            action.matched == false).ToList();
        int subcategoryLength = sublist.Count;
        
        List<Action> subtypelist = sublist.Where(action => action.Type == type).ToList();
        if (sublist.Count != 0)
        {
            foreach (Action action in subtypelist)
            {
                if (action.Compare(info))
                {
                    matched = true;
                    action.matched = true;
                }
            }
        }

        if (matched && subcategoryLength <= 1)
        {
            currentAction += 1;
        }

        if (!matched)
        {
            if ( sublist.Count > 0 && 
                wrongStepsList.Find(step => step == sublist[0].description) == null )
            {
                wrongStepsList.Add(sublist[0].description);
            }
        }
       
        return matched;
    }

    private void CheckScenarioCompleted()
    {
        // no unmatched actions left
        if (actionList.Find(action => action.matched == false) == null)
        {
            GameObject.Find("Preferences").GetComponent<EndScoreManager>().LoadEndScoreScene();
        }
    }

    void OnGUI()
    {
        GUIStyle style = GUI.skin.GetStyle("Label");
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 40;

        if (points > 0)
        {
            style.normal.textColor = Color.green;
        }
        else if (points < 0)
        {
            style.normal.textColor = Color.red;
        }
        else
        {
            style.normal.textColor = Color.white;
        }

        GUI.Label(new Rect(0, 0, Screen.width / 8, Screen.height / 10),
        "Points: " + points, style);
    }

    public void SetActionStatus(List<bool> items)
    {
        if ( items.Count == actionList.Count )
        {
            for ( int i = 0; i < actionList.Count; ++i )
            {
                actionList[i].matched = items[i];
            }
        }
    }
}
