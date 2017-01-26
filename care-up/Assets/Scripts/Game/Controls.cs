using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {
    
    [Serializable]
    public class KeyPreferences
    {
        // A, X
        public InputKey mouseClickKey = new InputKey(null, new ControllerKey(KeyCode.Joystick1Button0), null, true);
        // LeftTrigger
        public InputKey LeftDropKey  = new InputKey(new KeyBoardKey(KeyCode.Q, KeyCode.LeftShift), null,
                                                    new ControllerAxisKey("ControllerLeftTrigger"));
        // right trigger
        public InputKey RightDropKey = new InputKey(new KeyBoardKey(KeyCode.E, KeyCode.LeftShift), null,
                                                    new ControllerAxisKey("ControllerRightTrigger"));
        // X, square, left bumper
        public InputKey LeftUseKey   = new InputKey(new KeyBoardKey(KeyCode.Q),
                                                    new ControllerKey(KeyCode.Joystick1Button2, KeyCode.Joystick1Button4));
        // B, circle, right bumper
        public InputKey RightUseKey  = new InputKey(new KeyBoardKey(KeyCode.E),
                                                    new ControllerKey(KeyCode.Joystick1Button1, KeyCode.Joystick1Button5));
        // B, circle
        public InputKey closeObjectView = new InputKey(new KeyBoardKey(KeyCode.Q),
                                                    new ControllerKey(KeyCode.Joystick1Button1));
        // X, square
        public InputKey pickObjectView = new InputKey(new KeyBoardKey(KeyCode.E),
                                                    new ControllerKey(KeyCode.Joystick1Button2));
        // Y, triangle
        public InputKey CombineKey   = new InputKey(new KeyBoardKey(KeyCode.R),
                                                    new ControllerKey(KeyCode.Joystick1Button3));
        // back, select
        public InputKey GetHintKey   = new InputKey(new KeyBoardKey(KeyCode.Space),
                                                    new ControllerKey(KeyCode.Joystick1Button6, KeyCode.Joystick1Button8));

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
    };

    public KeyPreferences keyPreferences = new KeyPreferences();
    public float interactionDistance = 5.0f;

    static public bool keyUsed = false;

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
   
	void LateUpdate () {
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

        keyUsed = false;
    }

    public bool MouseClicked()
    {
        if (keyPreferences.mouseClickLocked)
        {
            return false;
        }

        return Input.GetMouseButtonDown(0) || keyPreferences.mouseClickKey.Pressed();
    }

    public bool RightMouseClicked()
    {
        return Input.GetMouseButtonDown(1) || keyPreferences.closeObjectView.Pressed();
    }
}
