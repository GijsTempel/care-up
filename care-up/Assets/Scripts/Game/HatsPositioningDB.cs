using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class HatsPositioningDB
{
    public class HatInfo
    {
        public int headIndex;
        public string name;
        public Vector3 position;
        public Vector3 rotation;
        public float scale;

        public HatInfo() { headIndex = 0; name = ""; position = rotation = Vector3.zero; scale = 0; }
        public HatInfo(int index, string n, Vector3 pos, Vector3 rot, float sscale)
        {
            headIndex = index;
            name = n;
            position = pos;
            rotation = rot;
            scale = sscale;
        }
    };

    public class HeadCategory
    {
        public int headIndex;
        public List<HatInfo> hats;

        public HeadCategory(int index, List<HatInfo> info) { headIndex = index; hats = info; }
    }

    private static List<HeadCategory> database = new List<HeadCategory>();

    public void Init(string filename = "HatsInfo")
    {
        // load up all items from xml into the list
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/" + filename);

        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlCatList = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode xmlCatNode in xmlCatList)
        {
            List<HatInfo> hatItems = new List<HatInfo>();
            string headIndex = (xmlCatNode.Attributes["index"] != null) ? xmlCatNode.Attributes["index"].Value : "";
            int index = 0;
            int.TryParse(headIndex, out index);

            foreach (XmlNode xmlHatNode in xmlCatNode.ChildNodes)
            {
                float posX, posY, posZ, rotX, rotY, rotZ, scale;
                float.TryParse(xmlHatNode.Attributes["x_pos"].Value, out posX);
                float.TryParse(xmlHatNode.Attributes["y_pos"].Value, out posY);
                float.TryParse(xmlHatNode.Attributes["z_pos"].Value, out posZ);
                float.TryParse(xmlHatNode.Attributes["x_rot"].Value, out rotX);
                float.TryParse(xmlHatNode.Attributes["y_rot"].Value, out rotY);
                float.TryParse(xmlHatNode.Attributes["z_rot"].Value, out rotZ);
                float.TryParse(xmlHatNode.Attributes["scale"].Value, out scale);

                string name = xmlHatNode.Attributes["name"].Value;

                hatItems.Add(new HatInfo(index, name, new Vector3(posX, posY, posZ),
                    new Vector3(rotX, rotY, rotZ), scale));
            }
            
            database.Add(new HeadCategory(index, hatItems));
        }

        PrintDatabase();
    }

    public void Clean()
    {
        database.Clear();
    }

    public HatInfo GetHatInfo(int headIndex, string hatName)
    {
        HeadCategory cat = database.Find(x => x.headIndex == headIndex);
        if (cat != default(HeadCategory))
        {
            return cat.hats.Find(x => x.name == hatName);
        }
        else return new HatInfo();
    }

    public void UpdateHatInfo(int headIndex, string hatName, HatInfo info)
    {
        HeadCategory cat = database.Find(x => x.headIndex == headIndex);
        if (cat != default(HeadCategory))
        {
            HatInfo found = cat.hats.Find(x => x.name == hatName);
            if(found != default(HatInfo))
            {
                cat.hats[cat.hats.IndexOf(found)] = info;
            }
            else
            {
                cat.hats.Add(info);
            }
        }
        else
        {
            List<HatInfo> list = new List<HatInfo>();
            list.Add(info);
            database.Add(new HeadCategory(headIndex, list));
        }
    }

    public static void PrintDatabase()
    {
        string output = "";
        foreach (HeadCategory c in database)
        {
            output += "Category: " + c.headIndex + "\n";
            foreach (HatInfo hat in c.hats)
            {
                output += "    " + hat.name + "\n";
                output += "    " + hat.position + "\n";
                output += "    " + hat.rotation + "\n";
                output += "    " + hat.scale + "\n\n";
            }
        }
        Debug.Log(output);
    }

    public void SaveInfoToXml()
    {

    }
}
