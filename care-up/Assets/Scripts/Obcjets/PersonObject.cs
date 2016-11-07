using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PersonObject : InteractableObject {

    private string topic = "";

    private AudioSource audioClip;
    static private ActionManager actionManager;

    protected override void Start()
    {
        base.Start();

        audioClip = GetComponent<AudioSource>();

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");
    }

    public void Talk()
    {
        if (audioClip.clip)
        {
            audioClip.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip not set.");
        }
        actionManager.OnTalkAction(topic);
    }
}
