using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHandsTransform : MonoBehaviour
{
    public Transform targetVRCamera;
    public GameObject RightHandRootBone;
    public GameObject LeftHandRootBone;

    void Start()
    {
        
    }

    void Update()
    {
        float x = targetVRCamera.position.x;
        float y = targetVRCamera.position.y;
        float z = targetVRCamera.position.z;
        
        transform.position = new Vector3(x, y, z);
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, targetVRCamera.eulerAngles.y, transform.eulerAngles.z);
        
        transform.eulerAngles = newRotation;

    }
}
