using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{

    void Start()
    {
        SoundManager.PlayMusicGame();

        StartCoroutine(MainGameLoop());
       
    }

    IEnumerator MainGameLoop()
    {
        yield return RoundScreen.ShowingNewRound();

        yield return BallController.instance.DoAnimatingBalls();

        yield return DoingFirstQuestion();

        yield return DoingPickZones();

    }

    IEnumerator DoingFirstQuestion()
    {
        QuestionScreen.instance.Show();

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

        yield return new WaitForSeconds(2);

        SmallBallController.instance.ReleaseHolder();
    }


}
