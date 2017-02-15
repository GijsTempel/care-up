using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class InteractableObject : MonoBehaviour {

    public string description;
    public bool muplipleMesh = false;

    protected Renderer rend;
    protected Shader onMouseOverShader;

    static private Shader onMouseOverShaderSihlouette;
    static private Shader onMouseOverShadeOutline;
    static private Shader onMouseExitShader;

    protected bool positionSaved = false;
    protected Vector3 savedPosition;
    protected Quaternion savedRotation;

    static protected CameraMode cameraMode;
    static protected Controls controls;
    static protected ActionManager actionManager;
    static protected GameObject itemDescription;

    protected virtual void Start()
    {
        rend = GetComponent<Renderer>();

        if (onMouseOverShaderSihlouette == null)
        {
            onMouseOverShaderSihlouette = Shader.Find("Outlined/Silhouetted Diffuse");
        }

        if (onMouseOverShadeOutline == null)
        {
            onMouseOverShadeOutline = Shader.Find("Outlined/Diffuse");
        }

        onMouseOverShader = (muplipleMesh) ? onMouseOverShadeOutline : onMouseOverShaderSihlouette;

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

    protected virtual void Update()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject == gameObject)
            {
                if (controls.CanInteract)
                {
                    if (rend.material.shader == onMouseExitShader)
                    {
                        SetShaderTo(onMouseOverShader);
                    }

                    if (!itemDescription.activeSelf)
                    {
                        itemDescription.GetComponentInChildren<Text>().text = (description == "") ? name : description;
                        Transform icons = itemDescription.transform.GetChild(0).GetChild(0);
                        icons.FindChild("UseIcon").gameObject.SetActive(gameObject.GetComponent<UsableObject>() != null);
                        icons.FindChild("TalkIcon").gameObject.SetActive(gameObject.GetComponent<PersonObject>() != null);
                        icons.FindChild("PickIcon").gameObject.SetActive(gameObject.GetComponent<PickableObject>() != null);
                        icons.FindChild("ExamIcon").gameObject.SetActive(gameObject.GetComponent<ExaminableObject>() != null);
                        itemDescription.SetActive(true);
                    }
                }
                else if (!controls.CanInteract && rend.material.shader == onMouseOverShader)
                {
                    SetShaderTo(onMouseExitShader);
                    itemDescription.SetActive(false);
                }
            }
            else
            {
                if (rend.material.shader == onMouseOverShader)
                {
                    SetShaderTo(onMouseExitShader);
                    itemDescription.SetActive(false);
                }
            }
        }
    }

    protected virtual void Reset()
    {
        if (rend)
        {
            SetShaderTo(onMouseExitShader);
            itemDescription.SetActive(false);
        }
    }

    protected bool ViewModeActive()
    {
        return cameraMode.CurrentMode == CameraMode.Mode.ObjectPreview;
    }

    public void SavePosition()
    {
        if (!positionSaved)
        {
            positionSaved = true;

            savedPosition = transform.position;
            savedRotation = transform.rotation;
        }
    }

    public void SavePosition(Vector3 pos, Quaternion rot)
    {
        if (!positionSaved)
        {
            positionSaved = true;

            savedPosition = pos;
            savedRotation = rot;
        }
    }

    public void LoadPosition()
    {
        transform.position = savedPosition;
        transform.rotation = savedRotation;
    }

    public void GetSavesLocation(out Vector3 position, out Quaternion rotation)
    {
        position = savedPosition;
        rotation = savedRotation;
    }

    protected virtual void SetShaderTo(Shader shader)
    {
        foreach (Material m in rend.materials)
        {
            m.shader = shader;
        }
        
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            foreach(Material m in r.materials)
            {
                m.shader = shader;
            }
        }
    }
}
