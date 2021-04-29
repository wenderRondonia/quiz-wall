using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadSpinManager : SingletonPersistance<LoadSpinManager>{

    public GameObject loader;
    public CanvasGroup canvasGroup;
    float fadingSpeed = 2f;

	public void Show(){
        if(loader.activeSelf){
            return;
        }
        StartCoroutine(FadingIn());

	}
    public void Hide(){
        if(!loader.activeSelf){
            return;
        }
        StartCoroutine(FadingOut());
    }

    IEnumerator FadingIn(){
        canvasGroup.blocksRaycasts = true;
        loader.SetActive(true);
        canvas.enabled=true;
        var deltaTime = 0.05f;
		while(canvasGroup.alpha<1){
			canvasGroup.alpha += deltaTime * fadingSpeed;
			yield return new WaitForSeconds(deltaTime);
		}
        canvasGroup.alpha=1;
	}
	
	IEnumerator FadingOut(){
        var deltaTime = 0.05f;
		while(canvasGroup.alpha>0){
			canvasGroup.alpha -= deltaTime*fadingSpeed;
			yield return new WaitForSeconds(deltaTime);
		}
        canvasGroup.alpha=0;
        canvasGroup.blocksRaycasts = false;
        loader.SetActive(false);
        canvas.enabled=false;

	}

}
