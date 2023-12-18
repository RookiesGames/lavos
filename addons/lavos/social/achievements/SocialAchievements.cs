using Lavos.Core;
using Lavos.Services.SocialPlatform;

namespace Lavos.Social.Achievements;

sealed class SocialAchievements : ISocialAchievements
{
    LazyRef<ISocialPlatformService> _service = new();

    public void ShowAchievements() => _service.Ref.ShowAchievements();
}