using System.Collections.Generic;

namespace CareUp.Actions
{
    /// <summary>
    /// Abstract class Action. Inherit every time of action from this one.
    /// </summary>
    public abstract class Action
    {
        public bool matched = false;
        public string shortDescr;
        public int pointValue;
        public bool notMandatory;
        public bool sceneDoneTrigger; // for test version, when all steps are optional
        public float quizTriggerTime;
        public string messageTitle;
        public string messageContent;
        public List<string> blockRequired;
        public List<string> blockUnlock;
        public List<string> blockLock;
        public string blockTitle;
        public string blockMessage;
        public string comment;
        public string commentUA;
        public string leftHandRequirement;
        public string rightHandRequirement;
        public string placeRequirement;
        public string secondPlaceRequirement;
        public string _topic;
        public float encounter;
        public int storedIndex;
        public float UITimeout = 0f;

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

        /// <summary>
        /// C-tor of abstract action class. Used for creating aciton instance from xml.
        /// </summary>
        /// <param name="t">Action type (Use, Examine, etc)</param>
        /// <param name="index">Index of action (see xml)</param>
        /// <param name="descr">Sentence from xml, describing action</param>
        /// <param name="audio">Name of audiofile, that will be played when hint used</param>
        public Action(ActionManager.ActionType t, int index, string sdescr,
            int points, bool notNeeded, float quizTime, string title, string content,
            List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
        {
            type = t;
            subindex = index;
            shortDescr = sdescr;
            pointValue = points;
            notMandatory = notNeeded;
            sceneDoneTrigger = false;
            quizTriggerTime = quizTime;
            messageTitle = title;
            messageContent = content;
            blockRequired = blockReq;
            blockUnlock = blockUnl;
            blockLock = blockL;
            blockTitle = blockTitl;
            blockMessage = blockMsg;
            encounter = encounterValue;
        }

        /// <summary>
        /// Abstract class
        /// </summary>
        /// <param name="info">Data of action. See overloaded functions.</param>
        /// <returns>True if ==</returns>
        public abstract bool Compare(string[] info);
        public abstract void ObjectNames(out string[] name);
    }

    /// <summary>
    /// Action takes two names (can be empty) and outputs 2 names (can be empty)
    /// for left and right hand. Empty meanss - no object in hand.
    /// </summary>
    public class CombineAction : Action
    {
        private string leftInput;
        private string rightInput;

        public string decombineText;

        public CombineAction(string left, string right, int index, string sdescr,
            int points, bool notNeeded, float quizTime, string title, string content,
            List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg,
            string decombineBtnText, float encounterValue)
            : base(ActionManager.ActionType.ObjectCombine, index, sdescr, points,
                  notNeeded, quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            leftInput = left;
            rightInput = right;

            decombineText = decombineBtnText;
        }

        /// <summary>
        /// Compares input values for left and right hands. Mirroring included.
        /// </summary>
        /// <param name="info">Object before combination</param>
        /// <returns>True if values are same.</returns>
        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
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

    /// <summary>
    /// Action of using static object.
    /// </summary>
    public class UseAction : Action
    {
        private string useInput;

        public string buttonText;

