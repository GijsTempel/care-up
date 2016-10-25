using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class InteractableObject : MonoBehaviour {

    public string objectId;

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

    private List<Vector3> framePositions = new List<Vector3>();

	void Start () {

        rend = GetComponent<Renderer>();

        if ( onMouseOverShader == null )
        {
            //onMouseOverShader = Shader.Find("Unlit/Color");
            onMouseOverShader = Shader.Find("Outlined/Silhouetted Diffuse");
        }

        if ( onMouseExitShader == null )
        {
            onMouseExitShader = Shader.Find("Standard");
        }

	}

    void Update()
    {
        framePositions.Add(transform.position);

        if (framePositions.Count > 15)
        {
            framePositions.RemoveAt(0);
        }

    }

    void OnMouseOver()
    {
        if (!viewMode)
        {
            rend.material.shader = onMouseOverShader;
        }
    }

    void OnMouseExit()
    {
        if (!viewMode)
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

        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            if ( viewMode )
            {
                body.constraints = RigidbodyConstraints.FreezePosition;
            }
            else
            {
                body.constraints = RigidbodyConstraints.None;
            }
        }

        rend.material.shader = onMouseExitShader;
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
