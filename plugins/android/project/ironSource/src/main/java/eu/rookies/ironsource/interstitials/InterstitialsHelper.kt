package eu.rookies.ironsource.interstitials

import android.util.Log
import com.ironsource.mediationsdk.IronSource
import com.ironsource.mediationsdk.adunit.adapter.utility.AdInfo
import com.ironsource.mediationsdk.logger.IronSourceError
import com.ironsource.mediationsdk.sdk.LevelPlayInterstitialListener
import eu.rookies.ironsource.AdInfoHelper
import eu.rookies.ironsource.IronSourceErrorHelper
import kotlinx.serialization.json.buildJsonObject

class InterstitialsHelper {
    companion object {
        private const val tag = "InterstitialAdsHelper"
        private val pendingEvents: MutableMap<EventKeys, String> = mutableMapOf()

        private fun addPendingEvent(key: EventKeys, value: String) {
            if (pendingEvents.containsKey(key)) {
                Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
            }
            pendingEvents[key] = value
        }

        fun init() {
            val levelPlayInterstitialListener = object : LevelPlayInterstitialListener {
                override fun onAdReady(adInfo: AdInfo) =
                    addPendingEvent(EventKeys.AD_READY, AdInfoHelper.toJson(adInfo).toString())

                override fun onAdLoadFailed(error: IronSourceError) =
                    addPendingEvent(
                        EventKeys.AD_LOAD_FAILED,
                        IronSourceErrorHelper.toJson(error).toString()
                    )

                override fun onAdOpened(adInfo: AdInfo) =
                    addPendingEvent(EventKeys.AD_OPENED, AdInfoHelper.toJson(adInfo).toString())

                override fun onAdShowSucceeded(adInfo: AdInfo) =
                    addPendingEvent(
                        EventKeys.AD_SHOW_SUCCEEDED,
                        AdInfoHelper.toJson(adInfo).toString()
                    )

                override fun onAdShowFailed(error: IronSourceError, adInfo: AdInfo) {
                    val json = buildJsonObject {
                        IronSourceErrorHelper.toJson(error)
                        AdInfoHelper.toJson(adInfo)
                    }
                    addPendingEvent(EventKeys.AD_SHOW_FAILED, json.toString())
                }

                override fun onAdClicked(adInfo: AdInfo) =
                    addPendingEvent(EventKeys.AD_CLICKED, AdInfoHelper.toJson(adInfo).toString())

                override fun onAdClosed(adInfo: AdInfo) =
                    addPendingEvent(EventKeys.AD_CLOSED, AdInfoHelper.toJson(adInfo).toString())
            }
            //
            IronSource.setLevelPlayInterstitialListener(levelPlayInterstitialListener)
        }

        //
        // Interface

        fun isInterstitialsReady(): Boolean = IronSource.isInterstitialReady()

        fun loadInterstitials() = IronSource.loadInterstitial()

        fun showInterstitials(placementName: String) = IronSource.showInterstitial(placementName)

        fun getPlacement(placementName: String): String {
            val placement = IronSource.getInterstitialPlacementInfo(placementName)
            val json = InterstitialPlacementHelper.toJson(placement)
            return json.toString()
        }

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
}