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

    public GoogleBillingStoreService()
    {
        Assert.IsTrue(IsPluginEnabled(), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public static bool IsPluginEnabled() => Engine.HasSingleton(PluginName);

    public void Initialize() => Plugin.CallVoid("init");

    public async Task<List<StoreProduct>> QueryProducts(IReadOnlyList<string> ids)
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
        var products = Plugin.CallStringArray("getProducts");
        foreach (var product in products)
        {
            var storeProduct = JsonHelper.Deserialize<StoreProduct>(product);
            _products.Add(storeProduct);
        }
        //
        return _products;
    }

    public StoreProduct GetProduct(string id)
    {
        var storeProduct = _products.Find(product => product.Id == id);
        if (storeProduct != null)
        {
            return storeProduct;
        }
        //
        var arg = Variant.CreateFrom(id);
        var product = Plugin.CallString("getProduct", arg);
        return JsonHelper.Deserialize<StoreProduct>(product);
    }

    public async Task<List<StoreProduct>> QuerySubscriptions(IReadOnlyList<string> ids)
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
        var products = Plugin.CallStringArray("getSubscriptions");
        foreach (var product in products)
        {
            var storeProduct = JsonHelper.Deserialize<StoreProduct>(product);
            _subscriptions.Add(storeProduct);
        }
        //
        return _subscriptions;
    }

    public StoreProduct GetSubscription(string id)
    {
        var storeProduct = _subscriptions.Find(product => product.Id == id);
        if (storeProduct != null)
        {
            return storeProduct;
        }
        //
        var arg = Variant.CreateFrom(id);
        var product = Plugin.CallString("getSubscription", arg);
        return JsonHelper.Deserialize<StoreProduct>(product);
    }

    public async Task<PurchaseResult> PurchaseProduct(string id)
    {
        var arg = Variant.CreateFrom(id);
        var ok = Plugin.CallBool("purchaseProduct", arg);
        if (!ok)
            return PurchaseResult.Error;
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
    public async Task<List<StoreProduct>> QueryPurchases()
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
        var list = new List<StoreProduct>();
        foreach (var id in Plugin.CallStringArray("getPendingPurchases"))
        {
            list.Add(GetProduct(id));
        }
        //
        return list;
    }
}
