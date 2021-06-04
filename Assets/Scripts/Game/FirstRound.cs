using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FirstRound
{

    public static IEnumerator DoingFirstRound()
    {
        yield return RoundScreen.ShowingNewRound(round:1);

        yield return BallController.instance.AnimatingInitalBalls(ballCount: 8);

        yield return BallController.UnlockingBalls(balls: 2);

        yield return DoingFirstQuestion();

        yield return SmallBallController.instance.DoingSmallBalls();

        yield return QuestionScreen.DoingQuestionCheck();

        SmallBallController.instance.ResetSmallBalls();
        SumController.instance.ResetSum();

        // ################## end cycle ##################

        yield return new WaitForSeconds(1);

        yield return RoundScreen.ShowingNewRound(round: 1);

        yield return new WaitForSeconds(1);

        yield return BallController.UnlockingBalls(balls: 3);

        yield return DoingSecondQuestion();

        yield return SmallBallController.instance.DoingSmallBalls();

        yield return QuestionScreen.DoingQuestionCheck();

        SmallBallController.instance.ResetSmallBalls();
        SumController.instance.ResetSum();

    }

    static IEnumerator DoingFirstQuestion()
    {

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(1, 2, QuestionReader.GetQuestionRandom());

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswersPreview();

        yield return new WaitForSeconds(0.5f);

        yield return CountdownScreen.instance.DoingCountdown();

        yield return new WaitForSeconds(0.5f);

        QuestionScreen.instance.ShowAnswers();

        yield return QuestionScreen.WaitingAnswer();

        yield return new WaitForSeconds(1);

    }

    static IEnumerator DoingSecondQuestion()
    {

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(2, 2, QuestionReader.GetQuestionRandom());

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswersPreview();

        yield return new WaitForSeconds(0.5f);

        yield return CountdownScreen.instance.DoingCountdown();

        yield return new WaitForSeconds(0.5f);

        QuestionScreen.instance.ShowAnswers();

        yield return QuestionScreen.WaitingAnswer();

        yield return new WaitForSeconds(1);

    }

}
