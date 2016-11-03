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
        PersonTalk
    };

    public string actionListName;

    private List<Action> actionList = new List<Action>();

    public List<Action> ActionList
    {
        get { return actionList; }
    }
    
    private int points = 0;
    private int currentAction = 0;

	void Start () {

        XmlDocument xmlFile = new XmlDocument();
        xmlFile.Load("Assets/Resources/Xml/Actions/" + actionListName + ".xml");
        XmlNodeList actions = xmlFile.FirstChild.NextSibling.ChildNodes; // xml is not a node, bug

        foreach ( XmlNode action in actions )
        {
            int index;
            int.TryParse(action.Attributes["index"].Value, out index);
            string type = action.Attributes["type"].Value;

            switch (type)
            {
                case "combine":
                    string left = action.Attributes["left"].Value;
                    string right = action.Attributes["right"].Value;
                    actionList.Add(new CombineAction(left, right, index));
                    break;
                case "use":
                    string use = action.Attributes["value"].Value;
                    actionList.Add(new UseAction(use, index));
                    break;
                case "talk":
                    //string topic = action.Attributes["topic"].Value;
                    actionList.Add(new TalkAction("", index));
                    break;
                default:
                    Debug.LogError("No action type found: " + type);
                    break;
            }
        }
    }

    public void OnCombineAction(string leftHand, string rightHand)
    {
        if (currentAction < actionList.Count)
        {
            string[] info = { leftHand, rightHand };
            points += Check(info, ActionType.ObjectCombine) ? 1 : -1;
        }
    }

    public void OnUseAction(string useObject)
    {
        string[] info = { useObject };
        points += Check(info, ActionType.ObjectUse) ? 1 : -1;
    }

    public void OnTalkAction(string topic)
    {
        string[] info = { topic };
        points += Check(info, ActionType.PersonTalk) ? 1 : -1;
    }

    public bool Check(string[] info, ActionType type)
    {
        bool matched = false;

        List<Action> sublist = actionList.Where(action =>
            action.SubIndex == currentAction &&
            action.matched == false).ToList();
        int subcategoryLength = sublist.Count;
        
        sublist = sublist.Where(action => action.Type == type).ToList();
        if (sublist.Count != 0)
        {
            foreach (Action action in sublist)
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

        return matched;
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
