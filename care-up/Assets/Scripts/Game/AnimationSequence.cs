using System.Xml;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for handling animation sequences.
/// </summary>
public class AnimationSequence  {
    
    public class RandomOption
    {
        public string audio;
        public string animation;

        public RandomOption(string au, string an)
        {
            audio = au;
            animation = an;
        }
    }

    public class SequenceStep
    {
        public List<SelectDialogue.DialogueOption> options;
        public List<RandomOption> randomOptions;
        public int correct;

        public SequenceStep()
        {
            options = new List<SelectDialogue.DialogueOption>();
            randomOptions = new List<RandomOption>();
        }

        public void GenerateCorrectOption()
        {
            if (randomOptions.Count > 0)
            {
                correct = Random.Range(0, randomOptions.Count);
            }
        }

        public string GetCorrectAnimation()
        {
            return randomOptions.ElementAt(correct).animation;
        }

        public string GetCorrectAudio()
        {
            return randomOptions.ElementAt(correct).audio;
        }
    }

    private List<SequenceStep> steps = new List<SequenceStep>();
    private int currentStep = 0;
    private int pointsEarned = 1;

    private CameraMode cameraMode;
    private ActionManager actionManager;

    public bool cheated = false;
    private bool completed = false;

    public bool Completed
    {
        get { return completed; }
    }

    /// <summary>
    /// Main c-tor of class. Loads info from file and starts seqence.
    /// </summary>
    /// <param name="filename">Filename of Xml sequence info</param>
    public AnimationSequence(string filename)
    {
        currentStep = 0;
        pointsEarned = 1;
        completed = false;

        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (cameraMode == null) Debug.LogError("No camera mode found");

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");

        LoadFromFile(filename);
    }

    /// <summary>
    /// Loads info from xml file and stores in list.
    /// </summary>
    /// <param name="filename">Name of the file</param>
    private void LoadFromFile(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/AnimationSequences/" + filename);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlSteps = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode xmlStep in xmlSteps)
        {
            SequenceStep step = new SequenceStep();
            XmlNodeList xmlOptions = xmlStep.ChildNodes;
            foreach (XmlNode xmlOption in xmlOptions)
            {
                if (xmlOption.Name == "option")
                {
                    string description = xmlOption.Attributes["text"].Value;
                    string animation = xmlOption.Attributes["animation"] != null ? xmlOption.Attributes["animation"].Value : "";
                    SelectDialogue.DialogueOption option = new SelectDialogue.DialogueOption(description, PlayAnimation, animation);
                    if (step.options.Count < 4)
                    {
                        step.options.Add(option);
                    }
                }
                else // randomoptions
                {
                    string audio = xmlOption.Attributes["audio"].Value;
                    string animation = xmlOption.Attributes["animation"].Value;
                    step.randomOptions.Add(new RandomOption(audio, animation));
                    step.GenerateCorrectOption();
                }
            }
            // shuffle
            step.options = step.options.OrderBy(x => Random.value).ToList();
            // add leave option
            //step.options.Add(new SelectDialogue.DialogueOption("Verlaten", PlayAnimation, "CM_Leave"));

            steps.Add(step);
        }
    }

    /// <summary>
    /// Handles result of selection of sequence step.
    /// </summary>
    /// <param name="animation">Data from selection.</param>
    private void PlayAnimation(string animation)
    {
        if (animation != "")
        {
            // leave sequence
            if (animation == "CM_Leave")
            {
                actionManager.RollSequenceBack();
                actionManager.UpdatePoints(-pointsEarned + (currentStep != 1 ? 1 : 0));
                currentStep = steps.Count;
                NextStep();
                PlayerAnimationManager.AbortSequence();
            }
            else
            {
                // if there's couple of correct options
                if (steps.ElementAt(currentStep-1).randomOptions.Count > 0)
                {
                    // if animation is correct
                    if (animation == steps.ElementAt(currentStep-1).GetCorrectAnimation())
                    {
                        actionManager.OnSequenceStepAction(animation);
                        pointsEarned++;
                        Object.Destroy(GameObject.Find("SelectionDialogue"));
                        PlayerAnimationManager.NextSequenceStep(false);
                    }
                    else
                    {
                        ActionManager.WrongAction();
                        actionManager.UpdatePoints(-1);
                    }
                }
                else //1 correct option
                {
                    if (animation == "Stick needle in arm")
                    {
                        actionManager.OnSequenceStepAction(animation);
                        pointsEarned++;
                        Object.Destroy(GameObject.Find("SelectionDialogue"));
                        GameObject.FindObjectOfType<InjectionPatient>().InjectNeedleInArmDialogue();
                        return;
                    }

                    if (animation == "Inject medicine slow and steady")
                    {
                        GameObject.FindObjectOfType<InjectionPatient>().InjectMedicineSlowlyDialogue();        
                    }
                    
                    if (animation == "Inject Pricking pen" || animation == "ThrowSyringe")
                    {
                        GameObject.Find("GameLogic").GetComponent<HandsInventory>().PutAllOnTable();
                    }

                    if (animation == "WaitAnotherMinute_heamogluco")
                    {
                        GameObject.Find("GameLogic").GetComponent<HandsInventory>().ForcePickItem("TestStrips", true);
                    }

                    actionManager.OnSequenceStepAction(animation);
                    pointsEarned++;
                    Object.Destroy(GameObject.Find("SelectionDialogue"));
                    PlayerAnimationManager.NextSequenceStep(false);
                }
            }
        }
    }


    /// <summary>
    /// Goes to next step of sequence. Handles creating/changing Dialogue.
    /// </summary>
    public void NextStep()
    {
        GameObject.FindObjectOfType<GameUI>().ShowIpad(true);

        GameObject dialogueObject = GameObject.Find("SelectionDialogue");

        if (currentStep < steps.Count)
        {
            if (dialogueObject == null)
            {
                dialogueObject = Object.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/SelectionDialogue"),
                        GameObject.Find("OverlayCamera").transform) as GameObject;
                dialogueObject.name = "SelectionDialogue";
            }

            SelectDialogue dialogue = dialogueObject.GetComponent<SelectDialogue>();
            dialogue.Init(false);

            if (steps.ElementAt(currentStep).randomOptions.Count > 0)
            {
                dialogue.SetText(steps.ElementAt(currentStep).GetCorrectAudio());
            }

            dialogue.AddOptions(steps.ElementAt(currentStep).options, cheated);

            cameraMode.ToggleCameraMode(CameraMode.Mode.SelectionDialogue);
        }
        else
        {
            if (dialogueObject != null)
            {
                // maybe obsolete
                Object.Destroy(dialogueObject.gameObject);
            }
            CameraMode cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
            cameraMode.ToggleCameraMode(CameraMode.Mode.Cinematic);
            cameraMode.animationEnded = true;
            cameraMode.cinematicToggle = false;
        }

        currentStep += 1;
    }

    /// <summary>
    /// Tutorial special func. Locks mouse movement or shows correct answer.
    /// </summary>
    /// <param name="value">True - lock mouse. False - unlock, show answer.</param>
    public void TutorialLock(bool value)
    {
        SelectDialogue dialogue = GameObject.Find("SelectionDialogue").GetComponent<SelectDialogue>();
        if ( value )
        {
            dialogue.tutorial_lock = true;
        }
        else
        {
            dialogue.tutorial_lock = false;
            dialogue.ShowAnswer();
        }
        cheated = dialogue.cheated = true;
    }

}
