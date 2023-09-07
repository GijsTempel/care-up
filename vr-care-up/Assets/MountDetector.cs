using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MountDetector : MonoBehaviour
{
    public enum upEnum
    {
        X,
        mX,
        Y,
        mY,
        Z,
        mZ
    }
    public upEnum upDirection;
    public float dotLimit = 0f;
    List<Transform> mountsDetected = new List<Transform>();
    List<String> mountNames = new List<string>();

    public Transform FindClosestMount()
    {
        
        float dist = float.PositiveInfinity;
        Transform closest = null;
        foreach(Transform p in mountsDetected)
        {
            if (p != null)
            {
                Vector3 upVector = transform.up;
                Vector3 pUpVector = p.transform.up;

                switch (upDirection)
                {
                    case upEnum.X:
                        upVector = transform.right;
                        pUpVector = p.transform.right;

                        break;
                    case upEnum.mX:
                        upVector = -transform.right;
                        pUpVector = -p.transform.right;

                        break;
                    case upEnum.Y:
                        upVector = transform.up;
                        pUpVector = p.transform.up;

                        
                        break;
                    case upEnum.mY:
                        upVector = -transform.up;
                        pUpVector = -p.transform.up;

                        break;
                    case upEnum.Z:
                        upVector = transform.forward;
                        pUpVector = p.transform.forward;

                        break;
                    case upEnum.mZ:
                        upVector = -transform.forward;
                        pUpVector = -p.transform.forward;
                        break;
                }

                float _dot = Vector3.Dot(upVector, pUpVector);
                if (_dot < dotLimit)
                    continue;
                float nextDist = Vector3.Distance(transform.position, p.position);
                if (nextDist < dist)
                {
                    dist = nextDist;
                    closest = p;
                }
            }
        }
        return closest;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (mountNames.Count != 0)
        {
            if (!(mountNames.Contains(collision.name)))
                return;
        }
        if (collision.tag != "MountingPoint")
            return;
        
        if (!(mountsDetected.Contains(collision.transform)))
            mountsDetected.Add(collision.transform);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (mountsDetected.Contains(collision.transform))
            mountsDetected.Remove(collision.transform);
    }

}
