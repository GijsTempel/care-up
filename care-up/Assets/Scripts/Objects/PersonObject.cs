using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PersonObject : InteractableObject {

    public GameObject hand; // temp

    private AudioSource audioClip;

    protected override void Start()
    {
        base.Start();

        audioClip = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && cameraMode.CurrentMode == CameraMode.Mode.Free) {
            if (controls.SelectedObject == gameObject && controls.CanInteract)
            {
                GameObject dialogueObject = Instantiate(Resources.Load<GameObject>("Prefabs/SelectionDialogue"),
                    Camera.main.transform.position + Camera.main.transform.forward * 3.0f,
                    Camera.main.transform.rotation) as GameObject;

                SelectDialogue dialogue = dialogueObject.GetComponent<SelectDialogue>();
                dialogue.Init();

                List<SelectDialogue.DialogueOption> list = new List<SelectDialogue.DialogueOption>();

                if (SceneManager.GetActiveScene().name == "Injection")
                {
                    SelectDialogue.DialogueOption rollOption =
                        new SelectDialogue.DialogueOption("Show me your hand, please.", Talk, "RollUpSleeves");
                    list.Add(rollOption);
                }
                else if(SceneManager.GetActiveScene().name == "Giving Medicine")
                {
                    SelectDialogue.DialogueOption rollOption;

                    rollOption = new SelectDialogue.DialogueOption("Could you sit up straight please", Talk, "SitUpStraight");
                    list.Add(rollOption);

                    rollOption = new SelectDialogue.DialogueOption("Just sit back and relax", Talk, "SitBackRelax");
                    list.Add(rollOption);

                    rollOption = new SelectDialogue.DialogueOption("Could you please stand up", Talk, "StandUp");
                    list.Add(rollOption);

                    rollOption = new SelectDialogue.DialogueOption("Arch your back please", Talk, "ArchBack");
                    list.Add(rollOption);
                }

                dialogue.AddOptions(list);

                cameraMode.ToggleCameraMode(CameraMode.Mode.SelectionDialogue);
            }
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

        switch (topic)
        {
            case "RollUpSleeves":
                RollUpSleeves(true);
                break;
            default:
                break;
        }

        actionManager.OnTalkAction(topic);
    }

    private void RollUpSleeves(bool value)
    {
        hand.SetActive(value);
    }
}
