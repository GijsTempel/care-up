using UnityEngine;
using System.Collections;

public class HandsInventory : MonoBehaviour {

    // position in air 
    public float horisontalOffset = 2.0f;
    public float distanceFromCamera = 3.0f;

    private InteractableObject leftHandObject;
    private InteractableObject rightHandObject;
	
	void Update () {

        if (leftHandObject)
        {
            leftHandObject.transform.position = Camera.main.transform.position +
                Camera.main.transform.forward * distanceFromCamera +
                Camera.main.transform.right * (-horisontalOffset);
            leftHandObject.InHandUpdate(false);
        }

        if (rightHandObject)
        {
            rightHandObject.transform.position = Camera.main.transform.position + 
                Camera.main.transform.forward * distanceFromCamera +
                Camera.main.transform.right * horisontalOffset;
            rightHandObject.InHandUpdate(true);
        }
        
        if ( Input.GetKeyDown(KeyCode.G) )
        {
            if ( leftHandObject )
            {
                leftHandObject.Drop();
                leftHandObject = null;
            }
            
            if ( rightHandObject )
            {
                rightHandObject.Drop();
                rightHandObject = null;
            }
        }

	}

    public void PickItem(InteractableObject item)
    {
        if ( leftHandObject == null )
        {
            leftHandObject = item;
            return;
        }
        else if ( rightHandObject == null )
        {
            rightHandObject = item;
            return;
        }
    }

}
