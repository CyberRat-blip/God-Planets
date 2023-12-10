using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class InAppPurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public Button noAdsButton; // Указатель на кнопку отключения рекламы

    [Obsolete]
    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }

        if (PlayerPrefs.GetInt("noAdsPurchased", 0) == 1)
        {
            if (noAdsButton != null)
            {
                noAdsButton.interactable = false;
            }
        }
    }

    [Obsolete]
    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("com.disablead", ProductType.NonConsumable);
        builder.AddProduct("com.5lives", ProductType.Consumable);
        builder.AddProduct("com.15lives", ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyNoAds()
    {
        BuyProductID("com.disablead");
    }

    public void Buy5Lives()
    {
        BuyProductID("com.5lives");
    }

    public void Buy15Lives()
    {
        BuyProductID("com.15lives");
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"Purchasing product asynchronously: '{product.definition.id}'");
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"OnInitializeFailed InitializationFailureReason:{error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, "com.disablead", StringComparison.Ordinal))
        {
            Debug.Log("You've disabled ads!");
            PlayerPrefs.SetInt("noAdsPurchased", 1);
            PlayerPrefs.Save();
            if (noAdsButton != null) noAdsButton.interactable = false;
        }
        else if (String.Equals(args.purchasedProduct.definition.id, "com.5lives", StringComparison.Ordinal))
        {
            Debug.Log("You've added 5 lives!");
            // Логика добавления 5 жизней
        }
        else if (String.Equals(args.purchasedProduct.definition.id, "com.15lives", StringComparison.Ordinal))
        {
            Debug.Log("You've added 15 lives!");
            // Логика добавления 15 жизней
        }
        else
        {
            Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        throw new NotImplementedException();
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        throw new NotImplementedException();
    }

    void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new NotImplementedException();
    }

    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        throw new NotImplementedException();
    }
}
