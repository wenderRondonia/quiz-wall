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
    bool clickedContinue;

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

        //LoadScreenManager.instance.LoadSceneScreen("Game");
        clickedContinue = true;
    }

    public IEnumerator ShowingWin()
    {
        clickedContinue = false;

        WonScreen.instance.textAmount.text = Prefs.GetMoney.ToStringMoney() ;
        
        Prefs.AddStoredMoney(Prefs.GetMoney);

        Prefs.SetMoney(0);
        
        WonScreen.instance.Show();

        yield return new WaitUntil(()=>clickedContinue);

        WonScreen.instance.Hide();

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
