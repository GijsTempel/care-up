using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTutorialTigger : MonoBehaviour
{
    public string triggerName;
    public float waitTime = 2f;
    float timeoutValue = 2f;
    PlayerScript playerScript;
    // Start is called before the first frame update
    void Start()
    {
        timeoutValue = waitTime;
        playerScript = GameObject.FindObjectOfType<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckConditions())
        {
            timeoutValue -= Time.deltaTime;
            if (timeoutValue <= 0)
                Execute();
        }
        else
            timeoutValue = waitTime;
    }

    bool CheckConditions()
    {

        for(int i = 0; i < transform.childCount; i++)
        {
            ActionExpectant e = transform.GetChild(i).GetComponent<ActionExpectant>();
            if (e != null && !e.isCurrentAction)
                return false;
            ItemInHandCheck itemInHandCheck = transform.GetChild(i).GetComponent<ItemInHandCheck>();
            if (itemInHandCheck != null && !itemInHandCheck.Check())
                return false;
        }
        if (playerScript.IsInCopyAnimationState())
            return false;
        return true;
    }

    void Execute()
    {
        VRCollarHolder collarHolder = GameObject.FindObjectOfType<VRCollarHolder>();
        if (collarHolder != null)
            collarHolder.TriggerTutorialAnimation(triggerName);
        gameObject.SetActive(false);
    }
}
