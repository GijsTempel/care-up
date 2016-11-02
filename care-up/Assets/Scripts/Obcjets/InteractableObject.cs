using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class InteractableObject : MonoBehaviour {

    [Serializable]
    public class ViewSettings
    {
        public float distanceFromCamera = 4.0f;
        public float rotationSensetivity = 90.0f;
    };

    public ViewSettings viewSettings = new ViewSettings();
    public Vector3 rotationInHand;

    private bool viewMode = false;
    private Vector3 savedPosition;
    private Quaternion savedRotation;

    private Renderer rend;
    static private Shader onMouseOverShader;
    static private Shader onMouseExitShader;

    static private CameraMode cameraMode;
    static private Controls controls;

    private List<Vector3> framePositions = new List<Vector3>();

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (onMouseOverShader == null)
        {
            onMouseOverShader = Shader.Find("Outlined/Silhouetted Diffuse");
        }

        if (onMouseExitShader == null)
        {
            onMouseExitShader = Shader.Find("Standard");
        }

        if (cameraMode == null)
        {
            cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
            if (cameraMode == null) Debug.LogError("No Camera Mode found.");
        }

        if (controls == null)
        {
            controls = GameObject.Find("GameLogic").GetComponent<Controls>();
            if (controls == null) Debug.LogError("No Controls found");
        }
    }

    void Update()
    {
        framePositions.Add(transform.position);

        if (framePositions.Count > 15)
        {
            framePositions.RemoveAt(0);
        }

        if ( !viewMode && cameraMode.CurrentMode == CameraMode.Mode.Free )
        {
            if (controls.SelectedObject == gameObject)
            {
                if (rend.material.shader == onMouseExitShader)
                {
                    rend.material.shader = onMouseOverShader;
                }
            }
            else
            {
                if (rend.material.shader != onMouseExitShader)
                {
                    rend.material.shader = onMouseExitShader;
                }
            }
        }
    }

    public void OnCrosshairOver()
    {
        if (!viewMode && cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            rend.material.shader = onMouseOverShader;
        }
    }

    public void OnCrosshairExit()
    {
        if (!viewMode && cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            rend.material.shader = onMouseExitShader;
        }
    }

    public void ToggleViewMode(bool value)
    {
        viewMode = value;
        if ( viewMode )
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
            if ( viewMode )
            {
                body.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                body.constraints = RigidbodyConstraints.None;
            }
        }

        rend.material.shader = onMouseExitShader;

        // making draw object on top of everything
        if ( viewMode )
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

    public void InHandUpdate(bool right)
    {
        transform.localRotation = Quaternion.Euler(rotationInHand + Camera.main.transform.rotation.eulerAngles);
    }

    public void Drop()
    {
        viewMode = false;
        gameObject.layer = 0;
        GetComponent<Collider>().enabled = true;

        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.constraints = RigidbodyConstraints.None;

            if (framePositions.Count > 0)
            {
                Vector3 deltaPosition = framePositions[framePositions.Count - 1] - framePositions[0];
                deltaPosition = deltaPosition * 3 / Time.fixedDeltaTime;
                body.AddForce(deltaPosition);
            }
        }
    }
}
