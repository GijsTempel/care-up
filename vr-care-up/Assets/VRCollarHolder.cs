using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VRCollarHolder : MonoBehaviour
{
    public Transform targetVRObject;
    Animator animator;
    bool isOpened = false;
    float openTimer = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {   
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
        animator.SetTrigger("Close");
        isOpened = false;
        openTimer = 5f;
    }
}
