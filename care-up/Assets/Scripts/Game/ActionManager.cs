using System.Xml;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CareUp.Actions;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// GameLogic script. Everything about actions is managed by this script.
/// </summary>
public class ActionManager : MonoBehaviour {

    // tutorial variables - do not affect gameplay
    [HideInInspector]
    public bool tutorial_hintUsed = false;
    private bool currentStepHintUsed = false;

    private Text pointsText;
    private Text percentageText;

    // list of types of actions
    public enum ActionType
    {
        ObjectCombine,
        ObjectUse,
        PersonTalk,
        ObjectUseOn,
        ObjectExamine,
        PickUp,
        SequenceStep
    };

    // name of the xml file with actions
    public string actionListName;

    // actual list of actions
    private List<Action> actionList = new List<Action>();

    // list of descriptions of steps, player got penalty on
    private List<string> stepsList = new List<string>();
    private List<string> stepsDescriptionList = new List<string>();
    private List<int> wrongStepIndexes = new List<int>();

    private int totalPoints = 0;         // max points of scene
    private int points = 0;              // current points
    private int currentActionIndex = 0;  // index of current action
    private Action currentAction;        // current action instance
    private int currentPointAward = 1;
    private bool penalized = false;

    // GameObjects that show player next step when hint used
    private List<GameObject> particleHints;
    private bool menuScene;
    private bool uiSet = false;

    public List<Action> ActionList
    {
        get { return actionList; }
    }

    /// <summary>
    /// List of wrong steps, merged into a string with line breaks.
    /// </summary>
    public List<string> StepsList
    {
        get { return stepsList; }
    }

    public List<string> StepsDescriptionList
    {
        get { return stepsDescriptionList; }
    }

