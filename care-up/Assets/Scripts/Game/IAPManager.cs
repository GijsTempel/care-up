using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;

public class IAPManager : MonoBehaviour, IStoreListener
{
#pragma warning disable CS0414
    private IExtensionProvider extensions;
#pragma warning restore CS0414

    private IStoreController controller;

    private IAppleExtensions m_AppleExtensions;

    Dictionary<string, string> introductory_info_dict = null;

    private void Start()
    {
#if UNITY_IOS
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct("CareUp_Lidmaatschap", ProductType.Subscription, new IDs
            {
                {"CareUp_Lidmaatschap", AppleAppStore.Name},
                {"CareUp_Lidmaatschap", GooglePlay.Name},
                {"CareUp_Lidmaatschap", MacAppStore.Name}
            });

            UnityPurchasing.Initialize(this, builder);
#else
        //Debug.Log("Not an iOS. Destroying IAP.");
        Destroy(this);
        return;
#endif
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

#if UNITY_IOS
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();
#endif
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {

    }

    public void OnPurchaseClicked(string productId = "CareUp_Lidmaatschap")
    {
        controller.InitiatePurchase(productId);
    }

    public bool SubscriptionPurchased(string id = "CareUp_Lidmaatschap")
    {
        Product sub = controller.products.WithID(id);
        if (sub.availableToPurchase && sub.receipt != null)
        {
            if (checkIfProductIsAvailableForSubscriptionManager(sub.receipt))
            {
                string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(sub.definition.storeSpecificId)) ? null : introductory_info_dict[sub.definition.storeSpecificId];
                SubscriptionManager p = new SubscriptionManager(sub, intro_json);
                SubscriptionInfo info = p.getSubscriptionInfo();
                Debug.Log("IAPManager::SubscriptionPurchased(CareUp_Lidmaatschap) == " +
                    (info.isSubscribed() == Result.True));
                return info.isSubscribed() == Result.True;
            }
        }

        Debug.Log("IAPManager::SubscriptionPurchased(CareUp_Lidmaatschap) == false");
        return false;
    }

    private bool checkIfProductIsAvailableForSubscriptionManager(string receipt)
    {
        var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
        if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload"))
        {
            Debug.Log("The product receipt does not contain enough information");
            return false;
        }
        var store = (string)receipt_wrapper["Store"];
        var payload = (string)receipt_wrapper["Payload"];

        if (payload != null)
        {
            switch (store)
            {
                case GooglePlay.Name:
                    {
                        var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                        if (!payload_wrapper.ContainsKey("json"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                            return false;
                        }
                        var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                        if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                            return false;
                        }
                        var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                        var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                        if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                            return false;
                        }
                        return true;
                    }
                case AppleAppStore.Name:
                case AmazonApps.Name:
                case MacAppStore.Name:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        return false;
    }
}