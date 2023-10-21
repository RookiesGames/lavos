package eu.rookies.google.admob.banner

import android.R.attr
import android.app.Activity
import android.content.Context
import android.graphics.Color
import android.util.Log
import android.view.Gravity
import android.view.View
import android.view.ViewGroup
import android.view.Window
import android.widget.FrameLayout
import com.google.android.gms.ads.AdListener
import com.google.android.gms.ads.AdRequest
import com.google.android.gms.ads.AdSize
import com.google.android.gms.ads.AdView
import eu.rookies.google.admob.EventKey


class Banners {
    companion object {
        private const val tag = "BannerHelper"
        private var banner: AdView? = null
        private var loaded: Boolean = false
        private var showing: Boolean = false
        private var position: BannerPosition = BannerPosition.None
        private val pendingEvents: MutableMap<EventKey, String> = mutableMapOf()

        private fun addPendingEvent(key: EventKey, value: String) {
            if (pendingEvents.containsKey(key)) {
                Log.w(tag, "Overriding pending event $key: ${pendingEvents[key]}")
            }
            pendingEvents[key] = value
        }

        fun isLoaded(): Boolean = loaded
        fun isShowing(): Boolean = showing

        fun setPosition(newPosition: BannerPosition) {
            position = newPosition
            updatePosition()
        }

        fun getPosition(): BannerPosition = position

        private fun updatePosition() {
            if (!showing) return
            val layoutParams = getLayoutParams()
            banner!!.layoutParams = layoutParams
        }

        private fun getLayoutParams(): FrameLayout.LayoutParams {
            val adParams = FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.WRAP_CONTENT,
                FrameLayout.LayoutParams.WRAP_CONTENT
            )
            adParams.gravity = getLayoutGravityForPositionCode(position)
            return adParams
        }

        private fun getLayoutGravityForPositionCode(position: BannerPosition): Int {
            val gravity: Int = when (position) {
                BannerPosition.Top -> Gravity.TOP or Gravity.CENTER_HORIZONTAL
                BannerPosition.Bottom -> Gravity.BOTTOM or Gravity.CENTER_HORIZONTAL
                BannerPosition.TopLeft -> Gravity.TOP or Gravity.LEFT
                BannerPosition.TopRight -> Gravity.TOP or Gravity.RIGHT
                BannerPosition.BottomLeft -> Gravity.BOTTOM or Gravity.LEFT
                BannerPosition.BottomRight -> Gravity.BOTTOM or Gravity.RIGHT
                else -> {
                    Gravity.NO_GRAVITY
                }
            }
            return gravity
        }

        fun createBanner(activity: Activity, context: Context) {
            banner = AdView(context)
            banner!!.setAdSize(AdSize.BANNER)
            banner!!.visibility = View.GONE
            banner!!.setBackgroundColor(Color.TRANSPARENT)
            banner!!.descendantFocusability = ViewGroup.FOCUS_BLOCK_DESCENDANTS
            setListeners()
            //
            val layoutParams = FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.WRAP_CONTENT,
                FrameLayout.LayoutParams.WRAP_CONTENT
            )
            activity.addContentView(banner, layoutParams)
            //
            loaded = false
            position = BannerPosition.Bottom
        }

        private fun setListeners() {
            banner!!.adListener = object : AdListener() {
                override fun onAdClicked() {
                    addPendingEvent(
                        EventKey.AD_CLICKED,
                        BannerAdInfo.toJson(banner!!.adUnitId).toString()
                    )
                }

                override fun onAdClosed() {
                    showing = false
                    addPendingEvent(
                        EventKey.AD_CLOSED,
                        BannerAdInfo.toJson(banner!!.adUnitId).toString()
                    )
                }

                override fun onAdFailedToLoad(adError: com.google.android.gms.ads.LoadAdError) {
                    loaded = false
                    addPendingEvent(
                        EventKey.AD_LOAD_FAILED,
                        eu.rookies.google.admob.LoadAdError.toJson(banner!!.adUnitId, adError)
                            .toString()
                    )
                }

                override fun onAdImpression() {
                    addPendingEvent(
                        EventKey.AD_IMPRESSION,
                        BannerAdInfo.toJson(banner!!.adUnitId).toString()
                    )
                }

                override fun onAdLoaded() {
                    loaded = true
                    addPendingEvent(
                        EventKey.AD_LOAD_COMPLETED,
                        BannerAdInfo.toJson(banner!!.adUnitId).toString()
                    )
                }

                override fun onAdOpened() {
                    showing = true
                    addPendingEvent(
                        EventKey.AD_OPENED,
                        BannerAdInfo.toJson((banner!!.adUnitId)).toString()
                    )
                }
            }
        }

        fun loadBanner(id: String) {
            val adRequest = AdRequest.Builder().build()
            banner!!.adUnitId = id
            banner!!.loadAd(adRequest)
        }

        fun show() {
            showing = true
            banner!!.visibility = View.VISIBLE
            banner!!.resume()
            updatePosition()
        }

        fun hide() {
            showing = false
            banner!!.visibility = View.GONE
            banner!!.pause()
        }

        fun destroyBanner() {
            loaded = false
            if (banner != null) {
                hide()
                banner!!.destroy()
                banner = null
            }
        }

        ///////////
        // Events

        fun hasPendingEvent(key: String): Boolean = pendingEvents.containsKey(EventKey.from(key))
        fun getPendingEvent(key: String): String = pendingEvents[EventKey.from(key)]!!
        fun clearPendingEvent(key: String) = pendingEvents.remove(EventKey.from(key))
    }
}