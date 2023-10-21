# AdMob Plugin

## Requirements

The Android Build Template needs to be installed in the project.

## Setup

### AndroidManifest changes

Add your application id to the `AndroidManifest.xml` file in the Android Build Template

```xml
<manifest>
  <application>
    <!-- Sample AdMob app ID: ca-app-pub-3940256099942544~3347511713 -->
    <meta-data
        android:name="com.google.android.gms.ads.APPLICATION_ID"
        android:value="ca-app-pub-xxxxxxxxxxxxxxxx~yyyyyyyyyy"/>
  </application>
</manifest>
```