
using UnityEngine;

public enum LoginMode{None=0,Facebook,Guest,Gmail}

public static class Prefs{

    public static void DeleteAll(){
        PlayerPrefs.DeleteKey("avatarUrl");
        PlayerPrefs.DeleteKey("customAvatar");
        PlayerPrefs.DeleteKey("nickname");
        PlayerPrefs.DeleteKey("LAST_TIME");
        PlayerPrefs.DeleteKey("percent");
        PlayerPrefs.DeleteKey("score");
        PlayerPrefs.DeleteKey("rank");
        PlayerPrefs.DeleteKey("creditos");
        PlayerPrefs.DeleteKey("dia");
        PlayerPrefs.DeleteKey("diaria");
        PlayerPrefs.DeleteKey("email_account");
        PlayerPrefs.DeleteKey("password");
        PlayerPrefs.DeleteKey("LOGIN_TYPE");

    }

    public static void SetBool(string key, bool state){
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }
 
    public static bool GetBool(string key,bool defaultValue=false){
        int value = PlayerPrefs.GetInt(key,defaultValue ? 1 : 0);
        return value==1;
     }

    public static LoginMode CurrentLoginMode{
        get { return (LoginMode)PlayerPrefs.GetInt("LOGIN_TYPE", 0); }
        set{
            PlayerPrefs.SetInt("LOGIN_TYPE", (int)value);
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

   
   
    public static void AddExperience(float amount){ SetExperience(GetExperience+amount); }
    public static void SetExperience(float experience){ PlayerPrefs.SetFloat("percent",experience);}
    public static float GetExperiencePercent{get{ return (GetExperience%100f);}}


    public static float GetExperience{get{ return PlayerPrefs.GetFloat("percent");}}
    public static void AddScore(int amount){ SetScore(GetScore+amount);}
    public static void SetScore(int score){ PlayerPrefs.SetInt("score",score); }
    public static int GetScore{get{return PlayerPrefs.GetInt("score");}}
    public static void SetRank(int score){ PlayerPrefs.SetInt("rank",score);}
    public static int GetRank{get{ return PlayerPrefs.GetInt("rank");}}
    public static int GetLevel{get{ return ((int)GetExperience/100+1); }}


     public static void SetPremio(int amount){ PlayerPrefs.SetInt("premio",amount);}
    public static int GetPremio{get{ return PlayerPrefs.GetInt("premio");}}
    
    public static void SetCreditos(int amount){ PlayerPrefs.SetInt("credito",Mathf.Clamp(amount,0,int.MaxValue));}
    public static int GetCreditos{get{ return Mathf.Clamp(PlayerPrefs.GetInt("credito"),0,int.MaxValue);}}
    public static void SubtractCreditos(int amount){ SetCreditos(GetCreditos-amount); }
    public static void AddCreditos(int amount){ SetCreditos(GetCreditos+amount);  }
    
    public static int GetAcumuladoMin{get{ return PlayerPrefs.GetInt("acumualadoMin");}}
    public static int GetAcumuladoMed{get{ return PlayerPrefs.GetInt("acumualadoMed");}}
    public static int GetAcumuladoMax{get{ return PlayerPrefs.GetInt("acumualadoMax");}}

    public static void SetAcumuladoMin(int amount){ PlayerPrefs.SetInt("acumualadoMin",amount); }
    public static void SetAcumuladoMed(int amount){ PlayerPrefs.SetInt("acumualadoMed",amount); }
    public static void SetAcumuladoMax(int amount){ PlayerPrefs.SetInt("acumualadoMax",amount); }

    public static void AddAcumuladoMin(int amount){ SetAcumuladoMin(GetAcumuladoMin+amount); }
    public static void AddAcumuladoMed(int amount){ SetAcumuladoMed(GetAcumuladoMed+amount); }
    public static void AddAcumuladoMax(int amount){ SetAcumuladoMax(GetAcumuladoMax+amount); }
    


  

    public static string GetRoletaLastTime{get{ return PlayerPrefs.GetString("LastTimeRoleta"); }}
    public static void SetRoletaLastTime(string lastTime){ PlayerPrefs.SetString("LastTimeRoleta",lastTime); }

    public static string GetFreeCoinsLastTime{get{ return PlayerPrefs.GetString("FreeCoinsRoleta"); }}
    public static void SetFreeCoinsLastTime(string lastTime){ PlayerPrefs.SetString("FreeCoinsRoleta",lastTime); }

    public static string GetDiaria{get{ return PlayerPrefs.GetString("diaria");}}
    public static void SetDiaria(string dia){ PlayerPrefs.SetString("diaria",dia); }
    public static int GetDia{get{ return PlayerPrefs.GetInt("dia"); }}
    public static void SetDia(int dia){ PlayerPrefs.SetInt("dia",dia);}



    
}
