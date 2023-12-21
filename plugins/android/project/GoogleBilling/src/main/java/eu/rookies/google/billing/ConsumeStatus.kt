package eu.rookies.google.billing

enum class ConsumeStatus(val id: Int) {
    None(0),
    InProgress(1),
    Completed(2),
    Error(3),
}