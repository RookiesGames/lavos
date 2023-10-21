using System;
using Lavos.Core;
using Lavos.Dependency;
using Lavos.Services.Ads;
using Lavos.Utils;

namespace Lavos.Ads.Rewarded;

sealed class RewardedAds : IRewardedAds, IProcessable
{
    const string Tag = nameof(RewardedAds);

    #region Events

    public event Action<RewardAdInfo> LoadCompleted;
    public event Action<LoadAdError> LoadFailed;
    public event Action<RewardAdInfo> Clicked;
    public event Action<RewardAdInfo> Closed;
    public event Action<RewardAdInfo> ShowSucceeded;
    public event Action<ShowAdError> ShowFailed;
    public event Action<RewardAdInfo> Impression;
    public event Action<RewardInfo> Rewarded;

    #endregion

    LazyRef<IAdsService> Service = new();

    public void Initialize() => ServiceLocator.Locate<IProcessorService>().Register(this);
    public void Shutdown() => ServiceLocator.Locate<IProcessorService>().Unregister(this);

    public bool IsReady() => Service.Ref.IsRewardedAdReady();

    public void Load(string id) => Service.Ref.LoadRewardedAd(id);
    public void Show(string id) => Service.Ref.ShowRewardedAd(id);

    public void Process(double delta)
    {
        foreach (var adEvent in Enum.GetValues<AdEvent>())
        {
            if (!Service.Ref.HasPendingRewardedAdEvent(adEvent)) continue;
            //
            var value = Service.Ref.GetPendingRewardedAdEvent(adEvent);
            switch (adEvent)
            {
                case AdEvent.LoadCompleted:
                    {
                        var info = JsonHelper.Deserialize<RewardAdInfo>(value);
                        LoadCompleted?.Invoke(info);
                        break;
                    }
                case AdEvent.LoadFailed:
                    {
                        var error = JsonHelper.Deserialize<LoadAdError>(value);
                        LoadFailed.Invoke(error);
                        break;
                    }
                case AdEvent.Clicked:
                    {
                        var info = JsonHelper.Deserialize<RewardAdInfo>(value);
                        Clicked?.Invoke(info);
                        break;
                    }
                case AdEvent.Closed:
                    {
                        var info = JsonHelper.Deserialize<RewardAdInfo>(value);
                        Closed?.Invoke(info);
                        break;
                    }
                case AdEvent.ShowSucceeded:
                    {
                        var info = JsonHelper.Deserialize<RewardAdInfo>(value);
                        ShowSucceeded?.Invoke(info);
                        break;
                    }
                case AdEvent.ShowFailed:
                    {
                        var error = JsonHelper.Deserialize<ShowAdError>(value);
                        ShowFailed?.Invoke(error);
                        break;
                    }
                case AdEvent.Impression:
                    {
                        var info = JsonHelper.Deserialize<RewardAdInfo>(value);
                        Impression?.Invoke(info);
                        break;
                    }
                case AdEvent.Rewarded:
                    {
                        var reward = JsonHelper.Deserialize<RewardInfo>(value);
                        Rewarded?.Invoke(reward);
                        break;
                    }
                default:
                    {
                        Log.Warn(Tag, $"Unhandled AdEvent {adEvent}");
                        break;
                    }
            }
            //
            Service.Ref.ClearPendingRewardedAdEvent(adEvent);
        }
    }
}