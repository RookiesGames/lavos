
using System;

namespace Lavos.Services.SocialPlatform.Dummy;

sealed class DummySocialPlatformService : ISocialPlatformService
{
    public void Initialize() { }

    public void SignIn() { }
    public void SignOut() { }
    public bool IsSignedIn() => false;

    public string GetPlayerProfile() => null;

    public void ShowAchievements() { }

    public void ShowLeaderboards(string id) { }
}