using UnityEngine;
using UnityEngine.UI;

public class LoginFirstScreen : MonoBehaviour{
    public Button google;
    public Button guest;
    public Button facebook;

    void Awake(){
        
        facebook.onClick.AddListener(OnClickFacebook);
        guest.onClick.AddListener(OnClickGuest);
        google.onClick.AddListener(OnClickGoogle);

    }
    void OnClickGoogle(){
        SoundManager.PlayClick();
        
        LoginManager.instance.LoginGmail();
    }

    void OnClickFacebook(){
        SoundManager.PlayClick();
        facebook.Focus();
        LoginManager.instance.LoginFacebook();
    }

    void OnClickGuest(){
        SoundManager.PlayClick();
        guest.Focus();
        LoginManager.instance.LoginGuest();
    }   

}
