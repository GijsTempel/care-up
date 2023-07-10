using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTriggerRaycast : MonoBehaviour
{
    private float lookAtTimer = 0f;
    int lookAtObjectId;
    const float MAX_RAY_TRIGGER_TIME = 1.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("@^^^^:" + lookAtTimer.ToString());
        CheckForCollisions();
    }

    void CheckForCollisions()
    {
        string ditObjName = "";
        int layerMask = 1 << 6;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.4f, layerMask))
        {
            ditObjName = hit.collider.name;
            int objectId = hit.collider.GetInstanceID();
            if (objectId == lookAtObjectId && hit.collider.GetComponent<ActionCollider>() != null &&
                hit.collider.GetComponent<ActionCollider>().isRayTrigger)
            {   

                lookAtTimer += Time.deltaTime;
                if (lookAtTimer > MAX_RAY_TRIGGER_TIME)
                {
                    hit.collider.GetComponent<ActionCollider>().RayTriggerAction();
                    lookAtTimer = 0f;
                }

            }
            else
            {
                lookAtTimer = 0f;
            }
            lookAtObjectId = objectId;
        }
        else
        {
            lookAtTimer = 0f;
        }
        Debug.Log("@ **Ray:" + ditObjName);
    }
}
