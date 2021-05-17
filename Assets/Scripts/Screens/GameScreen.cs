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
        SoundManager.PlayMusicGame();

        StartCoroutine(MainGameLoop());
       
    }

    IEnumerator MainGameLoop()
    {
        //yield return RoundScreen.ShowingNewRound();

       // yield return BallController.instance.DoAnimatingBalls();

        yield return DoingFirstQuestion();

       // yield return DoingPickZones();

        yield return new WaitForSeconds(1);
        SmallBallController.instance.StartSmallBall(2);
        SmallBallController.instance.StartSmallBall(3);
        SmallBallController.instance.ReleaseHolder();

    }

    IEnumerator DoingFirstQuestion()
    {
        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswersPreview();

        yield return new WaitForSeconds(0.5f);

        yield return CountdownScreen.instance.DoingCountdown();
        
        yield return new WaitForSeconds(0.5f);

        QuestionScreen.instance.ShowAnswers();

        yield return QuestionScreen.WaitingQuestionAnswer();
    }

    IEnumerator DoingPickZones()
    {
        PickZonesScreen.instance.Show();

        yield return PickZonesScreen.WaitingAnswer();

        yield return new WaitForSeconds(1);


    }


}
