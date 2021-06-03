using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SmallBallController : Singleton<SmallBallController>
{
    public Transform smallBallParent;
    public GameObject initalHolder;
    public AudioSource soundRelease;

    List<SmallBallBehaviour> smallBalls = new List<SmallBallBehaviour>();


    void Start()
    {
        
        smallBalls = smallBallParent.GetComponentsInChildren<SmallBallBehaviour>(true).ToList();
    }

    public List<SmallBallBehaviour> GetActiveSmallBalls()
    {
        List<SmallBallBehaviour> list = new List<SmallBallBehaviour>();
        smallBalls.ForEach(s => {
            if (s.isActiveAndEnabled)
                list.Add(s);

        });

        return list;
    }

    public List<int> GetDisabledSmallBalls()
    {
        List<int> list = new List<int>();
        smallBalls.ForEach(s=> {
            if (!s.isActiveAndEnabled)
                list.Add(s.transform.GetSiblingIndex());

        });

        return list;
    }

    public void ActivateRandomSmallBall()
    {
        int smallBallIndex = GetDisabledSmallBalls().SelectRandom();

        StartSmallBall(smallBallIndex);
    }

    public void StartSmallBall(int index)
    {
        smallBalls[index].gameObject.SetActive(true);
    }

    public void ReleaseHolder()
    {
        initalHolder.SetActive(false);
        soundRelease.Play();

    }

    public bool IsCompletedAllSmallBalls()
    {
        return GetActiveSmallBalls().TrueForAll(s=>s.HasReachSumArea()) ;
    }

    public void ResetSmallBalls()
    {

        initalHolder.SetActive(true);

        foreach (var smallBall in smallBalls)
        {
            
            smallBall.ResetSmallBall();
        }
    }

    public IEnumerator DoingSmallBalls()
    {
        ReleaseHolder();

        yield return new WaitUntil(() => IsCompletedAllSmallBalls());

        yield return new WaitForSeconds(2);



    }

}
