#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeletePlayerPrefs : EditorWindow{

    [MenuItem("Tools/PlayerPrefs/Save Editor")]
    static void SavePlayerPrefs(){

        //PlayerPrefs.SetInt("onlineworldcup_wins0",1);
        PlayerPrefs.Save();
		Debug.Log("Save Player Prefs Editor");

    }

    [MenuItem("Tools/PlayerPrefs/Delete Editor")]
    static void DeleteAllPlayerPrefs(){
        PlayerPrefs.DeleteAll();
		Debug.Log("Clear Player Prefs Editor");

    }


    //[MenuItem("Tools/PlayerPrefs/Delete Player")]
    //static void DeleteAllPlayerPrefsPlayer(){
    //    System.IO.File.Delete("/home/wender/.config/unity3d/Fox Bet/Truco Geek/Player.log");
	//	Debug.Log("Clear Player Prefs Player");
//
    //}


    [MenuItem("Tools/PlayerPrefs/Open Player")]
    static void OpenPlayerPrefs(){
        EditorUtility.RevealInFinder("/home/wender/.config/unity3d/Fox Bet/Truco Geek/Player.log");

    }

	[MenuItem("Tools/PlayerPrefs/Open Editor")]
    static void OpenPlayerPrefsEditor(){
        EditorUtility.RevealInFinder("/home/wender/.config/unity3d/Editor.log");
		
    }

}

#endif
