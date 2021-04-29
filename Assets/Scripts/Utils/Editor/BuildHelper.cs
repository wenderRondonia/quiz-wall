using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;

#endif

public static class BuildHelper
{


#if UNITY_EDITOR




    [MenuItem("Tools/Build Helper/Utils/Open Folder")]
    public static void OpenFolder()
    {
        string fullPath = Application.dataPath.Replace("/Assets", "/") + "Builds/AndroidBundle";
        Debug.Log("OpenFolder=" + fullPath);
        EditorUtility.RevealInFinder(fullPath);
    }

 



    [MenuItem("Tools/Build Helper/Windows/All Scenes")]
    public static void BuildWindowsDebugAll()
    {

        Debug.Log("BuildWindows");

        string fullPath = Application.dataPath.Replace("/Assets", "/") + "Builds/Windows/HalloweenSlot/HalloweenSlot.exe";

        SetDevMode();

        var allScenes = EditorBuildSettings.scenes.ToList();


        BuildPipeline.BuildPlayer(
            allScenes.ToArray(),
            fullPath,
            BuildTarget.StandaloneWindows64,
            BuildOptions.Development
        );
    }

    [MenuItem("Tools/Build Helper/Android/Debug/Set")]
    public static void SetDevMode()
    {
        PlayerSettings.Android.useCustomKeystore = false;
        EditorUserBuildSettings.buildAppBundle=false;
        EditorUserBuildSettings.development = true;

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        //SetLogging(true);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);

        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        Debug.Log("SetDevMode");
    }

    [MenuItem("Tools/Build Helper/Android/Release/Set")]
    public static void SetReleaseMode()
    {
        SetAndroid();
        EditorUserBuildSettings.buildAppBundle = true;
        EditorUserBuildSettings.development = false;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
       // SetLogging(false);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_2_0);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_2_0);

        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        Debug.Log("SetReleaseMode");
    }

    [MenuItem("Tools/Build Helper/Android/Set Pass")]
    public static void SetAndroid()
    {

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keyaliasName = "naxos";
        //var dataFolder = "C:/Users/wende/Documents/HalloweenSlotData/";
        var dataFolder = @"D:\Dropbox\Rednew\Freelances\Robert\Dev\HalloweenSlotData\";
        PlayerSettings.Android.keystorePass = System.IO.File.ReadAllLines(dataFolder + "data.dat")[0];
        PlayerSettings.Android.keyaliasPass = System.IO.File.ReadAllLines(dataFolder + "data.dat")[1];
    }

    [MenuItem("Tools/Build Helper/Android/Debug/All Scenes")]
    public static void BuildAndroidDev()
    {

        Debug.Log("BuildAndroidDev");
        EditorUserBuildSettings.buildAppBundle = false;
        string fullPath = Application.dataPath.Replace("/Assets", "/") + "Builds/Android/HalloweenSlotDebug.apk";
        // EditorUserBuildSettings.buildAppBundle=false;
        //SetLogging(true);
        var allScenes = EditorBuildSettings.scenes;
        PlayerSettings.Android.bundleVersionCode = PlayerSettings.Android.bundleVersionCode + 1;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        SetAndroid();

        BuildPipeline.BuildPlayer(
            allScenes,
            fullPath,
            BuildTarget.Android,
            BuildOptions.Development
        );

    }


    [MenuItem("Tools/Build Helper/Android/Release/All Scenes Apk")]
    public static void BuildAndroidReleaseAllApk()
    {

        Debug.Log("BuildAndroidReleaseAllApk");

        string fullPath = Application.dataPath.Replace("/Assets", "/") + "Builds/Android/HalloweenSlot.apk";

        var allScenes = EditorBuildSettings.scenes;
        PlayerSettings.Android.bundleVersionCode = PlayerSettings.Android.bundleVersionCode + 1;
        SetAndroid();
        //SetLogging(false);
        SetReleaseMode();
        // EditorUserBuildSettings.buildAppBundle=false;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;

        BuildPipeline.BuildPlayer(
            allScenes,
            fullPath,
            BuildTarget.Android,
            BuildOptions.None
        );

    }

    [MenuItem("Tools/Build Helper/Android/Release/All Scenes Bundle")]
    public static void BuildAndroidReleaseAllBundle()
    {


        Debug.Log("BuildAndroidReleaseAllBundle");

        SetReleaseMode();

        PlayerSettings.Android.bundleVersionCode = PlayerSettings.Android.bundleVersionCode + 1;

        BuildPipeline.BuildPlayer(
            EditorBuildSettings.scenes,
            Application.dataPath.Replace("/Assets", "/") + "Builds/AndroidBundle/HalloweenSlot.aab",
            BuildTarget.Android,
            BuildOptions.CompressWithLz4HC
        );


    }

    static void SetLogging(bool logging)
    {
        PlayerSettings.usePlayerLog = logging;
        PlayerSettings.SetStackTraceLogType(LogType.Assert, logging ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Error, logging ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Exception, logging ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Log, logging ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Warning, logging ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);
    }



#endif

}
