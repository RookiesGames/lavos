#if TOOLS
using Godot;

namespace Lavos.Plugins.Google.Billing;

[Tool]
public sealed partial class GoogleBillingExportEditorPlugin : EditorExportPlugin
{
    public override string _GetName() => GooglePlugins.PluginNames.GoogleBilling;

    public override bool _SupportsPlatform(EditorExportPlatform platform)
    {
        return platform is EditorExportPlatformAndroid;
    }

    public override string[] _GetAndroidLibraries(EditorExportPlatform platform, bool debug)
    {
        var aar = $"{_GetName()}.{(debug ? "debug" : "release")}.aar";
        return new string[]
        {
            $"{GooglePlugins.Root}/billing/.bin/android/{aar}"
        };
    }

    public override string[] _GetAndroidDependencies(EditorExportPlatform platform, bool debug)
    {
        return new[]
        {
            "com.android.billingclient:billing-ktx:6.0.1",
            "org.jetbrains.kotlinx:kotlinx-serialization-json-jvm:1.4.1"
        };
    }

    public override string _GetAndroidManifestElementContents(EditorExportPlatform platform, bool debug)
    {
        return "\t<uses-permission android:name=\"com.android.vending.BILLING\" />\n";
    }
}
#endif