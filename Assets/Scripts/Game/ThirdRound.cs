using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ThirdRound
{
    static List<bool> answers = new List<bool>();

    public static IEnumerator DoingThirdRound()
    {

        yield return RoundScreen.ShowingNewRound(round: 3);

        yield return BallController.instance.AnimatingInitalBalls(ballCount: 4);

        for (int i = 1; i <= 4; i++)
        {
            yield return DoingQuestion(question: i);
        }

        yield return SignContractScreen.instance.WaitingAnswer();


        AnswersScreen.instance.correctAnswers = answers;

        yield return AnswersScreen.instance.ShowingCorrectAnswers();


        if (SignContractScreen.instance.answer==ContractAnswer.Sign)
        {
            yield return SigningContract();
        }
        else
        {
            yield return ContinueGame();

        }

    }

    static IEnumerator DoingQuestion(int question)
    {

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(question, 4, QuestionReader.GetQuestionRandom());

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswers();

        yield return QuestionScreen.WaitingAnswer();
        
        answers.Add(QuestionScreen.instance.IsAnsweredRight());

    }

    static IEnumerator SigningContract()
    {
        yield return 0;
    }

    static IEnumerator ContinueGame()
    {
       

        SmallBallType[] ballTypes = new SmallBallType[answers.Count];
        for (int i=0; i < answers.Count;i++)
        {
            ballTypes[i] = answers[i] ? SmallBallType.Green : SmallBallType.Red;
        }

        yield return BallController.UnlockingBalls(
            balls: 4,
            ballTypes: ballTypes
        );

        yield return SmallBallController.instance.DoingSmallBalls();


        yield return EndingGame();

    }

    static IEnumerator EndingGame()
    {
        //TODO: End Game Check
        
        yield return 0;

        bool hasWon = false;

        if (hasWon)
        {
            WonScreen.instance.textAmount.text = "+ R$ 9 430";

            WonScreen.instance.Show();
        }
        else
        {
            LostScreen.instance.Show();
        }
    }
}
