using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FirstRound
{

    public static IEnumerator DoingFirstRound()
    {
        yield return RoundScreen.ShowingNewRound(round: 1);

        yield return BallController.instance.AnimatingInitalBalls(ballCount: 4);

        for (int i=1; i <= 2; i++)
        {
            yield return DoingQuestionRound( question: i );
        }        
    }

    static IEnumerator DoingQuestionRound(int question)
    {

        yield return BallController.UnlockingBalls(balls: 2);

        yield return DoingQuestion(question: 1);

        yield return SmallBallController.instance.DoingSmallBalls();

        yield return QuestionScreen.DoingQuestionCheck();

        bool answerRight = QuestionScreen.instance.IsAnsweredRight();
        SmallBallController.instance.GetActiveSmallBalls().ForEach(b => b.SetSmallBallType(answerRight ? SmallBallType.Green : SmallBallType.Red)); ;

        SmallBallController.instance.ResetSmallBalls();

        yield return new WaitForSeconds(1);
    }

    static IEnumerator DoingQuestion(int question)
    {

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(question, 2, QuestionReader.GetQuestionRandom());

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
