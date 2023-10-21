package eu.rookies.google.admob.banner

enum class BannerPosition(val key: Int) {
    None(0),
    Top(1),
    TopLeft(2),
    TopRight(3),
    Bottom(4),
    BottomLeft(5),
    BottomRight(6);

    companion object {
        fun fromInt(i: Int): BannerPosition = values().find { it.key == i } ?: None
    }
}