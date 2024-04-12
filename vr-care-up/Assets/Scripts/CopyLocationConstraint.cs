using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLocationConstraint : MonoBehaviour
{
    public bool CopyX = false;
    public bool CopyY = false;
    public bool CopyZ = false;
    public Transform targetTransform;

    void Update()
    {
        Vector3 currentPos = transform.position;
        if (CopyX)
            currentPos.x = targetTransform.position.x;
        if (CopyY)
            currentPos.y = targetTransform.position.y;
        if (CopyZ)
            currentPos.z = targetTransform.position.z;
        transform.position = currentPos;
    }
}
