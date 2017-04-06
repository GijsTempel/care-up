using System.Xml;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// GameLogic script. Everything about actions is managed by this script.
/// </summary>
public class ActionManager : MonoBehaviour {

    // tutorial variables - do not affect gameplay
    [HideInInspector]
    public bool tutorial_hintUsed = false;
    private bool currentStepHintUsed = false;

    public Text pointsText;
    
    // list of types of actions
    public enum ActionType
    {
        ObjectCombine,
        ObjectUse,
        PersonTalk,
        ObjectUseOn,
        ObjectExamine,
        PickUp
    };

    // name of the xml file with actions
    public string actionListName;

    // actual list of actions
    private List<Action> actionList = new List<Action>();
    // list of descriptions of steps, player got penalty on
    private List<string> wrongStepsList = new List<string>();

    private int totalPoints = 0;         // max points of scene
    private int points = 0;              // current points
    private int currentActionIndex = 0;  // index of current action
    private Action currentAction;        // current action instance
    
    // GameObjects that show player next step when hint used
    private List<GameObject> particleHints; 
    private bool menuScene;

    public List<Action> ActionList
    {
        get { return actionList; }
    }
    
    /// <summary>
    /// List of wrong steps, merged into a string with line breaks.
    /// </summary>
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

    /// <summary>
    /// Current points during runtime.
    /// </summary>
    public int Points
    {
        get { return points; }
        set { points = value; }
    }

    /// <summary>
    /// Total max points player can get on the scene.
    /// </summary>
    public int TotalPoints
    {
        get { return totalPoints; }
    }

    /// <summary>
    /// Index of current action.
    /// </summary>
    public int CurrentActionIndex
    {
        get { return currentActionIndex; }
        set { currentActionIndex = value; }
    }

    /// <summary>
    /// Description of the current action.
    /// </summary>
    public string CurrentDescription
    {
        get { return currentAction != null ? currentAction.description : ""; }
    }

    /// <summary>
    /// Name of the file of audioHint of current action.
    /// </summary>
    public string CurrentAudioHint
    {
        get { return currentAction.audioHint; }
    }

    /// <summary>
    /// If current action is UseAction, returns the name of the object that needs to be used.
    /// Returns empty string for every other case.
    /// </summary>
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

    /// <summary>
    /// Returns string[] of names of objects that need to be combined.
    /// Returns nothing if current action is not CombineAction.
    /// </summary>
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

    /// <summary>
    /// Returns data if current action is UseOnAction.
    /// string[0] = item that should be used on target
    /// string[1] = target item
    /// </summary>
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

    /// <summary>
    /// Returns topic string if current action is TalkAction.
    /// </summary>
    public string CurrentTopic
    {
        get
        {
            return (currentAction.Type == ActionType.PersonTalk) ? 
                ((TalkAction)currentAction).Topic : "";
        }
    }

    private Controls controls;

