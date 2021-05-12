using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : Singleton<BallController>
{
    public Transform SpotsParent;
    public Transform BallsParent;
    public AudioSource[] BallTicks;
    public AudioSource BallReload;
    public AudioSource SoundBallUp;
    public AudioSource SoundBallDown;


    Image GetBall(int ballIndex)
    {
        return BallsParent.GetChild(ballIndex).GetComponent<Image>();
    }

    Vector3 GetSpot(int spot)
    {
        return SpotsParent.GetChild(spot).position;
    }
    
    public IEnumerator DoAnimatingBalls()
    {

        yield return AnimatingInitalBalls();

        yield return AnimatingLastBallToExit1();

        yield return AnimatingLastBallToExit2();

    }

    IEnumerator AnimatingInitalBalls()
    {
        float delayDuration = 0.4f;

        AnimateBall(ball: 0, spots: new[] { 3 });
        BallTicks[2].Play();

        yield return new WaitForSeconds(delayDuration);

        AnimateBall(ball: 1, spots: new[] { 2 });
        BallTicks[2].Play();

        yield return new WaitForSeconds(delayDuration);

        AnimateBall(ball: 2, spots: new[] { 1 });
        BallTicks[2].Play();

        yield return new WaitForSeconds(delayDuration);

        AnimateBall(ball: 3, spots: new[] { 0 });
        BallTicks[2].Play();

        yield return new WaitForSeconds(delayDuration);
    }

    IEnumerator AnimatingLastBallToExit1()
    {
        AnimateBall(ball: 0, spots: new[] { 4, 5, 6 });
        SoundBallDown.Play();

        AnimateBall(ball: 1, spots: new[] { 3 });
        AnimateBall(ball: 2, spots: new[] { 2 });
        AnimateBall(ball: 3, spots: new[] { 1 });

        yield return new WaitForSeconds(0.5f);
        int smallBallIndex = SmallBallController.instance.GetDisabledSmallBalls().SelectRandom();

        SmallBallController.instance.StartSmallBall(smallBallIndex);
        
    }

    IEnumerator AnimatingLastBallToExit2()
    {
        AnimateBall(ball: 1, spots: new[] { 4, 5, 6 });
        SoundBallDown.Play();

        AnimateBall(ball: 2, spots: new[] { 3 });
        AnimateBall(ball: 3, spots: new[] { 2 });
        
        yield return new WaitForSeconds(0.5f);

        int smallBallIndex = SmallBallController.instance.GetDisabledSmallBalls().SelectRandom();
        SmallBallController.instance.StartSmallBall(smallBallIndex);

    }


    void AnimateBall(int ball, int[] spots, float duration = 0.4f, float rotations = 4f)
    {
        StartCoroutine(AnimatingBall(ball, spots, duration, rotations));
    }

    IEnumerator AnimatingBall(int ball,int[] spots,float duration = 0.4f,float rotations=4f)
    {
        Image imageBall = GetBall(ball);
        

        iTween.RotateAdd(imageBall.gameObject,Vector3.back * 360 * rotations, duration);

        Vector3[] path = new Vector3[spots.Length];
        for(int i=0; i < spots.Length; i++)
        {
            path[i] = GetSpot(spots[i]);
        }

        var hash = iTween.Hash(
            "easetype", iTween.EaseType.linear,
            "time", duration
        );

        if (path.Length==1)
        {
            hash.Add("position", path[0]);
        }
        else if(path.Length > 1)
        {
            hash.Add("path", path);
        }
        else
        {
            Debug.LogError("AnimatingBall failed no path");
        }

        
        iTween.MoveTo(imageBall.gameObject, hash);

        yield return imageBall.gameObject.WaitingItween();
    }
}
