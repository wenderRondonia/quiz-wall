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
            ballCount: 7,
            smallBallTypes: new[] {
                SmallBallType.Green, SmallBallType.Green,
                SmallBallType.White, SmallBallType.White, SmallBallType.White,
                SmallBallType.Red, SmallBallType.Red
            }
        );

        yield return PickZonesScreen.instance.DoingPickZones(PickZoneType.TwoZones);

        yield return BallController.UnlockingBalls(
            balls: 2,
            pickZones: PickZonesScreen.instance.GetZonesPicked()
        );

        yield return SmallBallController.instance.UnlockAndWaitSmallBalls();

        SmallBallController.instance.ResetSmallBalls();

        for (int i = 1; i <= 3; i++)
        {
            yield return DoingQuestion(i);
        }


        yield return BallController.UnlockingBalls(balls: 2);

        yield return SmallBallController.instance.UnlockAndWaitSmallBalls();

        SmallBallController.instance.ResetSmallBalls();

    }



    static IEnumerator DoingQuestion(int question)
    {

        QuestionScreen.instance.SetupQuestion(question, 3, QuestionReader.GetQuestionRandom());

        QuestionScreen.instance.ShowWithPickZone();

        yield return QuestionScreen.WaitingPickZoneAnswer();

        yield return BallController.UnlockingBalls(
          balls: 1,
          pickZones: new[] { QuestionScreen.instance.pickZoneController.zoneSelected }
        );

        QuestionScreen.instance.PrepareForAnswer();

        yield return QuestionScreen.WaitingAnswer();

        yield return QuestionScreen.DoingQuestionCheck();

        SmallBallController.instance.SetupCorrectAnswer();

        yield return SmallBallController.instance.UnlockAndWaitSmallBalls();

        SmallBallController.instance.ResetSmallBalls();

    }

}
