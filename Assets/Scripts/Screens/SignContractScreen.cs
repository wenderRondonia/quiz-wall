using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignContractScreen : Singleton<SignContractScreen>
{

    public GameObject panel;
    public Button buttonSign;
    public Button buttonPlay;

    void Start()
    {
        buttonSign.onClick.AddListener(OnButtonSign);
        buttonPlay.onClick.AddListener(OnButtonPlay);
    }

    void OnButtonSign()
    {
        SoundManager.PlayClick();
        buttonSign.Focus();

        //TODO
    }

    void OnButtonPlay()
    {

        SoundManager.PlayClick();
        buttonPlay.Focus();
        //TODO
    }


    public void Show()
    {
        FadePanel.FadeIn(panel);
    }

}
