using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class StoreItem
{
    public int index;
    public int price;
    public string name;
    public string category;
    public bool purchased;

    public StoreItem() { index = -1; price = 0; }
    public StoreItem(int i, int p, string n, string c, bool s)
        { index = i; price = p; name = n; category = c; purchased = s; }
}

public class StoreManager 
{
    private int currentCurrency = 0;
    private int currentPresents = 0;
    private List<StoreItem> storeItems = new List<StoreItem>();

    public int Currency { get { return currentCurrency; } }
    public int Presents { get { return currentPresents; } }

    public void Init(string storeXml = "Store")
    {
        // load up all items from xml into the list
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/" + storeXml);

        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlItemList = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode xmlSceneNode in xmlItemList)
        {
            int index = -1, price = 1;
            int.TryParse(xmlSceneNode.Attributes["index"].Value, out index);
            int.TryParse(xmlSceneNode.Attributes["price"].Value, out price);
            bool purchased = DatabaseManager.FetchField("Store", index.ToString()) == "true";

            string name = xmlSceneNode.Attributes["name"].Value;
            string category = xmlSceneNode.Attributes["name"].Value;

            storeItems.Add(new StoreItem(index, price, name, category, purchased));
        }

        // get amount of currency/presents saved
        int.TryParse(DatabaseManager.FetchField("Store", "Currency"), out currentCurrency);
        int.TryParse(DatabaseManager.FetchField("Store", "Presents"), out currentPresents);
    }

    public void ModifyCurrencyBy(int amount)
    {
        currentCurrency += amount;
        DatabaseManager.UpdateField("Store", "Currency", currentCurrency.ToString());
    }

    public void ModifyPresentsBy(int amount)
    {
        currentPresents += amount;
        DatabaseManager.UpdateField("Store", "Presents", currentPresents.ToString());
    }

    public bool Purchase(int itemIndex)
    {
        StoreItem item = storeItems.Find(x => x.index == itemIndex);
        if (item.index != -1 && currentCurrency >= item.price)
        {
            ModifyCurrencyBy(-item.price);
            item.purchased = true;
            DatabaseManager.UpdateField("Store", itemIndex.ToString(), "true");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetPurchasedState(int itemIndex)
    {
        StoreItem item = storeItems.Find(x => x.index == itemIndex);
        return (item.index != -1) ? item.purchased : false;
    }
    
    List<StoreItem> GetStoreItemsByCategory(string categoryName)
    {
        return storeItems.FindAll(x => x.category == categoryName);
    }

    public List<List<StoreItem>> GetAllStoreItemsCategorized()
    {
        List<string> catNames = new List<string>();
        foreach (StoreItem item in storeItems)
        {
            if (!catNames.Contains(item.category))
                catNames.Add(item.category);
        }

        List<List<StoreItem>> result = new List<List<StoreItem>>();
        foreach(string category in catNames)
            result.Add(GetStoreItemsByCategory(category));

        return result;
    }

    // random present usage?
    public StoreItem GetRandomStoreItem(bool notPurchased = true, bool weighedByPrice = true)
    {
        List<StoreItem> items = new List<StoreItem>(storeItems);

        items.RemoveAll(x => x.price == 0);

        if (notPurchased)
        {
            items.RemoveAll(x => x.purchased == true);
        }

        if (weighedByPrice)
        {
            // get all different prices
            List<int> prices = new List<int>();
            foreach (StoreItem i in items)
            {
                if (!prices.Contains(i.price))
                    prices.Add(i.price);
            }

            // balance them out
            prices.Sort();
            float priceSum = 0;
            foreach (int i in prices)
                priceSum += 1.0f / i;
            float r = Random.Range(0.0f, priceSum);
            int result = 0;
            do {
                r -= 1.0f / prices[result++];
            } while (r > 0);

            items.RemoveAll(x => x.price != prices[result-1]);
        }

        return items[Random.Range(0, items.Count - 1)];
    }

    /// <summary>
    /// Attempt to unpack present and get reward
    /// </summary>
    /// <returns>Recieved item is returned</returns>
    public StoreItem UnpackPresent()
    {
        if (storeItems.FindAll(x => x.purchased == false).Count == 0)
            return null;

        if (currentPresents == 0)
            return null;

        ModifyPresentsBy(-1);
        StoreItem item = GetRandomStoreItem();
        item.purchased = true;
        DatabaseManager.UpdateField("Store", item.index.ToString(), "true");

        return item;
    }
}
