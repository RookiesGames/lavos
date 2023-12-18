using System;
using Lavos.Dependency;

namespace Lavos.Services.SocialPlatform;

public interface ISocialPlatformService : IService
{
    void Initialize();

    void SignIn();
    void SignOut();
    bool IsSignedIn();

    string GetPlayerProfile();

    void ShowAchievements();

    void ShowLeaderboards(string id);
}