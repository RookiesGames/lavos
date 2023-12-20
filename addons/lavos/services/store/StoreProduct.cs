using System.Collections.Generic;
using Lavos.Services.Store.Subscriptions;

namespace Lavos.Services.Store;

public sealed class StoreProduct
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public ProductPrice Price { get; set; }
    public List<SubscriptionOfferDetails> SubscriptionsOfferDetails { get; set; }
}