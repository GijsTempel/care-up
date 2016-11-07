using UnityEngine;
using System.Collections;

public class CameraMode : MonoBehaviour {

	public enum Mode
    {
        Free,
        ObjectPreview
    };
    
    private Mode currentMode;

    private PickableObject selectedObject;
    private Vector3 savedPosition;
    private Quaternion savedRotation;

    private Controls controls;
    private PlayerMovement playerScript;
    private HandsInventory inventory;
    private UnityStandardAssets.ImageEffects.BlurOptimized blur;

    public Mode CurrentMode
    {
        get { return currentMode; }
    }

    void Start()
    {
        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        if (controls == null) Debug.LogError("No controls script found");

        playerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        if (playerScript == null) Debug.LogError("No Player script found");

        inventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        if (inventory == null) Debug.LogError("No inventory script found");

        blur = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
        if (blur == null) Debug.Log("No Blur Attached to main camera.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentMode == Mode.Free)
        {
            if (controls.SelectedObject)
            {
                selectedObject = controls.SelectedObject.GetComponent<PickableObject>();
                if (selectedObject != null) // if there is a component
                {
                    currentMode = Mode.ObjectPreview;
                    playerScript.enabled = false;

                    selectedObject.ToggleViewMode(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    blur.enabled = true;
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

            blur.enabled = false;
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

            blur.enabled = false;
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
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(0, 4*Screen.height/5, Screen.width, Screen.height/5), 
                "Press Q to put down, Press E to pick up", style);
        }
    }
}
