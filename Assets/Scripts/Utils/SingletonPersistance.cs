using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPersistance<T> : Singleton<T> where T : MonoBehaviour
{
    public virtual void Awake(){
        if(_instance!=null){
            Destroy(this.gameObject);
            return;
        }
        _instance= this as T;
        DontDestroyOnLoad(instance);
    }
}
