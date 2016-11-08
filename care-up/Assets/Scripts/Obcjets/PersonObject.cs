using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PersonObject : InteractableObject {

    public GameObject hand; // temp

    private AudioSource audioClip;
    static private ActionManager actionManager;

    protected override void Start()
    {
        base.Start();

        audioClip = GetComponent<AudioSource>();

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");
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
