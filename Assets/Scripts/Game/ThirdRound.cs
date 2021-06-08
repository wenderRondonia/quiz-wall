using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ThirdRound
{
    static List<bool> answers = new List<bool>();


    public static IEnumerator DoingFirstPart()
    {
        yield return RoundScreen.ShowingNewRound(round: 3);

        for (int i=1; i <=3;i++) {

            yield return BallController.instance.AnimatingInitalBalls(ballCount: 3);
            
            yield return BallController.instance.AnimatingLastBallToExit();

            QuestionScreen.instance.ResetQuestion();

            QuestionScreen.instance.SetupQuestion(i, 3, QuestionReader.GetQuestionRandom());

            QuestionScreen.instance.Show();
            QuestionScreen.instance.ShowAnswers();

            yield return QuestionScreen.WaitingAnswer();

            yield return QuestionScreen.DoingQuestionCheck();

            for (int b=0; b < 3;b++)
            {
                var ball = BallController.instance.GetBall(b);
                SmallBallType smallBallType = QuestionScreen.instance.IsAnsweredRight() ? SmallBallType.Green : SmallBallType.Red;
                
                ball.SetBallType(smallBallType);

            }

            yield return SmallBallController.instance.DoingSmallBalls();


        }
    }

    public static IEnumerator DoingSecondPart()
    {
        yield return RoundScreen.ShowingNewRound(round: 3);

        yield return BallController.instance.AnimatingInitalBalls(
           ballCount: 9,
           smallBallTypes: new[] {
                SmallBallType.Green, SmallBallType.Green, SmallBallType.Green,
                SmallBallType.White, SmallBallType.White, SmallBallType.White,
                SmallBallType.Red, SmallBallType.Red, SmallBallType.Red
           }
       );

        yield return PickZonesScreen.instance.DoingPickZones(PickZoneType.ThreeZones);

        yield return BallController.UnlockingBalls(
            balls:3,
            pickZones: PickZonesScreen.instance.GetZonesPicked() 
        );

        yield return SmallBallController.instance.DoingSmallBalls();



    }

    public static IEnumerator DoingThirdRound()
    {

        yield return RoundScreen.ShowingNewRound(round: 3);

        for (int i = 1; i <= 3; i++)
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

    }

   
}
