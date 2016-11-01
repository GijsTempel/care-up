using UnityEngine;
using System.Collections;

public class UsableObject : MonoBehaviour {

    private Renderer rend;
    static private Shader onMouseOverShader;
    static private Shader onMouseExitShader;
    static private CameraMode cameraMode;

    static private ActionManager actionManager;

	void Start () {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found");

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
    }

    void OnMouseOver()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            rend.material.shader = onMouseOverShader;
        }
    }

    void OnMouseExit()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            rend.material.shader = onMouseExitShader;
        }
    }

    public void Use()
    {
        actionManager.OnUseAction(gameObject.name);
    }
}
