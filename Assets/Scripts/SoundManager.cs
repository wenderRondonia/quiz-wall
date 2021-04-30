using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>{

	public AudioSource click;
	public AudioSource button;
	public AudioSource slider;

	public AudioSource musicMenu;
	public AudioSource musicGame;

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
    }

	public void UpdateSettings()
    {
		float musicVolume = Prefs.GetMusicOn ? 1 : 0;
		float soundVolume = Prefs.GetSoundOn ? 1 : 0;
		musicGame.volume = musicVolume;
		musicMenu.volume = musicVolume;

		var sounds = instance.GetComponentsInChildren<AudioSource>();
        foreach (var sound in sounds)
        {
            if (sound==musicGame || sound == musicMenu)
            {
				sound.volume = musicVolume;
            }
            else
            {
				sound.volume = soundVolume;
            }
        }

	}

	public static void PlayClick(){
		if(instance!=null)
			instance.click.Play();
	}
	

	public static void PlayMusicMenu(){
		if(instance!=null)
			instance.musicMenu.Play();
	}
	public static void PlayMusicGame()
	{
		if (instance != null)
			instance.musicGame.Play();
	}

}
