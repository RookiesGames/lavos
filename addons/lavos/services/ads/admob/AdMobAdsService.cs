using Godot;
using Lavos.Ads;
using Lavos.Ads.Banners;
using Lavos.Plugins.Google;

namespace Lavos.Services.Ads.AdMob;

sealed class AdMobAdsService : IAdsService
{
    const string PluginName = GooglePlugins.AdMobPluginName;
    readonly LavosPlugin Plugin;

    public AdMobAdsService()
    {
        Assert.IsTrue(Engine.HasSingleton(PluginName), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public static bool IsPluginEnabled() => Engine.HasSingleton(PluginName);

    public void Initialize() => Plugin.CallVoid("init");

    public void SetGDPR(bool consented) { }
    public void SetCPRA(bool permitted) { }
    public void SetChildRestricted(bool restricted) { }
    public void SetUserId(string id) { }

    #region Banner

    public void SetBannerPosition(BannerPosition position) => Plugin.CallVoid("setBannerPosition", (int)position);
    public BannerPosition GetBannerPosition() => (BannerPosition)Plugin.CallInt("getBannerPosition");

    public bool IsLoaded(string id) => Plugin.CallBool("isBannerLoaded");
    public bool IsShowing(string id) => Plugin.CallBool("isBannerShowing");
    public void CreateBanner(string id) => Plugin.CallVoid("createBanner");
    public void LoadBanner(string id) => Plugin.CallVoid("loadBanner", id);
    public void ShowBanner(string id) => Plugin.CallVoid("showBanner");
    public void HideBanner(string id) => Plugin.CallVoid("hideBanner");
    public void DestroyBanner() => Plugin.CallVoid("destroyBanner");

    public bool HasPendingBannerEvent(AdEvent @event) => Plugin.CallBool("hasPendingBannerEvent", AdEventsHelper.ToString(@event));
    public string GetPendingBannerEvent(AdEvent @event) => Plugin.CallString("getPendingBannerEvent", AdEventsHelper.ToString(@event));
    public void ClearPendingBannerEvent(AdEvent @event) => Plugin.CallVoid("clearPendingBannerEvent", AdEventsHelper.ToString(@event));

    #endregion

    #region Interstitials

    public bool IsInterstitialReady() => Plugin.CallBool("isInterstitialsReady");
    public void LoadInterstitial(string id) => Plugin.CallVoid("loadInterstitial", id);
    public void ShowInterstitial(string id) => Plugin.CallVoid("showInterstitial");

    public bool HasPendingInterstitialEvent(AdEvent @event) => Plugin.CallBool("hasPendingInterstitialEvent", AdEventsHelper.ToString(@event));
    public string GetPendingInterstitialEvent(AdEvent @event) => Plugin.CallString("getPendingInterstitialEvent", AdEventsHelper.ToString(@event));
    public void ClearPendingInterstitialEvent(AdEvent @event) => Plugin.CallVoid("clearPendingInterstitialEvent", AdEventsHelper.ToString(@event));

    #endregion

    #region Rewarded Ads

    public bool IsRewardedAdReady() => Plugin.CallBool("isRewardedAdReady");
    public void LoadRewardedAd(string id) => Plugin.CallVoid("loadRewardedAd", id);
    public void ShowRewardedAd(string id) => Plugin.CallVoid("showRewardedAd");

    public bool HasPendingRewardedAdEvent(AdEvent @event) => Plugin.CallBool("hasPendingRewardedAdEvent", AdEventsHelper.ToString(@event));
    public string GetPendingRewardedAdEvent(AdEvent @event) => Plugin.CallString("getPendingRewardedAdEvent", AdEventsHelper.ToString(@event));
    public void ClearPendingRewardedAdEvent(AdEvent @event) => Plugin.CallVoid("clearPendingRewardedAdEvent", AdEventsHelper.ToString(@event));

    #endregion
}
