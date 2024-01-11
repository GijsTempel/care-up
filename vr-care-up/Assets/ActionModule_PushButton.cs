using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_PushButton : MonoBehaviour
{
    public LayerMask layerMask = 0b11111111;
    public Transform rayPointerTransform;
    public GameObject buttonTop;
    public Transform maxPushTransform;
    bool pushRestarted = false;
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
        bool moveDown = false;
        if (!pressed && Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, maxRayDist, layerMask))
        {
            if (!pushRestarted && hit.distance / maxRayDist > 0.9f)
                pushRestarted = true;
            if (pushRestarted)
            {
                moveDown = true;
                Vector3 nextPosition = originalPosition - rayDirection.normalized * (maxRayDist - hit.distance);
                float pushDot = Vector3.Dot((nextPosition - maxPushTransform.position).normalized, rayDirection);
                if (pushDot < 0)
                {
                    nextPosition = originalPosition - rayDirection.normalized * maxPushDist;
                    pressed = true;
                }
                buttonTop.transform.position = nextPosition;
            }
        }
        if (!moveDown)
        {
            buttonTop.transform.position = Vector3.Lerp(buttonTop.transform.position, originalPosition, Time.deltaTime * 10f);
            if (Vector3.Distance(originalPosition, buttonTop.transform.position) < 0.00001f)
                pressed = false;
        }
        if(pressed && !wasPressed)
        {
            pushRestarted = false;
            actionTrigger.AttemptTrigger();
            Debug.Log("pressed");
        }
    }
}
