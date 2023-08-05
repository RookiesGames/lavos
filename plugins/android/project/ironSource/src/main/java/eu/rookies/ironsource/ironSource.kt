package eu.rookies.ironsource

import android.util.Log
import androidx.annotation.NonNull
import com.ironsource.mediationsdk.IronSource
import com.ironsource.mediationsdk.integration.IntegrationHelper
import com.ironsource.mediationsdk.sdk.InitializationListener
import eu.novuloj.ironsource.banner.BannersHelper
import eu.rookies.ironsource.interstitials.InterstitialsHelper
import eu.rookies.ironsource.rewardedads.RewardedAdsHelper
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class ironSource(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = "GoogleBilling"
    private val bannerHelper = BannersHelper()

    override fun getPluginName(): String = pluginName

    // Activity lifecycle
    override fun onMainResume() {
        super.onMainResume()
        IronSource.onResume(godot.requireActivity())
    }

    override fun onMainPause() {
        super.onMainPause()
        IronSource.onPause(godot.requireActivity())
    }

    @UsedByGodot
    fun init(appKey: String) {
        InterstitialsHelper.init()
        RewardedAdsHelper.init()
        //
        IronSource.init(
            godot.requireContext(),
            appKey,
            InitializationListener {
                Log.d(pluginName, "Initialization complete")
                IntegrationHelper.validateIntegration(godot.requireContext())
            },
            IronSource.AD_UNIT.REWARDED_VIDEO,
            IronSource.AD_UNIT.INTERSTITIAL,
            IronSource.AD_UNIT.BANNER
        )
        // Track network changes
        IronSource.shouldTrackNetworkState(godot.requireContext(), true)
    }

    @UsedByGodot
    fun launchTestSuite(appKey: String) {
        IronSource.setMetaData("is_test_suite", "enable")
        IronSource.init(
            godot.requireContext(),
            appKey,
            InitializationListener {
                Log.d(pluginName, "Initialization complete")
                IntegrationHelper.validateIntegration(godot.requireContext())
                //
                Log.i(pluginName, "Starting test suite...")
                IronSource.launchTestSuite(godot.requireContext());
            })
    }

    @UsedByGodot
    fun gdpr(consented: Boolean) = IronSource.setConsent(consented)

    @UsedByGodot
    fun cpra(permitted: Boolean) =
        IronSource.setMetaData("do_not_sell", if (permitted) "false" else "true")

    @UsedByGodot
    fun childRestricted(restricted: Boolean) =
        IronSource.setMetaData("is_child_directed", if (restricted) "true" else "false")

    @UsedByGodot
    fun setUserId(id: String) = IronSource.setUserId(id)

    /////////////
    // Rewarded ads

    @UsedByGodot
    fun isRewardedAdAvailable(): Boolean = RewardedAdsHelper.isRewardedAdAvailable()

    @UsedByGodot
    fun loadRewardedAd() = RewardedAdsHelper.loadRewardedAd()

    @UsedByGodot
    fun showRewardedAd(placementName: String) = RewardedAdsHelper.showRewardedAd(placementName)

    @UsedByGodot
    fun getRewardedAdPlacement(placementName: String): String =
        RewardedAdsHelper.getPlacement(placementName)

    @UsedByGodot
    fun hasPendingRewardedAdEvent(event: String): Boolean = RewardedAdsHelper.hasPendingEvent(event)

    @UsedByGodot
    fun getPendingRewardedAdEvent(event: String): String = RewardedAdsHelper.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingRewardedAdEvent(event: String) = RewardedAdsHelper.clearPendingEvent(event)

    /////////////
    // Interstitials

    @UsedByGodot
    fun isInterstitialsReady(): Boolean = InterstitialsHelper.isInterstitialsReady()

    @UsedByGodot
    fun loadInterstitials() = InterstitialsHelper.loadInterstitials()

    @UsedByGodot
    fun showInterstitials(placementName: String) =
        InterstitialsHelper.showInterstitials(placementName)

    @UsedByGodot
    fun getInterstitialPlacement(placementName: String): String =
        InterstitialsHelper.getPlacement(placementName)

    @UsedByGodot
    fun hasPendingInterstitialEvent(event: String): Boolean =
        InterstitialsHelper.hasPendingEvent(event)

    @UsedByGodot
    fun getPendingInterstitialEvent(event: String): String =
        InterstitialsHelper.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingInterstitialEvent(event: String) = InterstitialsHelper.clearPendingEvent(event)

    /////////////
    // Banners

    @UsedByGodot
    fun createBanner() = bannerHelper.createBanner(godot.requireActivity())

    @UsedByGodot
    fun loadBanner(placementName: String) = bannerHelper.loadBanner(placementName)

    @UsedByGodot
    fun destroyBanner() = bannerHelper.destroyBanner()

    @UsedByGodot
    fun hasPendingBannerEvent(event: String): Boolean = bannerHelper.hasPendingEvent(event)

    @UsedByGodot
    fun getPendingBannerEvent(event: String): String = bannerHelper.getPendingEvent(event)

    @UsedByGodot
    fun clearPendingBannerEvent(event: String) = bannerHelper.clearPendingEvent(event)
}