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

    Material ghostMaterial;
    GameObject currentGhostObject;
    const long ghostObjectTimeout = 100;
    long ghostTimeStamp = 0;

    void Start()
    {
        ghostMaterial = Resources.Load("Materials/GhostMat" , typeof(Material)) as Material; 
    }

    void Update()
    {
        if (currentGhostObject != null)
        {
            long currentTimeMil = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            if ((currentTimeMil - ghostTimeStamp) > ghostObjectTimeout)
            {
                HideGhost();
            }
        }
    }

    void CleanGhostRecursive(Transform parent)
    {
        Component[] components = parent.GetComponents<Component>();
        foreach (var component in components)
        {
            if (!(component.GetType() == typeof(MeshRenderer) || 
                component.GetType() == typeof(Transform) ||
                component.GetType() == typeof(MeshFilter) ||
                component.GetType() == typeof(RectTransform) 
            ))
            {
                Destroy(component);
            }
        }
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
        foreach (Transform child in parent)
        {
            if (!child.gameObject.activeSelf)
                Destroy(child.gameObject);
            else
                CleanGhostRecursive(child);
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
            CleanGhostRecursive(newInstance.transform);
            currentGhostObject = newInstance;
            currentGhostObject.name = currentGhostObject.name + "_ghost";
            return newInstance;
        }
        return null;
    }

    public void ShowGhost(GameObject baseObject)
    {
        ghostTimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        if (currentGhostObject == null)
            CreateGhostObject(baseObject);
    }

    public void HideGhost()
    {
        if (currentGhostObject != null)
        {
            Destroy(currentGhostObject);
            currentGhostObject = null;
        }
    }
}
