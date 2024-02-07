using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemDroppedTrigger : MonoBehaviour
{
    [SerializeField]
    public GameObject disappearing_particles_prefab;
    [SerializeField]
    public GameObject appering_particles_prefab;
    [SerializeField]
    public GameObject path_line_from_a_to_b;

    private void OnTriggerEnter(Collider collider)
    {
        PickableObject o = null;
        if (collider.TryGetComponent<PickableObject>(out o))
        {
            // it's not rare for an object to be a child of another pickable object
            // for this case, let's go through parents, looking for the actual parent instead
            Transform p = collider.transform.parent;
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
            StartCoroutine(o.OnItemDroppedOnGround(
                disappearing_particles_prefab, 
                appering_particles_prefab,
                path_line_from_a_to_b));
        }
    }
}
