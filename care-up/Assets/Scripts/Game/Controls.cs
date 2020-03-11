using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles controls of the player
/// </summary>
public class Controls : MonoBehaviour
{
    [Serializable]
    public class KeyPreferences
    {
        // A, X
        public InputKey mouseClickKey = new InputKey(null, new ControllerKey(KeyCode.Joystick1Button0), null, true);
        // LeftTrigger
        public InputKey LeftDropKey = new InputKey(new KeyBoardKey(KeyCode.Q, KeyCode.LeftShift), null,
                                                    new ControllerAxisKey("ControllerLeftTrigger"));
        // right trigger
        public InputKey RightDropKey = new InputKey(new KeyBoardKey(KeyCode.E, KeyCode.LeftShift), null,
                                                    new ControllerAxisKey("ControllerRightTrigger"));
        // X, square, left bumper
        public InputKey LeftUseKey = new InputKey(new KeyBoardKey(KeyCode.Q),
                                                    new ControllerKey(KeyCode.Joystick1Button2, KeyCode.Joystick1Button4));
        // B, circle, right bumper
        public InputKey RightUseKey = new InputKey(new KeyBoardKey(KeyCode.E),
                                                    new ControllerKey(KeyCode.Joystick1Button1, KeyCode.Joystick1Button5));
        // B, circle
        public InputKey closeObjectView = new InputKey(new KeyBoardKey(KeyCode.Q),
                                                    new ControllerKey(KeyCode.Joystick1Button1));
        // X, square
        public InputKey pickObjectView = new InputKey(new KeyBoardKey(KeyCode.E),
                                                    new ControllerKey(KeyCode.Joystick1Button2));
        // Y, triangle
        public InputKey CombineKey = new InputKey(new KeyBoardKey(KeyCode.R),
                                                    new ControllerKey(KeyCode.Joystick1Button3));
        // back, select
        //public InputKey GetHintKey   = new InputKey(new KeyBoardKey(KeyCode.Space),
        //                                            new ControllerKey(KeyCode.Joystick1Button6, KeyCode.Joystick1Button8));
        public InputKey GetHintKey = new InputKey();

        public InputKey Teleport = new InputKey(new KeyBoardKey(KeyCode.Space));

        public bool mouseClickLocked = false;
        public void SetAllLocked(bool value)
        {
            mouseClickLocked = value;
            mouseClickKey.locked = value;
            LeftDropKey.locked = value;
            RightDropKey.locked = value;
            LeftUseKey.locked = value;
            RightUseKey.locked = value;
            closeObjectView.locked = value;
            pickObjectView.locked = value;
            CombineKey.locked = value;
            GetHintKey.locked = value;
        }

        private bool[] locks;
        private bool toggleFlag = false;
        public void ToggleLock()
        {
            if (toggleFlag)
            {
                mouseClickLocked = locks[0];
                mouseClickKey.locked = locks[1];
                LeftDropKey.locked = locks[2];
                RightDropKey.locked = locks[3];
                LeftUseKey.locked = locks[4];
                RightUseKey.locked = locks[5];
                closeObjectView.locked = locks[6];
                pickObjectView.locked = locks[7];
                CombineKey.locked = locks[8];
                GetHintKey.locked = locks[9];
                locks = null;
            }
            else
            {
                locks = new bool[10];
                locks[0] = mouseClickLocked;
                locks[1] = mouseClickKey.locked;
                locks[2] = LeftDropKey.locked;
                locks[3] = RightDropKey.locked;
                locks[4] = LeftUseKey.locked;
                locks[5] = RightUseKey.locked;
                locks[6] = closeObjectView.locked;
                locks[7] = pickObjectView.locked;
                locks[8] = CombineKey.locked;
                locks[9] = GetHintKey.locked;
                SetAllLocked(true);
            }

            toggleFlag = !toggleFlag;
        }
    };

    public KeyPreferences keyPreferences = new KeyPreferences();
    public float interactionDistance;

#if UNITY_EDITOR
    public bool devInteractionDisplay = false;
#endif

    static public bool keyUsed = false;

    public GameObject selectedObject;
    public bool canInteract;

    private bool touchEnded = false;

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
        canInteract = false;
    }

    PlayerPrefsManager prefs;

    private void Start()
    {
        if (GameObject.Find("Preferences") != null)
        {
            prefs = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        }
    }

    /// <summary>
    /// Sets selectedObject to an object player is aimed at atm.
    /// Sets canInteract based of distance to aimed object.
    /// </summary>
	void LateUpdate()
    {
        // raycast only in this script
        Camera cam = null;
        foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
        {
            if (c.transform.parent != null)
            {
                if (c.transform.parent.name == "Head")
                    cam = c;
            }
        }
        if (cam == null)
            return;

        Vector3 screenPosition = (Input.touchCount > 0) ?
            new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y) :
            Input.mousePosition;
        Ray ray = ((prefs == null) ? false : prefs.VR) ?
            new Ray(cam.transform.position, cam.transform.forward)
            : cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.transform.gameObject;
            //canInteract = (hit.distance <= interactionDistance) ? true : false;
            canInteract = (interactionDistance == 0.0f) ? true : Vector2.Distance(
                new Vector2(cam.transform.position.x, cam.transform.position.z),
                new Vector2(hit.transform.position.x, hit.transform.position.z))
                <= interactionDistance ? true : false;
        }
        else
        {
            ResetObject();
        }

        UpdateUIDetection();

        keyUsed = false;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchEnded = true;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchEnded = false;
        }

        if (touchEnded)
        {
            ResetObject();
            if (GameObject.Find("ItemDescription") != null)
            {
                GameObject.Find("ItemDescription").SetActive(false);
            }
        }

#if UNITY_EDITOR
        if (devInteractionDisplay)
        {
            Vector3 origin = cam.transform.position;

            Vector3 from = new Vector3(0.0f, -1.0f, interactionDistance);
            for (float i = -1.0f; i < 1.0f; i += 0.1f)
            {
                for (float j = 0; j < 2.0f * Math.PI; j += 0.1f)
                {
                    float x = interactionDistance * Mathf.Sin(j);
                    float y = interactionDistance * Mathf.Cos(j);

                    Vector3 to = new Vector3(x, i, y);
                    Debug.DrawLine(origin + from, origin + to);
                    from = to;
                }
            }
        }
#endif
    }

    private void UpdateUIDetection()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            if (Input.touchCount > 0)
            {
                if (UnityEngine.EventSystems.EventSystem.current.
                    IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    ResetObject();
                }
            }
            else
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    ResetObject();
                }
            }
        }
    }

    /// <summary>
    /// Checks if "LeftMouse" clicked, including alternatives for gamepads.
    /// </summary>
    /// <returns>True if clicked.</returns>
    public bool MouseClicked()
    {
        if (keyPreferences.mouseClickLocked)
        {
            return false;
        }

        bool result = (Input.touchCount > 0) ?
            Input.GetTouch(0).phase == TouchPhase.Began
            : (Input.GetMouseButtonDown(0) || keyPreferences.mouseClickKey.Pressed());

        return result;
    }

    /// <summary>
    /// Checks if "RightMouse" clicked, including alternatives for gamepads.
    /// </summary>
    /// <returns>True if clicked.</returns>
    public bool RightMouseClicked()
    {
        return Input.GetMouseButtonDown(1) || keyPreferences.closeObjectView.Pressed();
    }

    public static bool MouseReleased()
    {
        bool result = (Input.touchCount > 0) ?
            Input.GetTouch(0).phase == TouchPhase.Ended : (Input.GetMouseButtonUp(0));

        return result;
    }
}
