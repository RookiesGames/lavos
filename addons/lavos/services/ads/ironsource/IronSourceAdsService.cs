using Godot;
using Lavos.Ads;
using Lavos.Ads.Banners;
using Lavos.Services.Ads.Helper;

namespace Lavos.Services.Ads.IronSource;

sealed class IronSourceAdsService : IAdsService
{
    const string PluginName = "ironSource";
    readonly LavosPlugin Plugin;

    public IronSourceAdsService()
    {
        Assert.IsTrue(IsPluginEnabled(), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public static bool IsPluginEnabled() => Engine.HasSingleton(PluginName);

    public void Initialize() => Plugin.CallVoid("init");

    public void SetGDPR(bool consented) => Plugin.CallVoid("gdpr", consented);
    public void SetCPRA(bool permitted) => Plugin.CallVoid("cpra", permitted);
    public void SetChildRestricted(bool restricted) => Plugin.CallVoid("childRestricted", restricted);
    public void SetUserId(string id) => Plugin.CallVoid("setUserId", id);

    #region Banner

    public void SetBannerPosition(BannerPosition position) => Plugin.CallVoid("setBannerPosition");
    public BannerPosition GetBannerPosition() => (BannerPosition)Plugin.CallInt("getBannerPosition");

    public bool IsLoaded(string id) => Plugin.CallBool("isBannerLoaded");
    public bool IsShowing(string id) => Plugin.CallBool("isBannerShowing");
    public void CreateBanner(string id) => Plugin.CallVoid("createBanner");
    public void LoadBanner(string id) => Plugin.CallVoid("loadBanner", id);
    public void ShowBanner(string id) => Plugin.CallVoid("showBanner", id);
    public void HideBanner(string id) => Plugin.CallVoid("hideBanner", id);
    public void DestroyBanner() => Plugin.CallVoid("destryoBanner");

    public bool HasPendingBannerEvent(AdEvent @event) => Plugin.CallBool("hasPendingBannerEvent", AdEventsHelper.ToString(@event));
    public string GetPendingBannerEvent(AdEvent @event) => Plugin.CallString("getPendingBannerEvent", AdEventsHelper.ToString(@event));
    public void ClearPendingBannerEvent(AdEvent @event) => Plugin.CallVoid("clearPendingBannerEvent", AdEventsHelper.ToString(@event));

    #endregion

    #region Interstitials

    public bool IsInterstitialReady() => Plugin.CallBool("isInterstitialReady");
    public void LoadInterstitial(string id) => Plugin.CallVoid("loadInterstitial");
    public void ShowInterstitial(string id) => Plugin.CallVoid("showInterstitial", id);
    public bool HasPendingInterstitialEvent(AdEvent @event) => Plugin.CallBool("hasPendingInterstitialEvent", AdEventsHelper.ToString(@event));
    public string GetPendingInterstitialEvent(AdEvent @event) => Plugin.CallString("getPendingInterstitialEvent", AdEventsHelper.ToString(@event));
    public void ClearPendingInterstitialEvent(AdEvent @event) => Plugin.CallVoid("clearPendingInterstitialEvent", AdEventsHelper.ToString(@event));

    #endregion

    #region Rewarded Ads

    public bool IsRewardedAdReady() => Plugin.CallBool("isRewardedAdAvailable");
    public void LoadRewardedAd(string id) => Plugin.CallVoid("loadRewardedAd");
    public void ShowRewardedAd(string id) => Plugin.CallVoid("showRewardedAd", id);

    public bool HasPendingRewardedAdEvent(AdEvent @event) => Plugin.CallBool("hasPendingRewardedAdEvent", AdEventsHelper.ToString(@event));
    public string GetPendingRewardedAdEvent(AdEvent @event) => Plugin.CallString("getPendingRewardedAdEvent", AdEventsHelper.ToString(@event));
    public void ClearPendingRewardedAdEvent(AdEvent @event) => Plugin.CallVoid("clearPendingRewardedAdEvent", AdEventsHelper.ToString(@event));

    #endregion
}
