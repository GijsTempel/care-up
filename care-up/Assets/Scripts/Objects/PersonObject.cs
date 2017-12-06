using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Player can perform Talk action to this object.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PersonObject : InteractableObject {

    [HideInInspector]
    public bool tutorial_talked = false;
    [HideInInspector]
    public bool tutorial_used = false;

    public string dialogueXml;

    private List<SelectDialogue.DialogueOption> optionsList;

    private List<GameObject> callers;

    private bool inhaling = false;
    bool direction = true;
    private float inhaleCounter = 1.0f;

    protected AudioSource audioSource;

    protected override void Start()
    {
        base.Start();

        callers = new List<GameObject>();
        optionsList = new List<SelectDialogue.DialogueOption>();
        LoadDialogueOptions(dialogueXml);

        audioSource = GetComponent<AudioSource>();

        inhaling = false;
        direction = true;
        inhaleCounter = 1.0f;
    }

    protected override void Update()
    {
        CallerUpdate();
        callers.Clear();
        
        if (inhaling)
        {
            float inhaleSpeed = 1/17.0f * Time.deltaTime;
            if ( direction )
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
                if ( inhaleCounter < 1.0f )
                {
                    direction = !direction;
                }
            }
            transform.localScale = Vector3.one * inhaleCounter;
        }
    }

    public void Talk(string topic = "")
    {
        if (ViewModeActive() || topic == "")
            return;

        tutorial_talked = true;

        if (topic == actionManager.CurrentTopic)
        {
            switch (topic)
            {
                case "RollUpSleeves":
                case "ExtendArmMakeFist":
                    GetComponent<Animator>().SetTrigger("ShowArm");
                    if (GetComponent<InjectionPatient>() != null)
                    {
                        GetComponent<InjectionPatient>().RollUpSleevesDialogue();
                    }
                    break;
                case "ComfortablePosition":
                    inhaling = true;
                    break;
                case "ShowBellyForInsulin":
                    GetComponent<Animator>().SetTrigger("ShowBellyForInsulin");
                    break;
                default:
                    break;
            }
        }

        actionManager.OnTalkAction(topic);
    }
    
    /// <summary>
    /// Loads available topics to talk from xml file.
    /// </summary>
    /// <param name="filename">Xml filename</param>
    private void LoadDialogueOptions(string filename)
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
                SelectDialogue.DialogueOption option = new SelectDialogue.DialogueOption(description, Talk, topic);
                optionsList.Add(option);
                ++count;
            }
            else
            {
                break;
            }
        }
        // for leave option
        optionsList.Add(new SelectDialogue.DialogueOption("Verlaten", Talk, ""));
    }

    /// <summary>
    /// Creates an instance of dialogue, switches camera mode
    /// </summary>
    public void CreateSelectionDialogue()
    {
        tutorial_used = true;
        GameObject dialogueObject = Instantiate(Resources.Load<GameObject>("Prefabs/SelectionDialogue"),
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
        if (cameraMode.CurrentMode == CameraMode.Mode.Free && !cameraMode.animating)
        {
            bool flag = false;
            bool clickFlag = true;
            foreach (GameObject caller in callers)
            {
                if (caller == controls.SelectedObject)
                {
                    flag = true;
                    if (caller.GetComponent<ExaminableObject>() != null)
                        clickFlag = false;
                }
            }

            if (flag)
            {
                if (controls.CanInteract)
                {
                    if (rend.material.shader == onMouseExitShader)
                    {
                        SetShaderTo(onMouseOverShader);
                    }

                    if (!itemDescription.activeSelf)
                    {
                        itemDescription.GetComponentInChildren<Text>().text = (description == "") ? name : description;
                        Transform icons = itemDescription.transform.GetChild(0).GetChild(0);
                        icons.Find("UseIcon").gameObject.SetActive(false);//gameObject.GetComponent<UsableObject>() != null);
                        icons.Find("TalkIcon").gameObject.SetActive(false);//gameObject.GetComponent<PersonObject>() != null);
                        icons.Find("PickIcon").gameObject.SetActive(false);//gameObject.GetComponent<PickableObject>() != null);
                        icons.Find("ExamIcon").gameObject.SetActive(false);//gameObject.GetComponent<ExaminableObject>() != null);
                        itemDescription.SetActive(true);
                    }

                    /*if (controls.MouseClicked() && clickFlag)
                    {
                        Reset();
                        CreateSelectionDialogue();
                    }*/
                }
                else if (!controls.CanInteract && rend.material.shader == onMouseOverShader)
                {
                    SetShaderTo(onMouseExitShader);
                    itemDescription.SetActive(false);
                }
            }
            else
            {
                if (rend.material.shader == onMouseOverShader)
                {
                    SetShaderTo(onMouseExitShader);
                    itemDescription.SetActive(false);
                }
            }
        }

        if (cameraMode.animating && rend.material.shader == onMouseOverShader)
        {
            SetShaderTo(onMouseExitShader);
            itemDescription.SetActive(false);
        }

        if (itemDescription.activeSelf && !player.itemControls.gameObject.activeSelf)
        {
            itemDescription.transform.GetChild(0).transform.position = Input.mousePosition + new Vector3(50.0f, 25.0f);
        }
    }
}
