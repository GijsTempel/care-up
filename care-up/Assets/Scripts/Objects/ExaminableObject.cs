using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Object that can be examined - opened in object preview.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class ExaminableObject : InteractableObject {

    [HideInInspector]
    public bool tutorial_picked = false;
    [HideInInspector]
    public bool tutorial_closed = false;
    [HideInInspector]
    public bool tutorial_examined = false;

    private Tutorial_UseOn tutorialUseOn;

    public Vector3 examineRotation;
    public bool audioExamine = false;
    public bool animationExamine = false;

    [Serializable]
    public class ViewSettings
    {
        public float distanceFromCamera = 2.0f;
        public float rotationSensetivity = 90.0f;
    };

    public ViewSettings viewSettings = new ViewSettings();
    public string state = "good";
    
    private bool viewMode = false;
    
    void Awake () {
        tutorialUseOn = GameObject.FindObjectOfType<Tutorial_UseOn> ();
    }

    /// <summary>
    /// Action examining, handles special cases aswell as switching camera modes.
    /// </summary>
    public void OnExamine()
    {
        if (!audioExamine)
        {
            tutorial_examined = true;
            ToggleViewMode (true);
            cameraMode.ToggleCameraMode(CameraMode.Mode.ObjectPreview);
            actionManager.OnExamineAction(name, state);
        }
        else
        {
            // both hands are part of rig and should count as Hand
            if (name == "LeftForeArm" || name == "RightForeArm")
            {
                Narrator.PlaySound("ExamineArm");
                actionManager.OnExamineAction("Hand", state);
            }
            else
            {
                Narrator.PlaySound("ExamineSmth?");
                actionManager.OnExamineAction(name, state);
            }
        }
    }

    /// <summary>
    /// Toggles object mode.
    /// </summary>
    /// <param name="value">True - Preview, False - closed</param>
    public void ToggleViewMode(bool value)
    {
        viewMode = value;
        if (viewMode)
        {
            tutorial_picked = true;
            if (!animationExamine)
            {
                SavePosition();
                transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(examineRotation);
            }
            else
            {
                PlayerAnimationManager.PlayAnimation("closeup_" +
                (inventory.LeftHandObject == gameObject ? "left" : "right"));
            }
        }
        else
        {
            tutorial_examined = false;
            tutorial_closed = true;
            if (tutorialUseOn != null) {
                tutorialUseOn.stopExamined = true;
            }
            if (!animationExamine)
            {
                LoadPosition ();

                if (inventory.LeftHandObject == gameObject ||
                    inventory.RightHandObject == gameObject)
                {
                    transform.localEulerAngles = Vector3.zero;
                }
            }
            else
            {
                PlayerAnimationManager.PlayAnimation("faraway_" +
                (inventory.LeftHandObject == gameObject ? "left" : "right"));
            }
        }

        if (animationExamine)
        {
            Reset();
            return;
        }

        // disable/endable physics
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

        Reset();

        // making draw object on top of everything
        if (viewMode)
        {
            // ObjectView layer drawn only on OverlayCamera
            foreach (Transform t in transform.GetComponentsInChildren<Transform>())
            {
                t.gameObject.layer = 8;
            }
        }
        else
        {
            // default
            foreach (Transform t in transform.GetComponentsInChildren<Transform>())
            {
                t.gameObject.layer = 0;
            }
        }
    }

    /// <summary>
    /// Handles position/rotation while in preview mode
    /// </summary>
    public void ViewModeUpdate()
    {
        if (animationExamine)
            return;

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

