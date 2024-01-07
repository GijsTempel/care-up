using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Hands.Samples.VisualizerSample;

public class GlovesControl : MonoBehaviour
{
    private int currentStateIndex = 0;
    private Material glovesMaterial;
    private string gloveRemovalPrefabNameLeft;
    private string gloveRemovalPrefabNameRight;

    public Material GetCurrentMaterial()
    {
        return glovesMaterial;
    }
    public int GetCurrentStateIndex()
    {
        return currentStateIndex;
    }
    
    public void SetGlovesMaterial(Material newManerial)
    {
        glovesMaterial = newManerial;
    }

    public string GetGloveRemovalPrefabName(bool isLeft = true)
    {
        if (isLeft)
            return gloveRemovalPrefabNameLeft;
        return gloveRemovalPrefabNameRight;
    }
    

    public void SetGloveRemovalPrefabsNames(string leftPref, string rightPref)
    {
        gloveRemovalPrefabNameLeft = leftPref;
        gloveRemovalPrefabNameRight = rightPref;
    }

    public void PutOnGloves(int stateIndex)
    {
        Debug.Log("@GlovesControl PutOnGloves:" + Random.Range(0, 9999).ToString());

        currentStateIndex = stateIndex;
        foreach(HandGloveControl h in GameObject.FindObjectsOfType<HandGloveControl>())
        {
            h.SetNewStateAndMaterial(stateIndex, glovesMaterial);
            if (h.tag == "RightHand")
                h.AddGloveRemovalPrefab(gloveRemovalPrefabNameRight);
            else
                h.AddGloveRemovalPrefab(gloveRemovalPrefabNameLeft);
        }
    }

    public void RemoveGloves()
    {
        foreach(HandGloveControl h in GameObject.FindObjectsOfType<HandGloveControl>())
            h.ResetToDefaultState();
        currentStateIndex = 0;
    }
}
