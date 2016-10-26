using UnityEngine;
using System.Collections;

// idea test
public abstract class Action
{

    protected ActionManager.ActionType type;
    
    public Action(ActionManager.ActionType t)
    {
        type = t;
    }

    public abstract bool Check(string left, string right);
}

public class CombineAction : Action
{

    private string leftInput;
    private string rightInput;

    public CombineAction(string left, string right)
        : base(ActionManager.ActionType.ObjectCombine)
    {
        leftInput = left;
        rightInput = right;
    }

    public override bool Check(string left, string right)
    {
        if (leftInput == left && rightInput == right ||
            rightInput == left && leftInput == right)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
