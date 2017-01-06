using UnityEngine;
using System.Collections;

public class CameraMode : MonoBehaviour {

	public enum Mode
    {
        Free,
        ObjectPreview,
        SelectionDialogue
    };
    
    private Mode currentMode = Mode.Free;

    private ExaminableObject selectedObject;

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
        if (controls.MouseClicked() && currentMode == Mode.Free)
        {
            if (controls.SelectedObject && controls.CanInteract)
            {
                selectedObject = controls.SelectedObject.GetComponent<ExaminableObject>();
                if (selectedObject != null) // if there is a component
                {
                    Debug.Log(controls.SelectedObject.name);
                    ToggleCameraMode(Mode.ObjectPreview);
                    selectedObject.ToggleViewMode(true);
                    selectedObject.OnExamine();
                    controls.ResetObject();
                }
            }
        }

        if (currentMode == Mode.ObjectPreview)
        {
            if (controls.keyPreferences.LeftUseKey.Pressed())
            {
                ToggleCameraMode(Mode.Free);

                selectedObject.ToggleViewMode(false);
                selectedObject = null;
            }
            else if (controls.keyPreferences.RightUseKey.Pressed())
            {
                PickableObject pickableObject = selectedObject.GetComponent<PickableObject>();
                if (pickableObject != null)
                {
                    ToggleCameraMode(Mode.Free);
                    pickableObject.GetComponent<ExaminableObject>().ToggleViewMode(false);
                    inventory.PickItem(pickableObject);
                    selectedObject = null;
                }
            }
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

    public void ToggleCameraMode(Mode mode)
    {
        if (currentMode == Mode.Free && mode == Mode.ObjectPreview)
        {
            playerScript.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            blur.enabled = true;
        }
        else if (currentMode == Mode.ObjectPreview && mode == Mode.Free)
        {
            playerScript.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            blur.enabled = false;
        }
        else if (currentMode == Mode.Free && mode == Mode.SelectionDialogue)
        {
            playerScript.enabled = false;
        }
        else if (currentMode == Mode.SelectionDialogue && mode == Mode.Free)
        {
            playerScript.enabled = true;
        }

        currentMode = mode;
        controls.ResetObject();
    }

    void OnGUI()
    {
        if ( currentMode == Mode.ObjectPreview )
        {
            string putKey = "Q";
            string pickKey = "E";

            string text = (selectedObject.GetComponent<PickableObject>() != null) ?
                "Press " + putKey + " to put down, Press " + pickKey + " to pick up" :
                "Press " + putKey + " to put down";

            GUIStyle style = GUI.skin.GetStyle("Label");
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 40;
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(0, 4*Screen.height/5, Screen.width, Screen.height/5), 
                text, style);
        }
    }
}
