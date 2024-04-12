using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGloveControl : MonoBehaviour
{
    int currentStateIndex = 0;
    public List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
    public Transform gloveRemovalMountPoint;

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

    public void AddGloveRemovalPrefab(string prefName)
    {
        Debug.Log("@AddGloveRemovalPrefab" + name + ": \"" + prefName + "\""  + Random.Range(0, 9999).ToString());
        if (gloveRemovalMountPoint == null)
            return;
        DeleteGloveRemovalActionTrigger();
        GameObject g = null;
        PrefabHolder prefabHolder = GameObject.FindObjectOfType<PrefabHolder>();
        if (prefName != "" && prefabHolder != null)
            g = prefabHolder.SpawnObject(prefName, gloveRemovalMountPoint);
        else
            Debug.Log("@ __))"  + name + ":" + ":" + Random.Range(0, 9999).ToString());

        if (g != null)
            Debug.Log("@ __:" + name + ":" + g.name + Random.Range(0, 9999).ToString());
        
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
                {
                    SetNewStateAndMaterial(currentControlState, glovesControl.GetCurrentMaterial());
                    string gloveRemPref;
                    if (tag == "RightHand")
                        gloveRemPref = glovesControl.GetGloveRemovalPrefabName(false);
                    else
                        gloveRemPref = glovesControl.GetGloveRemovalPrefabName(true);
                    AddGloveRemovalPrefab(gloveRemPref);
                }
                else
                {
                    ResetToDefaultState();
                }
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
        Invoke("DeleteGloveRemovalActionTrigger", 0.1f);
    }

    void DeleteGloveRemovalActionTrigger()
    {
        if (gloveRemovalMountPoint == null)
            return;
        if (gloveRemovalMountPoint.childCount > 0)
        {
            foreach (Transform g in gloveRemovalMountPoint)
                GameObject.Destroy(g.gameObject);
        }
    }

}
