using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMaterialControl : MonoBehaviour
{
    int currentStateIndex = 0;
    public List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();

    Dictionary<SkinnedMeshRenderer, List<Material>> defaultMaterials = new Dictionary<SkinnedMeshRenderer, List<Material>>();

    void Start()
    {
        foreach(SkinnedMeshRenderer sm in meshRenderers)
        {
            List<Material> currentMaterials = new List<Material>();
            foreach (Material m in sm.materials)
            {
                currentMaterials.Add(m);
            }
            defaultMaterials.Add(sm, currentMaterials);
        }
    }

    private void OnEnable()
    {
        GlovesControl glovesControl = GameObject.FindObjectOfType<GlovesControl>();
        if (glovesControl != null)
        {
            int currentControlState = glovesControl.GetCurrentStateIndex();
            if (currentStateIndex != currentControlState)
            {
                if (currentControlState != 0)
                    SetNewStateAndMaterial(currentControlState, glovesControl.GetCurrentMaterial());
            }
        }
    }    

    public void ResetMaterials()
    {
        foreach(SkinnedMeshRenderer sm in defaultMaterials.Keys)
        {
            Material[] newSetOfMaterials = sm.materials;

            for (int i = 0; i < defaultMaterials[sm].Count; i++)
                newSetOfMaterials[i] = defaultMaterials[sm][i];
            sm.materials = newSetOfMaterials;
        }
    }

    public void SetNewStateAndMaterial(int stateIndex, Material newMaterial)
    {
        Debug.Log("@HandMaterialControl SetNewStateAndMaterial:" + Random.Range(0, 9999).ToString());

        currentStateIndex = stateIndex;
        SetNewMaterial(newMaterial);
    }

    public void SetNewMaterial(Material newMaterial)
    {
        foreach(SkinnedMeshRenderer sm in defaultMaterials.Keys)
        {
            Material[] newSetOfMaterials = sm.materials;
            for (int i = 0; i < defaultMaterials[sm].Count; i++)
                newSetOfMaterials[i] = newMaterial;

            sm.materials = newSetOfMaterials;
        }
    }


    public void ResetToDefaultState()
    {
        if (currentStateIndex == 0)
            return;
        ResetMaterials();
        currentStateIndex = 0;
    }

}
