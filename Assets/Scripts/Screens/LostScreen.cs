using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LostScreen : Singleton<LostScreen>
{
    [SerializeField] GameObject panel;
    [SerializeField] Button buttonMap;
    [SerializeField] Button buttonRestart;


    void Start()
    {
        buttonMap.onClick.AddListener(OnButtonMap);
        buttonRestart.onClick.AddListener(OnButtonRestart);
    }

    void OnButtonMap()
    {

        SoundManager.PlayClick();
        buttonMap.Focus();

        LoadScreenManager.instance.LoadSceneScreen("Map");
        SoundManager.StopMusicGame();

    }

    void OnButtonRestart()
    {

        SoundManager.PlayClick();
        buttonRestart.Focus();

        LoadScreenManager.instance.LoadSceneScreen("Game");

    }


    public void Show()
    {
        FadePanel.FadeIn(panel);
    }

}
