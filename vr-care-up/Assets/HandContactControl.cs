using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandContactControl : MonoBehaviour
{
    Dictionary<PickableObject, int> pickableInAreaCounters = new Dictionary<PickableObject, int>();
    // public List<PickableObject> pickablesInArea = new List<PickableObject>();
    private void OnEnable()
    {
        ClearObjectsFromArea();
    }

    private void Update()
    {
        // string ss = "@ ta " + name + ":\n";
        // foreach(PickableObject p in pickablesInArea)
        // {
        //     if (p != null)
        //         ss += p.name + "\n";
        // }
        // Debug.Log(ss);
    }

    private void OnTriggerEnter(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        AddObjectToArea(pickableObject);

        // if (pickableObject != null && !(pickablesInArea.Contains(pickableObject)))
        // {
        //     pickablesInArea.Add(pickableObject);
        // }
    }

    private void OnTriggerExit(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        RemoveObjectFromArea(pickableObject);
    }

    public List<PickableObject> GetObjectsInArea()
    {
        List<PickableObject> pickableCurrentlyInArea = new List<PickableObject>();
        foreach(PickableObject p in pickableInAreaCounters.Keys)
        {
            if (pickableInAreaCounters[p] > 0)
                pickableCurrentlyInArea.Add(p);
        }

        return pickableCurrentlyInArea;
    }

    private void AddObjectToArea(PickableObject pickableObject)
    {
        if (pickableObject == null)
            return;
        if (pickableInAreaCounters.Keys.Contains(pickableObject))
            pickableInAreaCounters[pickableObject] += 1;
        else
            pickableInAreaCounters[pickableObject] = 1;
    }

    private void RemoveObjectFromArea(PickableObject pickableObject)
    {
        if (pickableInAreaCounters.Keys.Contains(pickableObject))
        {
            pickableInAreaCounters[pickableObject] -= 1;
            if (pickableInAreaCounters[pickableObject] <= 0)
                pickableInAreaCounters.Remove(pickableObject);
        }

        // if (pickableObject != null && (pickablesInArea.Contains(pickableObject)))
        //     pickablesInArea.Remove(pickableObject);
    }

    private void ClearObjectsFromArea()
    {
        pickableInAreaCounters.Clear();
    }
}
