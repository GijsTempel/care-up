using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    public float interactionDistance = 5.0f;

    private GameObject selectedObject;

    public GameObject SelectedObject
    {
        get { return selectedObject; }
    }

	void Start () {
	
	}
	
	void Update () {
        // raycast only here
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            selectedObject = hit.transform.gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                HandleUseObjects();
                HandleTalkPerson();
            }
        }
        else
        {
            selectedObject = null;
        }
    }

    bool HandleUseObjects()
    {
        bool flag = false;

        UsableObject usableObject = selectedObject.GetComponent<UsableObject>();
        if ( usableObject )
        {
            usableObject.Use();
            flag = true;
        }

        return flag;
    }

    bool HandleTalkPerson()
    {
        bool flag = false;

        PersonObject person = selectedObject.GetComponent<PersonObject>();
        if ( person )
        {
            person.Talk();
            flag = true;
        }

        return flag;
    }
}
