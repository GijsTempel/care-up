using UnityEngine;
using System.Collections;
using System;

public abstract class Action
{
    public bool matched = false;
    public string description;
    public string audioHint;

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

    public Action(ActionManager.ActionType t, int index, string descr, string audio)
    {
        type = t;
        subindex = index;
        description = descr;
        audioHint = audio;
    }
    
    public abstract bool Compare(string[] info);
    public abstract void ObjectNames(out string[] name);
}

public class CombineAction : Action
{
    private string leftInput;
    private string rightInput;

    public CombineAction(string left, string right, int index, string descr, string audio)
        : base(ActionManager.ActionType.ObjectCombine, index, descr, audio)
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

    public void GetObjects(out string left, out string right)
    {
        left = leftInput;
        right = rightInput;
    }

    public override void ObjectNames(out string[] name)
    {
        string[] res = { leftInput, rightInput };
        name = res;
    }
}

public class UseAction : Action
{
    private string useInput;

    public UseAction(string use, int index, string descr, string audio)
        : base(ActionManager.ActionType.ObjectUse, index, descr, audio)
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

    public string GetObjectName()
    {
        return useInput;
    }

    public override void ObjectNames(out string[] name)
    {
        string[] res = { useInput };
        name = res;
    }
}

public class TalkAction : Action
{
    private string topicInput;
    private string person = "Patient"; // TODO

    public TalkAction(string topic, int index, string descr, string audio)
        : base(ActionManager.ActionType.PersonTalk, index, descr, audio)
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

    public string Topic
    {
        get { return topicInput; }
    }

    public override void ObjectNames(out string[] name)
    {
        string[] res = { person };
        name = res;
    }
}

public class UseOnAction : Action
{
    private string item;
    private string target;

    public UseOnAction(string i, string t, int index, string descr, string audio)
        : base(ActionManager.ActionType.ObjectUseOn, index, descr, audio)
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

    public void GetInfo(out string i, out string t)
    {
        i = item;
        t = target;
    }

    public override void ObjectNames(out string[] name)
    {
        string[] res = { item, target };
        name = res;
    }
}

public class ExamineAction : Action
{
    private string item;
    private string expected;

    public ExamineAction(string i, string exp, int index, string descr, string audio)
        : base(ActionManager.ActionType.ObjectExamine, index, descr, audio)
    {
        item = i;
        expected = exp;
    }

    public override bool Compare(string[] info)
    {
        bool same = false;
        if (info.Length == 2)
        {
            if (info[0] == item && info[1] == expected)
            {
                same = true;
            }
        }
        return same;
    }

    public override void ObjectNames(out string[] name)
    {
        string[] res = { item };
        name = res;
    }
}

public class PickUpAction : Action
{
    private string item;

    public PickUpAction(string i, int index, string descr, string audio)
        : base(ActionManager.ActionType.PickUp, index, descr, audio)
    {
        item = i;
    }

    public override bool Compare(string[] info)
    {
        bool same = false;
        if (info.Length == 1)
        {
            if (info[0] == item)
            {
                same = true;
            }
        }
        return same;
    }

    public override void ObjectNames(out string[] name)
    {
        string[] res = { item };
        name = res;
    }
}