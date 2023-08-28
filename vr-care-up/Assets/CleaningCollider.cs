using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningCollider : MonoBehaviour
{
    public GameObject marker;
    public GameObject particles;
    public bool isCleaned = false;

    public TableCleaningAction cleaningMaster;

    void Start()
    {
    }

    void TriggerCleanup()
    {
        if (isCleaned)
            return;
        if (cleaningMaster.IsCleanActionAllowed())
        {
            isCleaned = true;
            // marker.SetActive(true);
            particles.SetActive(true);
            cleaningMaster.CleanActionCount();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Cloth")
        {
            Debug.Log(collision.gameObject.name);
            TriggerCleanup();
        }
    }
}
