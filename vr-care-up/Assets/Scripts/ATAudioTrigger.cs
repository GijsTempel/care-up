using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATAudioTrigger : MonoBehaviour
{
    public string controlObjectName = "";
    public string audioObjectName;

    public void Execute()
    {
        if (controlObjectName == "")
        {
            if (GameObject.Find(audioObjectName) != null)
            {
                GameObject.Find(audioObjectName).GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            GameObject controlObject = GameObject.Find(controlObjectName);
            if (controlObject != null)
            {
                if (controlObject.GetComponent<ShowHideObjects>() != null)
                {
                    foreach(GameObject g in controlObject.GetComponent<ShowHideObjects>().hidenObjects)
                    {
                        if (g.name == audioObjectName && g.GetComponent<AudioSource>() != null)
                        {
                            g.GetComponent<AudioSource>().Play();
                            break;
                        }
                    }
                }
            }
        }
    }
}
