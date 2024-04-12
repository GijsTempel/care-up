using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnTimeout : MonoBehaviour
{
    public float waitTime = 1.0f;

    void Update()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
            Destroy(gameObject);
    }
}