        public UseAction(string use, int index, string sdescr,
            string button, int points, bool notNeeded, float quizTime, string title, string content,
            List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.ObjectUse, index, sdescr, points, notNeeded,
                  quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            useInput = use;
            buttonText = button;
        }

        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
            bool same = false;
            if (info.Length == 1)
            {
                if (info[0] == useInput)
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

    /// <summary>
    /// Action interacting with person. Peformed when topic selected.
    /// </summary>
    public class TalkAction : Action
    {
        private string topicInput;
        private string person = "Patient"; // TODO

        public TalkAction(string topic, int index, string sdescr,
            int points, bool notNeeded, float quizTime, string title, string content, List<string> blockReq,
            List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.PersonTalk, index, sdescr, points, notNeeded,
                  quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            topicInput = topic;
            _topic = topic;
        }

        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
            bool same = false;
            if (info.Length == 1)
            {
                if (info[0] == topicInput)
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

    /// <summary>
    /// Action using one object on another.
    /// </summary>
    public class UseOnAction : Action
    {
        private string item;
        private string target;

        public string buttonText;

        public UseOnAction(string i, string t, int index, string sdescr,
            string button, int points, bool notNeeded, float quizTime, string title,
            string content, List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.ObjectUseOn, index, sdescr, points, notNeeded,
                  quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            item = i;
            target = t;
            buttonText = button;
        }

        /// <summary>
        /// Compares used objects.
        /// </summary>
        /// <param name="info">info[0] = item, info[1] = target</param>
        /// <returns>True if values same</returns>
        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
            bool same = false;
            if (info.Length == 2)
            {
                if (info[0] == item && info[1] == target)
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

    /// <summary>
    /// Action examining object. Expected var - is required state of the object.
    /// </summary>
    public class ExamineAction : Action
    {
        private string item;
        private string expected;

        public ExamineAction(string i, string exp, int index, string sdescr,
            int points, bool notNeeded, float quizTime, string title, string content,
           List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.ObjectExamine, index, sdescr, points,
                  notNeeded, quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            item = i;
            expected = exp;
        }

        /// <summary>
        /// Compares values
        /// </summary>
        /// <param name="info">info[0] = item, info[1] = state</param>
        /// <returns>True if values same</returns>
        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
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

    /// <summary>
    /// Action picking item.
    /// </summary>
    public class PickUpAction : Action
    {
        private string item;

        public PickUpAction(string i, int index, string sdescr,
            int points, bool notNeeded, float quizTime, string title, string content, List<string> blockReq,
           List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.PickUp, index, sdescr, points, notNeeded,
                  quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            item = i;
        }

        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
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

    public class SequenceStepAction : Action
    {
        private string stepName;

        public SequenceStepAction(string name, int index, string sdescr,
            int points, bool notNeeded, float quizTime, string title, string content,
            List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.SequenceStep, index, sdescr, points,
                  notNeeded, quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            stepName = name;
        }

        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
            if (info.Length == 1)
            {
                return info[0] == stepName;
            }
            else
                return false;
        }

        public override void ObjectNames(out string[] name)
        {
            string[] res = { stepName };
            name = res;
        }
    }

    public class ObjectDropAction : Action
    {
        private string objectName;
        private string dropPositionID;

        public ObjectDropAction(string name, string posId, int index, string sdescr,
            int points, bool notNeeded, float quizTime, string title,
            string content, List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.ObjectDrop, index, sdescr, points,
                  notNeeded, quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            objectName = name;
            dropPositionID = posId;
        }

        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
            if (info.Length == 2)
            {
                return (info[0] == objectName && info[1] == dropPositionID);
            }
            else
                return false;
        }

        public override void ObjectNames(out string[] name)
        {
            string[] res = { objectName, dropPositionID };
            name = res;
        }
    }

    public class MovementAction : Action
    {
        private string positionInput;

        public MovementAction(string position, int index, string sdescr,
            int points, bool notNeeded, float quizTime, string title,
            string content, List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.Movement, index, sdescr, points,
                  notNeeded, quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            positionInput = position;
        }

        public override bool Compare(string[] info)
        {
            if (info == null)
                return false;
            bool same = false;
            if (info.Length == 1)
            {
                if (info[0] == positionInput)
                {
                    same = true;
                }
            }
            return same;
        }

        public string GetInfo()
        {
            return positionInput;
        }

        public override void ObjectNames(out string[] name)
        {
            string[] res = { positionInput };
            name = res;
        }
    }
    public class GeneralAction : Action
    {
        public string ButtonText { get; }
        public string Action { get; }
        public string Item { get; }

        public GeneralAction(string itemValue, string actionValue, int index, int storedIndex, string sdescr,
            string buttonTextValue, int points, bool notNeeded, float quizTime, string title,
            string content, List<string> blockReq, List<string> blockUnl, List<string> blockL, string blockTitl, string blockMsg, float encounterValue)
            : base(ActionManager.ActionType.General, index, sdescr, points, notNeeded,
                  quizTime, title, content, blockReq, blockUnl, blockL, blockTitl, blockMsg, encounterValue)
        {
            this.storedIndex = storedIndex;
            Item = itemValue;
            Action = actionValue;
            ButtonText = buttonTextValue;
        }

        public override bool Compare(string[] info)
        {
            bool same = false;
            if (info != null)
            {
                if (info.Length == 1)
                {
                    if (info[0] == Item)
                    {
                        same = true;
                    }
                }
            }
            return same;
        }

        public override void ObjectNames(out string[] name)
        {
            string[] res = { Item };
            name = res;
        }
    }
}