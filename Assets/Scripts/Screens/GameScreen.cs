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

        yield return DoingFirstRound();


    }

    IEnumerator DoingFirstRound()
    {
        yield return RoundScreen.ShowingNewRound();

        yield return BallController.instance.AnimatingInitalBalls(ballCount: 8);

        yield return UnlockingBalls(balls: 2);

        yield return DoingFirstQuestion();

        yield return SmallBallController.instance.DoingSmallBalls();

        yield return DoingQuestionAnswer();

        SmallBallController.instance.ResetSmallBalls();
        SumController.instance.ResetSum();

        // ################## end cycle ##################

        yield return new WaitForSeconds(1);

        yield return RoundScreen.ShowingNewRound();

        yield return new WaitForSeconds(1);

        yield return UnlockingBalls(balls: 3);

        yield return DoingSecondQuestion();

        yield return SmallBallController.instance.DoingSmallBalls();

        yield return DoingQuestionAnswer();

        SmallBallController.instance.ResetSmallBalls();
        SumController.instance.ResetSum();

    }

    IEnumerator DoingFirstQuestion()
    {

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(1,2, QuestionReader.GetQuestionRandom());

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswersPreview();

        yield return new WaitForSeconds(0.5f);

        yield return CountdownScreen.instance.DoingCountdown();
        
        yield return new WaitForSeconds(0.5f);

        QuestionScreen.instance.ShowAnswers();

        yield return QuestionScreen.WaitingQuestionAnswer();
        
        yield return new WaitForSeconds(1);

    }


    IEnumerator DoingQuestionAnswer()
    {
        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowCorrectAnswer();

        //paying Money amount

        yield return new WaitForSeconds(2);

        QuestionScreen.instance.Hide();

        yield return new WaitForSeconds(2);

    }

    IEnumerator DoingSecondQuestion()
    {

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(2, 2, QuestionReader.GetQuestionRandom());

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswersPreview();

        yield return new WaitForSeconds(0.5f);

        yield return CountdownScreen.instance.DoingCountdown();

        yield return new WaitForSeconds(0.5f);

        QuestionScreen.instance.ShowAnswers();

        yield return QuestionScreen.WaitingQuestionAnswer();

        yield return new WaitForSeconds(1);

    }


    IEnumerator UnlockingBalls(int balls)
    {

        //Debug.Log("UnlockingBalls balls="+balls);

        for (int i = 0; i < balls; i++)
        {
            //Debug.Log("UnlockingBalls i=" + i);

            yield return BallController.instance.AnimatingLastBallToExit();

            SmallBallController.instance.ActivateRandomSmallBall();
            
            yield return new WaitForSeconds(0.5f);

        }

    }

    IEnumerator DoingPickZones()
    {
        PickZonesScreen.instance.Show();

        yield return PickZonesScreen.WaitingAnswer();

        yield return new WaitForSeconds(1);


    }


}
