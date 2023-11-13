using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VRCollarHolder : MonoBehaviour
{
    public Transform targetVRObject;
    public Animator tutAnimator;
    Animator animator;
    bool isOpened = false;
    float openTimer = 5f;
    float closeWaitTime = 1.5f;
    float closeWaitTimeValue = 1.5f;
    bool toClose = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerTutorialAnimation(string triggerName)
    {
        if (!isOpened)
        {
            closeWaitTimeValue = closeWaitTime;
            OpenTutorialShelf();
        }
        tutAnimator.SetTrigger(triggerName);
    }

    void Update()
    {   
        if (isOpened && closeWaitTimeValue > 0)
            closeWaitTimeValue -= Time.deltaTime;
        if (toClose && closeWaitTimeValue <= 0)
            CloseTutorialShelf();
        // if (!isOpened)
        // {
        //     openTimer -= Time.deltaTime;
        //     if (openTimer <= 0)
        //         OpenTutorialShelf();
        // }
        if (targetVRObject != null)
        {
            transform.position = targetVRObject.position;
            Vector3 newRotation = new Vector3(transform.eulerAngles.x, targetVRObject.eulerAngles.y, transform.eulerAngles.z);
            transform.eulerAngles = newRotation;
        }
    }

    public void OpenTutorialShelf()
    {
        if (isOpened)
            return;
        animator.SetTrigger("Open");
        isOpened = true;
    }

    public void CloseTutorialShelf()
    {
        if (!isOpened)
            return;
        if (closeWaitTimeValue > 0)
        {
            toClose = true;
            return;
        }
        toClose = false;
        animator.SetTrigger("Close");
        isOpened = false;
        openTimer = 5f;
    }
}
