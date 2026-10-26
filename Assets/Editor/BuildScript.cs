using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript {
    private static readonly string[] SCENES = new string[] {
        "Assets/Scenes/MainMenuScene.unity",
        "Assets/Scenes/GameScene.unity",
        "Assets/Scenes/GameOverScene.unity"
    };

    public static void BuildWindows() {
        Debug.Log("Starting Windows Standalone build...");
        string buildDirectory = "Builds/Windows";
        if (!Directory.Exists(buildDirectory)) {
            Directory.CreateDirectory(buildDirectory);
        }

        string outputPath = Path.Combine(buildDirectory, "LuaLander.exe");

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
            scenes = SCENES,
            locationPathName = outputPath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded) {
            Debug.Log($"Windows build succeeded: {summary.totalSize} bytes in {summary.totalTime.TotalSeconds:F1} seconds.");
            EditorApplication.Exit(0);
        } else {
            Debug.LogError($"Windows build failed with result: {summary.result}, errors: {summary.totalErrors}");
            EditorApplication.Exit(1);
        }
    }

    public static void BuildAndroid() {
        Debug.Log("Starting Android APK build...");
        bool isSupported = BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android);
        Debug.Log("=== IS_ANDROID_SUPPORTED: " + isSupported + " ===");
        if (!isSupported) {
            Debug.LogError("Android build target is NOT supported by this Unity Editor installation.");
            EditorApplication.Exit(1);
            return;
        }
        string buildDirectory = "Builds/Android";
        if (!Directory.Exists(buildDirectory)) {
            Directory.CreateDirectory(buildDirectory);
        }

        string outputPath = Path.Combine(buildDirectory, "LuaLander.apk");

        // Ensure active target is switched to Android
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        EditorUserBuildSettings.buildAppBundle = false;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
            scenes = SCENES,
            locationPathName = outputPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded) {
            Debug.Log($"Android build succeeded: {summary.totalSize} bytes in {summary.totalTime.TotalSeconds:F1} seconds.");
            EditorApplication.Exit(0);
        } else {
            Debug.LogError($"Android build failed with result: {summary.result}, errors: {summary.totalErrors}");
            EditorApplication.Exit(1);
        }
    }
}
