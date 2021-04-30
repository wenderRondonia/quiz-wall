#if DEVELOPMENT_BUILD 
    #define DEBUG_FADE
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FadePanel : SingletonPersistanceAuto<FadePanel>{


	public Action<GameObject> OnStartFadeIn;
	public Action<GameObject> OnStartFadeOut;
	public Action<GameObject> OnEndFadeIn;
	public Action<GameObject> OnEndFadeOut;

	//work with hierachy this way Canvas/Background/Panel
	//Background->black
	//Panel is the window
	//Canvas is gonna get CanvasGroup for fading
	public static Dictionary<GameObject,bool> fadingInDict = new Dictionary<GameObject,bool>();
	public static Dictionary<GameObject,bool> fadingOutDict = new Dictionary<GameObject,bool>();
	public static List<GameObject> fadedPanels = new List<GameObject>();
	public static float duration = 0.4f;
	public static bool IsAnyPanelShowing{get{
		return fadedPanels.Count>0;
	}}

	
	bool IsFadingIn(GameObject background){
		return fadingInDict.ContainsKey(background) && fadingInDict[background];
	}	

	void SetFadingIn(GameObject background,bool fadingIn){
		
		if(!fadingInDict.ContainsKey(background)){
			fadingInDict.Add(background,false);
		}
		fadingInDict[background] = fadingIn;
	}

	bool IsFadingOut(GameObject background){
		return fadingOutDict.ContainsKey(background) && fadingOutDict[background];
	}	

	void SetFadingOut(GameObject background,bool fadingOut){
		if(!fadingOutDict.ContainsKey(background)){
			fadingOutDict.Add(background,false);
		}
		fadingOutDict[background] = fadingOut;
	}

    public static void FadeIn(GameObject background,bool move=false,float delayFade=0f,bool scale=true){
		
		
        instance.StartCoroutine(instance.FadingIn(background,move,delayFade,scale));
    }
    public IEnumerator FadingIn(GameObject background,bool move= false, float delayFade=0f, bool scale = true) {
		if(IsFadingIn(background) || IsFadingOut(background))
			yield return new WaitWhile(()=>IsFadingIn(background) || IsFadingOut(background));
		if(background==null){
			yield break;
		}
        
		if(OnStartFadeIn!=null){
			OnStartFadeIn(background);
		}

        var fadePanel = background;



        var panel = background.transform.GetFirstChild().gameObject;
        var canvasObj = background.GetComponentInParent<Canvas>();
		var canvasGroup = canvasObj.GetOrAddComponent<CanvasGroup>();		

		SetFadingIn(background,true);
        canvasObj.enabled=true;
		background.SetActive(true);

		fadedPanels.Add(fadePanel);
		UpdateFadedPanels();

		var mode = canvasObj.renderMode;
		var dist = mode == RenderMode.ScreenSpaceOverlay ? -Screen.height :-6; 
		if(move){
			//yield return new WaitForEndOfFrame();
			iTween.MoveFrom (panel, panel.transform.position + new Vector3 (0f, dist, 0f), duration);
		}

        if (scale)
        {
			iTween.ScaleFrom(panel, panel.transform.localScale * 0.8f, duration);
		}

		canvasGroup.alpha = 0;
		if(delayFade>0)		
			yield return new WaitForSeconds(delayFade);

		var time=0f;
		var delta = 0.05f;
       
		var initial = canvasGroup.alpha;
		while (time < duration) {
			if(canvasGroup==null)
				break;
			canvasGroup.alpha  = Mathf.Lerp(initial,1,time/duration);
			yield return new WaitForSeconds(delta);
			time+=delta;
		}
		if(canvasGroup!=null)
        	canvasGroup.alpha = 1;
		
		if(move)
			yield return new WaitWhile(()=>panel!=null && panel.GetComponent<iTween>()!=null);
		
		
		SetFadingIn(background,false);
		//Debug.Log("FadeIn complete obj="+background.name);
		if(OnEndFadeIn!=null){
			OnEndFadeIn(background);
		}
	}

	void UpdateFadedPanels(){
		fadedPanels.RemoveAllNull();
		fadedPanels.RemoveAll(p=>!p.activeInHierarchy);
	}

	
    public static void FadeOut(GameObject background,bool move=false,bool scale=true){
	
		if(background==null){
			Debug.Log("FadeOut failed obj destroyed");
			return;
		}
		//Debug.Log("FadeOut obj="+background.name);
        
        instance.StartCoroutine(instance.FadingOut(background,move,scale));
    }
	public IEnumerator FadingOut(GameObject background,bool move=false,bool scale=true) {
		if(IsFadingIn(background) || IsFadingOut(background))
			yield return new WaitWhile(()=>IsFadingIn(background) || IsFadingOut(background));
		
	

		var panel = background.transform.GetFirstChild().gameObject;
        var canvasObj = background.GetComponentInParent<Canvas>();
		var canvasGroup = canvasObj.GetOrAddComponent<CanvasGroup>();
		
		SetFadingOut(background,true);	

		var fadePanel = background;

		if(OnStartFadeOut!=null){
			OnStartFadeOut(background);
		}

		#if DEBUG_FADE
		Debug.Log("FadingOut canvas="+canvasObj.name+" fadedPanels.Count="+fadedPanels.Count);
		#endif

		fadedPanels.Remove(fadePanel);
		UpdateFadedPanels();
        
		var original = Vector3.zero;
		var originalScale = Vector3.zero;

		if (background!=null){
			original = panel.transform.position;
			originalScale = panel.transform.localScale;

			var mode = canvasObj.renderMode;
			var dist = mode == RenderMode.ScreenSpaceOverlay ? -Screen.height :-6; 
			if(move)
				iTween.MoveAdd(panel, new Vector3 (0f,dist, 0f), duration*2);
			if (scale)
				iTween.ScaleAdd(panel, -Vector3.one*0.2f, duration);
		}
		
		var delta = 0.05f;
		var time = 0f;
        canvasGroup.alpha = 1;
		var initial = canvasGroup.alpha;
		
		while (time < duration) {
			if(canvasGroup==null)
				break;
			canvasGroup.alpha = Mathf.Lerp(initial,0,time/duration);
			time+=delta;
			yield return new WaitForSeconds(delta);
		}
		if(canvasGroup!=null)
        	canvasGroup.alpha = 0;
		if(panel!=null){
			if(move)
				yield return new WaitWhile(()=>background!=null && background.GetComponent<iTween>()!=null);
			if(panel!=null){
				panel.transform.position = original;
				panel.transform.localScale = originalScale;
				background.SetActive(false);
				//Debug.Log("FadeOut complete obj="+background.name);
			}else{
				//Debug.Log("Fadeout complete no obj");
			}
            canvasObj.enabled=false;
		}
		
		UpdateFadedPanels();

		//if(obj.GetComponent<iTween>()!=null)
		//	Destroy(obj.GetComponent<iTween>());
		SetFadingOut(background,false);

		if(OnEndFadeOut!=null){
			OnEndFadeOut(background);
		}

	}
	
}
