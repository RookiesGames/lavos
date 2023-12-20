
namespace Lavos.Services.Store.Subscriptions;

public sealed class PricingPhase
{
    public RecurrenceMode Mode { get; set; }
    public BillingPeriod Billing { get; set; }
    public ProductPrice Price { get; set; }
}