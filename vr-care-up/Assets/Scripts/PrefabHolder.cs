using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();
    public Transform spawnObjectHolder;


    public GameObject SpawnObject(string _name, Vector3 pos, Quaternion rot)
    {
        GameObject newInstance = SpawnObject(_name);
        newInstance.transform.position = pos;
        newInstance.transform.rotation = rot;
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

    public GameObject SpawnObject(string _name, Transform newParent = null)
    {
        GameObject baseObj = null;
        baseObj = GetPrefab(_name);
        GameObject newInstance = null;
        if (baseObj != null)
        {
            if (newParent != null)
            {
                newInstance = Instantiate(baseObj, newParent, false) as GameObject;
            }
            else if (spawnObjectHolder != null)
                newInstance = Instantiate(baseObj, transform.position, Quaternion.identity, spawnObjectHolder) as GameObject;
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