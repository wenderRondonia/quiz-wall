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

        yield return SmallBallController.instance.DoingSmallBalls();

        for (int i=1; i <= 3; i++)
        {
            yield return DoingQuestion(i);
        }


        yield return BallController.UnlockingBalls(balls: 2);

        SmallBallController.instance.ResetSmallBalls();
        SumController.instance.ResetSum();

    }

   

    static IEnumerator DoingQuestion(int question)
    {
        QuestionScreen.instance.ResetQuestion();

        QuestionScreen.instance.SetupQuestion(question, 3, QuestionReader.GetQuestionRandom());
        QuestionScreen.instance.ShowAnswers(interactable: false);
        QuestionScreen.instance.HideQuestionText();

        QuestionScreen.instance.pickZoneController.Show();

        yield return QuestionScreen.instance.pickZoneController.WaitingAnswer();

        int zone = QuestionScreen.instance.pickZoneController.zoneSelected;

        yield return BallController.UnlockingBalls(
          balls: 1,
          pickZones: new[] {zone}
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
