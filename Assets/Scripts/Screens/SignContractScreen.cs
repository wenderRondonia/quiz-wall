using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ContractAnswer
{
    NotAnswered,
    Sign,
    Continue
}

public class SignContractScreen : Singleton<SignContractScreen>
{

    public GameObject panel;
    public Button buttonSign;
    public Button buttonPlay;
    [Header("Runtime")]
    public ContractAnswer answer = ContractAnswer.NotAnswered;

    void Start()
    {
        buttonSign.onClick.AddListener(OnButtonSign);
        buttonPlay.onClick.AddListener(OnButtonPlay);
    }

    void OnButtonSign()
    {
        SoundManager.PlayClick();
        buttonSign.Focus();

        answer = ContractAnswer.Sign;
    }

    void OnButtonPlay()
    {

        SoundManager.PlayClick();
        buttonPlay.Focus();
        
        answer = ContractAnswer.Continue;

    }


    public void Show()
    {
        answer = ContractAnswer.NotAnswered;

        FadePanel.FadeIn(panel);
    }
    
    public void Hide()
    {
        
        FadePanel.FadeOut(panel);
    }

    public IEnumerator WaitingAnswer()
    {
        Show();

        yield return new WaitUntil(()=>answer!=ContractAnswer.NotAnswered);

        Hide();


    }

}
