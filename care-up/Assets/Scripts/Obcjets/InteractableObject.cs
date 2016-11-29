using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class InteractableObject : MonoBehaviour {

    public string description;

    private Renderer rend;
    static private Shader onMouseOverShader;
    static private Shader onMouseExitShader;

    static protected CameraMode cameraMode;
    static protected Controls controls;
    static protected ActionManager actionManager;
    static protected GameObject itemDescription;

    protected virtual void Start()
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

        if (actionManager == null)
        {
            actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
            if (actionManager == null) Debug.LogError("No action manager found");
        }

        if (itemDescription == null)
        {
            itemDescription = Instantiate(Resources.Load<GameObject>("Prefabs/ItemDescription"),
                Vector3.zero, Quaternion.identity) as GameObject;
            if (itemDescription == null)
            {
                Debug.LogError("No item description prefab found");
            } 
            else
            {
                itemDescription.name = "ItemDescription";
                itemDescription.SetActive(false);
            }
        }
    }

    protected virtual void Update() {
        
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject == gameObject)
            {
                if (rend.material.shader == onMouseExitShader)
                {
                    rend.material.shader = onMouseOverShader;

                    itemDescription.GetComponentInChildren<Text>().text = (description == "") ? name : description;
                    Transform icons = itemDescription.transform.GetChild(0);
                    if (gameObject.GetComponent<UsableObject>() != null)
                    {
                        icons.FindChild("UseIcon").gameObject.SetActive(true);
                    }
                    if (gameObject.GetComponent<PersonObject>() != null)
                    {
                        icons.FindChild("TalkIcon").gameObject.SetActive(true);
                    }
                    if (gameObject.GetComponent<PickableObject>() != null)
                    {
                        icons.FindChild("PickIcon").gameObject.SetActive(true);
                    }
                    if (gameObject.GetComponent<ExaminableObject>() != null)
                    {
                        icons.FindChild("ExamIcon").gameObject.SetActive(true);
                    }
                    itemDescription.SetActive(true);
                }
                else
                {
                    itemDescription.transform.position = transform.position - Camera.main.transform.forward;
                    itemDescription.transform.rotation = Camera.main.transform.rotation;
                }
            }
            else
            {
                if (rend.material.shader != onMouseExitShader)
                {
                    rend.material.shader = onMouseExitShader;
                    itemDescription.SetActive(false);
                    Transform icons = itemDescription.transform.GetChild(0);
                    for (int i = 0; i < icons.childCount; ++i)
                    {
                        icons.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    protected virtual void ResetShader()
    {
        rend.material.shader = onMouseExitShader;
    }

    protected bool ViewModeActive()
    {
        return cameraMode.CurrentMode == CameraMode.Mode.ObjectPreview;
    }
}
