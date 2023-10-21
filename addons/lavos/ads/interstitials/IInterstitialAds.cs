using Lavos.Dependency;
using System;

namespace Lavos.Ads.Interstitials;

public interface IInterstitialAds : IService
{
    #region Events

    event Action<InterstitialAdInfo> LoadCompleted;
    event Action<LoadAdError> LoadFailed;
    event Action<InterstitialAdInfo> Clicked;
    event Action<InterstitialAdInfo> Closed;
    event Action<InterstitialAdInfo> ShowSucceeded;
    event Action<ShowAdError> ShowFailed;
    event Action<InterstitialAdInfo> Impression;

    #endregion

    void Initialize();
    void Shutdown();

    bool IsReady();

    void Load(string id);
    void Show(string id);
}