using Godot;
using Lavos.Services.Store;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Lavos.Plugins.Google.GoogleBilling;

sealed class GoogleBilling : IStoreService
{
    const string PluginName = "GoogleBilling";
    readonly LavosPlugin Plugin;

    public GoogleBilling()
    {
        Assert.IsTrue(Engine.HasSingleton(PluginName), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public void Initialise()
    {
        Plugin.CallVoid("init");
    }

    public bool IsConnected()
    {
        return Plugin.CallBool("isConnected");
    }

    public void Connect()
    {
        Plugin.CallVoid("connect");
    }

    public void Disconnect()
    {
        Plugin.CallVoid("disconnect");
    }

    public async Task<IReadOnlyList<StoreProduct>> QueryProducts(IReadOnlyList<string> ids)
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
        var list = new List<StoreProduct>();
        foreach (var id in Plugin.CallStringArray("getProducts"))
        {
            list.Add(GetProduct(id));
        }
        //
        return list;
    }

    public async Task<IReadOnlyList<StoreProduct>> QuerySubscriptions(IReadOnlyList<string> ids)
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
        var list = new List<StoreProduct>();
        foreach (var id in Plugin.CallStringArray("getSubscriptions"))
        {
            list.Add(GetProduct(id));
        }
        //
        return list;
    }

    public StoreProduct GetProduct(string id)
    {
        var arg = Variant.CreateFrom(id);
        var product = Plugin.CallString("getProduct", arg);
        // TODO
        return null;
    }

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
    public async Task<IReadOnlyList<StoreProduct>> QueryPurchases()
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