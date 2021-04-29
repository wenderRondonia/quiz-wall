//#define FB

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if FB
using Facebook.Unity;
#endif

//id 368260283924423
public static class FacebookManager{

    const string getUserPicName = "me?fields=id,name,picture.height(128)";
    static List<string> loginPermissions = new List<string>() { "public_profile" };
    
    public static string  GetAccessToken{get{
        #if FB
        return AccessToken.CurrentAccessToken.TokenString;
        #else
        return "";
        #endif
    }}

    public static bool IsLoggedIn{get{
        #if FB
        return FB.IsLoggedIn;
        #else
        return false;
        #endif
    }}
    public static bool IsInitialized{get{
        #if FB
        return FB.IsInitialized;
        #else
        return false;
        #endif
    }}

    public static void Init(Action callback=null){
        #if FB
        
        if(FB.IsInitialized){
             if(callback!=null) callback();
            return;
        }

        FB.Init(()=>{
             if(callback!=null) callback();
        });

        #else

        #endif
    }

    public static void LogOut(){
        if(!FacebookManager.IsInitialized){
            return;
        }

        if(!FacebookManager.IsLoggedIn){
            return;
        }

        #if FB
        FB.LogOut();
        #else

        #endif
    }

    
    public static void LogIn(Action onsucceed,Action<string> onfail=null){
        #if FB
        
        FB.LogInWithReadPermissions(loginPermissions, result =>{ 
            var hasNoError = result == null || string.IsNullOrEmpty(result.Error);
            if (hasNoError && AccessToken.CurrentAccessToken!=null){
                Debug.Log("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");
                if(onsucceed!=null) onsucceed();
            }else{
                //If Facebook authentication failed, we stop the cycle with the message
                Debug.LogError("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
                if(onfail!=null) onfail(result.Error);
            }
        });

        #else
        
        if(onfail!=null) onfail("Facebook not implemented");

        #endif
    }


    
    /// <summary>
    /// retrieve Facebook Avatar and Username and setup
    /// </summary>
    public static void RetrieveData(Action<FacebookData> onsucceed=null,Action onfail=null){
       
        #if FB

        FB.API(getUserPicName, HttpMethod.GET,result =>{
            if (!string.IsNullOrEmpty(result.Error) || result.Cancelled){
                Debug.Log("GetRequiredDataFBAndSetOnPlayFab failed result="+result.Error);
                if(onfail!=null) onfail();
                return;
            }
            string nickname= "";
            string url="";
            
            
            if(result.ResultDictionary.ContainsKey("name")){
                nickname = result.ResultDictionary["name"] as string;
            }else{
                Debug.LogError("Error: FB Login dont have name field!");
            }
            if(result.ResultDictionary.ContainsKey("picture")){
                var picData = result.ResultDictionary["picture"] as IDictionary;
                var data = picData["data"] as IDictionary;
                url = data["url"] as string;
                Debug.Log("FacebookManager set avatar url="+url);
            }else{
                Debug.LogError("Error: FB Login dont have picture field!");

            }
                        
            Debug.Log(result.RawResult);

            var facebookData = new FacebookData(nickname,url);
            if(onsucceed!=null) onsucceed(facebookData);
        },new Dictionary<string,string>(){});

        #endif

    }


}

public class FacebookData{
    public string username;
    public string url;
    public FacebookData(string _username,string _url){
        username = _username;
        url = _url;

    }
}