using UnityEngine;
using System.Collections;

// idea test
public abstract class Action
{
    public bool matched = false;

    protected ActionManager.ActionType type;
    
    private int subindex = 0;

    public ActionManager.ActionType Type
    {
        get { return type; }
    }
   

    public int SubIndex
    {
        get { return subindex; }
    }

    public Action(ActionManager.ActionType t, int sub = 0)
    {
        type = t;
        subindex = sub;
    }
    
    public abstract bool Compare(string[] info);
}

public class CombineAction : Action
{

    private string leftInput;
    private string rightInput;

    public CombineAction(string left, string right, int sub = 0)
        : base(ActionManager.ActionType.ObjectCombine, sub)
    {
        leftInput = left;
        rightInput = right;
    }

    public override bool Compare(string[] info)
    {
        bool same = false;
        if (info.Length == 2)
        {
            if (leftInput == info[0] && rightInput == info[1] ||
                rightInput == info[0] && leftInput == info[1])
            {
                same = true;
            }
        }
        return same;
    }


}
