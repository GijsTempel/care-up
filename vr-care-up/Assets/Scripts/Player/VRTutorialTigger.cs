using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTutorialTigger : MonoBehaviour
{
    public string triggerName;
    public float waitTime = 2f;
    float timeoutValue = 2f;
    PlayerScript playerScript;
    HeadTriggerRaycast headTriggerRaycast;
    bool noConditions = false;
    // Start is called before the first frame update
    void Start()
    {
        timeoutValue = waitTime;
        playerScript = GameObject.FindObjectOfType<PlayerScript>();
        headTriggerRaycast = GameObject.FindObjectOfType<HeadTriggerRaycast>();
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
        if (noConditions)
            return false;
        if (playerScript.IsInCopyAnimationState())
            return false;
        if (headTriggerRaycast.IsLookingAtTeleport())
            return false;
        int conditionElements = 0;
        for(int i = 0; i < transform.childCount; i++)
        {
            ActionExpectant e = transform.GetChild(i).GetComponent<ActionExpectant>();
            if (e != null)
            {
                conditionElements++;
                if (!e.isCurrentAction)
                    return false;
            }
            ItemInHandCheck itemInHandCheck = transform.GetChild(i).GetComponent<ItemInHandCheck>();
            if (itemInHandCheck != null)
            {
                conditionElements++;
                if (!itemInHandCheck.Check())
                    return false;
            }
            if (conditionElements == 0)
            {
                noConditions = true;
                return false;
            }
        }
        return true;
    }

    public void Execute()
    {
        VRCollarHolder collarHolder = GameObject.FindObjectOfType<VRCollarHolder>();

        if (collarHolder != null)
        {
            if (triggerName != "")
                collarHolder.TriggerTutorialAnimation(triggerName);
            else
                collarHolder.CloseTutorialShelf();
        }
        
        for(int i = 0; i < transform.childCount; i++)
        {
            ActionExpectant e = transform.GetChild(i).GetComponent<ActionExpectant>();
            if (e != null)
                e.TryExecuteAction();
            ActionTrigger a = transform.GetChild(i).GetComponent<ActionTrigger>();
            if (a != null)
                a.AttemptTrigger();
        }
        gameObject.SetActive(false);
    }
}
