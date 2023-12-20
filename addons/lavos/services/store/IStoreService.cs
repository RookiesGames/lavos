using Lavos.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Services.Store;

public interface IStoreService : IService
{
    void Initialize();

    Task<List<StoreProduct>> QueryProducts(IReadOnlyList<string> ids);
    StoreProduct GetProduct(string id);

    Task<List<StoreProduct>> QuerySubscriptions(IReadOnlyList<string> ids);
    StoreProduct GetSubscription(string id);

    Task<PurchaseResult> PurchaseProduct(string id);
    Task<List<StoreProduct>> QueryPurchases();
}