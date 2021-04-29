using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>{

	public AudioSource click;
	public AudioSource coin;
	public AudioSource error;
	public AudioSource win;
    public AudioSource levelUp;

	public AudioSource music;

	public static void ConfigAudios(GameObject go){
        var audios = go.transform.GetComponentsInChildrenBFS<AudioSource>(includeInactive:true);
        
        var soundOnVolume = Prefs.GetSoundOn ? 1 : 0;
		foreach(var audio in audios){
			audio.volume = soundOnVolume;
		} 

    }
     

    public static void ConfigAllAudios(){
        var soundOnVolume = Prefs.GetSoundOn ? 1 : 0;
		var audios = FindObjectsOfType<AudioSource>();
		foreach(var audio in audios){
			audio.volume = soundOnVolume;
		} 


        var musicVolume = Prefs.GetMusicOn ? 1 : 0;
        if(SoundManager.instance!=null){
            SoundManager.instance.music.volume = musicVolume;
        }

    }
	public static void PlayLevelUp(){
		instance.levelUp.Play();
	}
	public static void PlayWin(){
		instance.win.Play();
	}
	public static void PlayCoin(){
		instance.coin.Play();
	}

	public static void PlayClick(){
		if(instance!=null)
		instance.click.Play();
	}
	public static void PlayError(){
		if(instance!=null)
			instance.error.Play();
	}

	public static void PlayMusic(){
		if(instance!=null)
			instance.music.Play();
	}
	
}
