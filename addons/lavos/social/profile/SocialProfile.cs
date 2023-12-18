using Lavos.Core;
using Lavos.Services.SocialPlatform;
using Lavos.Utils;

namespace Lavos.Social.Profile;

sealed class SocialProfile : ISocialProfile
{
    LazyRef<ISocialPlatformService> _service = new();

    public PlayerProfile GetPlayerProfile() {
        var value = _service.Ref.GetPlayerProfile();
        return JsonHelper.Deserialize<PlayerProfile>(value);
    }
}