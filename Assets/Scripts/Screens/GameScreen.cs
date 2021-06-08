using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : Singleton<GameScreen>
{
    public Sprite spriteHighlightedPin;
    public Sprite spriteDefaultPin;



    void Start()
    {
        QuestionReader.ReadQuestions();

        SoundManager.PlayMusicGame();

        StartCoroutine(MainGameLoop());
       
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

}
