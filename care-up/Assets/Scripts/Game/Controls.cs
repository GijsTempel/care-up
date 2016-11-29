using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {
    
    [Serializable]
    public class InputKey
    {
        public KeyCode mainKey;
        public KeyCode helpKey;

        public InputKey(KeyCode main, KeyCode help = KeyCode.None)
        {
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
        public InputKey LeftDropKey = new InputKey(KeyCode.Q, KeyCode.LeftShift);
        public InputKey RightDropKey = new InputKey(KeyCode.E, KeyCode.LeftShift);
        public InputKey LeftUseKey = new InputKey(KeyCode.Q);
        public InputKey RightUseKey = new InputKey(KeyCode.E);
        public InputKey CombineKey = new InputKey(KeyCode.R);
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
   
	void Update () {

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
    }
    
    void LateUpdate()
    {
        keyUsed = false;
    }

}
