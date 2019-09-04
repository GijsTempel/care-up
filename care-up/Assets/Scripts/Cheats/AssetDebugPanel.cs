using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssetBundles;



public class AssetDebugPanel : MonoBehaviour
{
    struct AssetDebugData
    {
        public string name;
        public InteractableObject.AssetSource source;
    }
    Dictionary<string, AssetDebugData> AssetDict = new Dictionary<string, AssetDebugData>();

    bool slideState = false;
    public GameObject ObjectList;
    public GameObject PrefabList;
    int activeTab = 0;
    // Start is called before the first frame update
    void Start()
    {
        SwitchTab(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchTab(int t)
    {
        if (t == activeTab)
            return;
        activeTab = t;
        ObjectList.SetActive(false);
        PrefabList.SetActive(false);
        if (activeTab == 0)
            ObjectList.SetActive(true);
        else if (activeTab == 1)
            PrefabList.SetActive(true);
    }

    public void CopyToClipboard(bool only_unique = false)
    {
        List<string> unique_names = new List<string>();

        var textEditor = new TextEditor();
        string str = "";
        foreach(string s in AssetDict.Keys)
        {
            if (!only_unique)
            {
                str += ((int)(AssetDict[s].source)).ToString() + ",";
                str += AssetDict[s].source.ToString() + ",";
                str += AssetDict[s].name;
                str += "\n";
            }
            else if (!unique_names.Contains(AssetDict[s].name))
            {
                unique_names.Add(AssetDict[s].name);
                str += AssetDict[s].name;
                str += "\n";
            }
        }
        textEditor.text = str;
        textEditor.SelectAll();
        textEditor.Copy();
    }

    public void RefrashAssetDict()
    {
        foreach (InteractableObject o in Resources.FindObjectsOfTypeAll<InteractableObject>())
        {
            string _key = o.assetSource.ToString() + "_" + o.name;
            if (!AssetDict.ContainsKey(_key))
            {
                AssetDebugData assetDebugData = new AssetDebugData();
                assetDebugData.name = o.name;
                assetDebugData.source = o.assetSource;
                AssetDict.Add(_key, assetDebugData);
            }
        }
    }

    public void UpdateListOfObjects()
    {
        if (activeTab == 0)
        {
            Transform content = transform.Find("ObjectList/Viewport/Content").transform;
            foreach (Transform child in content)
            {
                GameObject.Destroy(child.gameObject);
            }
            RefrashAssetDict();
            foreach(string o_key in AssetDict.Keys)
            {
                AssetDebugData o = AssetDict[o_key];
                GameObject assetStat = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/AssetStat"), content);
                string ASLabel = "";
                Color textColor = Color.white;
                switch (o.source)
                {
                    case InteractableObject.AssetSource.None:
                        ASLabel = "N";

                        break;
                    case InteractableObject.AssetSource.Included:
                        ASLabel = "I";
                        textColor = Color.yellow;
                        break;
                    case InteractableObject.AssetSource.Resources:
                        ASLabel = "R";
                        textColor = Color.red;
                        break;
                    case InteractableObject.AssetSource.Bundle:
                        ASLabel = "B";
                        textColor = Color.green;
                        break;
                    default:
                        break;
                }
                string t = ASLabel + " " + o.name ;
                assetStat.transform.Find("Text").GetComponent<Text>().text = t;
                assetStat.transform.Find("Text").GetComponent<Text>().color = textColor;

                Image ico = assetStat.transform.Find("Image").GetComponent<Image>();
                Sprite x = Resources.Load("Sprites/Prefab_Icons/x", typeof(Sprite)) as Sprite;
                Sprite l = Resources.Load("Sprites/Prefab_Icons/" + o.name, typeof(Sprite)) as Sprite;
                if (l != null)
                    ico.sprite = l;
                else
                    ico.sprite = x;

            }
        }
        else if (activeTab == 1)
        {

            Transform content = transform.Find("PrefabList/Viewport/Content").transform;
            foreach (Transform child in content)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach(string k in AssetBundleManager.m_LoadedAssetBundles.Keys)
			{
				GameObject bundleStat = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/BundleStat"), content);
                string ss = k;
                bundleStat.transform.Find("Text").GetComponent<Text>().text = ss;

                foreach(string a in AssetBundleManager.m_LoadedAssetBundles[k].m_AssetBundle.GetAllAssetNames())
                {
                    // print("- " + a + "  " + AssetBundleManager.m_LoadedAssetBundles[k].m_AssetBundle.LoadAsset(a).GetType().ToString());
                    string[] splitName = a.Split('/');
                    if(splitName.Length == 0)
                        continue;
                    if(splitName[splitName.Length - 1].Split('.').Length <= 1)
                        continue;
                    
                    if (splitName[splitName.Length - 1].Split('.')[1] == "prefab")
                    {
                        GameObject assetStat = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/AssetStat"), content);

                        string t = splitName[splitName.Length - 1].Split('.')[0];
                        assetStat.transform.Find("Text").GetComponent<Text>().text = t;

                        Image ico = assetStat.transform.Find("Image").GetComponent<Image>();
                        Sprite x = Resources.Load("Sprites/Prefab_Icons/x", typeof(Sprite)) as Sprite;
                        Sprite l = Resources.Load("Sprites/Prefab_Icons/" + t, typeof(Sprite)) as Sprite;
                        if (l != null)
                            ico.sprite = l;
                        else
                            ico.sprite = x;
                    }
                }
			}
            

        }
    }

    public void toggleSlide()
    {
        slide(!slideState);
    }

    public void slide(bool value)
    {
        if (slideState != value)
        {
            slideState = value;
            if (value)
                GetComponent<Animator>().SetTrigger("in");
            else
                GetComponent<Animator>().SetTrigger("out");

        }
    }
}
