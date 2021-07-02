using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ThirdRound
{
    static List<bool> answers = new List<bool>();

    public static IEnumerator DoingThirdRound()
    {

        yield return DoingFirstPart();

        yield return DoingSecondPart();

        yield return DoingFinalPart();

    }


    public static IEnumerator DoingFirstPart()
    {
        yield return RoundScreen.ShowingNewRound(round: 3);

        yield return BallController.instance.AnimatingInitalBalls(ballCount: 9);


        for (int i = 1; i <= 3; i++)
        {

            yield return BallController.UnlockingBalls(
                balls: 3
            );

            QuestionScreen.instance.ResetQuestion();

            QuestionScreen.instance.SetupQuestion(i, 3, QuestionReader.GetQuestionRandom());

            QuestionScreen.instance.Show();
            QuestionScreen.instance.ShowAnswers();

            yield return QuestionScreen.WaitingAnswer();

            yield return QuestionScreen.DoingQuestionCheck();

            SmallBallController.instance.SetupCorrectAnswer();

            yield return SmallBallController.instance.UnlockAndWaitSmallBalls();

            SmallBallController.instance.ResetSmallBalls();

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

        yield return UnlockingFirstThreeGoodBalls();

        yield return DoingQuestions();

        yield return UnlockingLastThreeGoodBalls();



    }

    static IEnumerator UnlockingFirstThreeGoodBalls()
    {
        yield return PickZonesScreen.instance.DoingPickZones(PickZoneType.ThreeZones);

        yield return BallController.UnlockingBalls(
            balls: 3,
            pickZones: PickZonesScreen.instance.GetZonesPicked()
        );

        yield return SmallBallController.instance.UnlockAndWaitSmallBalls();

        SmallBallController.instance.ResetSmallBalls();
    }

    static IEnumerator DoingQuestions()
    {
        for (int i = 1; i <= 3; i++)
        {
            QuestionScreen.instance.ResetQuestion();
            QuestionScreen.instance.SetupQuestion(i, 3, QuestionReader.GetQuestionRandom());

            QuestionScreen.instance.Show();
            QuestionScreen.instance.HideQuestionText();
            QuestionScreen.instance.ShowAnswers(interactable: false);

            QuestionScreen.instance.pickBallMultiplier.Show();
            yield return QuestionScreen.instance.pickBallMultiplier.WaitingAnswer();

            QuestionScreen.instance.pickZoneController.Show();
            yield return QuestionScreen.instance.pickZoneController.WaitingAnswer();

            yield return BallController.UnlockingBalls(
                balls: 1,
                ballMultiplier: QuestionScreen.instance.pickBallMultiplier.indexAnswered + 1,
                pickZones: new[] { QuestionScreen.instance.pickZoneController.zoneSelected }
            );

            QuestionScreen.instance.ShowQuestionText();
            QuestionScreen.instance.ShowAnswers(interactable: true);

            yield return QuestionScreen.WaitingAnswer();

            yield return QuestionScreen.DoingQuestionCheck();

            SmallBallController.instance.SetupCorrectAnswer();

            yield return SmallBallController.instance.UnlockAndWaitSmallBalls();

            SmallBallController.instance.ResetSmallBalls();

        }
    }

    static IEnumerator UnlockingLastThreeGoodBalls()
    {

        yield return BallController.UnlockingBalls(
            balls: 3
        );

        yield return SmallBallController.instance.UnlockAndWaitSmallBalls();

        SmallBallController.instance.ResetSmallBalls();
    }

    static IEnumerator DoingFinalPart()
    {
        yield return RoundScreen.ShowingNewRound(round: 3);

        yield return BallController.instance.AnimatingInitalBalls(
            ballCount: 4
        );

        yield return BallController.UnlockingBalls(balls: 4);

        for (int i = 1; i <= 4; i++)
        {
            yield return DoingQuestion(question: i);
        }

        yield return SignContractScreen.instance.WaitingAnswer();

        AnswersScreen.instance.correctAnswers = answers;

        yield return AnswersScreen.instance.ShowingCorrectAnswers();



        if (SignContractScreen.instance.answer == ContractAnswer.Sign)
        {

            DoSignContract();

        }
        else
        {

            SmallBallController.instance.SetCorrectAnswers(answers);

            yield return SmallBallController.instance.UnlockAndWaitSmallBalls();

            SmallBallController.instance.ResetSmallBalls();

        }
    }

    static void DoSignContract()
    {
        float contractMoney = FirstRound.MoneyGainedAtFirstRound;
        if (contractMoney == 0)
        {
            //testing
            contractMoney = 1200;
        }

        foreach (var answeredCorrect in answers)
        {
            if (answeredCorrect)
            {
                contractMoney += 3000;
            }
        }

        Prefs.SetMoney(contractMoney);
    }

    static IEnumerator DoingQuestion(int question)
    {
        //Debug.Log("DoingQuestion=" + question);

        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(question, 4, QuestionReader.GetQuestionRandom());

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswers();

        yield return QuestionScreen.WaitingAnswer();

        answers.Add(QuestionScreen.instance.IsAnsweredRight());

        yield return new WaitForSeconds(1.5f);

        //Debug.Log("DoingQuestion=" + question+" End");

    }


}
