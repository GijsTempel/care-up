using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchColliderHandler : MonoBehaviour
{
    public Transform indexFingerEndBone;
    public Transform thumbFingerEndBone;

    void Update()
    {
        // this isn't properly working due to bug in bone positioning yet
        transform.position = Vector3.Lerp(indexFingerEndBone.position, thumbFingerEndBone.position, 0.5f);
        transform.rotation.SetFromToRotation(indexFingerEndBone.position, thumbFingerEndBone.position);
    }
}
