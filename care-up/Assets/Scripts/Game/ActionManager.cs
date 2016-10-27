using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour {

    public enum ActionType
    {
        ObjectCombine,
        ObjectUse
    };

    private List<Action> actionList = new List<Action>();
    
    private int points = 0;
    private int currentAction = 0;

	void Start () {
        actionList.Add(new CombineAction("Cube", "Cube", 0));
        actionList.Add(new CombineAction("Cube", "Sphere", 0));
        actionList.Add(new CombineAction("Rectangle", "FlatSphere", 1));
    }

    public void OnCombineAction(string leftHand, string rightHand)
    {
        if (currentAction < actionList.Count)
        {
            string[] info = { leftHand, rightHand };
            points += Check(info, ActionType.ObjectCombine) ? 1 : -1;
        }
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
}
