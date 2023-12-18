using Lavos.Core;
using Lavos.Services.SocialPlatform;

namespace Lavos.Social.Leaderboards;

sealed class SocialLeaderboards : ISocialLeaderboards
{
    LazyRef<ISocialPlatformService> _service = new();

    public void ShowLeaderboards(string id) => _service.Ref.ShowLeaderboards(id);
}