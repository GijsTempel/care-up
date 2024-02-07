using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_ObjectDetector : MonoBehaviour
{
    public string objectName;
    bool isObjectIn = false;
    bool isTiggered = false;
    PlayerScript playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerScript>();
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == objectName)
        {
            isObjectIn = true;
            AttemptTrigger();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (isObjectIn && collision.name == objectName)
        {
            isTiggered = false;
            isObjectIn = false;
        }
    }

    public bool Check()
    {
        if (isObjectIn)
        {
            GameObject neededObject = GameObject.Find(objectName);
            if (neededObject != null && playerScript.GetHandWithThisObject(neededObject) == null)
                return true;
        }
        return false;
    }

    public bool AttemptTrigger()
    {
        if (isTiggered)
            return false;
        if (!isObjectIn)
            return false;

        ActionTrigger actionTrigger = transform.parent.gameObject.GetComponent<ActionTrigger>();
        if (actionTrigger != null)
        {
            isTiggered = true;
            return actionTrigger.AttemptTrigger();
        }

        return false;
    }
}
