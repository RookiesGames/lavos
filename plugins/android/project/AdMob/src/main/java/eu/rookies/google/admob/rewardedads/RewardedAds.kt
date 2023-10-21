package eu.rookies.google.admob.rewardedads

import android.app.Activity
import android.content.Context
import android.util.Log
import com.google.android.gms.ads.AdError
import com.google.android.gms.ads.AdRequest
import com.google.android.gms.ads.FullScreenContentCallback
import com.google.android.gms.ads.rewarded.RewardedAd
import com.google.android.gms.ads.rewarded.RewardedAdLoadCallback
import eu.rookies.google.admob.ShowAdError
import eu.rookies.google.admob.EventKey

class RewardedAds {
    companion object {
        private const val tag = "RewardedAdsHelper"
        private var rewardedAd: RewardedAd? = null
        private val pendingEvents = mutableMapOf<EventKey, String>()

        private fun addPendingEvent(key: EventKey, value: String) {
            if (pendingEvents.containsKey(key)) {
                Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
            }
            pendingEvents[key] = value
        }

        fun isReady(): Boolean = rewardedAd != null

        fun load(context: Context, id: String) {
            val adRequest = AdRequest.Builder().build()
            RewardedAd.load(context,
                id,
                adRequest,
                object : RewardedAdLoadCallback() {
                    override fun onAdFailedToLoad(adError: com.google.android.gms.ads.LoadAdError) {
                        rewardedAd = null
                        addPendingEvent(
                            EventKey.AD_LOAD_FAILED,
                            eu.rookies.google.admob.LoadAdError.toJson(id, adError).toString()
                        )
                    }

                    override fun onAdLoaded(ad: RewardedAd) {
                        rewardedAd = ad
                        setListeners()
                        addPendingEvent(
                            EventKey.AD_LOAD_COMPLETED,
                            RewardedAdInfo.toJson(rewardedAd!!).toString()
                        )
                    }
                }
            )
        }

        private fun setListeners() {
            rewardedAd?.fullScreenContentCallback = object : FullScreenContentCallback() {
                override fun onAdClicked() {
                    addPendingEvent(
                        EventKey.AD_CLICKED,
                        RewardedAdInfo.toJson(rewardedAd!!).toString()
                    )
                }

                override fun onAdDismissedFullScreenContent() {
                    addPendingEvent(
                        EventKey.AD_CLOSED,
                        RewardedAdInfo.toJson(rewardedAd!!).toString()
                    )
                    rewardedAd = null
                }

                override fun onAdFailedToShowFullScreenContent(adError: AdError) {
                    addPendingEvent(
                        EventKey.AD_SHOW_FAILED,
                        ShowAdError.toJson(rewardedAd!!.adUnitId, adError).toString()
                    )
                    rewardedAd = null
                }

                override fun onAdImpression() {
                    addPendingEvent(
                        EventKey.AD_IMPRESSION,
                        RewardedAdInfo.toJson(rewardedAd!!).toString()
                    )
                }

                override fun onAdShowedFullScreenContent() {
                    addPendingEvent(
                        EventKey.AD_SHOW_SUCCEEDED,
                        RewardedAdInfo.toJson(rewardedAd!!).toString()
                    )
                }
            }
        }

        fun show(activity: Activity) {
            rewardedAd?.let { ad ->
                ad.show(activity) { rewardItem ->
                    rewardedAd = null
                    addPendingEvent(
                        EventKey.AD_REWARDED,
                        RewardInfo.toJson(rewardItem).toString()
                    )
                }
            } ?: run {
                addPendingEvent(
                    EventKey.AD_SHOW_FAILED,
                    RewardedAdInfo.toJson(rewardedAd!!).toString()
                )
            }
        }

        ///////////
        // Events

        fun hasPendingEvents(key: String): Boolean = pendingEvents.containsKey(EventKey.from(key))
        fun getPendingEvent(key: String): String = pendingEvents[EventKey.from(key)]!!
        fun clearPendingEvent(key: String) = pendingEvents.remove(EventKey.from(key))
    }
}