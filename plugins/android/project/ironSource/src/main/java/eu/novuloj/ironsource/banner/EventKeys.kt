package eu.novuloj.ironsource.banner

enum class EventKeys(val key: String) {
    AD_LOADED("ad_loaded"),
    AD_LOAD_FAILED("ad_load_failed"),
    AD_CLICKED("ad_clocked"),
    AD_LEFT_APP("ad_left_app"),
    AD_SCREEN_PRESENTED("ad_screen_presented"),
    AD_SCREEN_DISMISSED("ad_screen_dismissed"),
}