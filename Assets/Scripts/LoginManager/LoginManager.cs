
#if DEVELOPMENT_BUILD
    #define DEBUG_LOGIN
    //#define FORCE_OFFLINE
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System;
using Random=UnityEngine.Random;

public class LoginManager : Singleton<LoginManager> {

	CanvasGroup _canvasGroup;
	public CanvasGroup canvasGroup{get{
		if(_canvasGroup==null) _canvasGroup = GetComponent<CanvasGroup>();
		return _canvasGroup;
	}}
	public LoginFirstScreen loginFirstScreen;
    public static LoginData lastLoginData;

    public static bool IsLoggedIn { get { 
            //TODO: implemented
            return false; 
    } }

	void Start(){
        
        canvas.enabled=true;
  
        if(IsLoggedIn)
            AfterLoginSteps(login:true,alreadyLogged:true);
        else{
           AutoLogin();
        }
    }

    void AutoLogin(){

        #if DEBUG_LOGIN
        Debug.Log("AutoLogin="+Prefs.CurrentLoginMode);
        #endif

        if(Prefs.CurrentLoginMode==LoginMode.None){
            return;
        }
        
        loginFirstScreen.gameObject.SetActive(false);

        switch(Prefs.CurrentLoginMode){
            case LoginMode.Facebook:
                LoginFacebook();
            break;
            case LoginMode.Guest:
                LoginGuest();
            break;
            case LoginMode.Gmail:
                LoginGmail();
               
            break;
        }
        
        
    }

	public static void LogOut(){
        
        LoadSpinManager.instance.Hide();


        if(FacebookManager.IsLoggedIn)
            FacebookManager.LogOut();

        //if(GoogleManager.IsLogged) GoogleManager.LogOut();
        

        Prefs.DeleteAll();
        
        AvatarManager.instance.DeleteAvatarButton();

        SoundManager.instance.ExecuteIn(0.1f,()=>{
            SceneManager.LoadScene(0);
        });
    }


	public void Hide(){
        StartCoroutine(Hiding());
    }

    IEnumerator Hiding(){
        loginFirstScreen.gameObject.SetActive(false);
        
        AdsManager.instance.RequestInterstitial();
        

        while(canvasGroup.alpha>0){
            canvasGroup.alpha -= Time.deltaTime*1.2f;
            yield return new WaitForEndOfFrame();
        }
    
       	canvas.enabled=false;
        
        LoadSpinManager.instance.Hide();
    }

    public void LoginGuest(){
        
        LoadSpinManager.instance.Show();

        //TODO: fireabse login
        
    }

    public void LoginGmail()
    {

        LoadSpinManager.instance.Show();

        //TODO: fireabse login


    }

    public void LoginFacebook(){
        
        var supportsFB = Application.isEditor || Application.isMobilePlatform;
        if(!supportsFB){
            AfterLoginSteps(false);
            return;
        }

        LoadSpinManager.instance.Show();

        FacebookManager.Init();
            
        this.ExecuteUntil(()=>FacebookManager.IsInitialized,()=>{ //initialize
            FacebookManager.LogIn(onsucceed:()=>{ //log facebook
                //TODO: fireabse login
            }, onfail:OnFailedLogin);
        },onMaxTimer:10,onMaxtime:()=>OnFailedLogin("Time Out"));
                
    }

  


    void OnSucceedLogin(LoginData loginData){

        lastLoginData = loginData;

               
        if(!string.IsNullOrEmpty(loginData.url)){
            Prefs.SetAvatarUrl(loginData.url);
            //PlayFabManager.UpdateUserAvatar(loginData.url);
            AvatarManager.instance.GenerateAvatarUrl();
        }
        
        loginData.RetrievePrefsInt("customAvatar",a=>Prefs.SetCustomAvatar(a));
        loginData.RetrievePrefsFloat("experience",e=>Prefs.SetExperience(e));
        loginData.RetrievePrefsFloat("musicvol",m=>Prefs.SetVolume(m));
        loginData.RetrievePrefsBool("music_on",m=>Prefs.SetMusicOn(m));
        loginData.RetrievePrefsBool("snd_on",s=>Prefs.SetSoundOn(s));
        
        //PlayerPerfil.UpdatePlayerPerfil();

        LoadSpinManager.instance.Hide();

        AfterLoginSteps(true);
        
         
    }

    void OnFailedLogin(string errorMessage){
        Prefs.CurrentLoginMode = LoginMode.None;
        LoadSpinManager.instance.Hide();
        AfterLoginSteps(false);
        MessageManager.instance.Show("Error: " + errorMessage);
    }

	public void AfterLoginSteps(bool login = true,bool alreadyLogged=false){
        
        Debug.Log("AfterLoginSteps login="+login);
        if(login){
            loginFirstScreen.gameObject.SetActive(false);
            if(alreadyLogged)
                Hide();
            else
                StartCoroutine(LoggingAfter());
        }else{
            LogOut();           
            
        }
    }

