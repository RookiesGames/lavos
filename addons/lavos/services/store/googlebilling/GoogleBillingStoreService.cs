using Godot;
using Lavos.Services.Store;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Lavos.Utils;

namespace Lavos.Services.GoogleBilling;

sealed class GoogleBillingStoreService : IStoreService
{
    const string PluginName = "GoogleBilling";
    readonly LavosPlugin Plugin;

    List<StoreProduct> _products = new();
    List<StoreProduct> _subscriptions = new();
    List<string> _tokens = new();

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
        if (!ok) return PurchaseResult.Error;
        //
        PurchaseStatus status;
        do
        {
            await Task.Delay(100);
            status = (PurchaseStatus)Plugin.CallInt("getPurchaseStatus");
        } while (status == PurchaseStatus.InProgress);
        //
        return status switch
        {
            PurchaseStatus.None => PurchaseResult.Canceled,
            PurchaseStatus.Error => PurchaseResult.Error,
            PurchaseStatus.Completed => PurchaseResult.Success,
            _ => PurchaseResult.Unknown,
        };
    }

    // TODO: Verify purchase flow, query-consume-acknowledge
    public async Task<QueryPurchasesStatus> QueryPurchases()
    {
        Plugin.CallVoid("queryPurchases");
        //
        QueryPurchasesStatus status;
        do
        {
            await Task.Delay(100);
            status = (QueryPurchasesStatus)Plugin.CallInt("getQueryPurchasesStatus");
        } while (status == QueryPurchasesStatus.InProgress);
        //
        if (status == QueryPurchasesStatus.Error)
        {
            Log.Error(PluginName, "Failed to query purchases");
            return status;
        }
        //
        _tokens = Plugin.CallStringArray("getPendingPurchases").ToList();
        return status;
    }

    public List<string> GetPendingPurchases() => _tokens;

    #endregion Purchase

    #region Consume

    public async Task<ConsumePurchaseStatus> ConsumePurchase(string token)
    {
        var arg = Variant.CreateFrom(token);
        Plugin.CallVoid("consumePurchase", arg);
        //
        ConsumePurchaseStatus status;
        do
        {
            await Task.Delay(100);
            status = (ConsumePurchaseStatus)Plugin.CallInt("getConsumePurchaseStatus");
        } while (status == ConsumePurchaseStatus.InProgress);
        //
        if (status == ConsumePurchaseStatus.Error)
        {
            Log.Error(PluginName, $"Failed to consume purchase. Token: {token}");
            return status;
        }
        //
        var products = Plugin.CallStringArray("getPendingConsumables");
        foreach (var product in products)
        {
            // TODO
            Log.Info(PluginName, $"{product} consume");
        }
        //
        var acknowledged = await AcknowledgePurchase(token);
        if (!acknowledged)
        {
            Log.Error(PluginName, $"Failed to acknowledge purchase. Token: {token}");
        }
        //
        return status;
    }

    async Task<bool> AcknowledgePurchase(string token)
    {
        var arg = Variant.CreateFrom(token);
        Plugin.CallVoid("acknowledgePurchase", arg);
        //
        AcknowledgePurchaseStatus status;
        do
        {
            await Task.Delay(100);
            status = (AcknowledgePurchaseStatus)Plugin.CallInt("getAcknowledgePurchaseStatus");
        } while (status == AcknowledgePurchaseStatus.InProgress);
        //        
        if (status == AcknowledgePurchaseStatus.Error)
        {
            Log.Error(PluginName, $"Failed to acknowledge purchase. Token: {token}");
            return false;
        }
        //
        Log.Debug($"Purchase acknowledged. Token: {token}");
        return true;
    }

    #endregion Consume
}
