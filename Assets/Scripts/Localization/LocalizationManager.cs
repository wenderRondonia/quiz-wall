using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class LocalizationManager : SingletonPersistance<LocalizationManager> {
    Dictionary<string, string> localizedText;
    bool isReady;
    static List<LocalizedText> localizedTexts=new List<LocalizedText>();

    // Use this for initialization
    public override void Awake(){
        base.Awake();
        Initialize();
    }

    

    public void Initialize(){
        
        var language = GetLanguage();
        //Debug.Log("LocalizationManager Initialize language="+language);
       
        LoadLocalizedResource(language);
        
    }

    public static void SetLanguage(SystemLanguage language){
        PlayerPrefs.SetString("language",language.ToString());
    }
    public static SystemLanguage GetLanguage(){
        var languageString = PlayerPrefs.GetString("language",Application.systemLanguage.ToString());
        //var succeed = System.Enum.TryParse(languageString, out SystemLanguage language);
        var language = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage),languageString);
        //if(!succeed) language = Application.systemLanguage;
        
        return language;
    } 

    public static string GetLanguageCode(){
        switch(GetLanguage()){
            case SystemLanguage.English: return "en";
            case SystemLanguage.Spanish: return "es";
            case SystemLanguage.Portuguese: return "pt";

        }
        return "en";
    }

    public static void SwitchLanguage(SystemLanguage language,bool updateLocalizedTexts=true){
        LocalizationManager.SetLanguage(language);
		LocalizationManager.instance.Initialize();
        if(updateLocalizedTexts)
		    LocalizationManager.instance.UpdateLocalizedTexts();

    }
    

    public static void Subscribe(LocalizedText localizedText){
        if(!localizedTexts.Contains(localizedText))
            localizedTexts.Add(localizedText);
    }
    public void UpdateLocalizedTexts(){
        Debug.Log("UpdateLocalizedTexts");
        localizedTexts.RemoveAll(t=>t==null);
        foreach(var l in localizedTexts)
            if(l!=null)
                l.UpdateUI();
    }
    
    public void LoadLocalizedResource(SystemLanguage language){
        var localizationFile = "localization-";
        switch(language){
            case SystemLanguage.Portuguese: break;
            case SystemLanguage.English: localizationFile+="en"; break;
            case SystemLanguage.Spanish: localizationFile+="es"; break;
            default: 
                Debug.Log("LocalizationManager language not supported="+language+" using english");
                localizationFile+="en";
            break;
        }

        //Debug.Log("LoadLocalizedResource filename="+localizationFile);
        var textAsset = Resources.Load<TextAsset>(localizationFile);
        if(textAsset==null){
            if(localizedText!=null)
                localizedText.Clear();
            isReady = true;
        }else{
            ParseLocalization(textAsset.text);
        }
    }
    public void LoadLocalizedFile(string fileName){
        string filePath = Path.Combine (Application.streamingAssetsPath, fileName);

        if (File.Exists (filePath)) {
            ParseLocalization(File.ReadAllText (filePath));
        } else {
            Debug.LogError ("Cannot find file!");
        }

    }

    public void ParseLocalization(string dataAsJson){
        
        var loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        localizedText = new Dictionary<string, string> ();

        for (int i = 0; i < loadedData.items.Length; i++){
            var key = loadedData.items[i].key.ToLowerInvariant();
            if(!localizedText.ContainsKey(key))
                localizedText.Add(key,loadedData.items[i].value); 
            else
                Debug.Log("__already exist localization key="+key);
        }
        isReady = true;

        //Debug.Log("LocalizationManager dictionary " + localizedText.Count + " entries");
    }


    public bool Has(string key){
        if(localizedText==null){
            Initialize();
        }
        key = key.Replace("\\n","\n");
        if (localizedText.ContainsKey(key.ToLowerInvariant())) {
            return true;
        }else{
            
            
            return false;
        }
    }

    public static string Get(string key){
        return instance.GetLocalized(key);
    } 
    public string GetLocalized(string key){
        string result = key;
        if(GetLanguage()==SystemLanguage.Portuguese)
            return result;
        if(!isReady){
            Debug.Log("GetLocalized failed isready false key="+key);
            return key;
        }
        if(localizedText==null){
            Debug.Log("GetLocalized failed null dictionary false key="+key);
            return key;
        }
        if (localizedText.ContainsKey(key.ToLowerInvariant())) {
            result = localizedText[key.ToLowerInvariant()];
            //Debug.Log("GetLocalized lang="+GetLanguage()+" no key="+key+" value="+result);

        }else{
            Debug.Log("GetLocalized lang="+GetLanguage()+" no key="+key);

        }

        return result;

    }

    public bool GetIsReady(){
        return isReady;
    }

}

[System.Serializable]
public class LocalizationData {
    public LocalizationItem[] items;
}

[System.Serializable]
public class LocalizationItem{
    public string key;
    public string value;
}