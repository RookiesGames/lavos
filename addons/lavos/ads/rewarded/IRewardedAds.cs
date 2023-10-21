using Lavos.Dependency;
using System;

namespace Lavos.Ads.Rewarded;

public interface IRewardedAds : IService
{
    #region Events

    event Action<RewardAdInfo> LoadCompleted;
    event Action<LoadAdError> LoadFailed;
    event Action<RewardAdInfo> Clicked;
    event Action<RewardAdInfo> Closed;
    event Action<RewardAdInfo> ShowSucceeded;
    event Action<ShowAdError> ShowFailed;
    event Action<RewardAdInfo> Impression;
    event Action<RewardInfo> Rewarded;

    #endregion

    void Initialize();
    void Shutdown();

    bool IsReady();

    void Load(string id);
    void Show(string id);
}