using Lavos.Ads;
using Lavos.Ads.Banners;

namespace Lavos.Services.Ads.Dummy;

sealed class DummyAdsService : IAdsService
{
    public void Initialize() { }
    public void SetGDPR(bool consented) { }
    public void SetCPRA(bool permitted) { }
    public void SetChildRestricted(bool restricted) { }
    public void SetUserId(string id) { }

    #region Banner

    public void SetBannerPosition(BannerPosition position) { }
    public BannerPosition GetBannerPosition() => BannerPosition.None;

    public bool IsLoaded(string id) => false;
    public bool IsShowing(string id) => false;
    public void CreateBanner(string id) { }
    public void LoadBanner(string id) { }
    public void ShowBanner(string id) { }
    public void HideBanner(string id) { }
    public void DestroyBanner() { }

    public bool HasPendingBannerEvent(AdEvent @event) => false;
    public string GetPendingBannerEvent(AdEvent @event) => null;
    public void ClearPendingBannerEvent(AdEvent @event) { }

    #endregion

    #region Interstitials

    public bool IsInterstitialReady() => false;

    public void LoadInterstitial(string id) { }
    public void ShowInterstitial(string id) { }

    public bool HasPendingInterstitialEvent(AdEvent @event) => false;
    public string GetPendingInterstitialEvent(AdEvent @event) => null;
    public void ClearPendingInterstitialEvent(AdEvent @event) { }

    #endregion

    #region Rewarded Ads

    public bool IsRewardedAdReady() => false;

    public void LoadRewardedAd(string id) { }
    public void ShowRewardedAd(string id) { }

    public bool HasPendingRewardedAdEvent(AdEvent @event) => false;
    public string GetPendingRewardedAdEvent(AdEvent @event) => null;
    public void ClearPendingRewardedAdEvent(AdEvent @event) { }

    #endregion
}