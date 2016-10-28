using UnityEngine;
using System.Collections;

public class HandsInventory : MonoBehaviour {

    // position in air 
    public float horisontalOffset = 2.0f;
    public float distanceFromCamera = 3.0f;

    private InteractableObject leftHandObject;
    private InteractableObject rightHandObject;

    private CombinationManager combinationManager;
    private GameObject interactableObjects;

    private ActionManager actionManager;

    void Start()
    {
        combinationManager = GameObject.Find("GameLogic").GetComponent<CombinationManager>();
        if (combinationManager == null) Debug.LogError("No combination manager found.");

        interactableObjects = GameObject.Find("Interactable Objects");
        if (interactableObjects == null) Debug.LogError("No Interactable Objets object found");

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No Action Manager found.");
    }
	
	void Update () {

        Vector3 leftHandPosition = Camera.main.transform.position +
                Camera.main.transform.forward * distanceFromCamera +
                Camera.main.transform.right * (-horisontalOffset);
        Vector3 rightHandPosition = Camera.main.transform.position +
                Camera.main.transform.forward * distanceFromCamera +
                Camera.main.transform.right * horisontalOffset;

        if (leftHandObject)
        {
            leftHandObject.transform.position = leftHandPosition;
            leftHandObject.InHandUpdate(false);
        }

        if (rightHandObject)
        {
            rightHandObject.transform.position = rightHandPosition;
            rightHandObject.InHandUpdate(true);
        }
        
        if ( Input.GetKeyDown(KeyCode.Q) )
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
        else if ( Input.GetKeyDown(KeyCode.R) )
        {
            string leftName = leftHandObject ? leftHandObject.name : "";
            string rightName = rightHandObject ? rightHandObject.name : "";

            string leftResult, rightResult;
            bool combined = combinationManager.Combine(leftName, rightName, out leftResult, out rightResult);

            if (combined)
            {
                if (leftName != leftResult)
                {
                    Destroy(leftHandObject.gameObject);
                    leftHandObject = null;
                    
                    if (leftResult != "")
                    {
                        GameObject leftObject = CreateObjectByName(leftResult, leftHandPosition);
                        leftHandObject = leftObject.GetComponent<InteractableObject>();
                    } 
                }

                if (rightName != rightResult)
                {
                    Destroy(rightHandObject.gameObject);
                    rightHandObject = null;
                    
                    if (rightResult != "")
                    {
                        GameObject rightObject = CreateObjectByName(rightResult, rightHandPosition);
                        rightHandObject = rightObject.GetComponent<InteractableObject>();
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject cubeObject = Instantiate(Resources.Load<GameObject>("Prefabs\\Cube"),
                            new Vector3(1.0f, 10.0f, 1.0f),
                            Quaternion.identity) as GameObject;
            cubeObject.transform.parent = interactableObjects.transform;
            cubeObject.name = "Cube";

            GameObject sphereObject = Instantiate(Resources.Load<GameObject>("Prefabs\\Sphere"),
                            new Vector3(-1.0f, 10.0f, -1.0f),
                            Quaternion.identity) as GameObject;
            sphereObject.transform.parent = interactableObjects.transform;
            sphereObject.name = "Sphere";
        }


    }

    public bool PickItem(InteractableObject item)
    {
        bool picked = false;
        if ( leftHandObject == null )
        {
            leftHandObject = item;
            picked = true;
        }
        else if ( rightHandObject == null )
        {
            rightHandObject = item;
            picked = true;
        }

        return picked;
    }

    private GameObject CreateObjectByName(string name, Vector3 position)
    {
        GameObject newObject = Instantiate(Resources.Load<GameObject>("Prefabs\\" + name),
                            position, Quaternion.identity) as GameObject;

        newObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        newObject.transform.parent = interactableObjects.transform;
        newObject.name = name;

        return newObject;
    }

}
