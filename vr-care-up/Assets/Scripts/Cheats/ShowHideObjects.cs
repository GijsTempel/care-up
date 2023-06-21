using System.Collections.Generic;
using UnityEngine;

public class ShowHideObjects : MonoBehaviour
{
    public List<GameObject> hidenObjects;

    public void _show(string _name, bool toShow, bool meshRenderer = false)
    {
        foreach (GameObject o in hidenObjects)
        {
            if (o != null)
            {
                if (o.name == _name || _name == "all")
                {
                    if (meshRenderer && (o.GetComponents<MeshRenderer>() != null || o.GetComponents<SkinnedMeshRenderer>() != null))
                    {
                        if (toShow)
                        {
                            if (o.GetComponent<MeshRenderer>()  != null)
                                o.GetComponent<MeshRenderer>().enabled = toShow;
                            else
                                o.GetComponent<SkinnedMeshRenderer>().enabled = toShow;
                        }
                        else
                        {
                            foreach (MeshRenderer m in o.GetComponents<MeshRenderer>())
                            {
                                m.enabled = true;
                            }
                            foreach (SkinnedMeshRenderer m in o.GetComponents<SkinnedMeshRenderer>())
                            {
                                m.enabled = true;
                            }
                        }
                    }
                    else
                    {
                        o.SetActive(toShow);
                    }
                }
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
