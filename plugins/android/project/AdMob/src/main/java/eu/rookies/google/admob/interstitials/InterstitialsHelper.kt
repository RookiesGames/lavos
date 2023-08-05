package eu.rookies.google.admob.interstitials

import android.app.Activity
import android.content.Context
import android.util.Log
import com.google.android.gms.ads.AdError
import com.google.android.gms.ads.AdRequest
import com.google.android.gms.ads.FullScreenContentCallback
import com.google.android.gms.ads.LoadAdError
import com.google.android.gms.ads.interstitial.InterstitialAd
import com.google.android.gms.ads.interstitial.InterstitialAdLoadCallback
import eu.rookies.google.admob.AdErrorHelper
import eu.rookies.google.admob.EventKeys
import eu.rookies.google.admob.LoadAdErrorHelper
import eu.rookies.google.admob.rewardedads.RewardedAdsHelper

class InterstitialsHelper {
    companion object {
        private const val tag = "InterstitialHelper"
        private val pendingEvents = mutableMapOf<EventKeys, String>()
        private var interstitialAd: InterstitialAd? = null

        private fun addPendingEvent(key: EventKeys, value: String) {
            if (pendingEvents.containsKey(key)) {
                Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
            }
            pendingEvents[key] = value
        }

        fun isReady(): Boolean = interstitialAd != null

        fun load(context: Context, id: String) {
            val adRequest = AdRequest.Builder()
                .build()
            InterstitialAd.load(
                context,
                id.ifEmpty { "ca-app-pub-3940256099942544/1033173712" },
                adRequest,
                object : InterstitialAdLoadCallback() {
                    override fun onAdFailedToLoad(adError: LoadAdError) {
                        Log.d(tag, "Failed to load interstitials. ${adError.toString()}")
                        interstitialAd = null
                        addPendingEvent(
                            EventKeys.AD_LOAD_FAILED,
                            LoadAdErrorHelper.toJson(adError).toString()
                        )
                    }

                    override fun onAdLoaded(ad: InterstitialAd) {
                        Log.d(tag, "Interstitial loaded")
                        interstitialAd = ad
                        setListeners()
                        addPendingEvent(
                            EventKeys.AD_READY,
                            InterstitialAdHelper.toJson(ad).toString()
                        )
                    }
                })
        }

        private fun setListeners() {
            interstitialAd?.fullScreenContentCallback = object : FullScreenContentCallback() {
                override fun onAdClicked() = addPendingEvent(EventKeys.AD_CLICKED, "")
                override fun onAdDismissedFullScreenContent() {
                    interstitialAd = null
                    addPendingEvent(EventKeys.AD_CLOSED, "")
                }

                override fun onAdFailedToShowFullScreenContent(adError: AdError) {
                    interstitialAd = null
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

        fun show(activity: Activity) = interstitialAd?.show(activity) ?: run {
            Log.e(
                tag,
                "Interstitial ad wasn't ready yet."
            )
        }

        ///////////
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