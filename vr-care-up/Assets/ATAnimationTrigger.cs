using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATAnimationTrigger : MonoBehaviour
{

    public String objectName = "";
    public String trigger = "";

    // Update is called once per frame
    public void Execute()
    {
        if (objectName != "" && trigger != "")
        {
            GameObject obj = GameObject.Find(objectName);
            if (obj != null && obj.GetComponent<Animator>() != null)
            {
                obj.GetComponent<Animator>().SetTrigger(trigger);
            }
        }
    }
}
