using UnityEngine;
using System.Collections;

public class HandsInventory : MonoBehaviour {

    // position in air 
    public float horisontalOffset = 2.0f;
    public float distanceFromCamera = 3.0f;

    private PickableObject leftHandObject;
    private PickableObject rightHandObject;

    private CombinationManager combinationManager;
    private GameObject interactableObjects;
    private Controls controls;
    private CameraMode cameraMode;

    private ActionManager actionManager;

    public GameObject LeftHandObject
    {
        get { return leftHandObject ? leftHandObject.gameObject : null; }
    }

    public GameObject RightHandObject
    {
        get { return rightHandObject ? rightHandObject.gameObject : null; }
    }

    void Start()
    {
        combinationManager = GameObject.Find("GameLogic").GetComponent<CombinationManager>();
        if (combinationManager == null) Debug.LogError("No combination manager found.");

        interactableObjects = GameObject.Find("Interactable Objects");
        if (interactableObjects == null) Debug.LogError("No Interactable Objets object found");

        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No Action Manager found.");

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        if (controls == null) Debug.LogError("No controls found");

        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (cameraMode == null) Debug.LogError("No camera mode found");
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
        
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.keyPreferences.LeftDropKey.Pressed())
            {
                if (leftHandObject)
                {
                    leftHandObject.Drop();
                    leftHandObject = null;
                }
            }

            if (controls.keyPreferences.RightDropKey.Pressed())
            {
                if (rightHandObject)
                {
                    rightHandObject.Drop();
                    rightHandObject = null;
                }
            }

            if (controls.keyPreferences.CombineKey.Pressed())
            {
                string leftName = leftHandObject ? leftHandObject.name : "";
                string rightName = rightHandObject ? rightHandObject.name : "";

                string[] currentObjects = actionManager.CurrentCombineObjects;
                bool combineAllowed = (currentObjects[0] == leftName && currentObjects[1] == rightName)
                    || (currentObjects[0] == rightName && currentObjects[1] == leftName);

                string leftResult, rightResult;
                bool combined = combinationManager.Combine(leftName, rightName, out leftResult, out rightResult);
                
                if (combined && combineAllowed)
                {
                    if (leftName != leftResult)
                    {
                        if (leftHandObject != null)
                        {
                            Destroy(leftHandObject.gameObject);
                            leftHandObject = null;
                        }

                        if (leftResult != "")
                        {
                            GameObject leftObject = CreateObjectByName(leftResult, leftHandPosition);
                            leftHandObject = leftObject.GetComponent<PickableObject>();
                        }
                    }

                    if (rightName != rightResult)
                    {
                        if (rightHandObject != null)
                        {
                            Destroy(rightHandObject.gameObject);
                            rightHandObject = null;
                        }

                        if (rightResult != "")
                        {
                            GameObject rightObject = CreateObjectByName(rightResult, rightHandPosition);
                            rightHandObject = rightObject.GetComponent<PickableObject>();
                        }
                    }
                }
            }

            if (controls.keyPreferences.LeftUseKey.Pressed())
            {
                if (leftHandObject != null)
                {
                    leftHandObject.Use();
                }
            }

            if (controls.keyPreferences.RightUseKey.Pressed())
            {
                if (rightHandObject)
                {
                    rightHandObject.Use();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
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

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject != null)
            {
                PickableObject item = controls.SelectedObject.GetComponent<PickableObject>();
                if (item != null)
                {
                    PickItem(item);
                    controls.ResetObject();
                }
            }
        }
    }

    public bool PickItem(PickableObject item, string hand = "")
    {
        bool picked = false;
        if (hand == "")
        {
            if (leftHandObject == null)
            {
                leftHandObject = item;
                picked = true;
            }
            else if (rightHandObject == null)
            {
                rightHandObject = item;
                picked = true;
            }
        }
        else if (hand == "left")
        {
            if (leftHandObject == null)
            {
                leftHandObject = item;
                picked = true;
            }
        }
        else if (hand == "right")
        {
            if (rightHandObject == null)
            {
                rightHandObject = item;
                picked = true;
            }
        }

        if (picked)
        {
            if (leftHandObject)
            {
                leftHandObject.GetComponent<Rigidbody>().useGravity = false;
                leftHandObject.GetComponent<Collider>().enabled = false;
            }
            if (rightHandObject)
            {
                rightHandObject.GetComponent<Rigidbody>().useGravity = false;
                rightHandObject.GetComponent<Collider>().enabled = false;
            }
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
