using Godot;
using Lavos.Services.Store;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using Lavos.Utils;

namespace Lavos.Services.GoogleBilling;

sealed class GoogleBillingStoreService : IStoreService
{
    public event Action<Purchase> ProductPurchased;

    const string PluginName = "GoogleBilling";
    readonly LavosPlugin Plugin;

    List<StoreProduct> _products = [];
    List<StoreProduct> _subscriptions = [];
    List<string> _tokens = [];

    public GoogleBillingStoreService()
    {
        Assert.IsTrue(IsPluginEnabled(), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public static bool IsPluginEnabled() => Engine.HasSingleton(PluginName);

    public void Initialize() => Plugin.CallVoid("init");

    #region Query

    public async Task<QueryProductsStatus> QueryProducts(IReadOnlyList<string> ids)
    {
        var args = Variant.CreateFrom(ids.ToArray());
        Plugin.CallVoid("queryProductsDetails", args);
        //
        QueryProductsStatus status;
        do
        {
            await Task.Delay(100);
            status = (QueryProductsStatus)Plugin.CallInt("getQueryProductsStatus");
        } while (status == QueryProductsStatus.InProgress);
        //
        if (status == QueryProductsStatus.Error)
        {
            Log.Error(PluginName, "Failed to query products");
            return status;
        }
        //
        _products.Clear();
        var products = Plugin.CallStringArray("getProducts");
        foreach (var product in products)
        {
            var storeProduct = JsonHelper.Deserialize<StoreProduct>(product);
            _products.Add(storeProduct);
        }
        //
        return status;
    }

    public List<StoreProduct> GetProducts() => _products;

    public StoreProduct GetProduct(string id)
    {
        var storeProduct = _products.Find(product => product.Id == id);
        if (storeProduct != null) return storeProduct;
        //
        var arg = Variant.CreateFrom(id);
        var productStr = Plugin.CallString("getProduct", arg);
        return JsonHelper.Deserialize<StoreProduct>(productStr);
    }

    public async Task<QuerySubscriptionsStatus> QuerySubscriptions(IReadOnlyList<string> ids)
    {
        var args = Variant.CreateFrom(ids.ToArray());
        Plugin.CallVoid("querySubscriptionsDetails", args);
        //
        QuerySubscriptionsStatus status;
        do
        {
            await Task.Delay(100);
            status = (QuerySubscriptionsStatus)Plugin.CallInt("getQuerySubscriptionsStatus");
        } while (status == QuerySubscriptionsStatus.InProgress);
        //
        if (status == QuerySubscriptionsStatus.Error)
        {
            Log.Error(PluginName, "Failed to query subscriptions");
            return status;
        }
        //
        _subscriptions.Clear();
        var products = Plugin.CallStringArray("getSubscriptions");
        foreach (var product in products)
        {
            var storeProduct = JsonHelper.Deserialize<StoreProduct>(product);
            _subscriptions.Add(storeProduct);
        }
        //
        return status;
    }

    public List<StoreProduct> GetSubscriptions() => _subscriptions;

    public StoreProduct GetSubscription(string id)
    {
        var storeProduct = _subscriptions.Find(product => product.Id == id);
        if (storeProduct != null) return storeProduct;
        //
        var arg = Variant.CreateFrom(id);
        var product = Plugin.CallString("getSubscription", arg);
        return JsonHelper.Deserialize<StoreProduct>(product);
    }

    #endregion Query

    #region Purchase

    public async Task<PurchaseResult> PurchaseProduct(string id)
    {
        var arg = Variant.CreateFrom(id);
        var ok = Plugin.CallBool("purchaseProduct", arg);
        if (!ok)
        {
            return PurchaseResult.Error;
        }
        //
        PurchaseStatus status;
        do
        {
            await Task.Delay(100);
            status = (PurchaseStatus)Plugin.CallInt("getPurchaseStatus");
        } while (status == PurchaseStatus.InProgress);
        //
        if (status == PurchaseStatus.Error)
        {
            return PurchaseResult.Error;
        }
        //
        return status switch
        {
            PurchaseStatus.None => PurchaseResult.Canceled,
            PurchaseStatus.Error => PurchaseResult.Error,
            PurchaseStatus.Completed => PurchaseResult.Success,
            _ => PurchaseResult.Unknown,
        };
    }

    public async Task<PurchaseResult> PurchaseSubscription(string id, OfferDetails offer)
    {
        var argId = Variant.CreateFrom(id);
        var argOfferToken = Variant.CreateFrom(offer.Token);
        var ok = Plugin.CallBool("purchaseSubscription", argId, argOfferToken);
        if (!ok)
        {
            return PurchaseResult.Error;
        }
        //
        PurchaseStatus status;
        do
        {
            await Task.Delay(100);
            status = (PurchaseStatus)Plugin.CallInt("getPurchaseStatus");
        } while (status == PurchaseStatus.InProgress);
        //
        if (status == PurchaseStatus.Error)
        {
            return PurchaseResult.Error;
        }
        //
        return status switch
        {
            PurchaseStatus.None => PurchaseResult.Canceled,
            PurchaseStatus.Error => PurchaseResult.Error,
            PurchaseStatus.Completed => PurchaseResult.Success,
            _ => PurchaseResult.Unknown,
        };
    }

    public async Task ProcessPendingPurchases()
    {
        var pendingPurchasesStatus = await QueryPendingPurchases();
        if (pendingPurchasesStatus == QueryPendingPurchasesStatus.Error)
        {
            return;
        }
        //
        var purchases = GetPendingPurchases();
        foreach (var purchase in purchases)
        {
            await ConsumePurchase(purchase);
        }
        //
        pendingPurchasesStatus = await QueryPendingPurchases();
        if (pendingPurchasesStatus == QueryPendingPurchasesStatus.Error)
        {
            return;
        }
        //
        purchases = GetPendingPurchases();
        foreach (var purchase in purchases)
        {
            await AcknowledgePurchase(purchase);
        }
    }

    #endregion Purchase

    #region Pending

    async Task<QueryPendingPurchasesStatus> QueryPendingPurchases()
    {
        Plugin.CallVoid("queryPurchases");
        //
        QueryPendingPurchasesStatus status;
        do
        {
            await Task.Delay(100);
            status = (QueryPendingPurchasesStatus)Plugin.CallInt("getQueryPurchasesStatus");
        } while (status == QueryPendingPurchasesStatus.InProgress);
        //
        switch (status)
        {
            case QueryPendingPurchasesStatus.Error: Log.Error(PluginName, "Failed to query purchases"); break;
            case QueryPendingPurchasesStatus.Completed: break;
            default: Log.Warn(PluginName, "Unhandled query pending purchase case"); break;
        }
        //
        return status;
    }

    List<Purchase> GetPendingPurchases()
    {
        var list = new List<Purchase>();
        var purchases = Plugin.CallStringArray("getPendingPurchases").ToList();
        foreach (var json in purchases)
        {
            var purchase = JsonHelper.Deserialize<Purchase>(json);
            if (purchase.PurchaseState == PurchaseState.Pending) continue;
            list.Add(purchase);
        }
        return list;
    }

    #endregion Pending

    #region Consume

    async Task ConsumePurchase(Purchase purchase)
    {
        var arg = Variant.CreateFrom(purchase.PurchaseToken);
        Plugin.CallVoid("consumePurchase", arg);
        //
        ConsumePurchaseStatus status;
        do
        {
            await Task.Delay(100);
            status = (ConsumePurchaseStatus)Plugin.CallInt("getConsumePurchaseStatus");
        } while (status == ConsumePurchaseStatus.InProgress);
        //
        switch (status)
        {
            case ConsumePurchaseStatus.Error: Log.Error(PluginName, $"Failed to consume purchase: {purchase.ProductId}"); break;
            case ConsumePurchaseStatus.Completed:
                {
                    Log.Debug(PluginName, $"Purchase consumed: {purchase.ProductId}");
                    ProductPurchased?.Invoke(purchase);
                }
                break;
            default: Log.Warn(PluginName, "Unhandled consume case"); break;
        }
    }

    async Task AcknowledgePurchase(Purchase purchase)
    {
        var arg = Variant.CreateFrom(purchase.PurchaseToken);
        Plugin.CallVoid("acknowledgePurchase", arg);
        //
        AcknowledgePurchaseStatus status;
        do
        {
            await Task.Delay(100);
            status = (AcknowledgePurchaseStatus)Plugin.CallInt("getAcknowledgePurchaseStatus");
        } while (status == AcknowledgePurchaseStatus.InProgress);
        //
        switch (status)
        {
            case AcknowledgePurchaseStatus.Error: Log.Error(PluginName, $"Failed to acknowledge purchase: {purchase.ProductId}"); break;
            case AcknowledgePurchaseStatus.Completed:
                {
                    Log.Debug(PluginName, $"Purchase acknowledged: {purchase.ProductId}");
                    ProductPurchased?.Invoke(purchase);
                }
                break;
            default: Log.Warn(PluginName, "Unhandled acknowledge case"); break;
        }
    }

    #endregion Consume
}