    /// <summary>
    /// Set some variables and load info from xml file.
    /// </summary>
    void Start () {

        string sceneName = SceneManager.GetActiveScene().name;
        menuScene = sceneName == "Menu" || sceneName == "SceneSelection" || sceneName == "EndScore";
        particleHints = new List<GameObject>();

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        if (controls == null) Debug.LogError("No controls found");
        
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Actions/" + actionListName);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);

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
                case "pickUp":
                    string itemPicked = action.Attributes["item"].Value;
                    actionList.Add(new PickUpAction(itemPicked, index, descr, audio));
                    break;
                default:
                    Debug.LogError("No action type found: " + type);
                    break;
            }
        }
        currentAction = actionList.First();
    }

    /// <summary>
    /// Handle pressing "Get Hint" key.
    /// Play audio hint, create particle hint, do penalty.
    /// </summary>
    void Update()
    {
        if (controls.keyPreferences.GetHintKey.Pressed())
        {
            if (Narrator.PlaySound(CurrentAudioHint)) // if sound played
            {
                string[] obj;
                currentAction.ObjectNames(out obj);
                GameObject parent;
                GameObject hintObject = Resources.Load<GameObject>("Prefabs/ParticleHint");

                if (obj.Length > 0)
                {
                    parent = GameObject.Find(obj[0]);
                    if (parent != null)
                    {
                        particleHints.Add(Instantiate(hintObject, parent.transform.position,
                             Quaternion.identity, parent.transform));
                    }
                }

                if (obj.Length == 2)
                {
                    parent = GameObject.Find(obj[1]);
                    if (parent != null)
                    {
                        particleHints.Add(Instantiate(hintObject, parent.transform.position,
                             Quaternion.identity, parent.transform));
                    }
                }

                tutorial_hintUsed = true;
                if (!currentStepHintUsed)
                {
                    points -= 1; // penalty for using hint
                    currentStepHintUsed = true;
                }
            }
        }

        if (!menuScene)
        {
            pointsText.text = points + " / " + totalPoints;
        }
    }

    /// <summary>
    /// Handle (trigger) Combine action.
    /// </summary>
    /// <param name="leftHand">Name of the object in left hand.</param>
    /// <param name="rightHand">Name of the object in right hand.</param>
    public void OnCombineAction(string leftHand, string rightHand)
    {
        string[] info = { leftHand, rightHand };
        bool occured = Check(info, ActionType.ObjectCombine);
        points += occured ? 1 : -1;

        Debug.Log("Combine " + leftHand + " and " + rightHand + " with result " + occured);

        CheckScenarioCompleted();
    }

    /// <summary>
    /// Handle (trigger) UseAction.
    /// </summary>
    /// <param name="useObject">Name of the used object.</param>
    public void OnUseAction(string useObject)
    {
        string[] info = { useObject };
        bool occured = Check(info, ActionType.ObjectUse);
        points += occured ? 1 : -1;

        Debug.Log("Use " + useObject + " with result " + occured);

        CheckScenarioCompleted();
    }

    /// <summary>
    /// Handle (trigger) Talk action.
    /// </summary>
    /// <param name="topic">Chosen topic</param>
    public void OnTalkAction(string topic)
    {
        string[] info = { topic };
        bool occured = Check(info, ActionType.PersonTalk);
        points += occured ? 1 : -1;

        Debug.Log("Say " + topic + " with result " + occured);

        CheckScenarioCompleted();
    }
    
    /// <summary>
    /// Handle (trigger) UseOn action.
    /// </summary>
    /// <param name="item">Item that is used on target</param>
    /// <param name="target">Item that was targeted</param>
    public void OnUseOnAction(string item, string target)
    {
        string[] info = { item, target };
        bool occured = Check(info, ActionType.ObjectUseOn);
        points += occured ? 1 : (target == "" ? 0 : -1);

        Debug.Log("Use " + item + " on " + target + " with result " + occured);

        CheckScenarioCompleted();
    }

    /// <summary>
    /// Handle (trigger) Examine aciton.
    /// </summary>
    /// <param name="item">Name of the examined item</param>
    /// <param name="expected">State of the examined item</param>
    public void OnExamineAction(string item, string expected)
    {
        string[] info = { item, expected };
        bool occured = Check(info, ActionType.ObjectExamine);
        points += occured ? 1 : 0; // no penalty
        
        Debug.Log("Examine " + item + " with state " + expected + " with result " + occured);

        CheckScenarioCompleted();
    }

    /// <summary>
    /// Handle (trigger) picking up action.
    /// Is not penalised, but needs to be checked.
    /// </summary>
    /// <param name="item">Name of the picked item</param>
    public void OnPickUpAction(string item)
    {
        string[] info = { item };
        bool occured = Check(info, ActionType.PickUp);
        points += occured ? 1 : 0; // no penalty

        if (occured)
        {
            Debug.Log("Pick Up " + item + " with result " + occured);
        }

        CheckScenarioCompleted();
    }

    /// <summary>
    /// Checks if triggered action is correct ( expected to be done in action list ).
    /// Plays WrongAction sound from Narrator if wrong.
    /// </summary>
    /// <param name="info">Info passed from Handling functions.</param>
    /// <param name="type">Type of the action</param>
    /// <returns>True if action expected and correct. False otherwise.</returns>
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

        if (matched)
        {
            currentStepHintUsed = false;
        }

        if (matched && subcategoryLength <= 1)
        {
            currentActionIndex += 1;
        }

        if (!matched && type != ActionType.ObjectExamine && type != ActionType.PickUp)
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

    /// <summary>
    /// Check if every action from action list is done and scene is completed.
    /// If yes - go to EndScore scene.
    /// Runs after every action check and clears particle hints.
    /// </summary>
    private void CheckScenarioCompleted()
    {
        // clear hints
        foreach (GameObject o in particleHints)
            Destroy(o);
        particleHints.Clear();

        // no unmatched actions left
        if (actionList.Find(action => action.matched == false) == null)
        {
            GameObject.Find("Preferences").GetComponent<EndScoreManager>().LoadEndScoreScene();
        }
    }

    /// <summary>
    /// Sets state of every action of the list.
    /// </summary>
    /// <param name="items">True = action completed</param>
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

    /// <summary>
    /// Force a step back.
    /// </summary>
    public void StepBack()
    {
        Action lastAction = actionList.Last(x => x.matched == true);
        lastAction.matched = false;
        currentActionIndex = lastAction.SubIndex;
        currentAction = lastAction;
    }
}
