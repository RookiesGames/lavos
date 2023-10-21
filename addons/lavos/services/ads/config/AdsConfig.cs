using Godot;
using Lavos.Ads.Banners;
using Lavos.Ads.Interstitials;
using Lavos.Ads.Rewarded;
using Lavos.Dependency;
using Lavos.Services.Ads.AdMob;
using Lavos.Services.Ads.IronSource;
using Lavos.Utils.Platform;

namespace Lavos.Services.Ads;

[GlobalClass]
public sealed partial class AdsConfig : Config
{
    const string Tag = nameof(AdsConfig);

    public enum Provider
    {
        None,
        AdMob,
        ironSource,
    }

    [Export] Provider adProvider;

    public override void Configure(IDependencyBinder binder)
    {
        void defaultBindings()
        {
            binder.Bind<IAdsService, DummyAdsService>();
            binder.Bind<IRewardedAds, DummyRewardedAds>();
            binder.Bind<IInterstitialAds, DummyInterstitialAds>();
            binder.Bind<IBannerAds, DummyBannerAds>();
        }

        if (!PlatformUtils.IsMobile)
        {
            adProvider = Provider.None;
        }
        //
        switch (adProvider)
        {
            case Provider.AdMob:
                {
                    if (AdMobAdsService.IsPluginEnabled())
                    {
                        Log.Info(Tag, $"AdMob plugin enabled");
                        binder.Bind<IAdsService, AdMobAdsService>();
                        binder.Bind<IRewardedAds, RewardedAds>();
                        binder.Bind<IInterstitialAds, InterstitialAds>();
                        binder.Bind<IBannerAds, BannerAds>();
                    }
                    else
                    {
                        Log.Warn(Tag, $"AdMob plugin disabled");
                        goto default;
                    }
                    break;
                }
            case Provider.ironSource:
                {
                    if (IronSourceAdsService.IsPluginEnabled())
                    {
                        Log.Info(Tag, $"ironSource plugin enabled");
                        binder.Bind<IAdsService, IronSourceAdsService>();
                        // binder.Bind<IRewardedAds, >();
                        // binder.Bind<IInterstitialAds, >();
                        // binder.Bind<IBannerAds, >();
                    }
                    else
                    {
                        Log.Warn(Tag, $"ironSource plugin disabled");
                        goto default;
                    }
                    break;
                }
            default:
                {
                    Log.Info(Tag, "No binding provided");
                    defaultBindings();
                    break;
                };
        }
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        resolver.Resolve<IAdsService>().Initialize();
        resolver.Resolve<IRewardedAds>().Initialize();
        resolver.Resolve<IInterstitialAds>().Initialize();
        resolver.Resolve<IBannerAds>().Initialize();
    }
}