//#define FIREBASE
#if FIREBASE
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
#endif
using System;
using UnityEngine;
using System.Collections.Generic;


public class FirebaseManager : SingletonPersistanceAuto<FirebaseManager>{
    
    #if FIREBASE
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    #endif
    
    bool firebaseInitialized;

    public static bool IsInitialized{get{return instance.firebaseInitialized;}}

    public static Dictionary<string,string> remoteConfig;

    
    
    public void Initialize(string userId){
        #if FIREBASE

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task=>{
            dependencyStatus = task.Result;
            InitializeFirebase(userId);
        });
        
        #endif

    }
    #if FIREBASE

    void InitializeFirebase(string userId) {
        
        if(dependencyStatus != DependencyStatus.Available){
            Debug.Log("Firebase Could not resolve all dependencies: " + dependencyStatus);
            return;
        }

        InitializeRemoteConfig();

        Debug.Log("Firebase Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        Debug.Log("Firebase Set user properties.");
     
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod,"Google");

        FirebaseAnalytics.SetUserId(userId);
        
        FirebaseAnalytics.SetSessionTimeoutDuration(new System.TimeSpan(0, 30, 0));
        firebaseInitialized = true;
    }

    void InitializeRemoteConfig(){
        var defaults = new Dictionary<string, object>();
               

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);

        Debug.Log("RemoteConfig configured and ready!");
        
    }

    #endif

    public static void Raise(string eventName,Dictionary<string, object> args=null){
        ///Debug.Log("Firebase.Raise="+eventName+" ignoring");
        
        
        #if FIREBASE
        if(args!=null){
            var parameters = ToParameters(args);
            try{
                FirebaseAnalytics.LogEvent(eventName,parameters);
            }catch(System.Exception e){
                Debug.Log("Firebase Failed to LogEvent="+e.Message);
            }
        }else{
            FirebaseAnalytics.LogEvent(eventName);
        }
        #endif
    }

    public static void FetchRemoteConfig(Action<Dictionary<string,string>> oncomplete=null){
        
        Debug.Log("FetchRemoteConfig");

        #if FIREBASE
        instance.ExecuteUntil(()=>IsInitialized,()=>{

            var fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(System.TimeSpan.Zero);
            fetchTask.ContinueWithOnMainThread(task=>{
                CheckRemoteConfig(task);
                remoteConfig= GetDictionary();
                if(oncomplete!=null) oncomplete(remoteConfig);
            });
        });
        #endif

    }

    public static string GetRemoteConfigValue(string key){
        var value = "";
        #if FIREBASE
        var value = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(key).StringValue;
        #endif
        return value;
    }

    #if FIREBASE

    static Dictionary<string,string> GetDictionary(){
        var result = new Dictionary<string,string>();
        var keys = Firebase.RemoteConfig.FirebaseRemoteConfig.Keys;
        foreach(var key in keys){
            var value = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(key).StringValue;
            //Debug.Log("GetDictionary "+key+"="+value);
            result.Add(key,value);
        }
        return result;
    }

     

    static void CheckRemoteConfig(System.Threading.Tasks.Task fetchTask) {
        if (fetchTask.IsCanceled) {
            Debug.Log("CheckRemoteConfig canceled.");
        } else if (fetchTask.IsFaulted) {
            Debug.Log("CheckRemoteConfig encountered an error.");
        } else if (fetchTask.IsCompleted) {
            Debug.Log("CheckRemoteConfig completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus) {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                Debug.Log("CheckRemoteConfig Remote data loaded and ready (last fetch time {0})."+info.FetchTime);
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason) {
                case Firebase.RemoteConfig.FetchFailureReason.Error:
                    Debug.Log("CheckRemoteConfig failed for unknown reason");
                    break;
                case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                    Debug.Log("CheckRemoteConfig throttled until " + info.ThrottledEndTime);
                    break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("CheckRemoteConfig Latest Fetch call still pending.");
                break;
        }
    }

    static Parameter[] ToParameters(Dictionary<string, object> args){
        var parameters = new Parameter[args.Count];
        int i=0;
        foreach(var kv in args){
            if(kv.Value is float) parameters[i] = new Parameter(kv.Key,(float)kv.Value);
            else if(kv.Value is int) parameters[i] = new Parameter(kv.Key,(int)kv.Value);
            else if(kv.Value is double) parameters[i] = new Parameter(kv.Key,(double)kv.Value);
            else parameters[i]  = new Parameter(kv.Key,kv.Value.ToString());
            
            i++;
        }

        return parameters;
    }

    
    #endif
}
