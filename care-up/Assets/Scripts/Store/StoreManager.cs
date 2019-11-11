using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using CareUpAvatar;

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

public class StoreCategory
{
    public List<StoreItem> items;
    public string name;
    public string icon;

    public StoreCategory() { items = new List<StoreItem>(); name = icon = ""; }
    public StoreCategory(List<StoreItem> list, string n, string i)
    { items = new List<StoreItem>(list); name = n; icon = i; }
}

public class CharacterItem
{
    public int index;
    public int price;
    public bool purchased;

    public PlayerAvatarData playerAvatar;
    public PlayerAvatarData defaultAvatarData;

    public CharacterItem() { index = -1; price = 0; }
    public CharacterItem(int indexValue, int priceValue, bool purchasedValue, PlayerAvatarData playerAvatarValue)
    {
        index = indexValue;
        price = priceValue;
        purchased = purchasedValue;
        playerAvatar = playerAvatarValue;
        defaultAvatarData = playerAvatarValue;
    }
}

public class StoreManager
{
    private int currentCurrency = 0;
    private int currentPresents = 0;
    private List<StoreCategory> storeItems = new List<StoreCategory>();
    private List<CharacterItem> characterItems = new List<CharacterItem>();

    public List<StoreCategory> StoreItems { get { return storeItems; } }

    public List<CharacterItem> CharacterItems { get { return characterItems; } }

    public int Currency { get { return currentCurrency; } }
    public int Presents { get { return currentPresents; } }

    public int GetItemIndex(int num)
    {
        if (num >= 0 && num < characterItems.Count)
        {
            int result = characterItems[num].index;
            return result;
        }
        return -1;
    }

    public void Init(string storeXml = "Store", string characterStoreXml = "CharacterStore")
    {
        bool devDropAllPurchases = false; // change this to true once to clear all purchases
        bool devAddCurrency = false; // change this to true once to get 100 currency

        storeItems = new List<StoreCategory>();
        characterItems = new List<CharacterItem>();

        // load up all items from xml into the list
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/" + storeXml);

        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlCatList = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode xmlCatNode in xmlCatList)
        {
            List<StoreItem> catItems = new List<StoreItem>();
            string catName = (xmlCatNode.Attributes["name"] != null) ? xmlCatNode.Attributes["name"].Value : "";
            foreach (XmlNode xmlSceneNode in xmlCatNode.ChildNodes)
            {
                int index = -1, price = 1;
                int.TryParse(xmlSceneNode.Attributes["index"].Value, out index);
                int.TryParse(xmlSceneNode.Attributes["price"].Value, out price);
                bool purchased = DatabaseManager.FetchField("Store", "StoreItem_" + index.ToString()) == "true";

                if (devDropAllPurchases)
                {
                    purchased = false;
                    DatabaseManager.UpdateField("Store", "StoreItem_" + index.ToString(), "false");
                }

                string name = xmlSceneNode.Attributes["name"].Value;
                string category = (xmlSceneNode.Attributes["category"] != null) ? xmlSceneNode.Attributes["category"].Value : catName;

                catItems.Add(new StoreItem(index, price, name, category, purchased));
            }
            string catIcon = (xmlCatNode.Attributes["icon"] != null) ? xmlCatNode.Attributes["icon"].Value : "";
            storeItems.Add(new StoreCategory(catItems, catName, catIcon));
        }

        textAsset = (TextAsset)Resources.Load("Xml/" + characterStoreXml);
        xmlFile.LoadXml(textAsset.text);

