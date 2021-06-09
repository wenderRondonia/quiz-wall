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

        moneyAmount.text = Prefs.GetMoney.ToStringMoney();
    }

    IEnumerator MainGameLoop()
    {

        yield return FirstRound.DoingFirstRound();

        yield return CheckingEndGame();

        yield return SecondRound.DoingSecondRound();

        yield return CheckingEndGame();

        ShowWin();

        yield return ThirdRound.DoingThirdRound();

        ShowWin();

    }   
    

    void ShowWin()
    {

        WonScreen.instance.textAmount.text = "+ R$ 9 430";

        WonScreen.instance.Show();
    }

    IEnumerator CheckingEndGame()
    {
        bool hasLost = false;

        if (hasLost)
        {

            LostScreen.instance.Show();

            yield return new WaitUntil(() => true);
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
