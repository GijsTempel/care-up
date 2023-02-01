using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();

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
