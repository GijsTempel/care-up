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
        actionList.Add(new CombineAction("Cube", "Cube"));
        actionList.Add(new CombineAction("Cube", "Sphere"));
        actionList.Add(new CombineAction("Rectangle", "FlatSphere"));
    }

    public void OnCombineAction(string leftHand, string rightHand)
    {
        if (currentAction < actionList.Count)
        {
            if (actionList[currentAction].Check(leftHand, rightHand))
            {
                points += 1; // correct
                currentAction += 1; // next action from list
            }
            else
            {
                points -= 1; // incorrect
            }
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
}
