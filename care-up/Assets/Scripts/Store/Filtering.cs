using System.Collections.Generic;
using UnityEngine;

public enum FilterParam
{
    Free,
    Purchased,
    OnSale
}

public class Filtering 
{
    private TabGroup tabGroup;

    private void Start()
    {
        tabGroup = GameObject.FindObjectOfType<TabGroup>();
    }

    private bool ShowPurchased(StoreItem item)
    {
        return item.purchased;
    }

    private void ShowFree() { }

    public List<StoreCategory> Filter(FilterParam filter)
    {
        List<StoreCategory> storeCategory = new List<StoreCategory>();
        List<StoreItem> storeItems = new List<StoreItem>();

        foreach (StoreCategory category in PlayerPrefsManager.storeManager.StoreItems)
        {
            storeItems = new List<StoreItem>();

            foreach (StoreItem item in category.items)
            {
                switch (filter)
                {
                    case FilterParam.Free:
                        { }
                        break;

                    case FilterParam.Purchased:
                        {
                            if (ShowPurchased(item))
                                storeItems.Add(item);
                        }
                        break;

                    case FilterParam.OnSale:
                        {
                            if (!ShowPurchased(item))
                                storeItems.Add(item);
                        }
                        break;
                }
            }
            storeCategory.Add(new StoreCategory(storeItems, category.name, category.icon));
        }
        return storeCategory;
    }
}
