using System;

namespace Lavos.Ads.Rewarded;

sealed class DummyRewardedAds : IRewardedAds
{
    #region Events
#pragma warning disable 0067
    public event Action<RewardAdInfo> LoadCompleted;
    public event Action<LoadAdError> LoadFailed;
    public event Action<RewardAdInfo> Clicked;
    public event Action<RewardAdInfo> Closed;
    public event Action<RewardAdInfo> ShowSucceeded;
    public event Action<ShowAdError> ShowFailed;
    public event Action<RewardAdInfo> Impression;
    public event Action<RewardInfo> Rewarded;
#pragma warning restore 0067
    #endregion

    public void Initialize() { }
    public void Shutdown() { }

    public bool IsReady() => false;

    public void Load(string id) { }
    public void Show(string id) { }
}