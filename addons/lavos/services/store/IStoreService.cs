using Lavos.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Services.Store;

public interface IStoreService : IService
{
    void Initialise();

    bool IsConnected();
    void Connect();
    void Disconnect();

    Task<IReadOnlyList<StoreProduct>> QueryProducts(IReadOnlyList<string> ids);
    Task<IReadOnlyList<StoreProduct>> QuerySubscriptions(IReadOnlyList<string> ids);

    StoreProduct GetProduct(string id);

    Task<PurchaseResult> PurchaseProduct(string id);
    Task<IReadOnlyList<StoreProduct>> QueryPurchases();
}