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

        CountdownScreen.instance.Show();

    }

}
