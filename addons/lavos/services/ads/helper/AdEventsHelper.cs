using Lavos.Ads;

namespace Lavos.Services.Ads.Helper;

static class AdEventsHelper
{
    public static string ToString(AdEvent @event)
    {
        return @event switch
        {
            AdEvent.LoadCompleted => "ad_load_completed",
            AdEvent.LoadFailed => "ad_load_failed",
            AdEvent.Opened => "ad_opened",
            AdEvent.Clicked => "ad_clicked",
            AdEvent.Closed => "ad_closed",
            AdEvent.ShowSucceeded => "ad_show_succeeded",
            AdEvent.ShowFailed => "ad_show_failed",
            AdEvent.Impression => "ad_impression",
            AdEvent.Available => "ad_available",
            AdEvent.Unavailable => "ad_unavailable",
            AdEvent.Rewarded => "ad_rewarded",
            AdEvent.LeftApp => "ad_left_app",
            AdEvent.ScreenPresented => "ad_screen_presented",
            AdEvent.ScreenDismissed => "ad_screen_dismissed",
            _ => null
        };
    }
}