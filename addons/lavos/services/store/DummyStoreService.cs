using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Services.Store;

sealed class DummyStoreService : IStoreService
{
    public void Initialize() { }

    public Task<QueryProductsStatus> QueryProducts(IReadOnlyList<string> ids) => Task.FromResult(QueryProductsStatus.Completed);
    public List<StoreProduct> GetProducts() => new();
    public StoreProduct GetProduct(string id) => null;

    public Task<QuerySubscriptionsStatus> QuerySubscriptions(IReadOnlyList<string> ids) => Task.FromResult(QuerySubscriptionsStatus.Completed);
    public List<StoreProduct> GetSubscriptions() => new();
    public StoreProduct GetSubscription(string id) => null;

    public Task<PurchaseResult> PurchaseProduct(string id) => Task.FromResult(PurchaseResult.Success);
    public Task<QueryPurchasesStatus> QueryPurchases() => Task.FromResult(QueryPurchasesStatus.Completed);
    public List<string> GetPendingPurchases() => new();

    public Task<ConsumePurchaseStatus> ConsumePurchase(string token) => Task.FromResult(ConsumePurchaseStatus.Completed);
}