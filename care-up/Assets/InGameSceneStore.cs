using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

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
    public GameObject checkoutWindow;
    public GameObject checkoutContent;
    public Text checkoutCounter;
    public Button ConfirmCheckoutButton;
    PlayerPrefsManager ppm = null;

    Sprite openTabImage = null;
    Sprite closeTabImage = null;
    Object SSPackageItemPrefab = Resources.Load<GameObject>("NecessaryPrefabs/UI/SSPackageItem");
    int currentTab = 0;
    List<SSPackageItem> SelectedItems = new List<SSPackageItem>();


    public void RemoveSelectedItem(SSPackageItem _selectedItem)
    {
        for (int i = 0; i < SelectedItems.Count; i++)
        {
            if (_selectedItem.packageSKU == SelectedItems[i].packageSKU && _selectedItem.titleText.text == SelectedItems[i].titleText.text)
            {
                SelectedItems[i].TuggleSelection();
                break;
            }
        }
        ShowCheckoutWindow(true);
    }
    public void SelectItem(SSPackageItem _item, bool toSelect)
    {
        if (SelectedItems == null)
            SelectedItems = new List<SSPackageItem>();

        if (!toSelect)
        {
            if (SelectedItems.Contains(_item))
                SelectedItems.Remove(_item);
        }
        else
        {
            if (!SelectedItems.Contains(_item))
                SelectedItems.Add(_item);
        }
        float fullPrice = 0f;
        for (int i = 0; i < SelectedItems.Count; i++)
        {
            fullPrice += SelectedItems[i].price;
        }
        ConfirmCheckoutButton.transform.Find("Text").GetComponent<Text>().text = "€" + fullPrice.ToString("F2");
        checkoutCounter.text = SelectedItems.Count.ToString();
        checkoutCounter.transform.parent.gameObject.SetActive(SelectedItems.Count > 0);
    }

    public void ShowCheckoutWindow(bool toShow = true)
    {
        if (SelectedItems.Count == 0 && !checkoutWindow.activeSelf)
            return;

        if (toShow)
        {
            Object SSPackageItemPrefabCheckout = Resources.Load<GameObject>("NecessaryPrefabs/UI/SSPackageItemCheckout");
            foreach (Transform child in checkoutContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                List<string> includedScenes = ppm.GetScenesInProduct(SelectedItems[i].packageSKU);
                GameObject packageButton = Instantiate(SSPackageItemPrefabCheckout, checkoutContent.transform) as GameObject;
                packageButton.GetComponent<SSPackageItem>().Setup(SelectedItems[i].titleText.text, includedScenes, SelectedItems[i].packageSKU, SelectedItems[i].price, this);
                packageButton.GetComponent<SSPackageItem>().SetInteractable(false);
            }
        }
        checkoutWindow.SetActive(toShow);
    }
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
        if (ppm == null)
            ppm = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (SSPackageItemPrefab == null)
            SSPackageItemPrefab = Resources.Load<GameObject>("NecessaryPrefabs/UI/SSPackageItem");
        //test data
        List<SSPackageData> SSPData = new List<SSPackageData>();
        List<SSPackageData> SSSData = new List<SSPackageData>();

        //string URLString = "https://leren.careup.online/ScenesStore.xml";
        //XmlTextReader reader = new XmlTextReader(URLString);
        //string ss = "";
        //while (reader.Read())
        //{
        //    switch (reader.NodeType)
        //    {
        //        case XmlNodeType.Element: // The node is an element.
        //            ss += ("<" + reader.Name);

        //            while (reader.MoveToNextAttribute()) // Read the attributes.
        //                ss += (" " + reader.Name + "='" + reader.Value + "'");
        //            ss += (">");
        //            break;
        //        case XmlNodeType.Text: //Display the text in each element.
        //            ss += (reader.Value);
        //            break;
        //        case XmlNodeType.EndElement: //Display the end of the element.
        //            ss += ("</" + reader.Name);
        //            ss += (">");
        //            break;
        //    }
        //}
        //Debug.Log(ss);

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/ScenesStore");
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        //XmlNodeList doorNodes = xmlFile.FirstChild.NextSibling.FirstChild.ChildNodes;
        foreach (XmlNode n in xmlFile.FirstChild.NextSibling.ChildNodes)
        {
            if (n.Attributes["name"] == null)
                continue;

            print(n.Attributes["name"].Value);
            foreach (XmlNode items in n.ChildNodes)
            {
                string itemTitle = "";
                string itemSKU = "";
                float itemPrice = 0f;

                if (items.Attributes["title"] != null)
                    itemTitle = items.Attributes["title"].Value;
                if (items.Attributes["SKU"] != null)
                    itemSKU = items.Attributes["SKU"].Value;
                if (items.Attributes["price"] != null)
                {
                    float.TryParse(items.Attributes["price"].Value, out itemPrice);
                }
                if (n.Attributes["name"].Value == "packages")
                    SSPData.Add(new SSPackageData(itemTitle, itemSKU, itemPrice));
                else if(n.Attributes["name"].Value == "scenes")
                    SSSData.Add(new SSPackageData(itemTitle, itemSKU, itemPrice));
            }
        }
        for (int i = 0; i < SSPData.Count; i++)
        {
            List<string> includedScenes = ppm.GetScenesInProduct(SSPData[i].SKU);
            GameObject packageButton = Instantiate(SSPackageItemPrefab, packageStoreContent.transform) as GameObject;
            packageButton.GetComponent<SSPackageItem>().Setup(SSPData[i].title, includedScenes, SSPData[i].SKU, SSPData[i].price, this);
        }
        for (int i = 0; i < SSSData.Count; i++)
        {
            GameObject sceneButton = Instantiate(SSPackageItemPrefab, sceneStoreContent.transform) as GameObject;
            sceneButton.GetComponent<SSPackageItem>().Setup(SSSData[i].title, null, SSSData[i].SKU, SSSData[i].price, this);
        }
    }
    void Start()
    {
        openTabImage = tabsButtons[0].GetComponent<Image>().sprite;
        closeTabImage = tabsButtons[1].GetComponent<Image>().sprite;
        LoadStoreData();
    }

}
