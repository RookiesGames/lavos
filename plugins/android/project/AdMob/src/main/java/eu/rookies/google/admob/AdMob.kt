package eu.rookies.google.admob

import android.util.Log
import com.google.android.gms.ads.AdView
import com.google.android.gms.ads.MobileAds
import com.google.android.gms.ads.interstitial.InterstitialAd
import eu.rookies.google.admob.banner.BannerHelper
import eu.rookies.google.admob.interstitials.InterstitialsHelper
import eu.rookies.google.admob.rewardedads.RewardedAdsHelper
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class AdMob(godot: Godot) : GodotPlugin(godot) {
    private var pluginName = "AdMob"

    private lateinit var bannerAd: AdView
    private var interstitialAd: InterstitialAd? = null

    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        MobileAds.initialize(godot.requireContext()) { status ->
            for (entry in status.adapterStatusMap.entries) {
                Log.d(pluginName, "${entry.key} - ${entry.value}")
            }
        }
    }

    /////////////
    // Banners

    @UsedByGodot
    fun createBanner(id: String) = BannerHelper.createBanner(godot.requireContext(), id)

    @UsedByGodot
    fun loadBanner() = BannerHelper.loadBanner()

    @UsedByGodot
    fun destroyBanner() = BannerHelper.destroyBanner()

    @UsedByGodot
    fun hasPendingBannerEvent(event: String): Boolean = BannerHelper.hasPendingEvent(event)

    @UsedByGodot
    fun getPendingBannerEvent(event: String): String = BannerHelper.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingBannerEvent(event: String) = BannerHelper.clearPendingEvent(event)

    /////////////
    // Interstitials

    @UsedByGodot
    fun isInterstitialsReady(): Boolean = InterstitialsHelper.isReady()

    @UsedByGodot
    fun loadInterstitials(id: String) =
        InterstitialsHelper.load(godot.requireContext(), id)

    @UsedByGodot
    fun showInterstitial() = InterstitialsHelper.show(godot.requireActivity())

    @UsedByGodot
    fun hasPendingInterstitialEvent(event: String): Boolean =
        InterstitialsHelper.hasPendingEvent(event)

    @UsedByGodot
    fun getPendingInterstitialEvent(event: String): String =
        InterstitialsHelper.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingInterstitialEvent(event: String) = InterstitialsHelper.clearPendingEvent(event)

    /////////////////
    // Rewarded Ads

    @UsedByGodot
    fun isRewardedAdReady(): Boolean = RewardedAdsHelper.isReady()

    @UsedByGodot
    fun loadRewardedAd(id: String) = RewardedAdsHelper.load(godot.requireContext(), id)

    @UsedByGodot
    fun showRewardedAd() = RewardedAdsHelper.show(godot.requireActivity())

    @UsedByGodot
    fun hasPendingRewardedAdEvent(event: String): Boolean =
        RewardedAdsHelper.hasPendingEvents(event)

    @UsedByGodot
    fun getPendingRewardedAdEvent(event: String): String = RewardedAdsHelper.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingRewardedAdEvent(event: String) = RewardedAdsHelper.clearPendingEvent(event)
}