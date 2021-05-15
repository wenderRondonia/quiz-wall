#if DEVELOPMENT_BUILD
#define DEBUG_LOADING
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreenManager : SingletonPersistance<LoadScreenManager>
{
	
	public GameObject panel;

	float deltaTime = 0.05f;
	float fadingSpeed = 1.5f;
	bool loadedScene;
	float speedMenu = 0.6f;
	float speed = 0.4f;
	float speedAfterLoaded = 0.6f;

	public void Show()
	{
		FadePanel.FadeIn(panel,scale: false);
	}

	public void Hide()
	{
		FadePanel.FadeOut(panel,scale:false);
	}

	public void LoadSceneScreen(string sceneName)
	{
		StartCoroutine(LoadingSceneScreen(sceneName));
	}

	IEnumerator LoadingSceneScreen(string sceneName)
	{
		loadedScene = false;

		//Debug.Log("LoadScreenManager.LoadingSceneScreen sceneName=" + sceneName);

		Show();
		//Debug.Log("LoadScreenManager.LoadingSceneScreen Show FadePanel.duration=" + FadePanel.duration);
		yield return new WaitForSeconds(FadePanel.duration);
		//Debug.Log("LoadScreenManager.LoadingSceneScreen LoadingAnimation");
		//StartCoroutine(LoadingAnimation());

		var startTime = System.DateTime.UtcNow;
		yield return new WaitForSeconds(0.5f);

		var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		yield return async;

		Resources.UnloadUnusedAssets();

#if DEBUG_LOADING
		var delta = System.DateTime.UtcNow - startTime;
		Debug.Log("LoadScreenManager.LoadingSceneScreen finished loading loadingtime=" + delta.TotalSeconds + " sceneName=" + sceneName);
#endif

		loadedScene = true;

		//Debug.Log("LoadScreenManager.LoadingSceneScreen Hide");

		//yield return new WaitUntil(() => bar.fillAmount >= 1f);
		Hide();

		yield return new WaitForSeconds(FadePanel.duration);

	}


}