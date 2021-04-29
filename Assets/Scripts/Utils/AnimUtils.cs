using UnityEngine;
using UnityEngine.UI;
using System;

public static class AnimUtils{
    public static void Focus(this Button go,float amount=0.2f,float time=0.2f){
		Focus(go.gameObject,amount,time);
	}

	public static void Focus(this GameObject go,float amount=0.2f,float time=0.2f){

		if(go.GetComponent<iTween>()!=null){
			if(go.IsTween("AnimfocusGo") || go.IsTween("AnimfocusBack") ){
				return;
			}
			
		}
		iTween.ScaleTo (
			go,
			iTween.Hash (
				"name","AnimfocusGo",
				"scale", go.transform.localScale + go.transform.localScale * amount ,
				"time", time,
				"ignoretimescale",true,
				"easetype","linear"
			));
		
		iTween.ScaleFrom (
			go,
			iTween.Hash (
				"name","AnimfocusBack",
				"scale",go.transform.localScale + go.transform.localScale * amount ,
				"time",time,
				"ignoretimescale",true,
				"easetype","linear"
			));
		
	}



}
