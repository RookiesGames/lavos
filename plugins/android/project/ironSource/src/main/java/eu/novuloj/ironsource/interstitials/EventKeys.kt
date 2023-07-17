package eu.novuloj.ironsource.interstitials

enum class EventKeys(val key: String) {
    AD_READY("ad_ready"),
    AD_LOAD_FAILED("ad_load_failed"),
    AD_OPENED("ad_opened"),
    AD_SHOW_SUCCEEDED("ad_show_succeeded"),
    AD_SHOW_FAILED("ad_show_failed"),
    AD_CLICKED("ad_clicked"),
    AD_CLOSED("ad_closed"),
}