using Godot;
using Lavos.Dependency;
using Lavos.Services.SocialPlatform.Android;
using Lavos.Services.SocialPlatform.Dummy;
using Lavos.Social.Achievements;
using Lavos.Social.Auth;
using Lavos.Social.Leaderboards;
using Lavos.Social.Profile;
using Lavos.Utils.Platform;

namespace Lavos.Services.SocialPlatform;

[GlobalClass]
public sealed partial class SocialPlatformConfig : Config
{
    const string Tag = nameof(SocialPlatformConfig);

    public enum Provider
    {
        None,
        Google,
    }

    [Export] Provider provider;

    public override void Configure(IDependencyBinder binder)
    {
        void defaultBindings()
        {
            binder.Bind<ISocialPlatformService, DummySocialPlatformService>();
            binder.Bind<ISocialAuth, DummySocialAuth>();
            binder.Bind<ISocialProfile, DummySocialProfile>();
            binder.Bind<ISocialAchievements, DummySocialAchievements>();
            binder.Bind<ISocialLeaderboards, DummySocialLeaderboards>();
        }
        //
        if (!PlatformUtils.IsMobile)
        {
            provider = Provider.None;
        }
        else
        {
            if (PlatformUtils.IsAndroid)
            {
                provider = Provider.Google;
            }
        }
        //
        switch (provider)
        {
            case Provider.Google:
                {
                    if (GooglePlayGamesSocialPlatformService.IsPluginEnabled())
                    {
                        Log.Info(Tag, "GooglePlayGames plugin enabled");
                        binder.Bind<ISocialPlatformService, GooglePlayGamesSocialPlatformService>();
                        binder.Bind<ISocialAuth, SocialAuth>();
                        binder.Bind<ISocialProfile, SocialProfile>();
                        binder.Bind<ISocialAchievements, SocialAchievements>();
                        binder.Bind<ISocialLeaderboards, SocialLeaderboards>();
                        break;
                    }
                    else
                    {
                        Log.Warn(Tag, $"GooglePlayGames plugin disabled");
                        goto default;
                    }
                }
            default:
                {
                    defaultBindings();
                    break;
                }
        }
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        resolver.Resolve<ISocialPlatformService>().Initialize();
    }
}