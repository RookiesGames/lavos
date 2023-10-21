using System;
using Lavos.Core;
using Lavos.Dependency;
using Lavos.Services.Ads;
using Lavos.Utils;

namespace Lavos.Ads.Banners;

public sealed class BannerAds : IBannerAds, IProcessable
{
    const string Tag = nameof(BannerAds);

    #region Events

    public event Action<BannerAdInfo> LoadCompleted;
    public event Action<LoadAdError> LoadFailed;
    public event Action<BannerAdInfo> Clicked;
    public event Action<BannerAdInfo> Closed;
    public event Action<BannerAdInfo> Opened;
    public event Action<BannerAdInfo> Impression;

    #endregion

    LazyRef<IAdsService> Service = new();

    public void Initialize() => ServiceLocator.Locate<IProcessorService>().Register(this);
    public void Shutdown() => ServiceLocator.Locate<IProcessorService>().Unregister(this);

    public void SetPosition(BannerPosition position) => Service.Ref.SetBannerPosition(position);
    public BannerPosition GetPosition() => Service.Ref.GetBannerPosition();
    
    public bool IsLoaded(string id) => Service.Ref.IsLoaded(id);
    public bool IsShowing(string id) => Service.Ref.IsShowing(id);

    public void Load(string id)
    {
        Destroy(id);
        Service.Ref.CreateBanner(id);
        Service.Ref.LoadBanner(id);
    }

    public void Show(string id) => Service.Ref.ShowBanner(id);
    public void Hide(string id) => Service.Ref.HideBanner(id);
    public void Destroy(string id) => Service.Ref.DestroyBanner();

    public void Process(double delta)
    {
        foreach (var adEvent in Enum.GetValues<AdEvent>())
        {
            if (!Service.Ref.HasPendingBannerEvent(adEvent)) continue;
            //
            var value = Service.Ref.GetPendingBannerEvent(adEvent);
            switch (adEvent)
            {
                case AdEvent.LoadCompleted:
                    {
                        var info = JsonHelper.Deserialize<BannerAdInfo>(value);
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
                        var info = JsonHelper.Deserialize<BannerAdInfo>(value);
                        Clicked?.Invoke(info);
                        break;
                    }
                case AdEvent.Closed:
                    {
                        var info = JsonHelper.Deserialize<BannerAdInfo>(value);
                        Closed?.Invoke(info);
                        break;
                    }
                case AdEvent.Opened:
                    {
                        var info = JsonHelper.Deserialize<BannerAdInfo>(value);
                        Opened?.Invoke(info);
                        break;
                    }
                case AdEvent.Impression:
                    {
                        var info = JsonHelper.Deserialize<BannerAdInfo>(value);
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
            Service.Ref.ClearPendingBannerEvent(adEvent);
        }
    }
}