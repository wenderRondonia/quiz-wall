using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    Canvas _canvas;
	public Canvas canvas{get{
		if(_canvas==null) _canvas = GetComponent<Canvas>();
		return _canvas;
	}}
    
    protected static T _instance;
    public static T instance{get{
        if(_instance==null) _instance = FindObjectOfType<T>();
        return _instance;
    }}

    public static T Instance(){
        return instance;
    }
    
}
