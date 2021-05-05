using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : Singleton<MenuScreen>
{
    public GameObject popup;

    [SerializeField] Button options;
    [SerializeField] Button singleplayer;
    [SerializeField] Button multiplayer;
    [SerializeField] Button ranking;
    [SerializeField] Text bankAmount;


    void Start()
    {
        options.onClick.AddListener(OnButtonClose);
        singleplayer.onClick.AddListener(OnButtonSingleplayer);
        multiplayer.onClick.AddListener(OnButtonMultiplayer);
        ranking.onClick.AddListener(OnButtonRanking);

     

    }

    void OnEnable()
    {
        bankAmount.text = "R$ " + Prefs.GetCreditos.ToStringNumber();
    }


    void OnButtonClose()
    {
        SoundManager.PlayClick();
        options.Focus();
        OptionsScreen.instance.Show();
    }

    void OnButtonSingleplayer()
    {
        SoundManager.PlayClick();
        singleplayer.Focus();
        LoadScreenManager.instance.LoadSceneScreen("Singleplayer");
        SoundManager.StopMusicMenu();
        SoundManager.PlayMusicGame();
    }

    void OnButtonMultiplayer()
    {
        SoundManager.PlayClick();
        multiplayer.Focus();
    }

    void OnButtonRanking()
    {
        SoundManager.PlayClick();
        ranking.Focus();
        RankingScreen.instance.Show();
    }

}
