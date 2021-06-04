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

        yield return SecondRound.DoingSecondRound();

    }    


}
