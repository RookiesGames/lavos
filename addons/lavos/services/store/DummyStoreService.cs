using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Services.Store;

sealed class DummyStoreService : IStoreService
{
    public void Initialize() { }

    public Task<List<StoreProduct>> QueryProducts(IReadOnlyList<string> ids) => Task.FromResult(new List<StoreProduct>());
    public StoreProduct GetProduct(string id) => null;

    public Task<List<StoreProduct>> QuerySubscriptions(IReadOnlyList<string> ids) => Task.FromResult(new List<StoreProduct>());
    public StoreProduct GetSubscription(string id) => null;

    public Task<PurchaseResult> PurchaseProduct(string id) => Task.FromResult(PurchaseResult.Unknown);
    public Task<List<StoreProduct>> QueryPurchases() => Task.FromResult(new List<StoreProduct>());
}