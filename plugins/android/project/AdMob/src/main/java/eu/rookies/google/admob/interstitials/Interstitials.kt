package eu.rookies.google.admob.interstitials

import android.app.Activity
import android.content.Context
import android.util.Log
import com.google.android.gms.ads.AdError
import com.google.android.gms.ads.AdRequest
import com.google.android.gms.ads.FullScreenContentCallback
import com.google.android.gms.ads.interstitial.InterstitialAd
import com.google.android.gms.ads.interstitial.InterstitialAdLoadCallback
import eu.rookies.google.admob.ShowAdError
import eu.rookies.google.admob.EventKey

class Interstitials {
    companion object {
        private const val tag = "InterstitialHelper"
        private var interstitialAd: InterstitialAd? = null
        private val pendingEvents = mutableMapOf<EventKey, String>()

        private fun addPendingEvent(key: EventKey, value: String) {
            if (pendingEvents.containsKey(key)) {
                Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
            }
            pendingEvents[key] = value
        }

        fun isReady(): Boolean = interstitialAd != null

        fun load(context: Context, id: String) {
            val adRequest = AdRequest.Builder().build()
            InterstitialAd.load(context,
                id,
                adRequest,
                object : InterstitialAdLoadCallback() {
                    override fun onAdFailedToLoad(adError: com.google.android.gms.ads.LoadAdError) {
                        interstitialAd = null
                        addPendingEvent(
                            EventKey.AD_LOAD_FAILED,
                            eu.rookies.google.admob.LoadAdError.toJson(id, adError).toString()
                        )
                    }

                    override fun onAdLoaded(ad: InterstitialAd) {
                        interstitialAd = ad
                        setListeners()
                        addPendingEvent(
                            EventKey.AD_LOAD_COMPLETED,
                            InterstitialAdInfo.toJson(ad).toString()
                        )
                    }
                })
        }

        private fun setListeners() {
            interstitialAd?.fullScreenContentCallback = object : FullScreenContentCallback() {
                override fun onAdClicked() {
                    addPendingEvent(
                        EventKey.AD_CLICKED,
                        InterstitialAdInfo.toJson(interstitialAd!!).toString()
                    )
                }

                override fun onAdDismissedFullScreenContent() {
                    addPendingEvent(
                        EventKey.AD_CLOSED,
                        InterstitialAdInfo.toJson(interstitialAd!!).toString()
                    )
                    interstitialAd = null
                }

                override fun onAdFailedToShowFullScreenContent(adError: AdError) {
                    addPendingEvent(
                        EventKey.AD_SHOW_FAILED,
                        ShowAdError.toJson(interstitialAd!!.adUnitId, adError).toString()
                    )
                    interstitialAd = null
                }

                override fun onAdImpression() {
                    addPendingEvent(
                        EventKey.AD_IMPRESSION,
                        InterstitialAdInfo.toJson(interstitialAd!!).toString()
                    )
                }

                override fun onAdShowedFullScreenContent() {
                    addPendingEvent(
                        EventKey.AD_SHOW_SUCCEEDED,
                        InterstitialAdInfo.toJson(interstitialAd!!).toString()
                    )
                }
            }
        }

        fun show(activity: Activity) {
            interstitialAd?.show(activity) ?: run {
                addPendingEvent(
                    EventKey.AD_SHOW_FAILED,
                    InterstitialAdInfo.toJson(interstitialAd!!).toString()
                )
            }
        }

        ///////////
        // Events

        fun hasPendingEvent(key: String): Boolean = pendingEvents.containsKey(EventKey.from(key))
        fun getPendingEvent(key: String): String = pendingEvents[EventKey.from(key)]!!
        fun clearPendingEvent(key: String) = pendingEvents.remove(EventKey.from(key))
    }
}