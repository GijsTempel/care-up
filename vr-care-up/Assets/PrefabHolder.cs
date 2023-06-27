using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();
    public Transform SpawnObjectHolder;

    GameObject SpawnObject(string _name)
    {
        GameObject baseObj = null;
        baseObj = GetPrefab(_name);
        GameObject newInstance = null;
        if (baseObj != null)
        {
            newInstance = Instantiate(baseObj, Vector3.zero, Quaternion.identity) as GameObject;
            if (SpawnObjectHolder != null)
                newInstance.transform.SetParent(SpawnObjectHolder);
        }
        return newInstance;
    }

    void Start()
    {
        foreach (Transform t in transform)
        {
            Prefabs.Add(t.gameObject.name, t.gameObject);
            t.gameObject.name = "_" + t.gameObject.name;
            t.gameObject.SetActive(false);
        }
    }
    public GameObject GetPrefab(string _name)
    {
        if (Prefabs.ContainsKey(_name))
        {
            return Prefabs[_name];
        }
        return null;
    }
}