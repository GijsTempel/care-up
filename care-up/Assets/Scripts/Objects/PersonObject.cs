using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class PersonObject : InteractableObject {

    public string dialogueXml;
    public GameObject hand; // temp

    private List<SelectDialogue.DialogueOption> optionsList;
    private AudioSource audioClip;

    private bool inhaling = false;
    bool direction = true;
    private float inhaleCounter = 1.0f;

    protected override void Start()
    {
        base.Start();

        optionsList = new List<SelectDialogue.DialogueOption>();
        LoadDialogueOptions(dialogueXml);

        audioClip = GetComponent<AudioSource>();

        inhaling = false;
        direction = true;
        inhaleCounter = 1.0f;
    }

    protected override void Update()
    {
        base.Update();
        if (controls.MouseClicked() && cameraMode.CurrentMode == CameraMode.Mode.Free) {
            if (controls.SelectedObject == gameObject && controls.CanInteract)
            {
                CreateSelectionDialogue();
            }
        }

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
            transform.parent.localScale = Vector3.one * inhaleCounter;
        }
    }

    public void Talk(string topic = "")
    {
        if (ViewModeActive())
            return;

        // play sound depending on topic (no sound yet)
        if (audioClip.clip)
        {
            audioClip.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip not set.");
        }

        if (topic == actionManager.CurrentTopic) {
            switch (topic)
            {
                case "RollUpSleeves":
                case "ExtendArmMakeFist":
                    RollUpSleeves(true);
                    break;
                case "ComfortablePosition":
                    inhaling = true;
                    break;
                default:
                    break;
            }
        }

        actionManager.OnTalkAction(topic);
    }

    private void RollUpSleeves(bool value)
    {
        hand.SetActive(value);
    }

    private void LoadDialogueOptions(string filename)
    {
        optionsList.Clear();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/PersonDialogues/" + filename);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlOptions = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode xmlOption in xmlOptions)
        {
            string description = xmlOption.Attributes["text"].Value;
            string topic = xmlOption.Attributes["topic"] != null ? xmlOption.Attributes["topic"].Value : "";

            SelectDialogue.DialogueOption option = new SelectDialogue.DialogueOption(description, Talk, topic);
            optionsList.Add(option);    
        }
    }

    private void CreateSelectionDialogue()
    {
        GameObject dialogueObject = Instantiate(Resources.Load<GameObject>("Prefabs/SelectionDialogue"),
                    Camera.main.transform.position + Camera.main.transform.forward * 3.0f,
                    Camera.main.transform.rotation) as GameObject;

        SelectDialogue dialogue = dialogueObject.GetComponent<SelectDialogue>();
        dialogue.Init();

        dialogue.AddOptions(optionsList.OrderBy(x => Random.value).ToList());

        cameraMode.ToggleCameraMode(CameraMode.Mode.SelectionDialogue);
    }
}
