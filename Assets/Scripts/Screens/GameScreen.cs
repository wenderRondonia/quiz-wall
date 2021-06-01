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

        yield return BallController.instance.DoAnimatingBalls();

        yield return DoingFirstQuestion();

        yield return SmallBallController.instance.DoingSmallBalls();

        yield return DoingQuestionAnswer();

        // ################## end cycle ##################

        yield return RoundScreen.ShowingNewRound();

        SmallBallController.instance.ActivateRandomSmallBall();

        SmallBallController.instance.ActivateRandomSmallBall();

        yield return DoingSecondQuestion();

        yield return SmallBallController.instance.DoingSmallBalls();

        yield return DoingQuestionAnswer();

    }

    IEnumerator DoingFirstQuestion()
    {

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(1,2, QuestionReader.questions[0]);

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

        yield return new WaitForSeconds(4);

        QuestionScreen.instance.Hide();

        SmallBallController.instance.ResetSmallBalls();

        yield return new WaitForSeconds(2);

    }

    IEnumerator DoingSecondQuestion()
    {
        yield return RoundScreen.ShowingNewRound();


        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(1, 2, QuestionReader.questions[1]);

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswersPreview();

        yield return new WaitForSeconds(0.5f);

        yield return CountdownScreen.instance.DoingCountdown();

        yield return new WaitForSeconds(0.5f);

        QuestionScreen.instance.ShowAnswers();

        yield return QuestionScreen.WaitingQuestionAnswer();

        yield return new WaitForSeconds(1);

    }




    IEnumerator DoingPickZones()
    {
        PickZonesScreen.instance.Show();

        yield return PickZonesScreen.WaitingAnswer();

        yield return new WaitForSeconds(1);


    }


}
