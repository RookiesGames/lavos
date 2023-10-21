using System;

namespace Lavos.Ads.Interstitials;

sealed class DummyInterstitialAds : IInterstitialAds
{    
    #region Events
#pragma warning disable 0067
    public event Action<InterstitialAdInfo> LoadCompleted;
    public event Action<LoadAdError> LoadFailed;
    public event Action<InterstitialAdInfo> Clicked;
    public event Action<InterstitialAdInfo> Closed;
    public event Action<InterstitialAdInfo> ShowSucceeded;
    public event Action<ShowAdError> ShowFailed;
    public event Action<InterstitialAdInfo> Impression;
#pragma warning restore 0067
    #endregion

    public void Initialize() { }
    public void Shutdown() { }

    public bool IsReady() => false;

    public void Load(string id) { }
    public void Show(string id) { }
}