namespace Lavos.Services.Store;

public sealed class StoreProduct
{
    public string Id { init; get; }
    public string Title { init; get; }
    public string Name { init; get; }
    public string Description { init; get; }
    public string Type { init; get; }
    public string FormattedPrice { init; get; }
    public string CurrencyCode { init; get; }
    public float Price { init; get; }
}