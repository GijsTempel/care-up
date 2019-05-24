using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[Serializable]
public class ObjectsIDs
{
    public int id;
    public int state;
    public string name;
};

public class ObjectsIDsController : MonoBehaviour
{
    public bool cheat = false;
    public bool buildActionList = false;
    public bool Ua = false;
    public bool testingMode = false;

    private List<ObjectsIDs> objectsIDsList = new List<ObjectsIDs>();
    public List<GameObject> hidenObjects;


    private void Start()
    {
        GetObjectsIDsInfo();
    }


    public void GetObjectsIDsInfo()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/GameObjects");
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList list = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode node in list)
        {
            ObjectsIDs objectsIDs = new ObjectsIDs();
            if (node.Attributes["id"].Value != null)
            {
                int.TryParse(node.Attributes["id"].Value, out objectsIDs.id);
            }

            if (node.Attributes["state"].Value != null)
            {
                int.TryParse(node.Attributes["state"].Value, out objectsIDs.state);
            }

            if (node.Attributes["name"] != null)
            {
                objectsIDs.name = node.Attributes["name"].Value;
            }

            objectsIDsList.Add(objectsIDs);
        }
    }

    public int FindByName(string name)
    {
        int n = 0;

        foreach (ObjectsIDs Obj in objectsIDsList)
        {
            if (Obj.name == name)
            {
                return (n);
            }
            n++;
        }
        return (-1);
    }

    public GameObject GetFromHidden(string name)
    {
        if (name != "" && hidenObjects.Count > 0)
        {
            foreach (GameObject go in hidenObjects)
            {
                if (go != null)
                {
                    if (name == go.name)
                        return go;
                }
            }
        }
        return null;
    }

    public ObjectsIDs GetObject(int n)
    {
        if (n < 0 || n > objectsIDsList.Count)
        {
            return (null);
        }
        return objectsIDsList[n];
    }

    public int GetID(int n)
    {
        if (n < 0 || n > objectsIDsList.Count)
        {
            return (-1);
        }
        return (objectsIDsList[n].id);
    }

    public int GetState(int n)
    {
        if (n < 0 || n > objectsIDsList.Count)
        {
            return (-1);
        }
        return (objectsIDsList[n].state);
    }

    public string GetName(int n)
    {
        if (n < 0 && n > objectsIDsList.Count)
        {
            return ("");
        }
        return (objectsIDsList[n].name);
    }

    public int GetIDByName(string name)
    {
        int byName = FindByName(name);

        if (byName != -1)
        {
            ObjectsIDs o = GetObject(byName);
            return o.id * 100 + o.state;
        }
        return (0);
    }

}
