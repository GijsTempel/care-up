using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    public float interactionDistance = 5.0f;

    private GameObject selectedObject;
    private bool canInteract;

    public GameObject SelectedObject
    {
        get { return selectedObject; }
    }

    public bool CanInteract
    {
        get { return canInteract; }
    }

    public void ResetObject()
    {
        selectedObject = null;
    }
	
	void Update () {

        // raycast only in this script
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.transform.gameObject;
            canInteract = (hit.distance <= interactionDistance) ? true : false;
        }
        else
        {
            ResetObject();
        }
    }
    
}
