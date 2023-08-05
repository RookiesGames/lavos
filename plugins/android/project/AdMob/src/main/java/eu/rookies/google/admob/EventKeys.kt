package eu.rookies.google.admob

enum class EventKeys(val key: String) {
    AD_READY("ad_ready"),
    AD_LOAD_FAILED("ad_load_failed"),

    AD_OPENED("ad_opened"),
    AD_CLICKED("ad_clicked"),
    AD_CLOSED("ad_closed"),

    AD_SHOW_SUCCEEDED("ad_show_succeeded"),
    AD_SHOW_FAILED("ad_show_failed"),

    AD_IMPRESSION("ad_impression"),

    // Rewarded
    AD_AVAILABLE("ad_available"),
    AD_UNAVAILABLE("ad_unavailable"),
    AD_REWARDED("ad_rewarded"),

    // Banner
    AD_LEFT_APP("ad_left_app"),
    AD_SCREEN_PRESENTED("ad_screen_presented"),
    AD_SCREEN_DISMISSED("ad_screen_dismissed"),
}