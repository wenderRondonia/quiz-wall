using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class SecondRound
{
    public static IEnumerator DoingSecondRound()
    {

        yield return RoundScreen.ShowingNewRound(round: 2);

        yield return BallController.instance.AnimatingInitalBalls(
            ballCount: 2,
            smallBallTypes: new[] { SmallBallType.Green, SmallBallType.Green }
        );

        yield return PickZonesScreen.instance.DoingPickZones(PickZoneType.TwoZones);

        yield return BallController.UnlockingBalls(
            balls: 2,
            pickZones: PickZonesScreen.instance.GetZonesPicked()
        );

        yield return SmallBallController.instance.DoingSmallBalls();

        yield return RoundScreen.ShowingNewRound(round: 2);


        yield return DoingQuestion(1);

        yield return RoundScreen.ShowingNewRound(round: 2);

        yield return DoingQuestion(2);

        yield return RoundScreen.ShowingNewRound(round: 2);

        yield return DoingQuestion(3);

        yield return RoundScreen.ShowingNewRound(round: 2);

        yield return BallController.instance.AnimatingInitalBalls(
            ballCount: 2,
            smallBallTypes: new[] { SmallBallType.Red, SmallBallType.Red }
        );

        yield return BallController.UnlockingBalls(balls: 2);

        SmallBallController.instance.ResetSmallBalls();
        SumController.instance.ResetSum();
    }

    static IEnumerator DoingQuestion(int question)
    {
        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(question, 3, QuestionReader.GetQuestionRandom());
        QuestionScreen.instance.pickBallMultiplier.Show();

        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowAnswers( interactable: false);
        QuestionScreen.instance.HideQuestionText();

        yield return BallController.instance.AnimatingInitalBalls(ballCount: 1);

        yield return QuestionScreen.instance.pickBallMultiplier.WaitingAnswer();

        QuestionScreen.instance.pickBallMultiplier.Hide();
        QuestionScreen.instance.pickZoneController.Show();

        yield return QuestionScreen.instance.pickZoneController.WaitingAnswer();

        int ballMultiplier = QuestionScreen.instance.pickBallMultiplier.indexAnswered + 1;
        int zone = QuestionScreen.instance.pickZoneController.zoneSelected;

        yield return BallController.UnlockingBalls(
          balls: ballMultiplier,
          pickZones: Enumerable.Repeat(zone, ballMultiplier).ToArray() 
        );

        QuestionScreen.instance.ShowQuestionText();
        QuestionScreen.instance.ShowAnswers(interactable: true);

        yield return QuestionScreen.WaitingAnswer();

        yield return QuestionScreen.DoingQuestionCheck();

        //TODO: check answer transform ball types

        yield return SmallBallController.instance.DoingSmallBalls();

        SmallBallController.instance.ResetSmallBalls();

        SumController.instance.ResetSum();
    }



}
