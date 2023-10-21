using Lavos.Ads;
using Lavos.Ads.Banners;
using Lavos.Dependency;

namespace Lavos.Services.Ads;

public interface IAdsService : IService
{
    void Initialize();

    void SetGDPR(bool consented);
    void SetCPRA(bool permitted);

    void SetChildRestricted(bool restricted);
    void SetUserId(string id);

    #region Banner

    void SetBannerPosition(BannerPosition position);
    BannerPosition GetBannerPosition();

    bool IsLoaded(string id);
    bool IsShowing(string id);
    void CreateBanner(string id);
    void LoadBanner(string id);
    void ShowBanner(string id);
    void HideBanner(string id);
    void DestroyBanner();

    bool HasPendingBannerEvent(AdEvent @event);
    string GetPendingBannerEvent(AdEvent @event);
    void ClearPendingBannerEvent(AdEvent @event);

    #endregion

    #region Interstitials

    bool IsInterstitialReady();

    void LoadInterstitial(string id);
    void ShowInterstitial(string id);

    bool HasPendingInterstitialEvent(AdEvent @event);
    string GetPendingInterstitialEvent(AdEvent @event);
    void ClearPendingInterstitialEvent(AdEvent @event);

    #endregion

    #region Rewarded Ads

    bool IsRewardedAdReady();

    void LoadRewardedAd(string id);
    void ShowRewardedAd(string id);

    bool HasPendingRewardedAdEvent(AdEvent @event);
    string GetPendingRewardedAdEvent(AdEvent @event);
    void ClearPendingRewardedAdEvent(AdEvent @event);

    #endregion
}