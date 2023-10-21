package eu.rookies.google.admob

import android.util.Log
import com.google.android.gms.ads.MobileAds
import eu.rookies.google.admob.banner.BannerPosition
import eu.rookies.google.admob.banner.Banners
import eu.rookies.google.admob.interstitials.Interstitials
import eu.rookies.google.admob.rewardedads.RewardedAds
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class AdMob(godot: Godot) : GodotPlugin(godot) {
    private var pluginName = AdMob::class.java.simpleName
    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        MobileAds.initialize(activity!!.applicationContext) { status ->
            for (entry in status.adapterStatusMap.entries) {
                Log.d(pluginName, "${entry.key} - ${entry.value}")
            }
        }
    }

    override fun onMainDestroy() {
        super.onMainDestroy()
        Banners.destroyBanner()
    }

    /////////////
    // Banners

    @UsedByGodot
    fun isBannerLoaded(): Boolean = Banners.isLoaded()

    @UsedByGodot
    fun isBannerShowing(): Boolean = Banners.isShowing()

    @UsedByGodot
    fun setBannerPosition(position: Int) =
        activity!!.runOnUiThread {
            Banners.setPosition(BannerPosition.fromInt(position))
        }

    @UsedByGodot
    fun getBannerPosition(): Int = Banners.getPosition().key

    @UsedByGodot
    fun createBanner() {
        activity!!.runOnUiThread {
            Banners.createBanner(activity!!, activity!!.applicationContext)
        }
    }

    @UsedByGodot
    fun loadBanner(id: String) {
        activity!!.runOnUiThread {
            Banners.loadBanner(id)
        }
    }

    @UsedByGodot
    fun showBanner() {
        activity!!.runOnUiThread {
            Banners.show()
        }
    }

    @UsedByGodot
    fun hideBanner() {
        activity!!.runOnUiThread {
            Banners.hide()
        }
    }

    @UsedByGodot
    fun destroyBanner() {
        activity!!.runOnUiThread {
            Banners.destroyBanner()
        }
    }

    @UsedByGodot
    fun hasPendingBannerEvent(event: String): Boolean = Banners.hasPendingEvent(event)

    @UsedByGodot
    fun getPendingBannerEvent(event: String): String = Banners.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingBannerEvent(event: String) = Banners.clearPendingEvent(event)

    /////////////
    // Interstitials

    @UsedByGodot
    fun isInterstitialsReady(): Boolean = Interstitials.isReady()

    @UsedByGodot
    fun loadInterstitial(id: String) {
        activity!!.runOnUiThread {
            Interstitials.load(activity!!.applicationContext, id)
        }
    }

    @UsedByGodot
    fun showInterstitial() {
        activity!!.runOnUiThread {
            Interstitials.show(activity!!)
        }
    }

    @UsedByGodot
    fun hasPendingInterstitialEvent(event: String): Boolean = Interstitials.hasPendingEvent(event)

    @UsedByGodot
    fun getPendingInterstitialEvent(event: String): String = Interstitials.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingInterstitialEvent(event: String) = Interstitials.clearPendingEvent(event)

    /////////////////
    // Rewarded Ads

    @UsedByGodot
    fun isRewardedAdReady(): Boolean = RewardedAds.isReady()

    @UsedByGodot
    fun loadRewardedAd(id: String) {
        activity!!.runOnUiThread {
            RewardedAds.load(activity!!.applicationContext, id)
        }
    }

    @UsedByGodot
    fun showRewardedAd() {
        activity!!.runOnUiThread {
            RewardedAds.show(activity!!)
        }
    }

    @UsedByGodot
    fun hasPendingRewardedAdEvent(event: String): Boolean = RewardedAds.hasPendingEvents(event)

    @UsedByGodot
    fun getPendingRewardedAdEvent(event: String): String = RewardedAds.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingRewardedAdEvent(event: String) = RewardedAds.clearPendingEvent(event)
}