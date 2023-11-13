using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemDroppedTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter : " + other.name);
        PickableObject o = null;
        if (other.TryGetComponent<PickableObject>(out o))
        {
            // it's not rare for an object to be a child of another pickable object
            // for this case, let's go through parents, looking for the actual parent instead
            Transform p = other.transform.parent;
            PickableObject po = null;
            while (p != null)
            {
                if (p.TryGetComponent<PickableObject>(out po))
                {
                    // if we found a parent,use that one instead
                    // (to avoid desync within child-parent transform
                    o = po;
                } 
                p = p.parent;
            }

            // initiate async sequence of teleporting object
            StartCoroutine(o.OnItemDroppedOnGround());
        }
    }
}
