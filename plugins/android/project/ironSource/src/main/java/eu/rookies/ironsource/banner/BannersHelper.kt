package eu.novuloj.ironsource.banner

import android.app.Activity
import android.util.Log
import com.ironsource.mediationsdk.ISBannerSize
import com.ironsource.mediationsdk.IronSource
import com.ironsource.mediationsdk.IronSourceBannerLayout
import com.ironsource.mediationsdk.adunit.adapter.utility.AdInfo
import com.ironsource.mediationsdk.logger.IronSourceError
import com.ironsource.mediationsdk.sdk.LevelPlayBannerListener
import eu.rookies.ironsource.AdInfoHelper
import eu.rookies.ironsource.IronSourceErrorHelper
import eu.rookies.ironsource.banner.EventKeys

class BannersHelper {
    private val tag = "BannersHelper"

    private lateinit var banner: IronSourceBannerLayout
    private val pendingEvents: MutableMap<EventKeys, String> = mutableMapOf()

    private fun addPendingEvent(key: EventKeys, value: String) {
        if (pendingEvents.containsKey(key)) {
            Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
        }
        pendingEvents[key] = value
    }

    private val levelPlayBannerListener = object : LevelPlayBannerListener {
        override fun onAdLoaded(adInfo: AdInfo) =
            addPendingEvent(EventKeys.AD_LOADED, AdInfoHelper.toJson(adInfo).toString())

        override fun onAdLoadFailed(error: IronSourceError) = addPendingEvent(
            EventKeys.AD_LOAD_FAILED,
            IronSourceErrorHelper.toJson(error).toString()
        )

        override fun onAdClicked(adInfo: AdInfo) =
            addPendingEvent(EventKeys.AD_CLICKED, AdInfoHelper.toJson(adInfo).toString())

        override fun onAdLeftApplication(adInfo: AdInfo) =
            addPendingEvent(EventKeys.AD_LEFT_APP, AdInfoHelper.toJson(adInfo).toString())

        override fun onAdScreenPresented(adInfo: AdInfo) =
            addPendingEvent(EventKeys.AD_SCREEN_PRESENTED, AdInfoHelper.toJson(adInfo).toString())

        override fun onAdScreenDismissed(adInfo: AdInfo) =
            addPendingEvent(EventKeys.AD_SCREEN_DISMISSED, AdInfoHelper.toJson(adInfo).toString())
    }

    //
    // Interface

    fun createBanner(activity: Activity) {
        banner = IronSource.createBanner(activity, ISBannerSize.BANNER)
        banner.levelPlayBannerListener = levelPlayBannerListener
    }

    fun loadBanner(placementName: String) = IronSource.loadBanner(banner, placementName)

    fun destroyBanner() = IronSource.destroyBanner(banner)

    //
    // Events

    fun hasPendingEvent(key: String): Boolean {
        val event = EventKeys.valueOf(key)
        return pendingEvents.containsKey(event)
    }

    fun getPendingEvent(key: String): String {
        val event = EventKeys.valueOf(key)
        return pendingEvents[event]!!
    }

    fun clearPendingEvent(key: String) {
        val event = EventKeys.valueOf(key)
        pendingEvents.remove(event)
    }
}