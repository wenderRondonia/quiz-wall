using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPersistanceAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    public static T instance{get{
        if(_instance==null) _instance = FindObjectOfType<T>();
        if(_instance==null) _instance = new GameObject(typeof(T).Name).AddComponent<T>();

        return _instance;
    }}


    public virtual void Awake(){
        if(FindObjectsOfType<T>().Length>1){
            Destroy(this.gameObject);
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(instance);
    }

}
