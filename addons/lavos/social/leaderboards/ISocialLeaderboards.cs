using Lavos.Dependency;

namespace Lavos.Social.Leaderboards;

public interface ISocialLeaderboards : IService
{
    void ShowLeaderboards(string id);
}