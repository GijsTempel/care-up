﻿using UnityEngine;
using System.Collections;
using System;

public class ExaminableObject : InteractableObject {

    [Serializable]
    public class ViewSettings
    {
        public float distanceFromCamera = 4.0f;
        public float rotationSensetivity = 90.0f;
    };

    public ViewSettings viewSettings = new ViewSettings();
    public string state = "good";
    
    private bool viewMode = false;
    private Vector3 savedPosition;
    private Quaternion savedRotation;

    public void OnExamine()
    {
        actionManager.OnExamineAction(name, state);
    }

    public void ToggleViewMode(bool value)
    {
        viewMode = value;
        if (viewMode)
        {
            savedPosition = transform.position;
            savedRotation = transform.localRotation;
        }
        else
        {
            transform.position = savedPosition;
            transform.localRotation = savedRotation;
        }

        GetComponent<Collider>().enabled = !viewMode;
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            if (viewMode)
            {
                body.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                body.constraints = RigidbodyConstraints.None;
            }
        }

        ResetShader();

        // making draw object on top of everything
        if (viewMode)
        {
            // ObjectView layer drawn only on OverlayCamera
            gameObject.layer = 8;
        }
        else
        {
            // default
            gameObject.layer = 0;
        }
    }

    public void ViewModeUpdate()
    {
        transform.position = Camera.main.transform.position
            + Camera.main.transform.forward * viewSettings.distanceFromCamera;

        if (Input.GetMouseButton(0))
        {
            float yRotation = Input.GetAxis("Mouse X") * viewSettings.rotationSensetivity;
            float xRotation = Input.GetAxis("Mouse Y") * viewSettings.rotationSensetivity;

            transform.Rotate(Camera.main.transform.up, -yRotation * Mathf.Deg2Rad, Space.World);
            transform.Rotate(Camera.main.transform.right, xRotation * Mathf.Deg2Rad, Space.World);
        }
    }
}

