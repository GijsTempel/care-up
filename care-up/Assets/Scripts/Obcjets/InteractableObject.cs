using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class InteractableObject : MonoBehaviour {

    private Renderer rend;
    static private Shader onMouseOverShader;
    static private Shader onMouseExitShader;

    static private CameraMode cameraMode;
    static private Controls controls;
    
    protected virtual void Start () {
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
	
	protected virtual void Update () {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
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

    protected virtual void ResetShader()
    {
        rend.material.shader = onMouseExitShader;
    }
}