        XmlNodeList xmlCharacterList = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode xmlSceneNode in xmlCharacterList)
        {
            int index = -1, price = 1;

            int.TryParse(xmlSceneNode.Attributes["index"].Value, out index);
            int.TryParse(xmlSceneNode.Attributes["price"].Value, out price);

            string gender = xmlSceneNode.Attributes["gender"].Value;
            int.TryParse(xmlSceneNode.Attributes["glassesType"].Value, out int glassesType);
            int.TryParse(xmlSceneNode.Attributes["bodyType"].Value, out int bodyType);
            int.TryParse(xmlSceneNode.Attributes["headType"].Value, out int headType);
            int.TryParse(xmlSceneNode.Attributes["mouth"].Value, out int mouthType);
            int.TryParse(xmlSceneNode.Attributes["eye"].Value, out int eyeType);
            string hatType = "";
            if (xmlSceneNode.Attributes["hatType"] != null)
                hatType = xmlSceneNode.Attributes["hatType"].Value;

            bool purchased = DatabaseManager.FetchField("Store", "CharacterItem_" + index.ToString()) == "true";

            if (devDropAllPurchases)
            {
                purchased = false;
                DatabaseManager.UpdateField("Store", "CharacterItem_" + index.ToString(), "false");
            }

            Gender characterGender = (gender == "Female") ? Gender.Female : Gender.Male;
            PlayerAvatarData playerAvatar = new PlayerAvatarData(characterGender, headType, bodyType, glassesType, hatType, mouthType, eyeType);
            CharacterItem characterItem = new CharacterItem(index, price, purchased, playerAvatar);
            //if (devDropAllPurchases)
            //{
            //    string[][] charactersCategory = DatabaseManager.FetchCategory("CharacterItem_" + index.ToString());
            //    if (charactersCategory != null)
            //    {
            //        foreach (string[] field in charactersCategory)
            //        {
            //            DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), "Sex", gender.ToString());
            //            DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), "Body", bodyType.ToString());
            //            DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), "Head", headType.ToString());
            //            DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), "Hat", hatType);
            //            DatabaseManager.UpdateField("CharacterItem_" + index.ToString(), "Glasses", glassesType.ToString());

            //        }
            //    }
            //}

            if (!devDropAllPurchases && purchased)
            {
                string[][] charactersCategory = DatabaseManager.FetchCategory("CharacterItem_" + index.ToString());
                if (charactersCategory != null)
                {
                    foreach (string[] field in charactersCategory)
                    {
                        switch (field[0])
                        {
                            case "Index":
                                int.TryParse(field[1], out index); break;
                            case "Price":
                                int.TryParse(field[1], out price); break;
                            case "Purchased":
                                bool.TryParse(field[1], out purchased); break;
                            case "Sex":
                                gender = field[1]; break;
                            //case "Head":
                            //    int.TryParse(field[1], out headType); break;
                            case "Body":
                                int.TryParse(field[1], out bodyType); break;
                            case "Glasses":
                                int.TryParse(field[1], out glassesType); break;
                            case "Hat":
                                hatType = field[1]; break;
                        }
                    }
                }
            }
            PlayerAvatarData customizedPlayerAvatar = new PlayerAvatarData(characterGender, headType, bodyType, glassesType, hatType, mouthType, eyeType);
            characterItem.playerAvatar = customizedPlayerAvatar;
            characterItems.Add(characterItem);
        }

        // get amount of currency/presents saved
        int.TryParse(DatabaseManager.FetchField("Store", "Currency"), out currentCurrency);
        //int.TryParse(DatabaseManager.FetchField("Store", "Presents"), out currentPresents);

        if (devAddCurrency) ModifyCurrencyBy(300);
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

    public StoreItem FindItemByIndex(int index)
    {
        StoreItem result = new StoreItem();

        foreach (StoreCategory cat in storeItems)
        {
            result = cat.items.Find(x => x.index == index);
            if (result != null && result.index != -1) break;
        }

        return result;
    }

    public CharacterItem FindCharacterByIndex(int index)
    {
        CharacterItem result = new CharacterItem();
        result = characterItems.Find(x => x.index == index);
        return result;
    }

    public bool Purchase(int itemIndex)
    {
        StoreItem item = FindItemByIndex(itemIndex);
        if (item.index != -1 && currentCurrency >= item.price)
        {
            ModifyCurrencyBy(-item.price);
            item.purchased = true;
            DatabaseManager.UpdateField("Store", "StoreItem_" + itemIndex.ToString(), "true");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AdjustCharacter(int itemIndex)
    {
        CharacterItem item = characterItems.Find(x => x.index == (itemIndex));
        if (itemIndex != CharacterInfo.index)
            CharacterInfo.SetCharacterCharacteristicsWU(item);
    }

    public bool PurchaseCharacter(int itemIndex)
    {
        if (itemIndex < 0)
            return false;
        CharacterItem item = characterItems.Find(x => x.index == (itemIndex));

        if (item != null)
        {
            if (item.index != -1 && currentCurrency >= item.price)
            {
                ModifyCurrencyBy(-item.price);
                item.purchased = true;
                DatabaseManager.UpdateField("Store", "CharacterItem_" + itemIndex.ToString(), "true"); // temporarily

                CharacterInfo.SetCharacterCharacteristicsWU(item);

                return true;
            }
        }
        return false;
    }

    public void Fitting(int itemIndex) { }

    public bool GetPurchasedState(int itemIndex)
    {
        StoreItem item = FindItemByIndex(itemIndex);
        return (item.index != -1) ? item.purchased : false;
    }

    public List<StoreItem> GetStoreItemsByCategoryName(string categoryName)
    {
        StoreCategory category = storeItems.Find(x => x.name == categoryName);
        return (category.name != "") ? category.items : null;
    }

    // random present usage?
    public StoreItem GetRandomStoreItem(bool notPurchased = true, bool weighedByPrice = true)
    {
        List<StoreItem> items = new List<StoreItem>();
        foreach (StoreCategory cat in storeItems)
        {
            foreach (StoreItem item in cat.items)
            {
                items.Add(item);
            }
        }

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
            do
            {
                r -= 1.0f / prices[result++];
            } while (r > 0);

            items.RemoveAll(x => x.price != prices[result - 1]);
        }

        return (items.Count > 0) ? items[Random.Range(0, items.Count - 1)] : null;
    }

    /// <summary>
    /// Attempt to unpack present and get reward
    /// </summary>
    /// <returns>Recieved item is returned</returns>
    public StoreItem UnpackPresent()
    {
        if (currentPresents == 0)
            return null;

        ModifyPresentsBy(-1);
        StoreItem item = GetRandomStoreItem();

        if (item != null)
        {
            item.purchased = true;
            DatabaseManager.UpdateField("Store", "StoreItem_" + item.index.ToString(), "true");
        }

        return item;
    }
}
