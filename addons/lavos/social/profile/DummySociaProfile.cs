using Lavos.Social;

namespace Lavos.Social.Profile;

sealed class DummySocialProfile : ISocialProfile
{
    public PlayerProfile GetPlayerProfile() => null;
}