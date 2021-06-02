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
    public AudioSource SoundPipe;
    public Image Ball;
    int finalSpot = 8;

    Image GetBall(int ballIndex)
    {

        if (ballIndex >= BallsParent.childCount)
        {
            Debug.Log("GetBall failed ballindex="+ballIndex);
            return null;
        }
        return BallsParent.GetChild(ballIndex).GetComponent<Image>();
    }

    public bool IsBallsAnimating()
    {
        for (int i=0; i < BallsParent.childCount;i++)
        {
            var ball = GetBall(i);
            if (ball.HasITween())
            {
                return true;
            }
        }

        return false;
    }

    Vector3 GetSpot(int spot)
    {
        if (spot >= SpotsParent.childCount)
        {
            Debug.Log("GetSpot spot="+ spot);
            return Vector3.zero;
        }
        return SpotsParent.GetChild(spot).position;
    }

    public IEnumerator AnimatingInitalBalls(int ballCount)
    {
        float delayDuration = 0.4f;                

        for (int i=0;i < ballCount;i++) {
            
            var newBall = GameObject.Instantiate(Ball,parent:BallsParent);
            newBall.gameObject.SetActive(true);
            newBall.transform.position = GetSpot(0);

            AnimateBall(ball: i, spots: new[] { finalSpot - i });
            
            BallTicks[2].Play();

            yield return new WaitForSeconds(delayDuration);
        }

        yield return new WaitWhile(IsBallsAnimating);

    }

    public IEnumerator AnimatingLastBallToExit()
    {
        SoundBallDown.Play();

        Debug.Log("AnimatingLastBallToExit");

        AnimateBall(ball: 0, spots: new[] { SpotsParent.childCount-3, SpotsParent.childCount - 2, SpotsParent.childCount - 1 }, oncomplete:image=> {
            Destroy(image.gameObject);
        });

        for (int i=1; i < BallsParent.childCount;i++)
        {
            Debug.Log("AnimatingLastBallToExit Ball i="+i+" spot="+ (finalSpot - i));
            AnimateBall(ball: i, spots: new[] { finalSpot - i +1 });
        }

        
        yield return new WaitWhile(IsBallsAnimating);

    }



    void AnimateBall(int ball, int[] spots, float duration = 0.6f, System.Action<Image> oncomplete = null)
    {
        StartCoroutine(AnimatingBall(ball, spots, duration,oncomplete));
    }

    IEnumerator AnimatingBall(int ball,int[] spots,float duration = 0.6f,System.Action<Image> oncomplete=null)
    {

        SoundPipe.Play();

        Image imageBall = GetBall(ball);

        if (imageBall == null)
        {
            yield break;
        }

        iTween.RotateAdd(imageBall.gameObject,iTween.Hash(
            "easetype",iTween.EaseType.linear,
            "looptype",iTween.LoopType.loop,
            "amount", Vector3.back * 360 * 2, 
            "time", duration
        ));

        Vector3[] path = new Vector3[spots.Length];
        for(int i=0; i < spots.Length; i++)
        {
            path[i] = GetSpot(spots[i]);
        }

        var hash = iTween.Hash(
            "name", "Movement",
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

        yield return imageBall.gameObject.WaitingItween("Movement");
        
        imageBall.RemoveTweens();

        SoundBallUp.Play();

        if (oncomplete != null) oncomplete(imageBall);

    }
}
