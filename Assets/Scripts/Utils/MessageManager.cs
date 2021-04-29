using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MessageManager : SingletonPersistance<MessageManager>{
 
    public GameObject popup;
    
    public Text title;
    public Button continueButton;
    public Toggle dontShowAgainToggle;
    Action onClose;
    bool autoClose=true;
    public override void Awake(){
        base.Awake();
        continueButton.onClick.AddListener(OnClickContinue);

    }

    void OnClickContinue(){
        SoundManager.PlayClick();
        if(autoClose)
            Hide();
    }


    public void Show(string message,Action onclose=null,bool dontShowAgain=false,bool autoClose=true,string buttonText="",bool hideClose=false){
        
        var hashMessage = message.GetHashCode();
        var dontShowAgainKey = "dontShowAgain_"+hashMessage;
        
        if(dontShowAgain && Prefs.GetBool(dontShowAgainKey)){
            return;
        }

        if(buttonText!=""){
            continueButton.GetComponentInChildren<Text>().text = buttonText;
        }

        title.text = message;
        onClose = onclose;
        this.autoClose = autoClose;
        dontShowAgainToggle.gameObject.SetActive(dontShowAgain);
        continueButton.gameObject.SetActive(!hideClose);
        if(dontShowAgain){
            
            dontShowAgainToggle.onValueChanged.AddListener(on=>{
                Prefs.SetBool(dontShowAgainKey,on);
            });
        }

        FadePanel.FadeIn(popup);
    }


    public void Hide(){
        FadePanel.FadeOut(popup);
        if(onClose!=null)
            onClose();
    }
}
