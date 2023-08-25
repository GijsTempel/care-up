using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningCollider : MonoBehaviour
{
    public GameObject marker;
    public bool isCleaned = false;

    TableCleaningAction cleaningMaster;

    void Start()
    {
        cleaningMaster = gameObject.GetComponentInParent<TableCleaningAction>();
    }

    void TriggerCleanup()
    {
        if (isCleaned)
            return;
        if (cleaningMaster.IsCleanActionAllowed())
        {
            isCleaned = true;
            marker.SetActive(true);
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
