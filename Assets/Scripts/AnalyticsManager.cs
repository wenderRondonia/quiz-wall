using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// interface for raising events.
/// </summary>
public static class AnalyticsManager{

   
    public static void Raise(string eventName,params object[] parameters){   
        var dict = new Dictionary<string,object>();
        for(int i=0;i<parameters.Length;i+=2){
            dict.Add((string)parameters[i],parameters[i+1]);
        }
       
        Raise(eventName,dict);
    }

    public static void Raise(string eventName,Dictionary<string, object> parameters=null) {        
        
        //TODO: firebase
    }

}