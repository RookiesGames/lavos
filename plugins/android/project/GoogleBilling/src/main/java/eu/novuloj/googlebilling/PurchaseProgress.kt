package eu.novuloj.googlebilling

enum class PurchaseProgress(val id: Int) {
    None(0),
    InProgress(1),
    Completed(2),
    Error(3),
}