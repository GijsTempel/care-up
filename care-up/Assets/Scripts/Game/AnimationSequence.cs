using System.Xml;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequence  {
    
    public class SequenceStep
    {
        public string animationToPlay;
        public List<SelectDialogue.DialogueOption> options;

        public SequenceStep(string anim, List<SelectDialogue.DialogueOption> opt)
        {
            animationToPlay = anim;
            options = opt;
        }
    }

    private List<List<SelectDialogue.DialogueOption>> steps = new List<List<SelectDialogue.DialogueOption>>();
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
            List<SelectDialogue.DialogueOption> step = new List<SelectDialogue.DialogueOption>();
            XmlNodeList xmlOptions = xmlStep.ChildNodes;
            foreach(XmlNode xmlOption in xmlOptions)
            {
                string description = xmlOption.Attributes["text"].Value;
                string animation = xmlOption.Attributes["animation"] != null ? xmlOption.Attributes["animation"].Value : "";
                SelectDialogue.DialogueOption option = new SelectDialogue.DialogueOption(description, PlayAnimation, animation);
                if (step.Count < 3) // add only 3 options as 4th is leaving
                {
                    step.Add(option);
                }
            }
            // shuffle
            step = step.OrderBy(x => Random.value).ToList();
            // add leave option
            step.Add(new SelectDialogue.DialogueOption("Leave", PlayAnimation, "CM_Leave"));
            
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
                if ( animation == "Inject Pricking pen" )
                {
                    GameObject.Find("GameLogic").GetComponent<HandsInventory>().PutAllOnTable();
                }

                Debug.Log("Play animation: " + animation);
                actionManager.Points++;
                pointsEarned++;
                NextStep();
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

            dialogue.AddOptions(steps[currentStep]);

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
