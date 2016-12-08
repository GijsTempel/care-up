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

    private int totalPoints = 0;
    private int points = 0;
    private int currentActionIndex = 0;
    private Action currentAction;

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

    public int TotalPoints
    {
        get { return totalPoints; }
    }

    public int CurrentActionIndex
    {
        get { return currentActionIndex; }
        set { currentActionIndex = value; }
    }

    public string CurrentDescription
    {
        get { return currentAction != null ? currentAction.description : ""; }
    }

    public string CurrentAudioHint
    {
        get { return currentAction.audioHint; }
    }

    public string CurrentUseObject
    {
        get
        {
            if (currentAction != null)
            {
                return (currentAction.Type == ActionType.ObjectUse) ?
                    ((UseAction)currentAction).GetObjectName() : "";
            }
            else
                return "";
        }
    }

    public string[] CurrentCombineObjects
    {
        get
        {
            string[] objects = new string [2];
            if ( currentAction.Type == ActionType.ObjectCombine )
            {
                string left, right;
                ((CombineAction)currentAction).GetObjects(out left, out right);
                objects[0] = left;
                objects[1] = right;
            }
            return objects;
        }
    }

    public string[] CurrentUseOnInfo
    {
        get
        {
            string[] info = new string[2];
            if (currentAction.Type == ActionType.ObjectUseOn)
            {
                string item, target;
                ((UseOnAction)currentAction).GetInfo(out item, out target);
                info[0] = item;
                info[1] = target;
            }
            return info;
        }
    }

    private Controls controls;

    void Start () {

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        if (controls == null) Debug.LogError("No controls found");

        XmlDocument xmlFile = new XmlDocument();
        xmlFile.Load("Assets/Resources/Xml/Actions/" + actionListName + ".xml");

        totalPoints = int.Parse(xmlFile.FirstChild.NextSibling.Attributes["points"].Value);
        XmlNodeList actions = xmlFile.FirstChild.NextSibling.ChildNodes; 

        foreach ( XmlNode action in actions )
        {
            int index;
            int.TryParse(action.Attributes["index"].Value, out index);
            string type = action.Attributes["type"].Value;
            string descr = action.Attributes["description"].Value;
            string audio = action.Attributes["audioHint"].Value;
            
            switch (type)
            {
                case "combine":
                    string left = action.Attributes["left"].Value;
                    string right = action.Attributes["right"].Value;
                    actionList.Add(new CombineAction(left, right, index, descr, audio));
                    break;
                case "use":
                    string use = action.Attributes["value"].Value;
                    actionList.Add(new UseAction(use, index, descr, audio));
                    break;
                case "talk":
                    string topic = action.Attributes["topic"].Value;
                    actionList.Add(new TalkAction(topic, index, descr, audio));
                    break;
                case "useOn":
                    string useItem = action.Attributes["item"].Value;
                    string target = action.Attributes["target"].Value;
                    actionList.Add(new UseOnAction(useItem, target, index, descr, audio));
                    break;
                case "examine":
                    string exItem = action.Attributes["item"].Value;
                    string expected = action.Attributes["expected"].Value;
                    actionList.Add(new ExamineAction(exItem, expected, index, descr, audio));
                    break;
                default:
                    Debug.LogError("No action type found: " + type);
                    break;
            }
        }
        currentAction = actionList.First();
    }

    void Update()
    {
        if (controls.keyPreferences.GetHintKey.Pressed())
        {
            if (Narrator.PlaySound(CurrentAudioHint)) // if sound played
            {
                points -= 1; // penalty for using hint
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
            action.SubIndex == currentActionIndex &&
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
            currentActionIndex += 1;
        }

        if (!matched)
        {
            if ( sublist.Count > 0 && 
                wrongStepsList.Find(step => step == sublist[0].description) == null )
            {
                wrongStepsList.Add(sublist[0].description);
            }
            Narrator.PlaySound("WrongAction");
        }
        else
        {
            List<Action> actionsLeft = actionList.Where(action =>
                action.SubIndex == currentActionIndex &&
                action.matched == false).ToList();
            currentAction = actionsLeft.Count > 0 ? actionsLeft.First() : null;
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
