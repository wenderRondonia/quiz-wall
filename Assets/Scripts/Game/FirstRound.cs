using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FirstRound
{
    public static float MoneyGainedAtFirstRound = 0;

    public static IEnumerator DoingFirstRound()
    {
        yield return RoundScreen.ShowingNewRound(round: 1);

        yield return BallController.instance.AnimatingInitalBalls(ballCount: 4);

        for (int i = 1; i <= 2; i++)
        {
            yield return DoingQuestionCycle(question: i);
        }

        MoneyGainedAtFirstRound = Prefs.GetMoney;
    }

    static IEnumerator DoingQuestionCycle(int question)
    {

        yield return BallController.UnlockingBalls(balls: 2);

        yield return ShowingQuestion(question: 1);

        yield return SmallBallController.instance.WaitingAnySmallBalls();

        QuestionScreen.instance.Hide();

        yield return SmallBallController.instance.WaitingSmallBalls();

        SmallBallController.instance.SetupCorrectAnswer();

        yield return QuestionScreen.DoingQuestionCheck();

        SmallBallController.instance.SetupCorrectAnswer();

        SmallBallController.instance.ResetSmallBalls();

        yield return new WaitForSeconds(1);

    }

    static IEnumerator ShowingQuestion(int question)
    {

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(question, 2, QuestionReader.GetQuestionRandom());

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswersPreview();

        yield return new WaitForSeconds(0.5f);

        yield return CountdownScreen.instance.DoingCountdown();

        yield return new WaitForSeconds(0.5f);

        QuestionScreen.instance.ShowAnswers();

        SmallBallController.instance.UnlockSmallBalls();

    }

}
