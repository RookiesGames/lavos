using System;

namespace Lavos.Ads.Banners;

sealed class DummyBannerAds : IBannerAds
{
    #region Events
#pragma warning disable 0067
    public event Action<BannerAdInfo> LoadCompleted;
    public event Action<LoadAdError> LoadFailed;
    public event Action<BannerAdInfo> Clicked;
    public event Action<BannerAdInfo> Closed;
    public event Action<BannerAdInfo> Opened;
    public event Action<BannerAdInfo> Impression;
#pragma warning restore 0067
    #endregion

    public void Initialize() { }
    public void Shutdown() { }

    public bool IsLoaded(string id) => false;
    public bool IsShowing(string id) => false;

    public void SetPosition(BannerPosition position) { }
    public BannerPosition GetPosition() { return BannerPosition.None; }

    public void Load(string id) { }
    public void Show(string id) { }
    public void Hide(string id) { }
    public void Destroy(string id) { }
}