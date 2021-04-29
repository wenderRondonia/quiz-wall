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

public static class LocalizationHelper{


    #if UNITY_EDITOR

    [MenuItem("Tools/Localization/Switch To English")]
    public static void SwitchToEn(){
        LocalizationManager.SwitchLanguage(SystemLanguage.English);
    }

    [MenuItem("Tools/Localization/Switch To Portuguese")]
    public static void SwitchToPt(){
        LocalizationManager.SwitchLanguage(SystemLanguage.Portuguese);
    }

    [MenuItem("Tools/Localization/Switch To Spanish")]
    public static void SwitchToEs(){
        LocalizationManager.SwitchLanguage(SystemLanguage.Spanish);
    }


    [MenuItem("Tools/Localization/Check Files")]
    public static void CheckFiles(){
        var languages = new []{SystemLanguage.English,SystemLanguage.Spanish};


        var guids = AssetDatabase.FindAssets(
            filter: "t:Script",
            searchInFolders:new string[]{"Assets"}
        );
        var prefix = "LocalizationManager.Get(";
        var allValues = new List<string>();

        foreach(var guid in guids){
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if(path.Contains("LocalizedText")) continue;
            if(path.Contains("LocalizedTextEditor")) continue;
            if(path.Contains("LocalizationManager")) continue;
            if(path.Contains("LocalizationHelper")) continue;


            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            if(textAsset==null)
                continue;
            var code = textAsset.text;
            var lines = code.Split('\n');
            foreach(var line in lines){
                var start = line.IndexOf(prefix);
                if(start<0)
                    continue;
                var lengthTarget = 0;
                for(lengthTarget=1; lengthTarget < line.Length-start;lengthTarget++){
                    var indexLetter = start+prefix.Length+lengthTarget;
                    if(indexLetter >= line.Length)
                        break;
                    var c = line[indexLetter];
                    if(c=='\"')
                        break;
                }
                var startIndex = start+prefix.Length + 1;
                var endIndex = Mathf.Clamp(lengthTarget-1,0,line.Length-1);
                var key = line.Substring(startIndex,endIndex);
                if(key.Length < 3)
                    continue;
                foreach(var language in languages){
                    LocalizationManager.SwitchLanguage(language,updateLocalizedTexts:false);
                    if(!LocalizationManager.instance.Has(key) ){
                        Debug.Log("not found key="+key+" language="+language+" path="+path);
                    }
                }
                
            }
        }
    }

    [MenuItem("Tools/Localization/Check Scenes")]
    public static void CheckScenes(){
        var languages = new []{SystemLanguage.English,SystemLanguage.Spanish};
       
        
        foreach(var scene in  EditorBuildSettings.scenes){
            var scenePath = scene.path;
            //Debug.Log("scene="+scenePath);
            var sceneAsset =AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            EditorSceneManager.OpenScene(scenePath,OpenSceneMode.Additive);
            var localizedTexts = Resources.FindObjectsOfTypeAll(typeof(LocalizedText));
            
            foreach(var language in languages){
                LocalizationManager.SwitchLanguage(language,updateLocalizedTexts:false);
                foreach(var localizedText in localizedTexts){
                    var lo =  (localizedText as LocalizedText);
                    var key = lo.myText.text;
                    if(!LocalizationManager.instance.Has(key) && lo.gameObject.scene.path == scene.path){
                        Debug.Log("not found key="+key+" path="+lo.gameObject.scene.name+"/"+lo.transform.GetPath());
                    }
                    
                }
            }
        
            EditorSceneManager.CloseScene(
                EditorSceneManager.GetSceneByName(sceneAsset.name),
                true
            );
        }
       
    }

    static string GetScenePath(string sceneName){
        return EditorBuildSettings.scenes.ToList().Find(p=>p.path.EndsWith("/"+sceneName+".unity")).path;
    }

    
    #endif

}