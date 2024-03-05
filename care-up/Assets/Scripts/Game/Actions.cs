using System.Collections.Generic;

namespace CareUp.Actions
{
    public class VideoAction
    {
        public string title = "";
        public string description = "";
        public int startFrame = 0;

        public VideoAction(string _title = "", string _description = "", int _frame = 0)
        {
            title = _title;
            description = _description;
            startFrame = _frame;
        }
    }

    public class ActionInfo
    {
        public int sequentialNumber = -1;
        public bool matched = false;
        public string shortDescr; 
        public int pointValue; 
        public bool notMandatory;
        public bool sceneDoneTrigger; // for test version, when all steps are optional
        public float quizTriggerTime; 
        public string messageTitle; 
        public string messageContent;
        public float messageDelay;
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
        public string topic;
        public float encounter; 
        public int storedIndex; 
        public bool ignorePosition = false; 
        public float UITimeout = 0f; 
        public string subjectTitle = ""; 

        public ActionManager.ActionType type;

        public int subindex = 0; 

        public ActionInfo() 
        {
            blockUnlock = new List<string>();
            blockRequired = new List<string>();
            blockLock = new List<string>();
        }

        public ActionInfo(ActionInfo i)
        {
            this.sequentialNumber = i.sequentialNumber;
            this.matched = i.matched;
            this.shortDescr = i.shortDescr;
            this.pointValue = i.pointValue;
            this.notMandatory = i.notMandatory;
            this.quizTriggerTime = i.quizTriggerTime;
            this.messageTitle = i.messageTitle;
            this.messageContent = i.messageContent;
            this.messageDelay = i.messageDelay;
            this.blockRequired = i.blockRequired;
            this.blockUnlock = i.blockUnlock;
            this.blockLock = i.blockLock;
            this.blockTitle = i.blockTitle;
            this.blockMessage = i.blockMessage;
            this.comment = i.comment;
            this.commentUA = i.commentUA;
            this.leftHandRequirement = i.leftHandRequirement;
            this.rightHandRequirement = i.rightHandRequirement;
            this.placeRequirement = i.placeRequirement;
            this.secondPlaceRequirement = i.secondPlaceRequirement;
            this.topic = i.topic;
            this.encounter = i.encounter;
            this.storedIndex = i.storedIndex;
            this.ignorePosition = i.ignorePosition;
            this.UITimeout = i.UITimeout;
            this.subjectTitle = i.subjectTitle;
            this.type = i.type;
            this.subindex = i.subindex;

            this.sceneDoneTrigger = false;
        }
    }

    /// <summary>
    /// Abstract class Action. Inherit every time of action from this one.
    /// </summary>
    public abstract class Action
    {
        public ActionInfo info;

        public bool compareActions(Action actionB)
        {
            bool result = false;
            if (info.type == actionB.info.type && info.shortDescr == actionB.info.shortDescr && info.messageTitle == actionB.info.messageTitle
                && info.messageContent == actionB.info.messageContent && info.storedIndex == actionB.info.storedIndex)
                result = true;
            return result;
        }

        public ActionManager.ActionType Type
        {
            get { return info.type; }
        }

        public int SubIndex
        {
            get { return info.subindex; }
        }

        /// <summary>
        /// C-tor of abstract action class. Used for creating aciton instance from xml.
        /// </summary>
        /// <param name="t">Action type (Use, Examine, etc)</param>
        /// <param name="index">Index of action (see xml)</param>
        /// <param name="descr">Sentence from xml, describing action</param>
        /// <param name="audio">Name of audiofile, that will be played when hint used</param>
        public Action(ActionInfo externalInfo)
        {
            info = new ActionInfo(externalInfo);
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

        public CombineAction(string left, string right, string decombineBtnText, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.ObjectCombine;

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

        public UseAction(string use, string button, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.ObjectUse;

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

        public TalkAction(string topic, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.PersonTalk;

            topicInput = topic;
            info.topic = topic;
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

        public UseOnAction(string i, string t, string button, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.ObjectUseOn;

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

        public ExamineAction(string i, string exp, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.ObjectExamine;

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

        public PickUpAction(string i, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.PickUp;

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

        public SequenceStepAction(string name, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.SequenceStep;

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

        public ObjectDropAction(string name, string posId, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.ObjectDrop;

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

        public MovementAction(string position, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.Movement;

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

        public GeneralAction(string itemValue, string actionValue, string buttonTextValue, 
            ActionInfo externalInfo) : base(externalInfo)
        {
            info.type = ActionManager.ActionType.General;

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