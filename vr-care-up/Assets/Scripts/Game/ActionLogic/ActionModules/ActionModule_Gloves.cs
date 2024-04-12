using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_Gloves : MonoBehaviour
{
    [Tooltip("If no material - remove gloves")]
    public Material glovesMaterial;
    [Tooltip("If 0 - remove gloves")]
    public int stateIndex = 0;
    public string gloveRemovalPrefabNameLeft;
    public string gloveRemovalPrefabNameRight;

    public void Execute()
    {
        Debug.Log("@ActionModule_Gloves Exec:" + Random.Range(0, 9999).ToString());
        GlovesControl glovesControl = GameObject.FindObjectOfType<GlovesControl>();
        if (glovesControl != null)
        {
            if (stateIndex == 0 || glovesMaterial == null)
            {
                glovesControl.RemoveGloves();
            }
            else
            {
                glovesControl.SetGlovesMaterial(glovesMaterial);
                glovesControl.SetGloveRemovalPrefabsNames(gloveRemovalPrefabNameLeft, gloveRemovalPrefabNameRight);
                glovesControl.PutOnGloves(stateIndex);
            }
        }

    }
}
