using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_AddObjectToScene : MonoBehaviour
{
    public string objectName;
    public string targetPositionObjectName;

    private void AddObject()
    {
        PrefabHolder prefabHolder = GameObject.FindObjectOfType<PrefabHolder>();
        GameObject targetPositionObject = GameObject.Find(targetPositionObjectName);

        if (prefabHolder != null && targetPositionObject != null)
        {
            GameObject newObj = prefabHolder.SpawnObject(objectName);
            if (newObj != null)
            {
                newObj.transform.position = targetPositionObject.transform.position;
                newObj.transform.rotation = targetPositionObject.transform.rotation;
            }
        }
        else
        {
            Debug.LogError("!PrefabHolder or targetPositionObject Holder not found");
        }
    }

    public void Execute()
    {
        AddObject();
    }
}
