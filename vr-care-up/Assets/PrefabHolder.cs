using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();
    public Transform spawnObjectHolder;

    public GameObject SpawnObject(string _name, Transform positionTransform)
    {
        GameObject newInstance = SpawnObject(_name);
        newInstance.transform.position = positionTransform.position;
        newInstance.transform.rotation = positionTransform.rotation;
        return newInstance;
    }

    public List<GameObject> GetObjects()
    {
        List<GameObject> _objects = new List<GameObject>();
        foreach(string key in Prefabs.Keys)
        {
            _objects.Add(Prefabs[key]);
        }
        return _objects;
    }

    public GameObject SpawnObject(string _name)
    {
        GameObject baseObj = null;
        baseObj = GetPrefab(_name);
        GameObject newInstance = null;
        if (baseObj != null)
        {
            newInstance = Instantiate(baseObj, Vector3.zero, Quaternion.identity) as GameObject;
            if (spawnObjectHolder != null)
                newInstance.transform.SetParent(spawnObjectHolder);
            newInstance.transform.position = spawnObjectHolder.position;
            newInstance.name = _name;
            newInstance.SetActive(true);
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