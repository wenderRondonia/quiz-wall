
using UnityEngine;

public enum LoginMode{None=0,Facebook,Guest,Gmail}

public static class Prefs{

    public static void DeleteAll(){
        PlayerPrefs.DeleteKey("avatarUrl");
        PlayerPrefs.DeleteKey("customAvatar");
        PlayerPrefs.DeleteKey("nickname");
        PlayerPrefs.DeleteKey("password");
        PlayerPrefs.DeleteKey("loginMode");

    }

    public static void SetBool(string key, bool state){
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }
 
    public static bool GetBool(string key,bool defaultValue=false){
        int value = PlayerPrefs.GetInt(key,defaultValue ? 1 : 0);
        return value==1;
    }

    #region LOGIN
    public static LoginMode CurrentLoginMode
    {
        get { return (LoginMode)PlayerPrefs.GetInt("loginMode", 0); }
        set
        {
            PlayerPrefs.SetInt("loginMode", (int)value);
            PlayerPrefs.Save();
        }
    }
    public static float GetVolume{get{ return PlayerPrefs.GetFloat("musicvol",0.5f); }}
    public static void SetVolume(float vol){ PlayerPrefs.SetFloat("musicvol",vol); }
    public static bool GetSoundOn{get{ return PlayerPrefs.GetInt("snd_on",1)==1; }}
    public static void SetSoundOn(bool on){ PlayerPrefs.SetInt("snd_on", on ? 1 : 0 );}
    public static bool GetMusicOn{get{ return PlayerPrefs.GetInt("music_on",1)==1; }}
    public static void SetMusicOn(bool on){ PlayerPrefs.SetInt("music_on", on ? 1 : 0 );}

    public static string GetEmail{get{ return PlayerPrefs.GetString("email_account");}}
    public static void SetEmail(string email){ PlayerPrefs.SetString("email_account",email); PlayerPrefs.Save();}
    public static string GetPass{get{ return PlayerPrefs.GetString("password");}}
    public static void SetPass(string pass){ PlayerPrefs.SetString("password",pass); PlayerPrefs.Save();}

    public static string GenerateRandomNickname{get{return "Jogador"+Random.Range(1,999);}}
    public static string GetNickname{get{ return PlayerPrefs.GetString("nickname",GenerateRandomNickname);}}
    public static void SetNickname(string nickname){ PlayerPrefs.SetString("nickname",nickname); PlayerPrefs.Save();}

    public static void SetAvatarUrl(string url){ PlayerPrefs.SetString("avatarUrl",url); }
    public static string GetAvatarUrl{get{ return PlayerPrefs.GetString("avatarUrl"); }}
    public static void SetCustomAvatar(int avatar){ PlayerPrefs.SetInt("customAvatar",avatar); }
    public static int GetCustomAvatar{get{ return PlayerPrefs.GetInt("customAvatar",-1);}}

    #endregion

    public static int GetLevelCompleted(int level){
        
        return PlayerPrefs.GetInt("LevelCompleted_" + level);
    }

    public static void SetLevelCompleted(int level, int completed){
      
        PlayerPrefs.SetInt("LevelCompleted_" + level, completed);
    }

    public static float GetMoney{get{ 
        return PlayerPrefs.GetFloat("Money");
    }}

    public static void SetMoney(float money)
    {
        PlayerPrefs.SetFloat("Money",money);
    }
    public static void AddMoney(float money)
    {
        float newMoney = GetMoney + money;
        newMoney = Mathf.Clamp(newMoney,0,int.MaxValue);
        SetMoney(newMoney);
    }

    public static float GeStoredtMoney{get{ 
        return PlayerPrefs.GetFloat("StoredtMoney");
    }}

    public static void SetStoredMoney(float storedtMoney)
    {
        PlayerPrefs.SetFloat("StoredtMoney", storedtMoney);
    }

    public static void AddStoredMoney(float storedtMoney)
    {
        SetMoney(GeStoredtMoney + storedtMoney);
    }

}
