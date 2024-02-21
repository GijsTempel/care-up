using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandContactControl : MonoBehaviour
{
    Dictionary<PickableObject, int> pickableInAreaCounters = new Dictionary<PickableObject, int>();
    PickupHighliteControl pickupHighliteControl;
    private void OnEnable()
    {
        ClearObjectsFromArea();
    }

    void Start()
    {
        pickupHighliteControl = GameObject.FindObjectOfType<PickupHighliteControl>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        AddObjectToArea(pickableObject);
        if (pickupHighliteControl != null)
            pickupHighliteControl.InitUpdateHighlite();
    }

    private void OnTriggerExit(Collider collision)
    {
        PickableObject pickableObject = collision.GetComponent<PickableObject>();
        RemoveObjectFromArea(pickableObject);
        if (pickupHighliteControl != null)
            pickupHighliteControl.InitUpdateHighlite();
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

    public void ClearObjectsFromArea()
    {
        pickableInAreaCounters.Clear();
        if (pickupHighliteControl != null)
            pickupHighliteControl.InitUpdateHighlite();
    }

    protected void OnDisable()
    {
        Debug.Log("@ ____" + name + ":Disabled");
        ClearObjectsFromArea();
    }
}
