using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PersonObject : MonoBehaviour {

    private string topic = "";

    private AudioSource audio;

    static private ActionManager actionManager;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");
    }

    public void Talk()
    {
        if (audio.clip)
        {
            audio.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip not set.");
        }
        actionManager.OnTalkAction(topic);
    }
}
