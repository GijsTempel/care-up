using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {
    
    [Serializable]
    public class KeyPreferences
    {
        public InputKey LeftDropKey  = new InputKey(new KeyBoardKey(KeyCode.Q, KeyCode.LeftShift), null,
                                                    new ControllerAxisKey("ControllerDrop", -1));
        public InputKey RightDropKey = new InputKey(new KeyBoardKey(KeyCode.E, KeyCode.LeftShift), null,
                                                    new ControllerAxisKey("ControllerDrop", 1));
        public InputKey LeftUseKey   = new InputKey(new KeyBoardKey(KeyCode.Q),
                                                    new ControllerKey(KeyCode.Joystick1Button2, KeyCode.Joystick1Button4));
        public InputKey RightUseKey  = new InputKey(new KeyBoardKey(KeyCode.E),
                                                    new ControllerKey(KeyCode.Joystick1Button1, KeyCode.Joystick1Button5));
        public InputKey CombineKey   = new InputKey(new KeyBoardKey(KeyCode.R),
                                                    new ControllerKey(KeyCode.Joystick1Button3));
        public InputKey GetHintKey   = new InputKey(new KeyBoardKey(KeyCode.Space),
                                                    new ControllerKey(KeyCode.Joystick1Button6, KeyCode.Joystick1Button8));
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
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Joystick1Button0);
    }
}
