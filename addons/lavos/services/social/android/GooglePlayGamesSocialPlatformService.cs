
using Godot;
using Lavos.Plugins.Google;

namespace Lavos.Services.SocialPlatform.Android;

sealed class GooglePlayGamesSocialPlatformService : ISocialPlatformService
{
    const string PluginName = GooglePlugins.PlayGamesName;
    readonly LavosPlugin Plugin;

    public GooglePlayGamesSocialPlatformService()
    {
        Assert.IsTrue(IsPluginEnabled(), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public static bool IsPluginEnabled() => Engine.HasSingleton(PluginName);

    public void Initialize() => Plugin.CallVoid("init");

    public void SignIn() => Plugin.CallVoid("signIn");
    public void SignOut() {}
    public bool IsSignedIn() => Plugin.CallBool("isSignedIn");

    public string GetPlayerProfile() => Plugin.CallString("getPlayer");

    public void ShowAchievements() => Plugin.CallVoid("showAchievements");

    public void ShowLeaderboards(string id) => Plugin.CallVoid("showLeaderboards", id);
}