using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;

/// <summary>
/// Player can perform Talk action to this object.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PersonObject : InteractableObject
{
    [HideInInspector]
    public bool tutorial_talked = false;
    [HideInInspector]
    public bool tutorial_used = false;
    public bool allowToTalk = true;

    public List<string> dialogueXmls;
    protected int currentDialogueIndex = 0;

    private List<SelectDialogue.DialogueOption> optionsList;

    private List<GameObject> callers;

    protected bool inhaling = false;
    private bool direction = true;
    private float inhaleCounter = 1.0f;

    protected AudioSource audioSource;

    protected bool lookAtCamera;

    public GameObject TalkBubbleAnchor = null;

    protected override void Start()
    {
        base.Start();

        callers = new List<GameObject>();
        optionsList = new List<SelectDialogue.DialogueOption>();

        if (dialogueXmls.Count > 0)
        {
            LoadDialogueOptions(dialogueXmls[0]);
        }
        else
        {
            LoadDialogueOptions("Greeting");
        }

        audioSource = GetComponent<AudioSource>();

        inhaling = false;
        direction = true;
        inhaleCounter = 1.0f;

        lookAtCamera = true;

        rend = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public bool hasTopic(string topic)
    {
        if (optionsList != null)
            foreach (SelectDialogue.DialogueOption sd in optionsList)
            {
                if (sd.attribute == topic)
                    return true;
            }
        return false;
    }

    protected override void Update()
    {
        CallerUpdate();
        callers.Clear();

        if (inhaling)
        {
            float inhaleSpeed = 1 / 17.0f * Time.deltaTime;
            if (direction)
            {
                inhaleCounter += inhaleSpeed;
                if (inhaleCounter > 1.1f)
                {
                    direction = !direction;
                }
            }
            else
            {
                inhaleCounter -= inhaleSpeed;
                if (inhaleCounter < 1.0f)
                {
                    direction = !direction;
                }
            }
            transform.localScale = Vector3.one * inhaleCounter;
        }
    }

    public virtual void Talk(string topic = "")
    {
        if (ViewModeActive() || topic == "CM_Leave" || topic == "")
            return;

        if (actionManager.CompareTopic(topic))
        {
            tutorial_talked = true;

            switch (topic)
            {
                case "DoubleCheck":
                   this.GetComponent<MoveToPoint>().toWalk = true;
                    break;
                default:
                    break;
            }

            NextDialogue();
        }

        actionManager.OnTalkAction(topic);
    }

    public void DialoqueTalk(string topic = "", List<SelectDialogue.DialogueOption> additionalOptions = null, string question = null)
    {
        Talk(topic);
    }

    public void NextDialogue()
    {
        ++currentDialogueIndex;
        if (currentDialogueIndex < dialogueXmls.Count)
        {
            LoadDialogueOptions(dialogueXmls[currentDialogueIndex]);
        }
    }

    public void SetDialogue(int d)
    {
        currentDialogueIndex = d;
        if (currentDialogueIndex < dialogueXmls.Count)
        {
            LoadDialogueOptions(dialogueXmls[currentDialogueIndex]);
        }
    }

    public void SkipGreetingDialogue()
    {
        if (currentDialogueIndex < dialogueXmls.Count)
        {
            if (dialogueXmls[currentDialogueIndex] == "Greeting")
            {
                NextDialogue();
            }
        }
    }

    /// <summary>
    /// Loads available topics to talk from xml file.
    /// </summary>
    /// <param name="filename">Xml filename</param>
    protected void LoadDialogueOptions(string filename)
    {
        optionsList.Clear();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/PersonDialogues/" + filename);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlOptions = xmlFile.FirstChild.NextSibling.ChildNodes;

        int count = 0;
        foreach (XmlNode xmlOption in xmlOptions)
        {
            string description = xmlOption.Attributes["text"].Value;
            string topic = xmlOption.Attributes["topic"] != null ? xmlOption.Attributes["topic"].Value : "";

            if (count < 3) // 3 options max, 4 is Close.
            {
                SelectDialogue.DialogueOption option = new SelectDialogue.DialogueOption(description, DialoqueTalk, topic);
                optionsList.Add(option);
                ++count;
            }
            else
            {
                break;
            }
        }
        // for leave option
        optionsList.Add(new SelectDialogue.DialogueOption("Verlaten", DialoqueTalk, "CM_Leave"));
    }

    /// <summary>
    /// Creates an instance of dialogue, switches camera mode
    /// </summary>
    public void CreateSelectionDialogue()
    {
        if (!allowToTalk)
        {
            return;
        }

        tutorial_used = true;
        GameObject dialogueObject = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/SelectionDialogue"),
                    GameObject.Find("OverlayCamera").transform) as GameObject;

        SelectDialogue dialogue = dialogueObject.GetComponent<SelectDialogue>();
        dialogue.Init();

        dialogue.AddOptions(optionsList);

        cameraMode.ToggleCameraMode(CameraMode.Mode.SelectionDialogue);
    }

    /// <summary>
    /// Person can consist of multiple parts, that should affect main Person class.
    /// </summary>
    /// <param name="caller">Part gameObject</param>
    public void CallUpdate(GameObject caller)
    {
        callers.Add(caller);
    }

    private void CallerUpdate()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free && !player.away)
        {
            bool flag = false;
            foreach (GameObject caller in callers)
            {
                if (caller == controls.SelectedObject)
                {
                    flag = true;
                }
            }

            bool selectedIsInteractable = (controls.SelectedObject != null && controls.CanInteract &&
                controls.SelectedObject.GetComponent<InteractableObject>() != null);
            if (flag && !cameraMode.animating)
            {
                if (controls.CanInteract)
                {                   
                    if (!itemDescription.activeSelf)
                    {
                        itemDescription.SetActive(true);
                    }

                    itemDescription.GetComponentInChildren<Text>().text = (description == "") ? name : description;
                }
                else if (!controls.CanInteract)
                {
                    SetShaderTo(onMouseExitShader);
                    itemDescription.SetActive(false);
                }
            }
            else
            {               
                if (!selectedIsInteractable)
                {
                    itemDescription.SetActive(false);
                }
            }
        }

        if (cameraMode.animating)
        {
            SetShaderTo(onMouseExitShader);
            itemDescription.SetActive(false);
        }

        if (itemDescription.activeSelf && !player.itemControls.gameObject.activeSelf)
        {
            itemDescription.transform.GetChild(0).transform.position = Input.mousePosition + new Vector3(50.0f, 25.0f);
        }
    }

    protected void OnAnimatorIK()
    {
        if (lookAtCamera)
        {
            Animator animator = GetComponent<Animator>();
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(Camera.main.transform.position);
        }
    }
}