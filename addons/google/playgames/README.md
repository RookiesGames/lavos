# GooglePlayGames Plugin

## Requirements

The Android Build Template needs to be installed in the project.

## Setup

### AndroidManifest changes
Add you application id to the `AndroidManifest.xml` file in the Android Build Template

```xml
<manifest>
  <application>
        <meta-data  
            android:name="com.google.android.gms.games.APP_ID"
            android:value="xxx" />
    </application>
</manifest>
```