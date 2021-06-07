using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WonScreen : Singleton<WonScreen>
{
    [SerializeField] GameObject panel;
    [SerializeField] Button buttonMenu;
    [SerializeField] Button buttonNext;
    public Text textAmount;

    void Start()
    {
        buttonMenu.onClick.AddListener(OnButtonMenu);
        buttonNext.onClick.AddListener(OnButtonNext);
    }

    void OnButtonMenu()
    {

        SoundManager.PlayClick();
        buttonMenu.Focus();

        LoadScreenManager.instance.LoadSceneScreen("Map");
        SoundManager.StopMusicGame();

    }

    void OnButtonNext()
    {

        SoundManager.PlayClick();
        buttonMenu.Focus();

        LoadScreenManager.instance.LoadSceneScreen("Game");

    }


    public void Show()
    {
        FadePanel.FadeIn(panel);
    }

    public void Hide()
    {
        FadePanel.FadeOut(panel);
    }

}
