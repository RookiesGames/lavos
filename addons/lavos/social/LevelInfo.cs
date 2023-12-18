
namespace Lavos.Social;

public sealed class LevelInfo
{
    public bool IsMaxLevel { get; set; }
    public long TotalXP { get; set; }
    public Level Current { get; set; }
    public Level Next { get; set; }
}