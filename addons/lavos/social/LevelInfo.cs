
namespace Lavos.Social;

public sealed class LevelInfo
{
    public bool IsMaxLevel { get; }
    public long TotalXP { get; }
    public Level Current { get; }
    public Level Next { get; }
}