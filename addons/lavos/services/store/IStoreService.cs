using Lavos.Dependency;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Lavos.Services.Store;

public interface IStoreService : IService
{
    event Action<Purchase> ProductPurchased;

    void Initialize();

    Task<QueryProductsStatus> QueryProducts(IReadOnlyList<string> ids);
    List<StoreProduct> GetProducts();
    StoreProduct GetProduct(string id);

    Task<QuerySubscriptionsStatus> QuerySubscriptions(IReadOnlyList<string> ids);
    List<StoreProduct> GetSubscriptions();
    StoreProduct GetSubscription(string id);

    Task<PurchaseResult> PurchaseProduct(string id);
    Task<PurchaseResult> PurchaseSubscription(string id, OfferDetails offer);
    
    Task ProcessPendingPurchases();
}