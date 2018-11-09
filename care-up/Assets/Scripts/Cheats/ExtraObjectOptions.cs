using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraObjectOptions : MonoBehaviour {
	public string TrashBin = "";
    public List<GameObject> hidenObjects;

    void Start () {
		
	}


    public void _show(string _name, bool value)
    {
        foreach (GameObject o in hidenObjects)
        {
            if (o.name == _name)
            {
                o.SetActive(value);
            }
        }
    }

    public void _toggle(string _name)
    {
        foreach (GameObject o in hidenObjects)
        {
            if (o.name == _name)
            {
                o.SetActive(!o.activeSelf);
            }
        }
    }
}
