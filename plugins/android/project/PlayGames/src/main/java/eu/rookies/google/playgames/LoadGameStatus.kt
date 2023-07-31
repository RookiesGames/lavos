package eu.rookies.google.playgames

enum class LoadGameStatus(val id: Int) {
    None(0),
    InProgress(1),
    Completed(2),
    Error(3)
}