using System.Xml;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public AnimationSequence(string filename)
    {
        currentStep = 0;
        pointsEarned = 1;

        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (cameraMode == null) Debug.LogError("No camera mode found");

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");

        LoadFromFile(filename);
    }

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
                    if (step.options.Count < 3) // add only 3 options as 4th is leaving
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
            step.options.Add(new SelectDialogue.DialogueOption("Leave", PlayAnimation, "CM_Leave"));

            steps.Add(step);
        }
    }

    private void PlayAnimation(string animation)
    {
        if (animation != "")
        {
            if (animation == "CM_Leave")
            {
                actionManager.StepBack();
                actionManager.Points -= pointsEarned + (currentStep != 1 ? 1 : 0);
                currentStep = steps.Count;
                NextStep();
            }
            else
            {
                if (steps.ElementAt(currentStep-1).randomOptions.Count > 0)
                {
                    if (animation == steps.ElementAt(currentStep-1).GetCorrectAnimation())
                    {
                        Debug.Log("Play animation: " + animation);
                        actionManager.Points++;
                        pointsEarned++;
                        NextStep();
                    }
                    else
                    {
                        Narrator.PlaySound("WrongAction");
                        actionManager.Points--;
                    }
                }
                else
                {
                    if (animation == "Inject Pricking pen" || animation == "ThrowSyringe")
                    {
                        GameObject.Find("GameLogic").GetComponent<HandsInventory>().PutAllOnTable();
                    }

                    Debug.Log("Play animation: " + animation);
                    actionManager.Points++;
                    pointsEarned++;
                    NextStep();
                }
            }
        }
        else
        {
            Narrator.PlaySound("WrongAction");
            actionManager.Points--;
        }
    }

    public void NextStep()
    {
        GameObject dialogueObject = GameObject.Find("SelectionDialogue");
        if (currentStep < steps.Count)
        {
            if (dialogueObject == null)
            {
                dialogueObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/SelectionDialogue"),
                        Camera.main.transform.position + Camera.main.transform.forward * 3.0f,
                        Camera.main.transform.rotation) as GameObject;
                dialogueObject.name = "SelectionDialogue";
            }

            SelectDialogue dialogue = dialogueObject.GetComponent<SelectDialogue>();
            dialogue.Init(false);

            if (steps.ElementAt(currentStep).randomOptions.Count > 0)
            {
                dialogue.SetText(steps.ElementAt(currentStep).GetCorrectAudio());
            }

            dialogue.AddOptions(steps.ElementAt(currentStep).options);

            cameraMode.ToggleCameraMode(CameraMode.Mode.SelectionDialogue);
        }
        else
        {
            Object.Destroy(dialogueObject.gameObject);
            cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
        }

        currentStep += 1;
    }

}
