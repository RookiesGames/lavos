using Lavos.Dependency;
using Lavos.Services.GoogleBilling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Services.Store;

public interface IStoreService : IService
{
    void Initialize();

    Task<QueryProductsStatus> QueryProducts(IReadOnlyList<string> ids);
    List<StoreProduct> GetProducts();
    StoreProduct GetProduct(string id);

    Task<QuerySubscriptionsStatus> QuerySubscriptions(IReadOnlyList<string> ids);
    List<StoreProduct> GetSubscriptions();
    StoreProduct GetSubscription(string id);

    Task<PurchaseResult> PurchaseProduct(string id);
    Task<QueryPurchasesStatus> QueryPurchases();
    List<string> GetPendingPurchases();

    Task<ConsumePurchaseStatus> ConsumePurchase(string token);
}