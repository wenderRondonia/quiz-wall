using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : Singleton<GameScreen>
{
    public Sprite spriteHighlightedPin;
    public Sprite spriteDefaultPin;
    public Text moneyAmount;
    public Image moneyBackground;
    void Start()
    {
        QuestionReader.ReadQuestions();

        SoundManager.PlayMusicGame();

        StartCoroutine(MainGameLoop());

        Prefs.SetMoney(0);

        moneyAmount.text = Prefs.GetMoney.ToStringMoney();

    }

    IEnumerator MainGameLoop()
    {

        yield return FirstRound.DoingFirstRound();
        
        yield return CheckingEndGame();
        
        yield return SecondRound.DoingSecondRound();
        
        yield return CheckingEndGame();
        
        yield return WonScreen.instance.ShowingWin();

        yield return ThirdRound.DoingThirdRound();

        yield return CheckingEndGame();

        yield return WonScreen.instance.ShowingWin();

        LoadScreenManager.instance.LoadSceneScreen("Map");

    }      

    IEnumerator CheckingEndGame()
    {
        

        bool hasLost = Prefs.GetMoney <= 0;

        if (hasLost)
        {

            LostScreen.instance.Show();

            yield return new WaitWhile(() => true);
        }

        AdsManager.instance.ShowInterstitial();
    }

    
    public void LerpMoneyTo(float lastmoney ,float money)
    {
        StartCoroutine(LerpingMoneyTo(lastmoney, money));
    }


    IEnumerator LerpingMoneyTo(float lastmoney, float money)
    {
        float time = 0;
        float duration = 1f;
        float delta = 0.1f;
        float currentMoney = Prefs.GetMoney;

        if (money > lastmoney){
            moneyBackground.color = Color.green;
        }else{
            moneyBackground.color = Color.red;
        }

        while (time < duration)
        {
            time += delta;

            currentMoney = Mathf.Lerp(lastmoney, money, time/duration);
            SetMoney(currentMoney);
                        
            yield return new WaitForSeconds(delta);
        }

        SetMoney(money);
        moneyBackground.color = Color.white;

    }

    public void SetMoney(float money)
    {
        moneyAmount.text = money.ToStringMoney();
        
    }

}
