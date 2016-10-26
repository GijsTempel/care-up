using UnityEngine;
using System.Collections;

public class CameraMode : MonoBehaviour {

	public enum Mode
    {
        Free,
        ObjectPreview
    };

    private Mode currentMode;

    private InteractableObject selectedObject;
    private Vector3 savedPosition;
    private Quaternion savedRotation;

    private PlayerMovement playerScript;
    private HandsInventory inventory;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        if (playerScript == null) Debug.LogError("No Player script found");

        inventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        if (inventory == null) Debug.LogError("No inventory script found");
    }

    void Update()
    {
        if ( Input.GetMouseButtonDown(0) && currentMode == Mode.Free )
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            if ( Physics.Raycast(ray, out hit) )
            {
                selectedObject = hit.transform.GetComponent<InteractableObject>();
                if ( selectedObject != null ) // if there is a component
                {
                    currentMode = Mode.ObjectPreview;
                    playerScript.enabled = false;

                    selectedObject.ToggleViewMode(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentMode == Mode.ObjectPreview)
        {
            currentMode = Mode.Free;
            playerScript.enabled = true;

            selectedObject.ToggleViewMode(false);
            selectedObject = null;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if ( Input.GetKeyDown(KeyCode.E) && currentMode == Mode.ObjectPreview )
        {
            currentMode = Mode.Free;
            playerScript.enabled = true;

            if ( !inventory.PickItem(selectedObject) )
            {
                selectedObject.ToggleViewMode(false);
            }
            selectedObject = null;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if ( currentMode == Mode.ObjectPreview )
        {
            if ( selectedObject == null )
            {
                Debug.LogError("No object selected for View Mode");
            }
            else
            {
                selectedObject.ViewModeUpdate();
            }
        }
    }

    void OnGUI()
    {
        if ( currentMode == Mode.ObjectPreview )
        {
            GUIStyle style = GUI.skin.GetStyle("Label");
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 40;
            GUI.Label(new Rect(0, 4*Screen.height/5, Screen.width, Screen.height/5), 
                "Press Q to put down, Press E to pick up", style);
        }
    }
}
