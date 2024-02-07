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
    public ActionModule_ActionTrigger actionTrigger;

    Vector3 rayDirection;
    Vector3 buttonOriginalPosition;
    Vector3 buttonOffset;
    float maxRayDist = 0.13f;
    bool pressed = false;

    void Start()
    {
        buttonOriginalPosition = buttonTop.transform.position;
        buttonOffset = buttonOriginalPosition - rayPointerTransform.position;
        rayDirection = (rayPointerTransform.position - transform.position).normalized;
        maxRayDist = Vector3.Distance(transform.position, rayPointerTransform.position);
        maxPushDist = Vector3.Distance(rayPointerTransform.position, maxPushTransform.position);
    }

    void FixedUpdate()
    {
        bool wasPressed = pressed;
        var topRayPos = buttonTop.transform.position - buttonOffset - rayDirection * 0.03f;
        
        Color mainRayColor = Color.green;
        Color topRayColor = Color.blue;
        bool isTopRayColliding = Physics.Raycast(topRayPos, rayDirection, maxRayDist * 0.5f, layerMask);
        if (isTopRayColliding)
            topRayColor = Color.yellow;
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, maxRayDist, layerMask))
        {
            mainRayColor = Color.red;
            Vector3 positionShiftVec = rayDirection.normalized * (maxRayDist - hit.distance);
            if (isTopRayColliding)
            {
                Vector3 nextButtonPosition = buttonOriginalPosition - positionShiftVec;
                float pushDot = Vector3.Dot(((rayPointerTransform.position - positionShiftVec) -
                    maxPushTransform.position).normalized, rayDirection);
                if (pushDot < 0)
                {
                    nextButtonPosition = buttonOriginalPosition - rayDirection.normalized * maxPushDist;
                    pressed = true;
                }
                buttonTop.transform.position = nextButtonPosition;
            }
        }
        else
        {
            if (Vector3.Distance(buttonOriginalPosition, buttonTop.transform.position) < 0.02f)
                pressed = false;
            buttonTop.transform.position = Vector3.Lerp(buttonTop.transform.position, buttonOriginalPosition, Time.deltaTime * 10f);
        }
        Debug.DrawRay(transform.position, rayDirection * maxRayDist, mainRayColor, 0.0f, false);
        Debug.DrawRay(topRayPos + new Vector3(0.001f, 0, 0.001f), rayDirection * maxRayDist * 0.5f, topRayColor, 0.0f, false);
        if(pressed && !wasPressed)
        {
            if (actionTrigger != null)
                actionTrigger.AttemptTrigger();
            Debug.Log("pressed");
        }
    }
}