	IEnumerator LoggingAfter(){

        Debug.Log("LoggingAfter start mode="+Prefs.CurrentLoginMode);

        //DoRetrieveCredits();

        
        //LeaderboardManager.RetrieveRankingData();        

        //PlayFabPrefs.RetrieveMySettings();

        //PlayFabPrefs.RetrieveMyExperience();

        yield return SignUpManager.instance.ShowingSignUp();

        //yield return UniqueDeviceLinkingPanel.instance.ProcessingUniqueDevice();

        //yield return LeaderboardWinners.instance.CheckingWinnersAll();
        
        //yield return Diaria.instance.CheckingDiaria();

        yield return new WaitWhile(() =>FadePanel.IsAnyPanelShowing);

        //AnalyticsManager.Raise("login", "type", Prefs.CurrentLoginMode);

        Hide();

    }


   

    public static string ValidateNickname(string nickname)
    {
        var errorMessage = "";
        if (string.IsNullOrEmpty(nickname))
        {
            errorMessage = LocalizationManager.Get("Campos não podem estar vazios.");
        }
        else if (!Regex.IsMatch(nickname, USERNAME_REGEX))
        {
            errorMessage = LocalizationManager.Get("Nome só pode conter caracteres A a Z e números de 1 a 9.");
        }
        else if(nickname.Length < 3)
        {
            errorMessage = LocalizationManager.Get("Nome deve ter mínimo de 3 caracteres.");
        }
       
        return errorMessage;
    }


    public const string EMAIL_REGEX = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
    public const string USERNAME_REGEX = @"^[A-Za-z0-9]*$";
    public static string ValidateLogin(string nickname,string email, string pass,string passConfirm,bool validateNickname=true){
        var errorMessage="";
        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(passConfirm)){
            errorMessage = LocalizationManager.Get("Campos não podem estar vazios.");
        }else if (!Regex.IsMatch(nickname, USERNAME_REGEX) && validateNickname){
            errorMessage = LocalizationManager.Get("Nome só pode conter caracteres A a Z e números de 1 a 9.");
        }else if (!Regex.IsMatch(email, EMAIL_REGEX) && !IsValid(email)){
            errorMessage = LocalizationManager.Get("Email em formato incorreto.");
        }else if (pass.Length < 6){
            errorMessage = LocalizationManager.Get("Senha deve ter mínimo de 6 caracteres.");
        }else if (!string.Equals(pass, passConfirm)){
            errorMessage = LocalizationManager.Get("Senhas não batem.");
        } 
        return errorMessage;
    }

    static bool IsValid(string emailaddress){
        try{
            MailAddress m = new MailAddress(emailaddress);
            return true;
        }catch (System.FormatException){
            return false;
        }
    }

}

[System.Serializable]
public class LoginData{
    public string nickname;
    public string url;
    public bool linked;
    public bool justCreated;
    public Dictionary<string,string> userData;
    public void OverrideEmpty(string n,string u){
        
        //if(string.IsNullOrEmpty(url)){
            url = u;
        //}

        if(string.IsNullOrEmpty(nickname) || nickname.Contains("Jogador") || nickname.Contains("Player") || nickname.Contains("Jugador")){
            nickname = n;
        }
    }

    public LoginData(string _nickname,string _url,bool _linked=false,bool _justCreated=false,Dictionary<string,string> _userData=null){
        nickname = _nickname;
        url = _url;
        linked = _linked;
        justCreated = _justCreated;
        userData = _userData;
    }

    public string GetPrefs(string prefs)
    {
        if (userData.ContainsKey(prefs))
        {
            return userData[prefs];
        }
        else
        {
            return "";
        }
    }


    public void RetrievePrefs(string prefs="",Action<string> onsucceed=null,Action onfail=null){
        if(userData.ContainsKey(prefs)){
            if(onsucceed!=null) onsucceed(userData[prefs]);
        }else{
            if(onfail!=null) onfail();
        }
    }
    public void RetrievePrefsBool(string prefs="",Action<bool> onsucceed=null,Action onfail=null){
        if(userData.ContainsKey(prefs)){
            var i = bool.Parse(userData[prefs]);
            if(onsucceed!=null) onsucceed(i);
        }else{
            if(onfail!=null) onfail();
        }
    }

    public void RetrievePrefsFloat(string prefs="",Action<float> onsucceed=null,Action onfail=null){

        if(userData.ContainsKey(prefs)){
            var i = float.Parse(userData[prefs]);
            if(onsucceed!=null) onsucceed(i);
        }else{
            if(onfail!=null) onfail();
        }
    }

    public void RetrievePrefsInt(string prefs="",Action<int> onsucceed=null,Action onfail=null){
        if(userData.ContainsKey(prefs)){
            var i = int.Parse(userData[prefs]);
            if(onsucceed!=null) onsucceed(i);
        }else{
            if(onfail!=null) onfail();
        }
    }

    public void RetrievePrefsDateTime(string prefs="",Action<DateTime> onsucceed=null,Action onfail=null){

        if(userData.ContainsKey(prefs)){
            string r = userData[prefs];
            var dateTime = DateTime.UtcNow;
            bool succeedParse = DateTime.TryParse(r,System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None,out dateTime);
            if(!succeedParse){
                #if DEBUG_PLAYFAB_PREFS
                Debug.LogError("failed parse prefs="+prefs+" value="+r);
                #endif
            }
            if(onsucceed!=null) onsucceed(dateTime);
        }else{
            if(onfail!=null) onfail();
        }

    }
}