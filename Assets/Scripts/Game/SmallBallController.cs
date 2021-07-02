using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SmallBallController : Singleton<SmallBallController>
{
    public SmallBallBehaviour prefabSmallBall;
    public Transform smallBallParent;
    public Transform spotsParent;
    public GameObject initalHolder;
    public AudioSource soundRelease;

    public List<SmallBallBehaviour> GetActiveSmallBalls()
    {
        return smallBallParent.GetComponentsInChildren<SmallBallBehaviour>().ToList();
    }

    public List<int> GetAvailableSpots()
    {
        var spots = Enumerable.Range(0, spotsParent.childCount).ToList();
        var balls = GetActiveSmallBalls();
        foreach (var ball in balls)
        {

            spots.Remove(ball.GetInitialSpot());
        }

        return spots;
    }

    public void ActivateRandomSmallBall(SmallBallType smallBallType = SmallBallType.White)
    {

        int smallBallIndex = GetAvailableSpots().SelectRandom();

        StartSmallBall(smallBallIndex, smallBallType);

    }

    public void StartSmallBall(int index, SmallBallType smallBallType = SmallBallType.White)
    {

        Vector3 spotPos = spotsParent.GetChild(index).position;
        var newSmallBall = GameObject.Instantiate(prefabSmallBall, spotPos, Quaternion.identity, smallBallParent);

        newSmallBall.transform.localScale = Vector3.one;
        newSmallBall.gameObject.SetActive(true);
        newSmallBall.SetInitialSpot(index);
        newSmallBall.SetSmallBallType(smallBallType);

        //Debug.Log("StartSmallBall index="+index+ " smallBallType="+ smallBallType);
    }



    public void ReleaseHolder()
    {
        initalHolder.SetActive(false);
        soundRelease.Play();

    }

    public bool IsCompletedAnySmallBalls()
    {
        return GetActiveSmallBalls().Any(s => s.HasReachSumArea());
    }


    public bool IsCompletedAllSmallBalls()
    {
        return GetActiveSmallBalls().TrueForAll(s => s.HasReachSumArea());
    }

    public void ResetSmallBalls()
    {
        initalHolder.SetActive(true);

        SumController.instance.ResetSum();

        smallBallParent.DestroyChildren();

    }

    public void UnlockSmallBalls()
    {
        ReleaseHolder();
    }


    public IEnumerator WaitingAnySmallBalls()
    {

        yield return new WaitUntil(() => IsCompletedAnySmallBalls());

        yield return new WaitForSeconds(1);

    }

    public IEnumerator WaitingSmallBalls()
    {

        yield return new WaitUntil(() => IsCompletedAllSmallBalls());

        yield return new WaitForSeconds(1);

    }

    public IEnumerator UnlockAndWaitSmallBalls()
    {
        UnlockSmallBalls();

        yield return WaitingSmallBalls();


    }

    public void SetupCorrectAnswer()
    {
        SetCorrectAnswer(QuestionScreen.instance.IsAnsweredRight());
    }

    public void SetCorrectAnswer(bool answerRight)
    {
        SmallBallType smallBallType = answerRight ? SmallBallType.Green : SmallBallType.Red;

        foreach (var ball in GetActiveSmallBalls())
        {
            ball.SetSmallBallType(smallBallType);
        }
    }

    public void SetCorrectAnswers(List<bool> answers)
    {
        SmallBallType[] ballTypes = new SmallBallType[answers.Count];

        var balls = GetActiveSmallBalls();

        for (int i = 0; i < answers.Count; i++)
        {
            ballTypes[i] = answers[i] ? SmallBallType.Green : SmallBallType.Red;

        }

        foreach (var ball in balls)
        {
            int ballIndex = ball.transform.GetSiblingIndex();
            ball.SetSmallBallType(ballTypes[ballIndex]);
        }

    }

}
