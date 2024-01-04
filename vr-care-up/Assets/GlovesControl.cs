using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Hands.Samples.VisualizerSample;

public class GlovesControl : MonoBehaviour
{
    private int currentStateIndex = 0;
    private Material glovesMaterial;


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

    public void PutOnGloves(int stateIndex)
    {
        Debug.Log("@GlovesControl PutOnGloves:" + Random.Range(0, 9999).ToString());

        currentStateIndex = stateIndex;
        foreach(HandMaterialControl h in GameObject.FindObjectsOfType<HandMaterialControl>())
        {
            h.SetNewStateAndMaterial(stateIndex, glovesMaterial);
        }
    }

    public void RemoveGloves()
    {
        foreach(HandMaterialControl h in GameObject.FindObjectsOfType<HandMaterialControl>())
            h.ResetToDefaultState();
        currentStateIndex = 0;
    }
}
