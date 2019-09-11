using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLocation : MonoBehaviour
{
    public Transform anchorObject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anchorObject != null)
        {
            transform.position = anchorObject.position;
        }
    }
}
