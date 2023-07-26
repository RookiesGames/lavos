using Godot;
using Lavos.Services.Store;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Lavos.Plugins.GoogleBilling;

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
        Plugin.CallVoid("queryProducts", args);
        //
        do
        {
            await Task.Delay(100);
        } while (!Plugin.CallBool("hasProducts"));
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
        Plugin.CallVoid("querySubscriptions", args);
        //
        do
        {
            await Task.Delay(100);
        } while (!Plugin.CallBool("hasSubscriptions"));
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
        return new StoreProduct()
        {
            Id = id,
            Title = Plugin.CallString("getProductTitle", arg),
            Name = Plugin.CallString("getProductName", arg),
            Description = Plugin.CallString("getProductDescription", arg),
            Type = Plugin.CallString("getProductType", arg),
            FormattedPrice = Plugin.CallString("getProductFormattedPrice", arg),
            CurrencyCode = Plugin.CallString("getProductPriceCurrencyCode", arg),
            Price = Plugin.CallFloat("getProductPriceAmount", arg),
        };
    }

    public async Task<PurchaseResult> PurchaseProduct(string id)
    {
        var arg = Variant.CreateFrom(id);
        var ok = Plugin.CallBool("purchaseProduct", arg);
        if (!ok) return PurchaseResult.Error;
        //
        PurchaseProgress status;
        do
        {
            await Task.Delay(100);
            status = (PurchaseProgress)Plugin.CallInt("getPurchaseStatus");
        } while (status == PurchaseProgress.InProgress);
        //
        return status switch
        {
            PurchaseProgress.None => PurchaseResult.Canceled,
            PurchaseProgress.Error => PurchaseResult.Error,
            PurchaseProgress.Completed => PurchaseResult.Success,
            _ => PurchaseResult.Unknown,
        };
    }

    public void QueryPurchases()
    {
    }
}