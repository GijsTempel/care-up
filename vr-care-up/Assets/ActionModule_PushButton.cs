using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_PushButton : MonoBehaviour
{
    public LayerMask layerMask = 0b11111111;
    public Transform rayPointerTransform;
    public GameObject buttonTop;
    public Transform maxPushTransform;
    float maxPushDist;
    public ActionTrigger actionTrigger;

    Vector3 rayDirection;
    Vector3 originalPosition;
    float maxRayDist = 0.13f;
    Vector3 originPos;
    bool pressed = false;
    void Start()
    {
        originalPosition = buttonTop.transform.position;
        rayDirection = (rayPointerTransform.position - transform.position).normalized;
        maxRayDist = Vector3.Distance(transform.position, rayPointerTransform.position);
        maxPushDist = Vector3.Distance(transform.position, maxPushTransform.position);
    }

    void Update()
    {
        bool wasPressed = pressed;
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, maxRayDist, layerMask))
        {
            Vector3 nextPosition = originalPosition - rayDirection.normalized * (maxRayDist - hit.distance);
            float pushDot = Vector3.Dot((nextPosition - maxPushTransform.position).normalized, rayDirection);
            if (pushDot < 0)
            {
                nextPosition = originalPosition - rayDirection.normalized * maxPushDist;
                pressed = true;
            }
            buttonTop.transform.position = nextPosition;
        }
        else
        {
            buttonTop.transform.position = Vector3.Lerp(buttonTop.transform.position, originalPosition, Time.deltaTime * 10f);
            pressed = false;
        }
        if(pressed && !wasPressed)
        {
            actionTrigger.AttemptTrigger();
            Debug.Log("pressed");
        }
    }
}
