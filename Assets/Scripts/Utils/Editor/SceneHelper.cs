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

public static class SceneHelper  {


    #if UNITY_EDITOR

    [MenuItem("Tools/Scenes/Menu")]
    public static void OpenMenu(){
        
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path,OpenSceneMode.Single);
    }

     [MenuItem("Tools/Scenes/Game")]
    public static void OpenGame(){
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[1].path,OpenSceneMode.Single);
    }

    #endif

}