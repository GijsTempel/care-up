using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingHandPickupControl : MonoBehaviour
{
    public List<PickableObject> pickablesInArea = new List<PickableObject>();

    private void OnEnable()
    {
        ClearObjectsFromArea();
    }

    private void Update()
    {
        string ss = "@ ta " + name + ":\n";
        foreach(PickableObject p in pickablesInArea)
        {
            if (p != null)
                ss += p.name + "\n";
        }
        Debug.Log(ss);
    }

    private void OnTriggerEnter(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        if (pickableObject != null && !(pickablesInArea.Contains(pickableObject)))
        {
            pickablesInArea.Add(pickableObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        RemoveObjectFromArea(pickableObject);
    }

    private void RemoveObjectFromArea(PickableObject pickableObject)
    {
        if (pickableObject != null && (pickablesInArea.Contains(pickableObject)))
            pickablesInArea.Remove(pickableObject);
    }

    private void ClearObjectsFromArea()
    {
        pickablesInArea.Clear();
    }
}
