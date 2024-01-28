using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Services.Store;

sealed class DummyStoreService : IStoreService
{
    public event Action<Purchase> ProductPurchased;

    public void Initialize() { }

    public Task<QueryProductsStatus> QueryProducts(IReadOnlyList<string> ids) => Task.FromResult(QueryProductsStatus.Completed);
    public List<StoreProduct> GetProducts() => [];
    public StoreProduct GetProduct(string id) => null;

    public Task<QuerySubscriptionsStatus> QuerySubscriptions(IReadOnlyList<string> ids) => Task.FromResult(QuerySubscriptionsStatus.Completed);
    public List<StoreProduct> GetSubscriptions() => [];
    public StoreProduct GetSubscription(string id) => null;

    public Task<PurchaseResult> PurchaseProduct(string id) => Task.FromResult(PurchaseResult.Success);
    public Task<PurchaseResult> PurchaseSubscription(string id, OfferDetails offer) => Task.FromResult(PurchaseResult.Success);
    
    public Task ProcessPendingPurchases() => Task.CompletedTask;
}