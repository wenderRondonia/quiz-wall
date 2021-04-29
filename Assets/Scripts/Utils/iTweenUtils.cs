using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class iTweenUtils  {

	public static void ExecuteOnCompleteTween( this MonoBehaviour mono,GameObject g,Action oncomplete=null, string _name=""){
		mono.StartCoroutine(ExecutingOnCompleteTween(
			g:g,oncomplete:oncomplete,_name:_name
		));
	}
	public static IEnumerator ExecutingOnCompleteTween(this GameObject g,Action oncomplete=null, string _name=""){
		yield return WaitingItween(g:g,_name:_name);
		oncomplete();
	}

	public static IEnumerator WaitingItween(this GameObject g, string _name=""){
		yield return new WaitWhile(()=>IsTween(g,_name));
	}

	public static IEnumerator WaitingItween(this Component g, string _name=""){
		yield return new WaitWhile(()=>HasITween(g,_name));
	}

	public static bool IsTween(this GameObject g, string _name=""){
		if(g==null){
			return false;
		}
		if(_name==""){
			return g.GetComponent<iTween>()!=null;
		}
		return g.GetComponents<iTween>().ToList().Exists(i=>i._name == _name);
	}

	public static bool HasITween(this Component t, string _name=""){
		if(t==null)
			return false;

		if(_name=="")
			return t.GetComponent<iTween>()!=null;

		return t.gameObject.GetComponents<iTween>().ToList().Exists(i=>i._name == _name);
	}

	public static int GetItweens(this Component t){
		return t.gameObject.GetComponents<iTween>().Length;
	}

	public static void RemoveTweens(this GameObject t,string tween=""){
		var itweens = t.GetComponents<iTween>();
		for(int i =0; i < itweens.Length;i++)
			if(tween=="" || tween!="" && tween == itweens[i]._name)
				GameObject.Destroy(itweens[i]);
	}

	public static void RemoveTweens(this Component t,string tween=""){
		var itweens = t.GetComponents<iTween>();
		for(int i =0; i < itweens.Length;i++)
			if(tween=="" || tween!="" && tween == itweens[i]._name)
				GameObject.Destroy(itweens[i]);
	}
}
