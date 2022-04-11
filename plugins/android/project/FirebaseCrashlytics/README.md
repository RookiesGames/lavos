# Firebase Crashlytics

## Setup

### Android

#### Project setup

1. Open your Godot Android project located in `res://android/build`
2. Open the `build.gradle` file and add the following:
```gradle
buildscript {
    // ...
    dependencies {
        //..

        // FirebaseCrashlytics
        classpath 'com.google.gms:google-services:4.3.10'
        classpath 'com.google.firebase:firebase-crashlytics-gradle:2.8.1'
    }
}
```
```gradle
apply plugin: 'com.android.application'

// FirebaseCrashlytics
apply plugin: 'com.google.gms.google-services'
apply plugin: 'com.google.firebase.crashlytics'
```

#### google-services.json

Place your `google-services.json` in the root of your Godot android project `res://android/build`

---

### iOS

#### Project setup

#### gogole

---