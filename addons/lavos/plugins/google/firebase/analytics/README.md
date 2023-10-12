# Firebase Analytics Addon

## Requirements

The Android Build Template needs to be installed in the project.

## Setup

### Config file

Download the `google-services.json` file from the Firebase dashboard and place it inside the Android Build Template project folder `<project>/android/build`

### Gradle changes

Add the following to the `build.gradle` file

```gradle
buildscript {
    ...

    dependencies {
        ...
        classpath 'com.google.gms:google-services:4.3.15'
    }
}
```

Below, add and apply the `google-services` plugin

```gradle
buildscript {
    ...
}

plugins {
    id 'com.google.gms.google-services' version '4.3.15' apply true
}
```