using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Builder {
    private static void BuildStandaloneWindows64() {
        // Set architecture in BuildSettings
        EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget.StandaloneWindows64;

        // Setup build options (e.g. scenes, build output location)
        var options = new BuildPlayerOptions {
            // Change to scenes from your project
            scenes = new[]
            {
                "Assets/Scenes/Main Menu.unity",
                "Assets/Scenes/NuMap.unity",
                "Assets/Scenes/Evaluation.unity"
            },
            // Change to location the output should go
            locationPathName = "./../BuiltGame/DisasterFamily.exe",
            options = BuildOptions.CleanBuildCache | BuildOptions.StrictMode,
            target = BuildTarget.StandaloneWindows64
        };

        var report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded) {
            Debug.Log($"Build successful - Build written to {options.locationPathName}");
        }
        else if (report.summary.result == BuildResult.Failed) {
            Debug.LogError($"Build failed");
        }
    }

    // This function will be called from the build process
    public static void Build() {
        // Build EmbeddedLinux ARM64 Unity player
        BuildStandaloneWindows64();
    }
}
