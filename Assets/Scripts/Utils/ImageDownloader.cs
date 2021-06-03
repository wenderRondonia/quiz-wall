using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Networking;
public class ImageDownloader : SingletonPersistanceAuto<ImageDownloader>{
    
    static Dictionary<string,Sprite> pics = new Dictionary<string, Sprite>();

    public static Sprite RetrivePic(string url){
        if(IsLoaded(url)){
            return pics[url];
        }else{
            return null;
        }
    }
    public static bool IsLoaded(string url){
        return pics.ContainsKey(url);
    }
    public static void GetPic(string url,Action<Sprite> onsucceed=null,Action onfail=null,bool forceFetch=false){
        instance.StartCoroutine(GettingPic(url,onsucceed,onfail,forceFetch));
    }
    public static IEnumerator GettingPic(string url,Action<Sprite> onsucceed=null,Action onfail=null,bool forceFetch=false){

        if(string.IsNullOrEmpty(url)){
            Debug.Log("GettingPic failed url null or empty");
            if(onfail!=null) onfail();
            yield break;
        }
        if(!forceFetch){
            if(pics.ContainsKey(url)){
                Debug.Log("cached pic url="+url);
                if(onsucceed!=null) onsucceed(pics[url]);
                yield break;
            }
        }

        Debug.Log("GettingPic start url=" + url);
        var www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();




        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("GettingPic failed url=" + url +" error="+www.error);

            // We got an Error or an Error Texture
            if (onfail!=null) onfail();        
        }else {
            Texture2D tex = DownloadHandlerTexture.GetContent(www);
            
            Debug.Log("GettingPic succeed url=" + url);

            var sprite =  Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            if(pics.ContainsKey(url))
               pics[url] = sprite;
            else
                pics.Add(url,sprite);
                
            if(onsucceed!=null) onsucceed(sprite);
        }

    }

}