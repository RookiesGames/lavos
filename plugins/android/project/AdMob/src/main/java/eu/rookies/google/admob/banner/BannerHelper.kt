package eu.rookies.google.admob.banner

import android.content.Context
import android.util.Log
import com.google.android.gms.ads.AdListener
import com.google.android.gms.ads.AdRequest
import com.google.android.gms.ads.AdSize
import com.google.android.gms.ads.AdView
import com.google.android.gms.ads.LoadAdError
import eu.rookies.google.admob.EventKeys
import eu.rookies.google.admob.LoadAdErrorHelper

class BannerHelper {
    companion object {
        private const val tag = "BannerHelper"
        private lateinit var banner: AdView
        private val pendingEvents: MutableMap<EventKeys, String> = mutableMapOf()

        private fun addPendingEvent(key: EventKeys, value: String) {
            if (pendingEvents.containsKey(key)) {
                Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
            }
            pendingEvents[key] = value
        }

        fun createBanner(context: Context, id: String) {
            banner = AdView(context)
            banner.setAdSize(AdSize.BANNER)
            banner.adUnitId = id.ifEmpty { "ca-app-pub-3940256099942544/6300978111" }
            setListeners()
        }

        private fun setListeners() {
            banner.adListener = object : AdListener() {
                override fun onAdClicked() = addPendingEvent(EventKeys.AD_CLICKED, "")
                override fun onAdClosed() = addPendingEvent(EventKeys.AD_CLOSED, "")
                override fun onAdFailedToLoad(adError: LoadAdError) {
                    addPendingEvent(
                        EventKeys.AD_LOAD_FAILED,
                        LoadAdErrorHelper.toJson(adError).toString()
                    )
                }

                override fun onAdImpression() = addPendingEvent(EventKeys.AD_IMPRESSION, "")
                override fun onAdLoaded() = addPendingEvent(EventKeys.AD_READY, "")
                override fun onAdOpened() = addPendingEvent(EventKeys.AD_OPENED, "")
            }
        }

        fun loadBanner() {
            val adRequest = AdRequest.Builder().build()
            banner.loadAd(adRequest)
        }

        fun destroyBanner() = banner.destroy()

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