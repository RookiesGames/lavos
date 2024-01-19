
namespace Lavos.Services.Store;

public sealed class Purchase
{
    public string OrderId { get; set; }
    public string PackageName { get; set; }
    public string ProductId { get; set; }
    public ulong PurchaseTime { get; set; }
    public PurchaseState PurchaseState { get; set; }
    public string PurchaseToken { get; set; }
    public int Quantity { get; set; }
    public bool Acknowledged { get; set; }
}