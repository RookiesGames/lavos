using System.Collections.Generic;

namespace Lavos.Services.Store;

public sealed class OfferDetails
{
    public string Id { get; set; }
    public string Token { get; set; }
    public List<string> Tags { get; set; }
}