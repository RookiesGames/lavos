package eu.rookies.ironsource.rewardedads

import android.util.Log
import com.ironsource.mediationsdk.IronSource
import com.ironsource.mediationsdk.adunit.adapter.utility.AdInfo
import com.ironsource.mediationsdk.logger.IronSourceError
import com.ironsource.mediationsdk.model.Placement
import com.ironsource.mediationsdk.sdk.LevelPlayRewardedVideoListener
import eu.rookies.ironsource.AdInfoHelper
import eu.rookies.ironsource.IronSourceErrorHelper
import kotlinx.serialization.json.buildJsonObject

class RewardedAdsHelper {
    companion object {
        private const val tag = "RewardedAdsHelper"
        private val pendingEvents: MutableMap<EventKeys, String> = mutableMapOf()

        private fun addPendingEvent(key: EventKeys, value: String) {
            if (pendingEvents.containsKey(key)) {
                Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
            }
            pendingEvents[key] = value
        }

        fun init() {
            val levelPlayRewardedVideoListener = object : LevelPlayRewardedVideoListener {
                override fun onAdAvailable(adInfo: AdInfo) =
                    addPendingEvent(EventKeys.AD_AVAILABLE, AdInfoHelper.toJson(adInfo).toString())

                override fun onAdUnavailable() = addPendingEvent(EventKeys.AD_UNAVAILABLE, "")

                override fun onAdOpened(adInfo: AdInfo) =
                    addPendingEvent(
                        EventKeys.AD_OPENED,
                        AdInfoHelper.toJson(adInfo).toString()
                    )

                override fun onAdShowFailed(error: IronSourceError, adInfo: AdInfo) {
                    val json = buildJsonObject {
                        IronSourceErrorHelper.toJson(error)
                        AdInfoHelper.toJson(adInfo)
                    }
                    addPendingEvent(EventKeys.AD_SHOW_FAILED, json.toString())
                }

                override fun onAdClicked(placement: Placement, adInfo: AdInfo) {
                    val json = buildJsonObject {
                        PlacementHelper.toJson(placement)
                        AdInfoHelper.toJson(adInfo)
                    }
                    addPendingEvent(EventKeys.AD_CLICKED, json.toString())
                }

                override fun onAdRewarded(placement: Placement, adInfo: AdInfo) {
                    val json = buildJsonObject {
                        PlacementHelper.toJson(placement)
                        AdInfoHelper.toJson(adInfo)
                    }
                    addPendingEvent(EventKeys.AD_REWARDED, json.toString())
                }

                override fun onAdClosed(adInfo: AdInfo) =
                    addPendingEvent(EventKeys.AD_CLOSED, AdInfoHelper.toJson(adInfo).toString())
            }
            //
            IronSource.setLevelPlayRewardedVideoListener(levelPlayRewardedVideoListener)
        }

        //
        // Interface

        fun isRewardedAdAvailable(): Boolean = IronSource.isRewardedVideoAvailable()

        fun loadRewardedAd() = IronSource.loadRewardedVideo()

        fun showRewardedAd(placementName: String) = IronSource.showRewardedVideo(placementName)

        fun getPlacement(placementName: String): String {
            val placement = IronSource.getRewardedVideoPlacementInfo(placementName)
            val json = PlacementHelper.toJson(placement)
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