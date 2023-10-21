using Lavos.Core;
using Lavos.Dependency;
using Lavos.Services.Ads;
using Lavos.Utils;
using System;

namespace Lavos.Ads.Interstitials;

sealed class InterstitialAds : IInterstitialAds, IProcessable
{
    const string Tag = nameof(InterstitialAds);

    #region Events

    public event Action<InterstitialAdInfo> LoadCompleted;
    public event Action<LoadAdError> LoadFailed;
    public event Action<InterstitialAdInfo> Clicked;
    public event Action<InterstitialAdInfo> Closed;
    public event Action<InterstitialAdInfo> ShowSucceeded;
    public event Action<ShowAdError> ShowFailed;
    public event Action<InterstitialAdInfo> Impression;

    #endregion

    LazyRef<IAdsService> Service = new();

    public void Initialize() => ServiceLocator.Locate<IProcessorService>().Register(this);
    public void Shutdown() => ServiceLocator.Locate<IProcessorService>().Unregister(this);

    public bool IsReady() => Service.Ref.IsInterstitialReady();

    public void Load(string id) => Service.Ref.LoadInterstitial(id);
    public void Show(string id) => Service.Ref.ShowInterstitial(id);

    public void Process(double delta)
    {
        foreach (var adEvent in Enum.GetValues<AdEvent>())
        {
            if (!Service.Ref.HasPendingInterstitialEvent(adEvent)) continue;
            //
            var value = Service.Ref.GetPendingInterstitialEvent(adEvent);
            switch (adEvent)
            {
                case AdEvent.LoadCompleted:
                    {
                        var info = JsonHelper.Deserialize<InterstitialAdInfo>(value);
                        LoadCompleted?.Invoke(info);
                        break;
                    }
                case AdEvent.LoadFailed:
                    {
                        var error = JsonHelper.Deserialize<LoadAdError>(value);
                        LoadFailed?.Invoke(error);
                        break;
                    }
                case AdEvent.Clicked:
                    {
                        var info = JsonHelper.Deserialize<InterstitialAdInfo>(value);
                        Clicked?.Invoke(info);
                        break;
                    }
                case AdEvent.Closed:
                    {
                        var info = JsonHelper.Deserialize<InterstitialAdInfo>(value);
                        Closed?.Invoke(info);
                        break;
                    }
                case AdEvent.ShowSucceeded:
                    {
                        var info = JsonHelper.Deserialize<InterstitialAdInfo>(value);
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
                        var info = JsonHelper.Deserialize<InterstitialAdInfo>(value);
                        Impression?.Invoke(info);
                        break;
                    }
                default:
                    {
                        Log.Warn(Tag, $"Unhandled AdEvent {adEvent}");
                        break;
                    }
            }
            //
            Service.Ref.ClearPendingInterstitialEvent(adEvent);
        }
    }
}