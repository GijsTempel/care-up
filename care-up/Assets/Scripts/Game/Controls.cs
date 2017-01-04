using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {
    
    [Serializable]
    public class InputKey
    {
        public KeyCode controllerKey;
        public KeyCode altControllerKey;
        public KeyCode mainKey;
        public KeyCode helpKey;

        public InputKey(KeyCode controller, KeyCode main, KeyCode alt = KeyCode.None, KeyCode help = KeyCode.None)
        {
            controllerKey = controller;
            altControllerKey = alt;
            mainKey = main;
            helpKey = help;
        }

        public bool Pressed()
        {
            bool pressed = Input.GetKeyDown(mainKey);

            if (helpKey != KeyCode.None)
            {
                pressed = pressed && Input.GetKey(helpKey);
            }
            
            pressed = Input.GetKeyDown(controllerKey) || pressed;

            if (altControllerKey != KeyCode.None)
            {
                pressed = Input.GetKeyDown(altControllerKey) || pressed;
            }
        
            if (pressed)
            {
                if (!Controls.keyUsed)
                {
                    Controls.keyUsed = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }
    }

    [Serializable]
    public class KeyPreferences
    {
        public InputKey LeftDropKey = new InputKey(KeyCode.Joystick1Button6, KeyCode.Q, KeyCode.None, KeyCode.LeftShift);
        public InputKey RightDropKey = new InputKey(KeyCode.Joystick1Button7, KeyCode.E, KeyCode.None, KeyCode.LeftShift);
        public InputKey LeftUseKey = new InputKey(KeyCode.Joystick1Button2, KeyCode.Q, KeyCode.Joystick1Button4);
        public InputKey RightUseKey = new InputKey(KeyCode.Joystick1Button1, KeyCode.E, KeyCode.Joystick1Button5);
        public InputKey CombineKey = new InputKey(KeyCode.Joystick1Button3, KeyCode.R);
        public InputKey GetHintKey = new InputKey(KeyCode.Joystick1Button6, KeyCode.Space, KeyCode.Joystick1Button8);
    };

    public KeyPreferences keyPreferences = new KeyPreferences();
    public float interactionDistance = 5.0f;

    static private bool keyUsed = false;

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
