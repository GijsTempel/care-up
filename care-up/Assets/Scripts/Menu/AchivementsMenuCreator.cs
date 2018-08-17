using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class AchivementsMenuCreator : MonoBehaviour {
    public GameObject aList;
    public TextAsset xmlFile;
    float scrollHeight = 0f;

    // Use this for initialization
    void Awake () {
        string xData = xmlFile.text;
        addAchivements(xData);
    }

    void addAchivements(string xmlData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));
        string xmlPathPattern = "//achivements/achivement";
        XmlNodeList nodeList = xmlDoc.SelectNodes(xmlPathPattern);
        int n = 0;
        foreach (XmlNode node in nodeList)
        {
            XmlNode xName = node.FirstChild;
            if (aList.transform.Find(xName.InnerText) == null)
            {

                XmlNode xIconID = xName.NextSibling;
                XmlNode xDescr = xIconID.NextSibling;
                XmlNode xFull = xDescr.NextSibling;

                GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/AchievementsButton"));
                AchivementsManuButton aButton = go.GetComponent<AchivementsManuButton>();

                go.name = xName.InnerText;
                go.transform.parent = aList.transform;
                go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                scrollHeight += go.GetComponent<RectTransform>().sizeDelta.y;

                aButton.title = xFull.InnerText;
                aButton.desc = xDescr.InnerText;
                if (!string.IsNullOrEmpty(xIconID.InnerText))
                {
                    aButton.iconID = int.Parse(xIconID.InnerText);
                }
            }
            n++;
        }
        Vector2 h = aList.GetComponent<RectTransform>().sizeDelta;
        h.y = scrollHeight;
        aList.GetComponent<RectTransform>().sizeDelta = h;
    }

}
