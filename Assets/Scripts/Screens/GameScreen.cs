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

        QuestionScreen.instance.Show();

        CountdownScreen.instance.Show();

        yield return QuestionScreen.WaitingQuestionAnswer();

        PickZonesScreen.instance.Show();

        yield return PickZonesScreen.WaitingAnswer();



    }

}
