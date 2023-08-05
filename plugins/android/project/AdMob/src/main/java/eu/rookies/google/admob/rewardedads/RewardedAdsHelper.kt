package eu.rookies.google.admob.rewardedads

import android.app.Activity
import android.content.Context
import android.media.metrics.Event
import android.util.Log
import com.google.android.gms.ads.AdError
import com.google.android.gms.ads.AdRequest
import com.google.android.gms.ads.FullScreenContentCallback
import com.google.android.gms.ads.LoadAdError
import com.google.android.gms.ads.OnUserEarnedRewardListener
import com.google.android.gms.ads.rewarded.RewardedAd
import com.google.android.gms.ads.rewarded.RewardedAdLoadCallback
import eu.rookies.google.admob.AdErrorHelper
import eu.rookies.google.admob.EventKeys
import eu.rookies.google.admob.LoadAdErrorHelper

class RewardedAdsHelper {
    companion object {
        private const val tag = "RewardedAdsHelper"
        private var rewardedAd: RewardedAd? = null
        private val pendingEvents = mutableMapOf<EventKeys, String>()

        private fun addPendingEvent(key: EventKeys, value: String) {
            if (pendingEvents.containsKey(key)) {
                Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
            }
            pendingEvents[key] = value
        }

        fun isReady(): Boolean = rewardedAd != null

        fun load(context: Context, id: String) {
            val adRequest = AdRequest.Builder().build()
            RewardedAd.load(
                context,
                id.ifEmpty { "" },
                adRequest,
                object : RewardedAdLoadCallback() {
                    override fun onAdFailedToLoad(adError: LoadAdError) {
                        rewardedAd = null
                        addPendingEvent(
                            EventKeys.AD_LOAD_FAILED,
                            LoadAdErrorHelper.toJson(adError).toString()
                        )
                    }

                    override fun onAdLoaded(ad: RewardedAd) {
                        rewardedAd = ad
                        addPendingEvent(EventKeys.AD_READY, RewardedAdHelper.toJson(ad).toString())
                    }
                })
        }

        private fun setListeners() {
            rewardedAd?.fullScreenContentCallback = object : FullScreenContentCallback() {
                override fun onAdClicked() = addPendingEvent(EventKeys.AD_CLICKED, "")
                override fun onAdDismissedFullScreenContent() {
                    rewardedAd = null
                    addPendingEvent(EventKeys.AD_SCREEN_DISMISSED, "")
                }

                override fun onAdFailedToShowFullScreenContent(adError: AdError) {
                    rewardedAd = null
                    addPendingEvent(
                        EventKeys.AD_SHOW_FAILED,
                        AdErrorHelper.toJson(adError).toString()
                    )
                }

                override fun onAdImpression() = addPendingEvent(EventKeys.AD_IMPRESSION, "")
                override fun onAdShowedFullScreenContent() =
                    addPendingEvent(EventKeys.AD_SHOW_SUCCEEDED, "")
            }
        }

        fun show(activity: Activity) {
            rewardedAd?.let { ad ->
                ad.show(activity) { rewardItem ->
                    addPendingEvent(
                        EventKeys.AD_REWARDED,
                        RewardItemHelper.toJson(rewardItem).toString()
                    )
                }
            } ?: run { Log.e(tag, "Rewarded ad wasn't ready yet.") }
        }

        ///////////
        // Events

        fun hasPendingEvents(key: String): Boolean {
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