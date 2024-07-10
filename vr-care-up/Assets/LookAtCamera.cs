using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform mainCamera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null)
            FindMainCamera();

        if (mainCamera != null)
        {
            transform.LookAt(mainCamera);
        }
    }

    void FindMainCamera()
    {
        GameObject mainCameraObj = GameObject.FindWithTag("MainCamera");
        if (mainCameraObj != null)
            mainCamera = mainCameraObj.transform;

    }
}
