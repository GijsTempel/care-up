using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PersonObjectPart : InteractableObject {

    private PersonObject person = null;

    protected override void Start()
    {
        base.Start();

        Transform parent = transform.parent;
        while (person == null)
        {
            if (parent != null)
            {
                person = parent.GetComponent<PersonObject>();
                parent = parent.parent;
            } 
            else
            {
                break;
            }
        }
    }

    protected override void Update()
    {
        if (person != null)
        {
            person.CallUpdate(gameObject);
        }
        else
        {
            Debug.Log("No person");
        }
    }
}
