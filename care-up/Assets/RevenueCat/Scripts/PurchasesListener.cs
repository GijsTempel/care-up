using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PurchasesListener : Purchases.UpdatedCustomerInfoListener
{
    // the one to purchase
    public Purchases.Package masterPackage = null;

    public override void CustomerInfoReceived(Purchases.CustomerInfo customerInfo)
    {
        // display new CustomerInfo
        Debug.Log("customer info?");
        Debug.Log("contains key? " + customerInfo.ActiveSubscriptions.Contains("online.careup.monthly"));

        // hardcheck for the only product we have, refactor code in case we'll use more then one product
        if (customerInfo.Entitlements.Active.ContainsKey("online.careup.monthly")) {
            PlayerPrefsManager.AddSKU("__ALL");
        }
    }

    public void Init()
    {
        var purchases = GetComponent<Purchases>();
        purchases.SetDebugLogsEnabled(true);
        purchases.GetOfferings((offerings, error) =>
        {
            if (error != null)
            {
                // show error
            }
            else
            {
                masterPackage = offerings.Current.AvailablePackages.First();
            }
        });
        purchases.SyncPurchases();
    }

    public void BeginPurchase(Purchases.Package package = null)
    {
        // make default as a master package, because that's the only one we have
        if (package == null)
        {
            package = masterPackage;
        }

        if (package == null)
        {
            // master package still can be null
            Debug.LogWarning("masterPackage = null");
            return;
        }

        var purchases = GetComponent<Purchases>();
        purchases.PurchasePackage(package, (productIdentifier, customerInfo, userCancelled, error) =>
        {
            if (!userCancelled)
            {
                if (error != null)
                {
                    // show error
                }
                else
                {
                    // show updated Customer Info
                    Debug.Log("did we purchase something successfully here?");
                    // i assume we did
                    if (customerInfo.Entitlements.Active.ContainsKey("online.careup.monthly")) {
                        PlayerPrefsManager.AddSKU("__ALL");
                    }
                }
            }
            else
            {
                // user cancelled, don't show an error
            }
        });
    }

    void RestoreClicked()
    {
        var purchases = GetComponent<Purchases>();
        purchases.RestorePurchases((customerInfo, error) =>
        {
            if (error != null)
            {
                // show error
            }
            else
            {
                // show updated Customer Info
            }
        });
    }
}