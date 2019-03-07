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

    private List<ObjectsIDs> objectsIDsList = new List<ObjectsIDs>();
    public List<GameObject> hidenObjects;   

    public void GetObjectsIDsInfo()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Combinations/GameObjects");
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

    //public ObjectsIDs[] Objects =
    //{
    //      new ObjectsIDs("syringePack", 30, 0),
    //      new ObjectsIDs("Syringe", 2, 1),
    //      new ObjectsIDs("AbsorptionNeedle", 20, 0),
    //      new ObjectsIDs("InjectionNeedle", 20, 0),
    //      new ObjectsIDs("cloth_02_folded", 33, 0),
    //      new ObjectsIDs("cloth_02", 53,1),
    //      new ObjectsIDs("water_syringe_pack", 35, 0),
    //      new ObjectsIDs("water_syringe_pack_no_cover", 35, 1),
    //      new ObjectsIDs("water_syringe_pack_empty", 35, 2),
    //      new ObjectsIDs("water_syringe_pack_cover", 36, 0),
    //      new ObjectsIDs("lube_syringe_pack", 48, 0),
    //      new ObjectsIDs("lube_syringe_pack_noCover", 48, 1),
    //      new ObjectsIDs("lube_syringe_pack_empty", 48, 2),
    //      new ObjectsIDs("lube_syringe_pack_cover", 48, 0),
    //      new ObjectsIDs("w0_A", -5, 0),
    //      new ObjectsIDs("m1_A", -6, 0),
    //      new ObjectsIDs("m1_B", -7, 0),
    //      new ObjectsIDs("Sink", -10, 0),
    //      new ObjectsIDs("GauzeTrayFull", 38,0),
    //      new ObjectsIDs("cotton_ball", 40,0),
    //      new ObjectsIDs("catheter_bag_packed", 42,0),
    //      new ObjectsIDs("catheter_bag_packed_opened", 42,1),
    //      new ObjectsIDs("catheter_bag_packed_empty", 42,2),
    //      new ObjectsIDs("catheter", 43,0),
    //      new ObjectsIDs("catheter_inner", 44,0),
    //      new ObjectsIDs("x_test", 101,0),
    //      new ObjectsIDs("CWB_inner_open", 50,0),
    //      new ObjectsIDs("CWB_inner_open_in2", 50,1),
    //      new ObjectsIDs("catheter_bag_twisted", 46,0),
    //      new ObjectsIDs("fixator_folded", 52,0),
    //      new ObjectsIDs("fixation_folded_buttons", 53,0),
    //      new ObjectsIDs("Sink_active2", -15,0),
    //      new ObjectsIDs("PaperTowelInHand", 60,5),
    //      new ObjectsIDs("MedicineBag", 102,00),
    //      new ObjectsIDs("TabletRound1", 103,00),
    //};
}
