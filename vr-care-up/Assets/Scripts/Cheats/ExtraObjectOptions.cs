using System.Collections.Generic;
using UnityEngine;

public class ExtraObjectOptions : MonoBehaviour
{
    public string TrashBin = "";

    public List<GameObject> neededObjects;
    public WalkToGroupVR nearestWalkToGroup;

    public Dictionary<string, string> neededObjectsData = new Dictionary<string, string>();
    public Dictionary<string, string> neededObjectsArticle = new Dictionary<string, string>();


    void Start()
    {
        foreach (GameObject obj in neededObjects)
        {
            if(obj != null)
            {
                if (obj.GetComponent<ObjectDataHolder>() != null)
                {
                    if (!neededObjectsData.ContainsKey(obj.name))
                    {
                        neededObjectsData.Add(obj.name, obj.GetComponent<ObjectDataHolder>().description);
                        neededObjectsArticle.Add(obj.name, obj.GetComponent<ObjectDataHolder>().nameArticle);
                    }
                }
            }
        }       
    }

    public string HasNeeded(string str)
    {
        if (neededObjectsData.ContainsKey(str))
            return neededObjectsData[str];
        return "";
    }

    public string HasNeededArticle(string str)
    {
        if (neededObjectsArticle.ContainsKey(str))
            return neededObjectsArticle[str];
        return "";
    }
}
