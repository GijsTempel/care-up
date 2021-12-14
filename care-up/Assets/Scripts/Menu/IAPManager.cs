using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private static IAppleExtensions m_AppleExtensions;

    public List<string> purchasedScenes = new List<string>();

    // Product identifiers for all products capable of being purchased: 
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.

    //public static string kProductIDSubscription = "subscription";

    // Apple App Store-specific product identifier for the subscription product.

    //private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    void Start()
    {
        Debug.Log("IAP start");
        
        purchasedScenes.Clear();

        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 

        { // Product List
        builder.AddProduct("BMVS", ProductType.Subscription, new IDs(){
                { "1575230619", AppleAppStore.Name },
            });
        builder.AddProduct("VVSO", ProductType.Subscription, new IDs(){
                { "1576183963", AppleAppStore.Name },
            });
        builder.AddProduct("CATMSO", ProductType.Subscription, new IDs(){
                { "1576184294", AppleAppStore.Name },
            });
        builder.AddProduct("BGM", ProductType.Subscription, new IDs(){
                { "1576184926", AppleAppStore.Name },
            });
        builder.AddProduct("INSIG", ProductType.Subscription, new IDs(){
                { "1576187688", AppleAppStore.Name },
            });
        builder.AddProduct("IILTGF", ProductType.Subscription, new IDs(){
                { "1576188084", AppleAppStore.Name },
            });
        builder.AddProduct("SCII", ProductType.Subscription, new IDs(){
                { "1576188373", AppleAppStore.Name },
            });
        builder.AddProduct("GFTP", ProductType.Subscription, new IDs(){
                { "1576188275", AppleAppStore.Name },
            });
        builder.AddProduct("ISSI", ProductType.Subscription, new IDs(){
                { "1576188663", AppleAppStore.Name },
            });
        builder.AddProduct("ISHTGF", ProductType.Subscription, new IDs(){
                { "1576188663", AppleAppStore.Name },
            });
        builder.AddProduct("SCAED", ProductType.Subscription, new IDs(){
                { "1576188950", AppleAppStore.Name },
            });
        builder.AddProduct("FRAXI", ProductType.Subscription, new IDs(){
                { "1576188843", AppleAppStore.Name },
            });
        builder.AddProduct("THD", ProductType.Subscription, new IDs(){
                { "1576189523", AppleAppStore.Name },
            });
        builder.AddProduct("THR", ProductType.Subscription, new IDs(){
                { "1576197117", AppleAppStore.Name },
            });
        builder.AddProduct("TMMW", ProductType.Subscription, new IDs(){
                { "1576197409", AppleAppStore.Name },
            });
        builder.AddProduct("TVNM", ProductType.Subscription, new IDs(){
                { "1576197990", AppleAppStore.Name },
            });
        builder.AddProduct("AMVP", ProductType.Subscription, new IDs(){
                { "1576198218", AppleAppStore.Name },
            });
        builder.AddProduct("TMOT", ProductType.Subscription, new IDs(){
                { "1576198465", AppleAppStore.Name },
            });
        builder.AddProduct("MSSI", ProductType.Subscription, new IDs(){
                { "1576198665", AppleAppStore.Name },
            });
        builder.AddProduct("OSTMOPS", ProductType.Subscription, new IDs(){
                { "1576199363", AppleAppStore.Name },
            });
        builder.AddProduct("SOC", ProductType.Subscription, new IDs(){
                { "1576199767", AppleAppStore.Name },
            });
        builder.AddProduct("VSV", ProductType.Subscription, new IDs(){
                { "1576200395", AppleAppStore.Name },
            });
        builder.AddProduct("SMPA", ProductType.Subscription, new IDs(){
                { "1576200732", AppleAppStore.Name },
            });
        builder.AddProduct("SMPF", ProductType.Subscription, new IDs(){
                { "1576201010", AppleAppStore.Name },
            });
        builder.AddProduct("SMPD", ProductType.Subscription, new IDs(){
                { "1576202323", AppleAppStore.Name },
            });
        builder.AddProduct("ZOKR", ProductType.Subscription, new IDs(){
                { "1576202605", AppleAppStore.Name },
            });
        }

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }
    
    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        Debug.Log("IsInitialized? " + (m_StoreController != null) + " " + (m_StoreExtensionProvider != null));
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product:" + product.definition.id.ToString()));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    /// <summary>
    /// This will be called when Unity IAP has finished initialising.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP OnInitialized");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();

        // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
        // On non-Apple platforms this will have no effect; OnDeferred will never be called.
        m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
        
        Dictionary<string, string> introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();
        // Sample code for expose product sku details for apple store
        //Dictionary<string, string> product_details = m_AppleExtensions.GetProductDetails();

        //Debug.Log("Available items:");
        foreach (var item in controller.products.all)
        {
            if (item.availableToPurchase)
            {
                /*Debug.Log(string.Join(" - ",
                    new[]
                    {
                        item.metadata.localizedTitle,
                        item.metadata.localizedDescription,
                        item.metadata.isoCurrencyCode,
                        item.metadata.localizedPrice.ToString(),
                        item.metadata.localizedPriceString,
                        item.transactionID,
                        item.receipt
                    }));*/

                // this is the usage of SubscriptionManager class
                if (item.receipt != null) {
                    if (item.definition.type == ProductType.Subscription) {
                        if (checkIfProductIsAvailableForSubscriptionManager(item.receipt)) {
                            string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
                            SubscriptionManager p = new SubscriptionManager(item, intro_json);
                            SubscriptionInfo info = p.getSubscriptionInfo();
                            
                            if (info.isSubscribed() == Result.True)
                            {
                                Debug.Log("Product purchased detected: " + info.getProductId());
                                AddSceneToPurchased(info.getProductId());
                            }

                            //Debug.Log("product id is: " + info.getProductId());
                            //Debug.Log("purchase date is: " + info.getPurchaseDate());
                            //Debug.Log("subscription next billing date is: " + info.getExpireDate());
                            //Debug.Log("is subscribed? " + info.isSubscribed().ToString());
                            //Debug.Log("is expired? " + info.isExpired().ToString());
                            //Debug.Log("is cancelled? " + info.isCancelled());
                            //Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
                            //Debug.Log("product is auto renewing? " + info.isAutoRenewing());
                            //Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
                            //Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
                            //Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
                            //Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
                            //Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());
                        }
                        else {
                            Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
                        }
                    } else {
                        Debug.Log("the product is not a subscription product");
                    }
                } else {
                    Debug.Log("the product should have a valid receipt");
                } 
            }
        }
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

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    /// <summary>
    /// iOS Specific.
    /// This is called as part of Apple's 'Ask to buy' functionality,
    /// when a purchase is requested by a minor and referred to a parent
    /// for approval.
    ///
    /// When the purchase is approved or rejected, the normal purchase events will fire.
    /// </summary>
    /// <param name="item">Item.</param>
    private void OnDeferred(Product item)
    {
        Debug.Log("Purchase deferred: " + item.definition.id);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        /*
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
            // ScoreManager.score += 100;
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        // Or ... a subscription product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The subscription item has been successfully purchased, grant this to the player.
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }
        */

        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        AddSceneToPurchased(args.purchasedProduct.definition.id);

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    void AddSceneToPurchased(string id)
    {
        if (purchasedScenes.IndexOf(id) < 0)
            purchasedScenes.Add(id);

        PlayerPrefsManager.AddSKU(id);
    }
}
