using Lavos.Dependency;

namespace Lavos.Social.Profile;

public interface ISocialProfile : IService
{
    PlayerProfile GetPlayerProfile();
}