# GooglePlayGames Plugin

## Requirements

The Android Build Template needs to be installed in the project.

## Setup

Follow the steps described in the [Getting Started](https://developers.google.com/games/services/android/signin) guide from the official documentation

### AndroidManifest changes
Add you application id to the `AndroidManifest.xml` file in the Android Build Template

```xml
<!-- AndroidManifest.xml -->
<manifest>
  <application>
        <meta-data  
            android:name="com.google.android.gms.games.APP_ID"
            android:value="@string/game_services_project_id" />
    </application>
</manifest>
```

Add the project Id to the resources

```xml
<!-- res/values/strings.xml -->
<resources>
  <!-- Replace 0000000000 with your game’s project id.  -->
  <string translatable="false"  name="game_services_project_id"> 0000000000 </string>
</resources>
```