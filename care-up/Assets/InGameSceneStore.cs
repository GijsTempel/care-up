using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSPackageData
{
    public string title;
    public string SKU;
    public List<string> scenes;
    public float price;
    public SSPackageData(string _title, string _SKU, float _price)
    {
        title = _title;
        SKU = _SKU;
        price = _price;
    }
}
public class InGameSceneStore : MonoBehaviour
{
    public List<GameObject> tabs;
    public List<GameObject> tabsButtons;
    public GameObject packageStoreContent;
    public GameObject sceneStoreContent;

    Sprite openTabImage = null;
    Sprite closeTabImage = null;
    Object SSPackageItemPrefab = Resources.Load<GameObject>("NecessaryPrefabs/UI/SSPackageItem");
    int currentTab = 0;
    public void SwitchTab(int newTab)
    {
        if (newTab < tabs.Count && newTab != currentTab)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].SetActive(i == newTab);
                if (i != newTab)
                    tabsButtons[i].GetComponent<Image>().sprite = closeTabImage;
                else
                    tabsButtons[i].GetComponent<Image>().sprite = openTabImage;

            }
        }
        currentTab = newTab;
    }
    void LoadStoreData()
    {
        if (SSPackageItemPrefab == null)
            SSPackageItemPrefab = Resources.Load<GameObject>("NecessaryPrefabs/UI/SSPackageItem");
        //test data
        List<SSPackageData> SSPData = new List<SSPackageData>();

        SSPData.Add(new SSPackageData("Bloedglucose pakket", "aaa", 123f));
        SSPData.Add(new SSPackageData("Handhygiëne pakket", "aaa", 123f));
        SSPData.Add(new SSPackageData("Injecteren pakket", "aaa", 123f));
        SSPData.Add(new SSPackageData("Katheteriseren pakket", "aaa", 123f));

        string[] _sstrings = new string[] { 
            "Injecteren intramusculair (loodrechttechniek)",
            "Injecteren subcutaan (huidplooitechniek)",
            "Injecteren subcutaan (loodrechttechniek)",
            "Subcutaan injecteren met insulinepen",
            "Voorbereiden medicijn injecteren ampul",
            "Voorbereiden medicijn injecteren flacon",
            "Voorbereiden medicijn injecteren oplossing" };

        List<string> __scenes = new List<string>();
        __scenes.AddRange(_sstrings);

        for (int i = 0; i < SSPData.Count; i++)
        {
            GameObject packageButton = Instantiate(SSPackageItemPrefab, packageStoreContent.transform) as GameObject;
            packageButton.GetComponent<SSPackageItem>().Setup(SSPData[i].title, __scenes, "", SSPData[i].price);
        }
    }
    void Start()
    {
        openTabImage = tabsButtons[0].GetComponent<Image>().sprite;
        closeTabImage = tabsButtons[1].GetComponent<Image>().sprite;
        LoadStoreData();
    }

}
