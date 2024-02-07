using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_AddObjectToHand : MonoBehaviour
{

    public string objectName;
    public bool toLeftHand;
    // Start is called before the first frame update


    private void AddObject()
    {
        PrefabHolder prefabHolder = GameObject.FindObjectOfType<PrefabHolder>();
        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();

        if (prefabHolder != null && player != null)
        {
            player.PickUpObject(prefabHolder.SpawnObject(objectName).GetComponent<PickableObject>(), toLeftHand);
        }
        else
        {
            Debug.LogError("!Prefab Holder not found");
        }
    }

    public void Execute()
    {
        AddObject();
    }
}
