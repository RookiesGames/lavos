using Lavos.Dependency;
using System;

namespace Lavos.Ads.Banners;

public interface IBannerAds : IService
{
    #region Events

    event Action<BannerAdInfo> LoadCompleted;
    event Action<LoadAdError> LoadFailed;
    event Action<BannerAdInfo> Clicked;
    event Action<BannerAdInfo> Closed;
    event Action<BannerAdInfo> Opened;
    event Action<BannerAdInfo> Impression;

    #endregion

    void Initialize();
    void Shutdown();

    bool IsLoaded(string id);
    bool IsShowing(string id);

    void SetPosition(BannerPosition positon);
    BannerPosition GetPosition();
    
    void Load(string id);
    void Show(string id);
    void Hide(string id);
    void Destroy(string id);
}