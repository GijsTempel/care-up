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
    bool pressed = false;

    void Start()
    {
        originalPosition = buttonTop.transform.position;
        rayDirection = (rayPointerTransform.position - transform.position).normalized;
        Debug.Log(rayDirection);
        maxRayDist = Vector3.Distance(transform.position, rayPointerTransform.position);
        maxPushDist = Vector3.Distance(rayPointerTransform.position, maxPushTransform.position);
    }

    void Update()
    {
        bool wasPressed = pressed;
        Color c = Color.green;
        // Vector3 topRayPosition = 
        
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, maxRayDist, layerMask))
        {
            c = Color.red;

            if (!pressed)
            {
                if (!pushRestarted && hit.distance / maxRayDist > 0.85f)
                    pushRestarted = true;
                // if (pushRestarted)
                // {
                    Vector3 positionShiftVec = rayDirection.normalized * (maxRayDist - hit.distance); ;
                    Vector3 nextPosition = originalPosition - positionShiftVec;
                    float pushDot = Vector3.Dot(((rayPointerTransform.position - positionShiftVec) -
                        maxPushTransform.position).normalized, rayDirection);
                    if (pushDot < 0)
                    {
                        nextPosition = originalPosition - rayDirection.normalized * maxPushDist;
                        pressed = true;
                    }
                    buttonTop.transform.position = nextPosition;
                // }
            }
        }
        else
        {
            if (Vector3.Distance(originalPosition, buttonTop.transform.position) < 0.01f)
                pressed = false;
            buttonTop.transform.position = Vector3.Lerp(buttonTop.transform.position, originalPosition, Time.deltaTime * 10f);
        }
        Debug.DrawRay(transform.position, rayDirection * maxRayDist, c, 0.0f, false); 
        if(pressed && !wasPressed)
        {
            pushRestarted = false;
            if (actionTrigger != null)
                actionTrigger.AttemptTrigger();
            Debug.Log("pressed");
        }
    }
}
