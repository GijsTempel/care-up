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
        public bool excluded;

        public HatInfo() { headIndex = 0; name = ""; position = rotation = Vector3.zero; scale = 1; excluded = false;}
        public HatInfo(int index, string n, Vector3 pos, Vector3 rot, float sscale, bool excl)
        {
            headIndex = index;
            name = n;
            position = pos;
            rotation = rot;
            scale = sscale;
            excluded = excl;
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
            //Debug.Log(headIndex);
            int index = 0;
            int.TryParse(headIndex, out index);

            foreach (XmlNode xmlHatNode in xmlCatNode.ChildNodes)
            {
                bool excl=false;
                float posX, posY, posZ, rotX, rotY, rotZ, scale;
                float.TryParse(xmlHatNode.Attributes["x_pos"].Value, out posX);
                float.TryParse(xmlHatNode.Attributes["y_pos"].Value, out posY);
                float.TryParse(xmlHatNode.Attributes["z_pos"].Value, out posZ);
                float.TryParse(xmlHatNode.Attributes["x_rot"].Value, out rotX);
                float.TryParse(xmlHatNode.Attributes["y_rot"].Value, out rotY);
                float.TryParse(xmlHatNode.Attributes["z_rot"].Value, out rotZ);
                float.TryParse(xmlHatNode.Attributes["scale"].Value, out scale);
                string name = xmlHatNode.Attributes["name"].Value;
                if(xmlHatNode.Attributes["excluded"] != null){
                    bool.TryParse(xmlHatNode.Attributes["excluded"].Value, out excl);
                }

                hatItems.Add(new HatInfo(index, name, new Vector3(posX, posY, posZ),
                    new Vector3(rotX, rotY, rotZ), scale, excl));
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

    public void UpdateHatInfo(int headIndex, HatInfo info)
    {
        HeadCategory cat = database.Find(x => x.headIndex == headIndex);
        if (cat != default(HeadCategory))
        {
            HatInfo found = cat.hats.Find(x => x.name == info.name);
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
        string output = "Hats positioning DB output: \n";
        foreach (HeadCategory c in database)
        {
            output += "Category: " + c.headIndex + "\n";
            foreach (HatInfo hat in c.hats)
            {
                output += "    __Name: " + hat.name;
                output += ". Scale: " + hat.scale + "\n";
                output += "    Pos: " + hat.position + "\n";
                output += "    Rot: " + hat.rotation + "\n";
                output += "    Exc: " + hat.excluded + "\n";
            }
        }
        Debug.Log(output);
    }

    public void SaveInfoToXml(string filename = "HatsInfo.xml")
    {
        XmlDocument doc = new XmlDocument();
        XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(docNode);

        XmlNode root = doc.CreateElement("HatsData");
        doc.AppendChild(root);

        foreach(HeadCategory cat in database)
        {
            XmlNode headNode = doc.CreateElement("Head");
            root.AppendChild(headNode);

            XmlAttribute headIndex = doc.CreateAttribute("index");
            headIndex.Value = cat.headIndex.ToString();
            headNode.Attributes.Append(headIndex);

            foreach(HatInfo hat in cat.hats)
            {
                XmlNode hatNode = doc.CreateElement("Hat");
                headNode.AppendChild(hatNode);

                XmlAttribute hatName = doc.CreateAttribute("name");
                hatName.Value = hat.name;
                hatNode.Attributes.Append(hatName);
                
                XmlAttribute posX = doc.CreateAttribute("x_pos");
                posX.Value = hat.position.x.ToString();
                hatNode.Attributes.Append(posX);

                XmlAttribute posY = doc.CreateAttribute("y_pos");
                posY.Value = hat.position.y.ToString();
                hatNode.Attributes.Append(posY);

                XmlAttribute posZ = doc.CreateAttribute("z_pos");
                posZ.Value = hat.position.z.ToString();
                hatNode.Attributes.Append(posZ);

                XmlAttribute rotX = doc.CreateAttribute("x_rot");
                rotX.Value = hat.rotation.x.ToString();
                hatNode.Attributes.Append(rotX);

                XmlAttribute rotY = doc.CreateAttribute("y_rot");
                rotY.Value = hat.rotation.y.ToString();
                hatNode.Attributes.Append(rotY);

                XmlAttribute rotZ = doc.CreateAttribute("z_rot");
                rotZ.Value = hat.rotation.z.ToString();
                hatNode.Attributes.Append(rotZ);

                XmlAttribute scale = doc.CreateAttribute("scale");
                scale.Value = hat.scale.ToString();
                hatNode.Attributes.Append(scale);

                if(hat.excluded)
                {
                     XmlAttribute excluded = doc.CreateAttribute("excluded");
                    excluded.Value = hat.excluded.ToString();
                    hatNode.Attributes.Append(excluded);
                }
            }
        }
        doc.Save(Application.dataPath + "/Resources/Xml/" + filename);
        Debug.Log("Hats positioning xml saved.");
    }
}