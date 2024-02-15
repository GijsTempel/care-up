using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MountGhostHighlit : MonoBehaviour
{
    Type[] typesToRemove = { 
            typeof(PickableObject),
            typeof(GrabHandPose),
            typeof(ActionModule_AnimationTrigger),
            typeof(ActionModule_ActionExpectant),
            typeof(MountDetector),
            typeof(MountGhostHighlit),
         };

    public Material ghostMaterial;
    GameObject currentGhostObject;
    

    void Start()
    {
        ghostMaterial = Resources.Load("Materials/GhostMat" , typeof(Material)) as Material; 
    }

    void DisableComponentsRecursive(Transform parent)
    {
        // Check if the child has any of the types from typesToFind
        Component[] components = parent.GetComponents<Component>();
        foreach (var component in components)
        {
            // if (typesToRemove.Contains(component.GetType()))
            if (!(component.GetType() == typeof(MeshRenderer) || 
                component.GetType() == typeof(Transform) ||
                component.GetType() == typeof(MeshFilter) ||
                component.GetType() == typeof(RectTransform) 
            ))
            {
                // Disable the component
                Destroy(component);
                // (component as Behaviour).enabled = false;
            }
        }
        foreach (Transform child in parent)
        {
            DisableComponentsRecursive(child);
        }
    }

    void ChangeMaterialsRecursive(Transform parent)
    {
        // Change the material of the parent object
        MeshRenderer parentRenderer = parent.GetComponent<MeshRenderer>();
        if (parentRenderer != null)
        {
            Material[] parentMaterials = new Material[parentRenderer.sharedMaterials.Length];
            for (int i = 0; i < parentMaterials.Length; i++)
            {
                parentMaterials[i] = ghostMaterial;
            }
            parentRenderer.sharedMaterials = parentMaterials;
        }

        // Change materials of all children
        foreach (Transform child in parent)
        {
            ChangeMaterialsRecursive(child);
        }
    }

    GameObject CreateGhostObject(GameObject obj)
    {
        if (currentGhostObject != null)
        {
            Destroy(currentGhostObject);
        }
        if (obj != null)
        {
            GameObject newInstance = Instantiate(obj, transform.position, transform.rotation, transform) as GameObject;
            DisableComponentsRecursive(newInstance.transform);
            ChangeMaterialsRecursive(newInstance.transform);
            currentGhostObject = newInstance;
            currentGhostObject.name = currentGhostObject.name + "_ghost";
            return newInstance;
        }
        return null;
    }

    public void ShowGhost(GameObject baseObject)
    {
        if (currentGhostObject == null)
            CreateGhostObject(baseObject);
        // GameObject currentGhostObject = GetGhostObject(ghostName);
        // if (currentGhostObject == null)
        //     return;
        // currentGhostObject.SetActive(true);
        // long currentTimeMil = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        // ghostTimeStamps[ghostName] = currentTimeMil;
    }

    public void HideGhost()
    {
        if (currentGhostObject != null)
        {
            Destroy(currentGhostObject);
            currentGhostObject = null;
        }
        // GameObject currentGhostObject = GetGhostObject(ghostName);
        // if (currentGhostObject == null)
        //     return;
        // currentGhostObject.SetActive(false);
        // if (ghostTimeStamps.Keys.Contains(ghostName))
        //     ghostTimeStamps.Remove(ghostName);
    }
}
