using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpManager : Singleton<SignUpManager>{

    public GameObject panel;
    public Button confirm;
    public Button avatar;
    public InputField nicknameInputField;
    public Image avatarPic;

    bool editedNickname;
    bool editedAvatar;

    void Start()
    {
        
        confirm.onClick.AddListener(OnClickConfirm);
        nicknameInputField.onValueChanged.AddListener(OnNicknameChanged);
        avatar.onClick.AddListener(OnClickAvatar);
    }

    void OnNicknameChanged(string nickname)
    {
        
        editedNickname = nickname.Length >= 3;

        if (editedAvatar && editedNickname)
        {
            confirm.interactable = true;
        }

    }

    void OnClickConfirm()
    {
        SoundManager.PlayClick();
        confirm.Focus();

        string nickname = nicknameInputField.text;
        string validateNickname = LoginManager.ValidateNickname(nickname);
        
        if (validateNickname!="")
        {
            MessageManager.instance.Show(validateNickname);
            return;
        }

        LoadSpinManager.instance.Show();
        /*
        //TODO:
        PlayFabManager.UploadUserNickname(nickname, onsucceed: () => {
            Prefs.SetNickname(nickname);
            PlayerPerfil.UpdatePlayerPerfil();
            
            this.ExecuteIn(0.6f, () => {
                LoadSpinManager.instance.Hide();
                Hide();
            });

            
        }, onfailed: () => {
            this.ExecuteIn(0.6f, () => {
                LoadSpinManager.instance.Hide();
                MessageManager.instance.Show(LocalizationManager.Get("Nome não disponível!"));
            });
        });
        */
        
    }

    void OnClickAvatar()
    {
        SoundManager.PlayClick();
        editedAvatar = true;
        avatar.Focus();
        AvatarManager.instance.Show();
    }


    public void Hide()
    {
        AvatarManager.instance.onClosePanel -= OnCloseAvatarManager;
        FadePanel.FadeOut(panel);
        this.ExecuteIn(0.5f, () => {
            canvas.enabled = false;
        });
    }

    public void Show()
    {
        AvatarManager.instance.onClosePanel += OnCloseAvatarManager;
        canvas.enabled = true;
        FadePanel.FadeIn(panel);
    }

    void OnCloseAvatarManager()
    {
        avatarPic.sprite = AvatarManager.instance.GetAvatarSprite();

        if (editedAvatar && editedNickname)
        {
            confirm.interactable = true;
        }


    }


    public IEnumerator ShowingSignUp()
    {
        if (LoginManager.lastLoginData.linked)
        {
            Debug.Log("ShowingSignUp canceling due accountWasLinked");

            yield break;
        }


        if (LoginManager.lastLoginData.GetPrefs("SignUp") == "True")
        {
            Debug.Log("ShowingSignUp canceling due SignUp=True");
            yield break;

        }

        if (Prefs.GetCustomAvatar!=-1 || (Prefs.GetNickname!="" && !Prefs.GetNickname.Contains("Jogador")) )
        {
            Debug.Log("ShowingSignUp canceling due already define customAvatar"+Prefs.GetCustomAvatar+" or nickname="+Prefs.GetNickname);
            yield break;
                
        }

        Debug.Log("ShowingSignUp start");

        LoadSpinManager.instance.Hide();

        SignUpManager.instance.Show();

        yield return new WaitWhile(() => SignUpManager.instance.canvas.enabled);

        Debug.Log("ShowingSignUp end");

        LoadSpinManager.instance.Show();

        //TODO

        //PlayFabPrefs.UploadPrefs("SignUp", "True");


    }

}
