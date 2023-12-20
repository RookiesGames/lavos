
using System.Collections.Generic;

namespace Lavos.Services.Store.Subscriptions;

public sealed class SubscriptionOfferDetails
{
    public string Id { get; set; }
    public OfferDetails Offer { get; set; }
    public List<PricingPhase> Phases { get; set; }
}