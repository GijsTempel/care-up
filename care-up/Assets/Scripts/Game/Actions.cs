using UnityEngine;
using System.Collections;
using System;

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

    public Action(ActionManager.ActionType t, int index)
    {
        type = t;
        subindex = index;
    }
    
    public abstract bool Compare(string[] info);
}

public class CombineAction : Action
{
    private string leftInput;
    private string rightInput;

    public CombineAction(string left, string right, int index)
        : base(ActionManager.ActionType.ObjectCombine, index)
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

public class UseAction : Action
{
    private string useInput;

    public UseAction(string use, int index)
        : base(ActionManager.ActionType.ObjectUse, index)
    {
        useInput = use;
    }

    public override bool Compare(string[] info)
    {
        bool same = false;
        if (info.Length == 1)
        {
            if ( info[0] == useInput )
            {
                same = true;
            }
        }
        return same;
    }
}

public class TalkAction : Action
{
    private string topicInput; // ?

    public TalkAction(string topic, int index)
        : base(ActionManager.ActionType.PersonTalk, index)
    {
        topicInput = topic;
    }

    public override bool Compare(string[] info)
    {
        bool same = false;
        if (info.Length == 1)
        {
            if ( info[0] == topicInput )
            {
                same = true;
            }
        }
        return same;
    }
}

public class UseOnAction : Action
{
    private string item;
    private string target;

    public UseOnAction(string i, string t, int index)
        : base(ActionManager.ActionType.ObjectUseOn, index)
    {
        item = i;
        target = t;
    }

    public override bool Compare(string[] info)
    {
        bool same = false;
        if ( info.Length == 2 )
        {
            if ( info[0] == item && info[1] == target)
            {
                same = true;
            }
        }
        return same;
    }
}