    public List<int> WrongStepIndexes
    {
        get { return wrongStepIndexes; }
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

    public float PercentageDone
    {
        get
        {
            int cur = actionList.IndexOf(currentAction);
            int tot = actionList.Count;
            return 100.0f * cur / tot;
        }
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
    /// Heavy function, use only once, never on update
    /// </summary>
    public string CurrentDescription
    {
        get {
            List<Action> sublist = actionList.Where(action =>
                action.SubIndex == currentActionIndex &&
                action.matched == false).ToList();

            string result = "";
            foreach (Action a in sublist)
            {
                result += " - " + a.shortDescr + "\n";
            }

            return result;
        }
    }

    /// <summary>
    /// Extra description ( for extended hints )
    /// Heavy function, use only once, never on update
    /// </summary>
    public string CurrentExtraDescription
    {
        get {
            List<Action> sublist = actionList.Where(action =>
                action.SubIndex == currentActionIndex &&
                action.matched == false).ToList();

            string result = "";
            foreach (Action a in sublist)
            {
                result += " - " + a.descr + "\n";
            }

            return result;
        }
    }

    /// <summary>
    /// Name of the file of audioHint of current action.
    /// </summary>
    public string CurrentAudioHint
    {
        get { return currentAction.audioHint; }
    }

    // new comparison looks for all actions of type withing current index
    public bool CompareUseObject(string name)
    {
        bool result = false;

        List<Action> sublist = actionList.Where(action =>
                action.SubIndex == currentActionIndex &&
                action.matched == false).ToList();
        foreach(Action a in sublist)
        {
            if (a.Type == ActionType.ObjectUse)
            {
                if (((UseAction)a).GetObjectName() == name)
                    result = true;
            }
        }

        return result;
    }
    
    public bool CompareCombineObjects(string left, string right)
    {
        bool result = false;

        List<Action> sublist = actionList.Where(action =>
                action.SubIndex == currentActionIndex &&
                action.matched == false).ToList();
        foreach (Action a in sublist)
        {
            if (a.Type == ActionType.ObjectCombine)
            {
                string _left, _right;
                ((CombineAction)a).GetObjects(out _left, out _right);
                if ((_left == left && _right == right) ||
                    (_right == left && _left == right))
                    result = true;
            }
        }

        return result;
    }
    
    public bool CompareUseOnInfo(string item, string target)
    {
        bool result = false;

        List<Action> sublist = actionList.Where(action =>
                action.SubIndex == currentActionIndex &&
                action.matched == false).ToList();
        foreach (Action a in sublist)
        {
            if (a.Type == ActionType.ObjectUseOn)
            {
                string _item, _target;
                ((UseOnAction)a).GetInfo(out _item, out _target);
                if (_item == item && _target == target)
                    result = true;
            }
        }

        return result;
    }
    
    public bool CompareTopic(string t)
    {
        bool result = false;

        List<Action> sublist = actionList.Where(action =>
                action.SubIndex == currentActionIndex &&
                action.matched == false).ToList();
        foreach (Action a in sublist)
        {
            if (a.Type == ActionType.PersonTalk)
            {
                if (((TalkAction)a).Topic == t)
                    result = true;
            }
        }

        return result;
    }

    public string CurrentButtonText
    {
        get
        {
            string result = "";
            
            List<Action> sublist = actionList.Where(action =>
                   action.SubIndex == currentActionIndex &&
                   action.matched == false).ToList();

            foreach (Action a in sublist)
            {
                if (a.Type == ActionType.ObjectUse)
                {
                    result = ((UseAction)a).buttonText;
                }
                else if (currentAction.Type == ActionType.ObjectUseOn)
                {
                    result = ((UseOnAction)a).buttonText;
                }
            }

            return result;
        }
    }

    private Controls controls;

    /// <summary>
    /// Set some variables and load info from xml file.
    /// </summary>
    void Awake()
    {

        string sceneName = SceneManager.GetActiveScene().name;
        menuScene = sceneName == "Menu" || sceneName == "SceneSelection" || sceneName == "EndScore";
        particleHints = new List<GameObject>();

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        if (controls == null) Debug.LogError("No controls found");

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Actions/" + actionListName);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);

        //totalPoints = int.Parse(xmlFile.FirstChild.NextSibling.Attributes["points"].Value);
        XmlNodeList actions = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode action in actions)
        {
            int index;
            int.TryParse(action.Attributes["index"].Value, out index);
            string type = action.Attributes["type"].Value;
            string descr = action.Attributes["description"].Value;
            string audio = action.Attributes["audioHint"].Value;

            string fDescr = "";
            if (action.Attributes["fullDescription"] != null)
            {
                fDescr = action.Attributes["fullDescription"].Value;
            }
            else
            {
                string[] splits = descr.Split('(', ')');
                if (splits.Length >= 2)
                {
                    descr = splits[0];
                    fDescr = splits[1];
                }
                else
                {
                    Debug.LogError("Description in xml file \"" + actionListName + "\" is set wrong. \n" +
                        "\'fullDescription\' field is not set and cannot split \'description\' properly. \n" +
                        "Index: " + index + ". Descr: " + descr);
                }
            }

            string extra = "";
            if (action.Attributes["extra"] != null)
            {
                extra = action.Attributes["extra"].Value;
            }

            string buttonText = "";
            if (action.Attributes["buttonText"] != null)
            {
                buttonText = action.Attributes["buttonText"].Value;
            }
            else
            {
                buttonText = descr;
            }

            int pointsValue = 1;
            if (action.Attributes["points"] != null)
            {
                int.TryParse(action.Attributes["points"].Value, out pointsValue);
            }

            bool notNeeded = action.Attributes["optional"] != null;

            switch (type)
            {
                case "combine":
                    string left = action.Attributes["left"].Value;
                    string right = action.Attributes["right"].Value;
                    actionList.Add(new CombineAction(left, right, index, descr, fDescr, audio, extra, pointsValue, notNeeded));
                    break;
                case "use":
                    string use = action.Attributes["value"].Value;
                    actionList.Add(new UseAction(use, index, descr, fDescr, audio, extra, buttonText, pointsValue, notNeeded));
                    break;
                case "talk":
                    string topic = action.Attributes["topic"].Value;
                    actionList.Add(new TalkAction(topic, index, descr, fDescr, audio, extra, pointsValue, notNeeded));
                    break;
                case "useOn":
                    string useItem = action.Attributes["item"].Value;
                    string target = action.Attributes["target"].Value;
                    actionList.Add(new UseOnAction(useItem, target, index, descr, fDescr, audio, extra, buttonText, pointsValue, notNeeded));
                    break;
                case "examine":
                    string exItem = action.Attributes["item"].Value;
                    string expected = action.Attributes["expected"].Value;
                    actionList.Add(new ExamineAction(exItem, expected, index, descr, fDescr, audio, extra, pointsValue, notNeeded));
                    break;
                case "pickUp":
                    string itemPicked = action.Attributes["item"].Value;
                    actionList.Add(new PickUpAction(itemPicked, index, descr, fDescr, audio, extra, pointsValue, notNeeded));
                    break;
                case "sequenceStep":
                    string stepName = action.Attributes["value"].Value;
                    actionList.Add(new SequenceStepAction(stepName, index, descr, fDescr, audio, extra, pointsValue, notNeeded));
                    break;
                default:
                    Debug.LogError("No action type found: " + type);
                    break;
            }
        }

        if (xmlFile.FirstChild.NextSibling.Attributes["points"] != null)
        {
            totalPoints = int.Parse(xmlFile.FirstChild.NextSibling.Attributes["points"].Value);
        }
        else
        {
            totalPoints = 0;
            foreach (Action a in actionList)
            {
                totalPoints += a.pointValue;
            }
        }

        foreach(Action a in actionList)
        {
            stepsList.Add(a.shortDescr);
            stepsDescriptionList.Add(a.extraDescr);
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
            if (Narrator.PlayHintSound(CurrentAudioHint)) // if sound played
            {
                string[] obj;
                currentAction.ObjectNames(out obj);
                GameObject parent;

                if (obj.Length > 0)
                {
                    parent = GameObject.Find(obj[0]);
                    if (parent != null)
                    {
                        CreateParticleHint(parent.transform);
                    }
                }

                if (obj.Length == 2)
                {
                    parent = GameObject.Find(obj[1]);
                    if (parent != null)
                    {
                        CreateParticleHint(parent.transform);
                    }
                }

                tutorial_hintUsed = true;
                if (!currentStepHintUsed)
                {
                    UpdatePoints(-1); // penalty for using hint
                    currentStepHintUsed = true;
                }
            }
        }

        if (!menuScene && uiSet)
        {
            if (pointsText.gameObject.activeSelf)
            {
                pointsText.text = points.ToString();// + " / " + totalPoints;
            }

            if (percentageText.gameObject.activeSelf)
            {
                percentageText.text = Mathf.RoundToInt(PercentageDone).ToString() + "%";
            }
        }
    }

    public void CreateParticleHint(Transform obj)
    {
        GameObject particles = Instantiate(Resources.Load<GameObject>("Prefabs/ParticleHint"),
            obj.position, Quaternion.Euler(-90f, 0f, 0f), obj);
        particles.transform.localScale = new Vector3(
            particles.transform.localScale.x / particles.transform.lossyScale.x,
             particles.transform.localScale.y / particles.transform.lossyScale.y,
              particles.transform.localScale.z / particles.transform.lossyScale.z);
        particles.name = "ParticleHint";
        particleHints.Add(particles);
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
        UpdatePoints(occured ? 1 : -1);
        
        Debug.Log("Combine " + leftHand + " and " + rightHand + " with result " + occured);

        if (!CheckScenarioCompleted() && occured)
            ActionManager.CorrectAction();
    }

    /// <summary>
    /// Handle (trigger) UseAction.
    /// </summary>
    /// <param name="useObject">Name of the used object.</param>
    public void OnUseAction(string useObject)
    {
        string[] info = { useObject };
        bool occured = Check(info, ActionType.ObjectUse);
        UpdatePoints(occured ? 1 : -1);

        Debug.Log("Use " + useObject + " with result " + occured);

        if (!CheckScenarioCompleted() && occured)
            ActionManager.CorrectAction();
    }

    /// <summary>
    /// Handle (trigger) Talk action.
    /// </summary>
    /// <param name="topic">Chosen topic</param>
    public void OnTalkAction(string topic)
    {
        string[] info = { topic };
        bool occured = Check(info, ActionType.PersonTalk);
        UpdatePoints(occured ? 1 : -1);

        Debug.Log("Say " + topic + " with result " + occured);

        if (!CheckScenarioCompleted() && occured)
            ActionManager.CorrectAction();
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
        UpdatePoints(occured ? 1 : (target == "" ? 0 : -1));

        Debug.Log("Use " + item + " on " + target + " with result " + occured);

        if (!CheckScenarioCompleted() && occured)
            ActionManager.CorrectAction();
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
        UpdatePoints(occured ? 1 : 0); // no penalty

        Debug.Log("Examine " + item + " with state " + expected + " with result " + occured);

        if (!CheckScenarioCompleted() && occured)
            ActionManager.CorrectAction();
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
        UpdatePoints(occured ? 1 : 0); // no penalty

        if (occured)
        {
            Debug.Log("Pick Up " + item);
        }

        if (!CheckScenarioCompleted() && occured)
            ActionManager.CorrectAction();
    }

    public void OnSequenceStepAction(string stepName)
    {
        string[] info = { stepName };
        bool occured = Check(info, ActionType.SequenceStep);
        UpdatePoints(occured ? 1 : -1);

        Debug.Log("Sequence step: " + stepName + " with result " + occured);

        if (!CheckScenarioCompleted() && occured)
            ActionManager.CorrectAction();
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
                    
                    //inserted checklist stuff
                    RobotUITabChecklist.StrikeStep(actionList.IndexOf(action));
                    // end checklist
                }
            }
        }

        if (matched)
        {
            currentStepHintUsed = false;
			GameObject.FindObjectOfType<GameUI>().ButtonBlink(false);
        }
        
        if (matched && subcategoryLength <= 1)
        {
            currentActionIndex += 1;
        }
        
        if (!matched && type != ActionType.ObjectExamine && type != ActionType.PickUp)
        {
            int index = actionList.IndexOf(currentAction);
            if ( sublist.Count > 0 && !wrongStepIndexes.Contains(index) )
            {
                wrongStepIndexes.Add(index);
            }
            
            RobotUIMessageTab messageCenter = GameObject.FindObjectOfType<RobotUIMessageTab>();
            messageCenter.NewMessage("Verkeerde handeling!", sublist[0].extraDescr, RobotUIMessageTab.Icon.Error);

            ActionManager.WrongAction();

            penalized = true;

            if (type == ActionType.SequenceStep)
            {
                GameObject.Find("WrongAction").GetComponent<TimedPopUp>().Set(sublist[0].extraDescr);
            }
        }
        else
        {
			
            currentPointAward = currentAction.pointValue;
            List<Action> actionsLeft = actionList.Where(action =>
                action.SubIndex == currentActionIndex &&
                action.matched == false).ToList();
            
            currentAction = actionsLeft.Count > 0 ? actionsLeft.First() : null;

            // now we have not mandatory actions, let's skip them and add to mistakes
            List<Action> skippableActions = actionsLeft.Where(action => action.notMandatory == true).ToList();
            if (actionsLeft.Count == skippableActions.Count) // all of them are skippable
            {
                wrongStepIndexes.Add(actionList.IndexOf(currentAction));
                currentActionIndex += 1;
                
                // get next actions with new index
                actionsLeft = actionList.Where(action =>
                    action.SubIndex == currentActionIndex &&
                    action.matched == false).ToList();

                currentAction = actionsLeft.Count > 0 ? actionsLeft.First() : null;
            }
        }

        
        return matched;
    }

    /// <summary>
    /// Check if every action from action list is done and scene is completed.
    /// If yes - go to EndScore scene.
    /// Runs after every action check and clears particle hints.
    /// </summary>
    private bool CheckScenarioCompleted()
    {
        // clear hints
        foreach (GameObject o in particleHints)
            Destroy(o);
        particleHints.Clear();
        if (actionList.Find(action => action.matched == false && action.notMandatory == false) == null)
        {
            Narrator.PlaySound("LevelComplete", 0.1f);
            StartCoroutine(DelayedEndScene(5.0f));
            return true;
        }

        else return false;
    }

    private IEnumerator DelayedEndScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
		GameObject.FindObjectOfType<GameUI>().ShowDonePanel(true);
        //GameObject.Find("Preferences").GetComponent<EndScoreManager>().LoadEndScoreScene();
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
    /// Rolls animation sequence back.
    /// </summary>
    public void RollSequenceBack()
    {
        Action lastAction = actionList.Last(x => x.matched == true);
        while (lastAction.Type == ActionType.SequenceStep)
        {
            lastAction.matched = false;
            lastAction = actionList.Last(x => x.matched == true);
        }
        lastAction.matched = false;
        currentActionIndex = lastAction.SubIndex;
        currentAction = lastAction;
        GameObject.FindObjectOfType<Cheat_CurrentAction>().UpdateAction();
    }

    public static void PlayAddPointSound()
    {
        Narrator.PlaySound("PointScored", 0.1f);
        // todo move somewhere else
        if (GameObject.Find("_Dev") != null)
        {
            GameObject.Find("_Dev").GetComponent<Cheat_CurrentAction>().UpdateAction();
        }
    }

    /* not used
    public void OnGameOver()
    {
        Transform gameOver = GameObject.Find("UI").transform.Find("GameOver");
        gameOver.gameObject.SetActive(true);
            
        if (GameObject.Find("GameLogic") != null)
        {
            controls.keyPreferences.ToggleLock();
            GameObject.Find("GameLogic").GetComponent<GameTimer>().enabled = false;
        }

        PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();
        Crosshair crosshair = GameObject.Find("Player").GetComponent<Crosshair>();
        Animator animator = player.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        
        player.enabled = false;
        crosshair.enabled = false;

        animator.speed = 0.0f;
        Time.timeScale = 0f;

        AudioSource[] audio = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audio)
        {
            a.Pause();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    } */

    public void OnRetryButtonClick()
    {
        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel(
            SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuButtonClick()
    {
        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("Menu");
    }

    public static void WrongAction()
    {
        RobotManager.RobotWrongAction();
        Narrator.PlaySound("WrongAction");
    }

    public static void CorrectAction()
    {
        RobotManager.RobotCorrectAction();
        ActionManager.PlayAddPointSound();
    }

    public void UpdatePoints(int value)
    {
        if (value <= 0)
            return;

        value *= (penalized ? currentPointAward/2 : currentPointAward);
        penalized = false;

        points += value;

        if (points < 0)
        {
            points = 0;
        }
    }

    public void UpdatePointsDirectly(int value)
    {
        points += (penalized ? value/2 : value);
        penalized = false;

        if (points < 0)
        {
            points = 0;
        }
    }

    public void ActivatePenalty()
    {
        penalized = true;
    }

    public void SetUIObjects(Text points, Text percentage)
    {
        uiSet = true;

        pointsText = points;
        percentageText = percentage;
    }
}
