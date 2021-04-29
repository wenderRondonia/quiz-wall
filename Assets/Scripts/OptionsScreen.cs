using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsScreen : SingletonPersistance<OptionsScreen>
{
	public GameObject panel;

	[SerializeField] Button close;

	[SerializeField] Button music;
	[SerializeField] Button sounds;

	[SerializeField] Button profile;

	[SerializeField] Button privacy;

	[SerializeField] Button rateus;

	
	public override void Awake(){
		base.Awake();

		sounds.onClick.AddListener(OnClickSounds);
		music.onClick.AddListener(OnClickMusic);
		rateus.onClick.AddListener(OnClickRateUs);
		close.onClick.AddListener(OnClickClose);
		profile.onClick.AddListener(OnClickProfile);


	}

	void OnDisable(){
		UploadSettings();
	}
	
	public void Show(){
		FadePanel.FadeIn(panel);
		this.ExecuteIn(0.05f,()=>{
			UpdateUI();
		});
	}

	public void Hide(){
		FadePanel.FadeOut(panel);
	}


	void UpdateUI(){
		sounds.transform.Find("SoundOn").gameObject.SetActive(Prefs.GetSoundOn);
		sounds.transform.Find("SoundOff").gameObject.SetActive(!Prefs.GetSoundOn);

		music.transform.Find("MusicOn").gameObject.SetActive(Prefs.GetMusicOn);
		music.transform.Find("MusicOn").gameObject.SetActive(!Prefs.GetMusicOn);


	}

	void OnClickProfile()
    {
		SoundManager.PlayClick();
		profile.Focus();
		
		//TODO
		//ProfilScreen.instance.Show();
	}

	void OnClickSounds(){
		SoundManager.PlayClick();
		Prefs.SetSoundOn(!Prefs.GetSoundOn);
		AudioListener.volume = Prefs.GetSoundOn ? 1f : 0;
		UpdateUI();
		
	}

	void OnClickMusic()
	{
		SoundManager.PlayClick();
		Prefs.SetMusicOn(!Prefs.GetMusicOn);
		SoundManager.instance.music.enabled = false;
		UpdateUI();

	}

	void OnClickRateUs(){
		SoundManager.PlayClick();	
		rateus.Focus();
		
		string link = "market://details?id=" + Application.identifier;
		
		Debug.Log("RateUs="+link);

		Application.OpenURL(link);

	}


    void OnClickClose(){
        SoundManager.PlayClick();
		close.Focus();
		Hide();
    }


	public static void UploadSettings()
    {
		//TODO: implement firebase
    }

	public static void RetrieveSettings()
    {
		//TODO: implement firebase
	}


}
