package eu.novuloj.ironsource.rewardedads

enum class EventKeys(val key: String) {
    AD_AVAILABLE("ad_available"),
    AD_UNAVAILABLE("ad_unavailable"),
    AD_OPENED("ad_opened"),
    AD_SHOW_FAILED("ad_show_failed"),
    AD_CLICKED("ad_clicked"),
    AD_REWARDED("ad_rewarded"),
    AD_CLOSED("ad_closed"),
}