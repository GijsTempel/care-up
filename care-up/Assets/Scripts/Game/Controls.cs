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

            HandleUseObjects();
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
        if ( usableObject && Input.GetMouseButtonDown(0) )
        {
            usableObject.Use();
        }

        return flag;
    }
}
